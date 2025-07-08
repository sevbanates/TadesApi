using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TadesApi.Db.Entities;
using SendWithBrevo;
using TadesApi.BusinessService.AuthServices.Interfaces;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Caching;
using TadesApi.Core.Models.ConstantKeys;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Core.Security;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;

namespace TadesApi.BusinessService.AuthServices.Services;

public class LoginService : ILoginService
{
    private readonly IRepository<User> _entityRepository;
    private readonly IRepository<ForgotPassword> _forgotPasswordRepository;
    private readonly IRepository<SysControllerActionTotal> _securityActionTotalRepository;
    private readonly IMapper _mapper;
    private readonly IEmailHelper _emailHelper;

    public LoginService(IMapper mapper,
        IRepository<User> entityRepository,
        IRepository<ForgotPassword> forgotPasswordRepository,
        IEmailHelper emailHelper,
        IRepository<SysControllerActionTotal> securityActionTotalRepository)
    {
        _entityRepository = entityRepository;
        _forgotPasswordRepository = forgotPasswordRepository;
        _securityActionTotalRepository = securityActionTotalRepository;
        _mapper = mapper;
        _emailHelper = emailHelper;
    }

    public ActionResponse<LoginUserInfo> SendConfirmCode(LoginModel input)
    {
        ActionResponse<LoginUserInfo> returnResponse = new();

        var user = _entityRepository.TableNoTracking.FirstOrDefault(x => x.UserName == input.UserName);

        if (user == null || user.IsActive == false)
            return returnResponse.ReturnResponseError("Wrong username or password");

        var hashedPassword = Hasher.GenerateHash(input.Password + user.PasswordSalt);

        if (user.Password.Trim() != hashedPassword)
            return returnResponse.ReturnResponseError("Wrong username or password");

        Random rnd = new();
        var confirmCode = rnd.Next(100000, 999999);

        SendLoginConfirmEmail(user, confirmCode.ToString());

        // Cache Confirm Code for 3 minutes
        user.LoginCode = confirmCode.ToString();
        user.LoginCodeExpireDate = DateTime.Now.AddMinutes(3);
        _entityRepository.Update(user);

        LoginUserInfo viewModel = new()
        {
            UserId = user.Id,
            Email = user.Email.Substring(0, 2) + "*****" + user.Email.Substring(user.Email.IndexOf("@"))
        };

        returnResponse.Entity = viewModel;
        return returnResponse;
    }

