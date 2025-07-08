using MimeKit.Utils;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VideoPortalApi.Core.Email;
using VideoPortalApi.Core;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.CoreHelper;
using VideoPortalApi.Db.Entities;
using VideoPortalApi.Db.Infrastructure;
using VideoPortalApi.QueService.Enum;
using VideoPortalApi.QueService.Email.RoomInviteManagement.Interfaces;

namespace VideoPortalApi.QueService.Email.RoomInviteManagement.Services
{
    public class RoomInviteJob : IRoomInviteJob
    {
        private readonly IAdminRepository<AppJobs> _appJobsRepository;
        private readonly IOptions<FileSettings> _fileSetting;
        private readonly IEmailService _emailService;
        public RoomInviteJob(IAdminRepository<AppJobs> appJobsRepository, IOptions<FileSettings> fileSetting, IEmailService emailService)
        {
            _appJobsRepository = appJobsRepository;
            _fileSetting = fileSetting;
            _emailService = emailService;
        }

        public bool SendRoomInvite(RoomInviteMailModel roomInviteMailModel)
        {
            AppJobs appJob = _appJobsRepository.Table.FirstOrDefault(x => x.Id == roomInviteMailModel.AppJobId);

            var retryCount = appJob.Retry + 1;

            bool isSuccess = true;
            string errMessage = "";

            try
            {
                SendRoomInviteEmail(roomInviteMailModel);

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

        private void SendRoomInviteEmail(RoomInviteMailModel roomInviteMailModel)
        {
            EmailMessage mail = new();
            MimeMessage message = new();


            mail.ToAdd(roomInviteMailModel.Email, $"{roomInviteMailModel.FirstName} {roomInviteMailModel.LastName}");
            //mail.ToAdd("ramazan.yildiz@betechinnovation.com", $"Ramazan");
            //mail.ToAdd("sevban.ates@betechinnovation.com", $"Sevban");
            //mail.ToAdd("rguldu@gmail.com", $"Resul");
            var filePath = Path.Combine(_fileSetting.Value.TemplatePath, "RoomInviteMailTemplate.html");
            var fileItems = File.ReadAllText(filePath);
            

            fileItems = fileItems.Replace("{kullanici}", "Dear" + " " + roomInviteMailModel.FirstName + " " + roomInviteMailModel.LastName);
            fileItems = fileItems.Replace("{descr}", $"Your event details are below");
            fileItems = fileItems.Replace("{first_item}", roomInviteMailModel.ConsultantName + " " + roomInviteMailModel.ConsultantLastName);
            fileItems = fileItems.Replace("{second_item}", roomInviteMailModel.InviteLink);

            var bodybuilder = new BodyBuilder();

            var image = bodybuilder.LinkedResources.Add(Path.Combine(_fileSetting.Value.TemplatePath, "newlogo.jpg"));
            image.ContentId = MimeUtils.GenerateMessageId();
            bodybuilder.HtmlBody = fileItems.Replace("logo", image.ContentId);
            message.Body = bodybuilder.ToMessageBody();

            mail.HtmlBody = message.Body;
            mail.Subject = "Room Invite";

            _emailService.SendWithHtmlBody(mail);
        }
    }
}
