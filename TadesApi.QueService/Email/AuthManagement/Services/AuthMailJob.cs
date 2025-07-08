using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using System;
using System.IO;
using System.Linq;
using VideoPortalApi.Core;
using VideoPortalApi.Core.Email;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.CoreHelper;
using VideoPortalApi.Db;
using VideoPortalApi.Db.Entities;
using VideoPortalApi.Db.Infrastructure;
using VideoPortalApi.QueService.Email.AuthManagement.Interfaces;
using VideoPortalApi.QueService.Enum;
using VideoPortalApi.QueService.Helper;

namespace VideoPortalApi.QueService.Email.AuthManagement.Services
{
    public class AuthMailJob : IAuthMailJob
    {
        private readonly IAdminRepository<AppJobs> _appJobsRepository;
        private readonly IAdminRepository<User> _usersRepository;
        private readonly AdminDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IOptions<FileSettings> _fileSetting;
        private readonly IEncryption _encryptionService;
        public AuthMailJob(IAdminRepository<AppJobs> appJobsRepository,
                            IEmailService emailService,
                            AdminDbContext dbContext,
                            IAdminRepository<User> usersRepository,
                            IOptions<FileSettings> fileSetting,
                            IEncryption encryptionService
                            )
        {
            _appJobsRepository = appJobsRepository;
            _dbContext = dbContext;
            _emailService = emailService;
            _usersRepository = usersRepository;
            _fileSetting = fileSetting;
            _encryptionService = encryptionService;
        }

        //*** Login Confirmation Email
        public bool SendLoginConfirmationMail(LoginConfirmMailModel loginConfirmMailModel)
        {
            AppJobs appJob = _appJobsRepository.Table.FirstOrDefault(x => x.Id == loginConfirmMailModel.AppJobId);

            var retryCount = appJob.Retry + 1;

            bool isSuccess = true;
            string errMessage = "";

            try
            {
                SendEmailForConfirmationCode(loginConfirmMailModel);
                appJob.ComplatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message.XLeft(1024);
                isSuccess = false;
            }

            appJob.Retry = retryCount;
            appJob.IsSuccess = isSuccess;
            appJob.ProcessDate = DateTime.Now;
            //appJob.Er = DateTime.Now;

            _appJobsRepository.Update(appJob);

            if (!isSuccess)
            {
                if (retryCount < 4)
                {
                    //*** Rabbit Mq hata fırlatılır 3 den fazla ise  True dondurulerek Job  Kapatılıur

                    throw new ArgumentException(errMessage);
                }
            }

            return true;
        }
        private void SendEmailForConfirmationCode(LoginConfirmMailModel loginConfirmMailModel)
        {
            EmailMessage mail = new();
            MimeMessage message = new();

            mail.ToAdd(loginConfirmMailModel.Email, $"{loginConfirmMailModel.FirstName} {loginConfirmMailModel.LastName}");

            mail.Subject = $"Your Two-Factor Login Code";

            var filePath = Path.Combine(_fileSetting.Value.TemplatePath, "LoginConfirmMailTemplate.html");
            var fileItems = File.ReadAllText(filePath);

            fileItems = fileItems.Replace("{kullanici}", loginConfirmMailModel.FirstName);
            fileItems = fileItems.Replace("{descr}", $"Your Two-Factor Login Code: {loginConfirmMailModel.ConfirmCode}");
            fileItems = fileItems.Replace("{link}", RabbitMQFileSettings.ApplicationUrl);

            var bodybuilder = new BodyBuilder();

            var image = bodybuilder.LinkedResources.Add(Path.Combine(_fileSetting.Value.TemplatePath, "newlogo.jpg"));
            image.ContentId = MimeUtils.GenerateMessageId();
            bodybuilder.HtmlBody = fileItems.Replace("logo", image.ContentId);
            message.Body = bodybuilder.ToMessageBody();

            mail.HtmlBody = message.Body;

            _emailService.SendWithHtmlBody(mail);
        }
        //*** Forgotten Password Email
        public bool SendForgotPasswordMail(ForgotPasswordMailModel forgotPasswordMailModel)
        {
            AppJobs appJob = _appJobsRepository.Table.FirstOrDefault(x => x.Id == forgotPasswordMailModel.AppJobId);
            User userRec = _usersRepository.TableNoTracking.FirstOrDefault(x => x.Id == forgotPasswordMailModel.UserId);
            userRec.Email = _encryptionService.DecryptText(userRec.Email);

            var retryCount = appJob.Retry + 1;

            bool isSuccess = true;
            string errMessage = "";

            try
            {
                SendEmailToForgottenPassword(userRec, forgotPasswordMailModel.Token);
                appJob.ComplatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message.XLeft(1024);
                isSuccess = false;
            }

            appJob.Retry = retryCount;
            appJob.IsSuccess = isSuccess;
            appJob.ProcessDate = DateTime.Now;
            //appJob.Er = DateTime.Now;

            _appJobsRepository.Update(appJob);

            if (!isSuccess)
            {
                if (retryCount < 4)
                {
                    //*** Rabbit Mq hata fırlatılır 3 den fazla ise  True dondurulerek Job  Kapatılıur

                    throw new ArgumentException(errMessage);
                }
            }

            return true;
        }
        private void SendEmailToForgottenPassword(User userRec, string token)
        {
            EmailMessage mail = new EmailMessage();
            MimeMessage message = new MimeMessage();

            mail = EmailHelper.SetMailReceiver(mail, userRec);            

            mail.Subject = $"Global Psychoeducational Solutions {"Password Reset Request"}";

            var filePath = Path.Combine(_fileSetting.Value.TemplatePath, "ForgetPasswordMailTemplate.html");
            var fileItems = File.ReadAllText(filePath);

            fileItems = fileItems.Replace("{kullanici}", userRec.FirstName + "" + userRec.LastName);
            fileItems = fileItems.Replace("{descr}", $"Your Password Reset Request Has Been Received");
            fileItems = fileItems.Replace("{link}", string.Format(RabbitMQFileSettings.ResetPasswordUrl, token));

            var bodybuilder = new BodyBuilder();
            var image = bodybuilder.LinkedResources.Add(Path.Combine(_fileSetting.Value.TemplatePath, "newlogo.jpg"));
            image.ContentId = MimeUtils.GenerateMessageId();
            bodybuilder.HtmlBody = fileItems.Replace("logo", image.ContentId);
            message.Body = bodybuilder.ToMessageBody();

            mail.HtmlBody = message.Body;

            _emailService.SendWithHtmlBody(mail);
        }

