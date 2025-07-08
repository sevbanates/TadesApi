using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using VideoPortalApi.Core;
using VideoPortalApi.Core.Email;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.CoreHelper;
using VideoPortalApi.Db.Entities;
using VideoPortalApi.Db.Infrastructure;
using VideoPortalApi.QueService.Email.ScheduleEventManagement.Interfaces;

namespace VideoPortalApi.QueService.Email.ScheduleEventManagement.Services;

public class ScheduleEventJob : IScheduleEventJob
{
    private readonly IAdminRepository<AppJobs> _appJobsRepository;
    private readonly IEmailService _emailService;
    private readonly IOptions<FileSettings> _fileSetting;

    public ScheduleEventJob(IAdminRepository<AppJobs> appJobsRepository, IOptions<FileSettings> fileSetting, IEmailService emailService)
    {
        _appJobsRepository = appJobsRepository;
        _fileSetting = fileSetting;
        _emailService = emailService;
    }

    public bool SendScheduleEventNotificationMail(ScheduleEventNotificationMailModel input)
    {
        var appJob = _appJobsRepository.Table.FirstOrDefault(x => x.Id == input.AppJobId);
        if (appJob == null) return false;

        var retryCount = appJob.Retry + 1;
        var isSuccess = true;
        var errMessage = "";

        try
        {
            SendScheduleStatusEmail(input);
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

        if (isSuccess) return true;
        if (retryCount < 4)
            //*** Rabbit Mq hata fırlatılır 3 den fazla ise  True dondurulerek Job  Kapatılıur
            throw new ArgumentException(errMessage);

        return true;
    }

    public bool SendScheduleEventConsultantNotificationMail(ScheduleEventNotificationMailModel input)
    {
        var appJob = _appJobsRepository.Table.FirstOrDefault(x => x.Id == input.AppJobId);
        if (appJob == null) return false;

        var retryCount = appJob.Retry + 1;
        var isSuccess = true;
        var errMessage = "";

        try
        {
            SendConsultantScheduleStatusEmail(input);
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

        if (isSuccess) return true;
        if (retryCount < 4)
            //*** Rabbit Mq hata fırlatılır 3 den fazla ise  True dondurulerek Job  Kapatılıur
            throw new ArgumentException(errMessage);

        return true;
    }

    private void SendScheduleStatusEmail(ScheduleEventNotificationMailModel input)
    {
        EmailMessage mail = new();
        MimeMessage message = new();

        mail.ToAdd(input.Email, $"{input.FirstName} {input.LastName}");
        mail.ToAdd("uenlkr4e@gmail.com", "hsyn");
        var filePath = Path.Combine(_fileSetting.Value.TemplatePath, "ScheduleEventNotificationMailTemplate.html");
        var fileItems = File.ReadAllText(filePath);

        fileItems = fileItems.Replace("{kullanici}", "Dear" + " " + input.FirstName + " " + input.LastName);
        fileItems = fileItems.Replace("{descr}", "Your event details are below");
        fileItems = fileItems.Replace("{first_item}", "Consultant: " + input.ConsultantName + " " + input.ConsultantLastName);
        fileItems = fileItems.Replace("{second_item}", input.EventDate);
        fileItems = fileItems.Replace("{third_item}", input.EventStartTime);
        fileItems = fileItems.Replace("{fourth_item}", input.EventEndTime);
        fileItems = fileItems.Replace("{fifth_item}", input.Status);

        if (input.SendRoomLink)
            fileItems = fileItems.Replace("{roomLink}", $"Room link: <a href = \"{input.RoomLink}\" > Click to join </a>");

        if (input.SendIntakeFormLink)
            fileItems = fileItems.Replace("{intakeformlink}",
                "Intake Form link: <a href = \"https://portal.globalpsychsolutions.com/intakeForm\" > Intake Form </a>");

        var bodybuilder = new BodyBuilder();

        var image = bodybuilder.LinkedResources.Add(Path.Combine(_fileSetting.Value.TemplatePath, "newlogo.jpg"));
        image.ContentId = MimeUtils.GenerateMessageId();
        bodybuilder.HtmlBody = fileItems.Replace("logo", image.ContentId);
        message.Body = bodybuilder.ToMessageBody();

        mail.HtmlBody = message.Body;

        _emailService.SendWithHtmlBody(mail);
    }

    private void SendConsultantScheduleStatusEmail(ScheduleEventNotificationMailModel input)
    {
        EmailMessage mail = new();
        MimeMessage message = new();

        mail.ToAdd(input.Email, $"{input.ConsultantName} {input.ConsultantLastName}");
        mail.ToAdd("uenlkr4e@gmail.com", "hsyn");

        var filePath = Path.Combine(_fileSetting.Value.TemplatePath, "ScheduleEventNotificationMailTemplate.html");
        var fileItems = File.ReadAllText(filePath);

        fileItems = fileItems.Replace("{kullanici}", "Dear" + " " + input.ConsultantName + " " + input.ConsultantLastName);
        fileItems = fileItems.Replace("{descr}", "Your event details are below");
        fileItems = fileItems.Replace("{first_item}", "Attendee: " + input.FirstName + " " + input.LastName);
        fileItems = fileItems.Replace("{second_item}", input.EventDate);
        fileItems = fileItems.Replace("{third_item}", input.EventStartTime);
        fileItems = fileItems.Replace("{fourth_item}", input.EventEndTime);
        fileItems = fileItems.Replace("{fifth_item}", input.Status);

        if (input.SendRoomLink)
            fileItems = fileItems.Replace("{roomLink}", $"Room link: <a href = \"{input.RoomLink}\" > Click to join </a>");

        if (input.SendIntakeFormLink)
            fileItems = fileItems.Replace("{intakeformlink}",
                "Intake Form link: <a href = \"https://portal.globalpsychsolutions.com/intakeForm\" > Intake Form </a>");

        var bodybuilder = new BodyBuilder();

        var image = bodybuilder.LinkedResources.Add(Path.Combine(_fileSetting.Value.TemplatePath, "newlogo.jpg"));
        image.ContentId = MimeUtils.GenerateMessageId();
        bodybuilder.HtmlBody = fileItems.Replace("logo", image.ContentId);
        message.Body = bodybuilder.ToMessageBody();

        mail.HtmlBody = message.Body;

        _emailService.SendWithHtmlBody(mail);
    }
}