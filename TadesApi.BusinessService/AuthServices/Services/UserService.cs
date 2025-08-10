using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendWithBrevo;
using System;
using System.Collections.Generic;
using System.Linq;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ConstantKeys;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Core.Security;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.ViewModels.Client;

namespace TadesApi.BusinessService.AuthServices.Services;

public class UserService : BaseServiceNg<User, UserBasicDto, CreateUserDto, UpdateUserDto>, IUserService
{
    private readonly IEmailHelper _emailHelper;
    private readonly IRepository<UserRequest> _userRequestRepo;
    private readonly IRepository<AccounterUsers> _accUsersRepository;
    public UserService(
        IRepository<User> entityRepository,
        ILocalizationService locManager,
        IMapper mapper,
        IEmailHelper emailHelper,
        ICurrentUser session, IRepository<UserRequest> userRequestRepo, IRepository<AccounterUsers> accUsersRepository) : base(entityRepository, locManager, mapper, session)
    {
        _emailHelper = emailHelper;
        _userRequestRepo = userRequestRepo;
        _accUsersRepository = accUsersRepository;
    }


    public ActionResponse<UserBasicDto> GetWebSiteUser(long userId)
    {
        var response = new ActionResponse<UserBasicDto>();

        var user = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == userId);
        if (user == null)
        {
            return response;
        }
        response.Entity = _mapper.Map(user, new UserBasicDto());
        return response;
    }

    public PagedAndSortedResponse<UserBasicDto> GetMulti(UserSearchInput input)
    {
        var query = _entityRepository.TableNoTracking
            .Where(x => x.IsActive == input.IsActive)
            .WhereIf(!_session.IsAdmin, x => x.Id == _session.UserId)
            .WhereIf(!string.IsNullOrEmpty(input.Search),
                x => x.FirstName.Contains(input.Search) || x.LastName.Contains(input.Search))
            .WhereIf(input.StartDate.HasValue, x => x.CreDate >= input.StartDate)
            .WhereIf(input.EndDate.HasValue, x => x.CreDate <= input.EndDate)
            .WhereIf(input.RoleId.HasValue, x => x.RoleId == input.RoleId);
            //.WhereIf(input.ClientId.HasValue, x => x.ClientId == input.ClientId && !x.IsPrimaryForClient);

        var totalCount = query.Count();
        var pagedAndSortedData = CommonFunctions.GetPagedAndSortedData(query, input.Limit, input.Page, input.SortDirection, input.SortBy);
        return new PagedAndSortedResponse<UserBasicDto>
        {
            EntityList = _mapper.Map<List<UserBasicDto>>(pagedAndSortedData),
            TotalCount = totalCount
        };
    }

    public new ActionResponse<UserBasicDto> Create(CreateUserDto input)
    {
        var toCreate = _mapper.Map<User>(input);
        if (_entityRepository.TableNoTracking.Any(x => x.UserName == input.UserName))
            return new ActionResponse<UserBasicDto>().ReturnResponseError(
                $"{toCreate.UserName} already exists! Please enter a different user name.");

        if (_entityRepository.TableNoTracking.Any(x => x.Email == input.Email))
            return new ActionResponse<UserBasicDto>().ReturnResponseError($"{toCreate.Email} already exist.");

        var salt = Hasher.GetSalt();
        var hashedPassword = Hasher.GenerateHash(input.Password + salt);
        toCreate.Password = hashedPassword;
        toCreate.PasswordSalt = salt;
        toCreate.UserName = toCreate.UserName.ToLower();
        //toCreate.ClientId = 1;
        _entityRepository.Insert(toCreate);
        
        // send email to the user
        var recipients = new List<Recipient>
        {
            new(email: toCreate.Email, name: toCreate.FirstName),
        };
        var parameters = new Dictionary<string, string>
        {
            { EmailParams.FirstName, input.FirstName },
            { EmailParams.UserName, input.UserName }
        };
        const string subject = "Your account has been created!";
        //_emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.AccountCreated);
        
        return new ActionResponse<UserBasicDto> { Entity = _mapper.Map<UserBasicDto>(toCreate) };
    }

    public new ActionResponse<UserBasicDto> Update(long id, UpdateUserDto input)
    {
        if (_entityRepository.TableNoTracking.Any(x => x.UserName == input.UserName && x.Id != id))
            return new ActionResponse<UserBasicDto>().ReturnResponseError(
                $"{input.UserName} already exists! Please enter a different user name.");

        if (_entityRepository.TableNoTracking.Any(x => x.Email == input.Email && x.Id != id))
            return new ActionResponse<UserBasicDto>().ReturnResponseError($"{input.Email} already exist.");

        var toUpdate = _entityRepository.Table.FirstOrDefault(x => x.Id == id);
        if (toUpdate == null || toUpdate.GuidId != input.GuidId)
            return new ActionResponse<UserBasicDto>().ReturnResponseError("User not found!");

        _mapper.Map(input, toUpdate);
        _entityRepository.Update(toUpdate);
        return new ActionResponse<UserBasicDto> { Entity = _mapper.Map<UserBasicDto>(toUpdate) };
    }


    public ActionResponse<UserBasicDto> UpdateProfile(UpdateProfileDto input)
    {
        var user = _entityRepository.Table.FirstOrDefault(x => x.Id == _session.UserId);
        if (user == null)
            return new ActionResponse<UserBasicDto>().ReturnResponseError("User not found!");

        if (_entityRepository.TableNoTracking.Any(x => x.Email == input.Email && x.Id != user.Id))
            return new ActionResponse<UserBasicDto>().ReturnResponseError($"{input.Email} already exist.");

        _mapper.Map(input, user);
        _entityRepository.Update(user);
        return new ActionResponse<UserBasicDto> { Entity = _mapper.Map<UserBasicDto>(user) };
    }

    public ActionResponse<bool> UpdateSelfPassword(UpdateSelfPasswordDto input)
    {
        var user = _entityRepository.Table.FirstOrDefault(x => x.Id == _session.UserId);
        if (user == null)
            return new ActionResponse<bool>().ReturnResponseError("User not found!");

        var oldPassword = Hasher.GenerateHash(input.OldPassword + user.PasswordSalt);
        if (user.Password.NE(oldPassword))
            return new ActionResponse<bool>().ReturnResponseError("Old password is incorrect!");

        var salt = Hasher.GetSalt();
        var hashedPassword = Hasher.GenerateHash(input.NewPassword1 + salt);
        user.Password = hashedPassword;
        user.PasswordSalt = salt;
        _entityRepository.Update(user);
        return new ActionResponse<bool> { Entity = true };
    }

    public ActionResponse<bool> UpdateOthersPassword(long userId, UpdateOthersPasswordDto input)
    {
        var user = _entityRepository.Table.FirstOrDefault(x => x.Id == userId);
        if (user == null || user.GuidId != input.GuidId)
            return new ActionResponse<bool>().ReturnResponseError("User not found!");

        var salt = Hasher.GetSalt();
        var hashedPassword = Hasher.GenerateHash(input.Password + salt);
        user.Password = hashedPassword;
        user.PasswordSalt = salt;
        _entityRepository.Update(user);

        return new ActionResponse<bool> { Entity = true };
    }

    public ActionResponse<bool> SendRequest(UserRequestCreateDto dto)
    {
        var response = new ActionResponse<bool>();

        
        var targetUserId = _entityRepository.TableNoTracking
            .Where(x => x.Email == dto.TargetUserEmail)
            .Select(x => x.Id)
            .FirstOrDefault();
        if (targetUserId.IsInitial())
        {
            response.IsSuccess = false;
            response.ReturnMessage.Add("Girilen mail adresine ait kullanıcı bulunamadı.");
            return response;
        }
        // Aynı kullanıcıya daha önce istek gönderilmiş mi kontrolü
        var exists = _userRequestRepo.TableNoTracking.Any(x =>
            x.RequesterId == _session.UserId && x.TargetUserEmail == dto.TargetUserEmail && x.Status == RequestStatus.Pending);

      
        if (exists)
        {
            response.IsSuccess = false;
            response.ReturnMessage.Add("Zaten bekleyen bir isteğiniz var.");
            return response;
        }

        var request = new UserRequest
        {
            RequesterId = _session.UserId,
            TargetUserEmail = dto.TargetUserEmail,
            TargetUserId = targetUserId,
            Status = RequestStatus.Pending,
            //CreDate = DateTime.Now,
            //GuidId = Guid.NewGuid()
        };

        _userRequestRepo.Insert(request);
        response.IsSuccess = true;
        response.Entity = true;
        return response;
    }

    public ActionResponse<bool> HandleRequest(UserRequestActionDto dto)
    {
        var response = new ActionResponse<bool>();
        var request = _userRequestRepo.Table.FirstOrDefault(x => x.Id == dto.RequestId && x.TargetUserEmail == _session.Email);

        if (request == null || request.Status != RequestStatus.Pending)
        {
            response.IsSuccess = false;
            response.ReturnMessage.Add("İstek bulunamadı veya zaten işlenmiş.");
            return response;
        }

        if (dto.Accept)
        {
            request.Status = RequestStatus.Accepted;
            var user = _entityRepository.GetById(_session.UserId);
            user.AccounterId = request.RequesterId;
            _entityRepository.Update(user);
        }
        else
        {
            request.Status = RequestStatus.Rejected;
        }

        _userRequestRepo.Update(request);
        response.IsSuccess = true;
        response.Entity = true;
        return response;
    }

    public ActionResponse<bool> GetMyAllRequests()
    {
        throw new NotImplementedException();
    }

    public ActionResponse<AccounterUserDto> GetAccounterUsers()
    {
        var userIds = _accUsersRepository.TableNoTracking.Where(x => x.AccounterUserId == _session.UserId)
            .Select(x => x.TargetUserUserId).ToList();
        var users = _entityRepository.TableNoTracking.Where(x => userIds.Contains(x.Id)).Select(x =>
            new AccounterUserDto
            {
                FullName = x.FirstName + " " + x.LastName,
                UserId = x.Id
            }).ToList();
        return new ActionResponse<AccounterUserDto> { EntityList = users };
    }
}