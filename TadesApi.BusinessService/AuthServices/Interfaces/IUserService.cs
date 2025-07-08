using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Models.ViewModels.Client;

namespace TadesApi.BusinessService.AuthServices.Interfaces;

public interface IUserService : IBaseServiceNg<CreateUserDto, UpdateUserDto, UserBasicDto, PagedAndSortedInput>
{
    //ActionResponse<UserBasicDto> CreateUser(CreateUserDto input);
    //ActionResponse<UserBasicDto> GetUser(long id, Guid guidId);
    ActionResponse<UserBasicDto> GetWebSiteUser(long userId);
    PagedAndSortedResponse<UserBasicDto> GetMulti(UserSearchInput input);

    // ActionResponse<UserBasicDto> UpdateUser(long id, UpdateUserDto input);
    //ActionResponse<UserBasicDto> GetProfile();
    ActionResponse<UserBasicDto> UpdateProfile(UpdateProfileDto input);
    ActionResponse<bool> UpdateSelfPassword(UpdateSelfPasswordDto input);
    ActionResponse<bool> UpdateOthersPassword(long userId, UpdateOthersPasswordDto input);

    //ActionResponse<bool> DeleteUser(long userId, Guid guidId);
}