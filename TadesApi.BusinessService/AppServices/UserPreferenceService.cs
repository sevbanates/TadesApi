using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TadesApi.BusinessService.AppServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.ViewModels.User;

namespace TadesApi.BusinessService.AppServices
{
    public class UserPreferenceService : IUserPreferenceService
    {
        private readonly IRepository<UserPreferences> _preferenceRepository;
        private readonly IRepository<AccounterUsers> _accounterUsersRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUser _currentUser;

        public UserPreferenceService(
            IRepository<UserPreferences> preferenceRepository,
            IRepository<AccounterUsers> accounterUsersRepository,
            IRepository<User> userRepository,
            ICurrentUser currentUser)
        {
            _preferenceRepository = preferenceRepository;
            _accounterUsersRepository = accounterUsersRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public ActionResponse<AccounterUserSelectionResponseDto> GetAccessibleUsers()
        {
            var response = new ActionResponse<AccounterUserSelectionResponseDto>();

            try
            {
                if (!_currentUser.IsAccounter)
                {
                    return response.ReturnResponseError("Bu işlem sadece muhasebeciler tarafından kullanılabilir.");
                }

                // Accounter'ın erişebileceği kullanıcıları getir
                var accessibleUserIds = _accounterUsersRepository.TableNoTracking
                    .Where(au => au.AccounterUserId == _currentUser.UserId && !au.IsDeleted)
                    .Select(au => au.TargetUserUserId)
                    .ToList();

                var accessibleUsers = _userRepository.TableNoTracking
                    .Where(u => accessibleUserIds.Contains(u.Id) && !u.IsDeleted && u.IsActive)
                    .Select(u => new AccounterAccessibleUserDto
                    {
                        UserId = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Email = u.Email,
                        IsSelected = u.Id == _currentUser.SelectedUserId
                    })
                    .ToList();

                response.Entity = new AccounterUserSelectionResponseDto
                {
                    AccessibleUsers = accessibleUsers,
                    CurrentSelectedUserId = _currentUser.SelectedUserId
                };

                return response;
            }
            catch (Exception ex)
            {
                return response.ReturnResponseError($"Kullanıcılar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public ActionResponse<bool> SetSelectedUser(SetSelectedUserDto dto)
        {
            var response = new ActionResponse<bool>();

            try
            {
                if (!_currentUser.IsAccounter)
                {
                    return response.ReturnResponseError("Bu işlem sadece muhasebeciler tarafından kullanılabilir.");
                }

                // Eğer bir kullanıcı seçilmişse, bu kullanıcıya erişim yetkisi olup olmadığını kontrol et
                //if (dto.SelectedUserId.HasValue)
                //{
                //    var hasAccess = _accounterUsersRepository.TableNoTracking
                //        .Any(au => au.AccounterUserId == _currentUser.UserId && 
                //                  au.TargetUserUserId == dto.SelectedUserId.Value && 
                //                  !au.IsDeleted);

                //    if (!hasAccess)
                //    {
                //        return response.ReturnResponseError("Seçilen kullanıcıya erişim yetkiniz bulunmamaktadır.");
                //    }
                //}

                // CurrentUser'daki SelectedUserId'yi güncelle (session için)
                _currentUser.SelectedUserId = dto.SelectedUserId;

                // Kalıcı olarak kaydet
                SaveUserPreference(_currentUser.UserId, "SELECTED_USER_ID", dto.SelectedUserId?.ToString());

                response.Entity = true;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                return response.ReturnResponseError($"Seçili kullanıcı ayarlanırken hata oluştu: {ex.Message}");
            }
        }

        public ActionResponse<long?> GetSelectedUserId()
        {
            var response = new ActionResponse<long?>();

            try
            {
                response.Entity = _currentUser.SelectedUserId;
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                return response.ReturnResponseError($"Seçili kullanıcı ID'si alınırken hata oluştu: {ex.Message}");
            }
        }

        public ActionResponse<bool> InitializeSelectedUserFromPreferences()
        {
            var response = new ActionResponse<bool>();

            try
            {
                if (!_currentUser.IsAccounter)
                {
                    response.Entity = false;
                    return response;
                }

                var preference = _preferenceRepository.TableNoTracking
                    .FirstOrDefault(p => p.UserId == _currentUser.UserId && 
                                        p.PreferenceKey == "SELECTED_USER_ID" && 
                                        !p.IsDeleted);

                if (preference != null && !string.IsNullOrEmpty(preference.PreferenceValue))
                {
                    if (long.TryParse(preference.PreferenceValue, out long selectedUserId))
                    {
                        // Hala erişim yetkisi var mı kontrol et
                        var hasAccess = _accounterUsersRepository.TableNoTracking
                            .Any(au => au.AccounterUserId == _currentUser.UserId && 
                                      au.TargetUserUserId == selectedUserId && 
                                      !au.IsDeleted);

                        if (hasAccess)
                        {
                            _currentUser.SelectedUserId = selectedUserId;
                        }
                    }
                }

                response.Entity = true;
                return response;
            }
            catch (Exception ex)
            {
                return response.ReturnResponseError($"Kullanıcı tercihleri yüklenirken hata oluştu: {ex.Message}");
            }
        }

        private void SaveUserPreference(long userId, string key, string value)
        {
            try
            {
                var existingPreference = _preferenceRepository.Table
                    .FirstOrDefault(p => p.UserId == userId && p.PreferenceKey == key && !p.IsDeleted);

                if (existingPreference != null)
                {
                    existingPreference.PreferenceValue = value;
                    existingPreference.SelectedUserId = string.IsNullOrEmpty(value) ? null : long.Parse(value);
                    _preferenceRepository.Update(existingPreference);
                }
                else
                {
                    var newPreference = new UserPreferences
                    {
                        UserId = userId,
                        PreferenceKey = key,
                        PreferenceValue = value,
                        SelectedUserId = string.IsNullOrEmpty(value) ? null : long.Parse(value),
                        CreDate = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _preferenceRepository.Insert(newPreference);
                }
            }
            catch (Exception)
            {
                // Log error but don't throw - preferences are not critical
            }
        }
    }
}

