using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendWithBrevo;
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
using TadesApi.Models.ViewModels.Client;

namespace TadesApi.BusinessService.AuthServices.Services;

public class UserService : BaseServiceNg<User, UserBasicDto, CreateUserDto, UpdateUserDto>, IUserService
{
    private readonly IEmailHelper _emailHelper;

    public UserService(
        IRepository<User> entityRepository,
        ILocalizationService locManager,
        IMapper mapper,
        IEmailHelper emailHelper,
        ICurrentUser session) : base(entityRepository, locManager, mapper, session)
    {
        _emailHelper = emailHelper;
        
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
        _emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.AccountCreated);
        
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

}