        //*** New Generated User Email
        public bool SendGeneratedUserMail(NewCustomerMailModel customerMail)
        {
            AppJobs appJob = _appJobsRepository.Table.FirstOrDefault(x => x.Id == customerMail.AppJobId);
            User userRec = _usersRepository.TableNoTracking.FirstOrDefault(x => x.Id == customerMail.UserId);
            userRec.Email = userRec.Email;

            var retryCount = appJob.Retry + 1;

            bool isSuccess = true;
            string errMessage = "";

            try
            {
                SendNewUserMail(userRec, customerMail.GeneratedPassword);
                appJob.ComplatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message.XLeft(1024);
                isSuccess = false;
            }

            appJob.Retry = retryCount;
            appJob.IsSuccess = isSuccess;
            appJob.ProcessDate = DateTime.Now;
            //appJob.Er = DateTime.Now;

            _appJobsRepository.Update(appJob);

            if (!isSuccess)
            {
                if (retryCount < 4)
                {
                    //*** Rabbit Mq hata fırlatılır 3 den fazla ise  True dondurulerek Job  Kapatılıur

                    throw new ArgumentException(errMessage);
                }
            }

            return true;
        }
        private void SendNewUserMail(User userRec, string generatedPassword)
        {
            EmailMessage mail = new EmailMessage();
            MimeMessage message = new MimeMessage();

            mail = EmailHelper.SetMailReceiver(mail, userRec);

            mail.Subject = $"Global Psychoeducational Solutions {"Kullanıcı Hesabınız Oluşturuldu"}";

            var filePath = Path.Combine(_fileSetting.Value.TemplatePath, "CreatedUserMailTemplate.html");
            var fileItems = File.ReadAllText(filePath);

            fileItems = fileItems.Replace("{kullanici}", userRec.FirstName);
            fileItems = fileItems.Replace("{UserName}", _encryptionService.DecryptText(userRec.UserName));
            fileItems = fileItems.Replace("{Password}", generatedPassword);
            fileItems = fileItems.Replace("{descr}", "Global Psychoeducational Solutions account has been created!");
            fileItems = fileItems.Replace("{link}", RabbitMQFileSettings.ApplicationUrl);

            var bodybuilder = new BodyBuilder();
            var image = bodybuilder.LinkedResources.Add(Path.Combine(_fileSetting.Value.TemplatePath, "newlogo.jpg"));
            image.ContentId = MimeUtils.GenerateMessageId();
            bodybuilder.HtmlBody = fileItems.Replace("logo", image.ContentId);
            message.Body = bodybuilder.ToMessageBody();

            mail.HtmlBody = message.Body;

            _emailService.SendWithHtmlBody(mail);
        }

    }
}