    private void SendLoginConfirmEmail(User user, string confirmationCode)
    {
        var recipients = new List<Recipient>
        {
            new(user.FirstName + " " + user.LastName, user.Email)
        };
        var parameters = new Dictionary<string, string>
        {
            {EmailParams.FullName, user.FirstName + " " + user.LastName},
            {EmailParams.Description, confirmationCode}
        };
        const string subject = "Login Confirmation Code";
        _emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.LoginConfirm);
    }

    public ActionResponse<UserViewModel> Login(LoginModel input)
    {
        ActionResponse<UserViewModel> toReturn = new();

        var user = _entityRepository.TableNoTracking.FirstOrDefault(x => x.UserName == input.UserName);

        if (user == null || user.IsActive == false)
            return toReturn.ReturnResponseError("Wrong username or password");

        var hashedPassword = Hasher.GenerateHash(input.Password + user.PasswordSalt);

        if (user.Password.Trim() != hashedPassword)
            return toReturn.ReturnResponseError("Wrong username or password");

        // Confirm Code Control
        //var correctConfirmCode = user.LoginCode;
        //if (input.Code != "180923" && input.Code != correctConfirmCode)
        //    return toReturn.ReturnResponseError("Wrong code or code expired!");
        
        UserViewModel successfulLoginResponse = new()
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CompanyId = user.CompanyId,
            IsFirstLogin = user.IsFirstLogin,
            GuidId = user.GuidId,
            RoleId = user.RoleId,
            LastLoginTime= user.LastLoginTime,
            CanClientEdit = user.RoleId == RoleConstant.Client && (DateTime.Now - user.CreDate).TotalHours <= 48,
            //ClientId = user.ClientId
        };

        var token = TokenManagement.CreateToken(successfulLoginResponse.Id, successfulLoginResponse.FirstName,
            successfulLoginResponse.Email, successfulLoginResponse.UserName, successfulLoginResponse.RoleId);
        successfulLoginResponse.Token = token;

        var controllerActionTotals = _securityActionTotalRepository.TableNoTracking.Where(x => x.RoleId == successfulLoginResponse.RoleId)
            .ToList();
        successfulLoginResponse.SecurityTotalList = _mapper.Map<List<SysControllerActionTotalViewModel>>(controllerActionTotals);
        toReturn.Entity = successfulLoginResponse;

        user.LastLoginTime = DateTime.Now;
        _entityRepository.Update(user);

        return toReturn;
    }

    public ActionResponse<bool> ForgotPassword(string email)
    {
        ActionResponse<bool> response = new();

        var user = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Email == email);

        if (user == null)
            return response.ReturnResponseError("User account is not found");

        var lastOldRec = _forgotPasswordRepository.Table.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == user.Id);

        if (lastOldRec is not null)
            if ((CommonFunctions.Now - lastOldRec.SendMailDate).TotalMinutes < 30 && lastOldRec.ChangedPassDate == null)
                return response.ReturnResponseError("You have an active password reset request!");

        ForgotPassword forgotPasswordRec = new()
        {
            UserId = user.Id,
            Email = user.Email,
            SendMailDate = CommonFunctions.Now,
            Token = GeneratePasswordResetToken()
        };

        _forgotPasswordRepository.Insert(forgotPasswordRec);
        SendForgotPasswordEmail(user, forgotPasswordRec.Token);

        return response;
    }

    private void SendForgotPasswordEmail(User user, string token)
    {
       var recipients = new List<Recipient>
        {
            new(user.FirstName + " " + user.LastName, user.Email)
        };
        var parameters = new Dictionary<string, string>
        {
            {EmailParams.FormUrl, "https://portal.globalpsychsolutions.com/reset-password/" + token},
        };
        const string subject = "Password Reset Request";
        _emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.ForgotPassword);
    }

    public ActionResponse<bool> ResetPassword(ResetPasswordModel input)
    {
        ActionResponse<bool> response = new();
        var forgotPasswordRequest = _forgotPasswordRepository.Table.FirstOrDefault(x => x.Token == input.Token);

        if (forgotPasswordRequest == null)
            return response.ReturnResponseError("Password reset request can not be found!");

        if ((CommonFunctions.Now - forgotPasswordRequest.SendMailDate).TotalMinutes > 30)
            return response.ReturnResponseError("Password reset request has expired!");

        if (forgotPasswordRequest.ChangedPassDate != null)
            return response.ReturnResponseError("Password reset request has already been used!");

        var user = _entityRepository.Table.FirstOrDefault(x => x.Id == forgotPasswordRequest.UserId);
        if (user == null)
        {
            return response.ReturnResponseError("User account is not found!");
        }

        var salt = Hasher.GetSalt();
        var hashedPassword = Hasher.GenerateHash(input.Password + salt);
        user.Password = hashedPassword;
        user.PasswordSalt = salt;

        forgotPasswordRequest.ChangedPassDate = DateTime.Now;

        _entityRepository.Update(user);
        _forgotPasswordRepository.Update(forgotPasswordRequest);

        return response;
    }

    public ActionResponse<bool> FirstLoginChangePassword(FirstLoginChangePasswordModel input)
    {
        ActionResponse<bool> response = new();
        var user = _entityRepository.Table.FirstOrDefault(x => x.Id == input.Id);
        if (user == null)
            return response.ReturnResponseError("User account is not found!");
        if (user.IsFirstLogin == false)
            return response.ReturnResponseError("Invalid request!");

        var oldHashedPassword = Hasher.GenerateHash(input.OldPassword + user.PasswordSalt);
        if (user.Password.Trim() != oldHashedPassword)
            return response.ReturnResponseError("Old password is wrong!");

        var salt = Hasher.GetSalt();
        var newHashedPassword = Hasher.GenerateHash(input.Password1 + salt);
        user.Password = newHashedPassword;
        user.PasswordSalt = salt;
        user.IsFirstLogin = false;

        _entityRepository.Update(user);

        return response;
    }

    private string GeneratePasswordResetToken()
    {
        return Guid.NewGuid().ToString("N");
    }

    public ActionResponse<bool> Logout()
    {
        ActionResponse<bool> resp = new()
        {
            Entity = true
        };

        return resp;
    }
}