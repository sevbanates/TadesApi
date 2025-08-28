using Microsoft.Extensions.Options;
using SendWithBrevo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Extensions;
using TadesApi.CoreHelper;
using TadesApi.Models.CustomModels;

namespace TadesApi.BusinessService.Common.Services;

public class EmailHelper : IEmailHelper
{
    private readonly IOptions<FileSettings> _fileSetting;

    public EmailHelper(IOptions<FileSettings> fileSetting)
    {
        _fileSetting = fileSetting;
    }

    public ActionResponse<bool> SendEmail(string subject, string htmlContent, string toEmail, string toName)
    {
        var response = new ActionResponse<bool>();
        //var client = new BrevoClient("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");
        //var sender = new Sender("Global Psychoeducational Solutions", "admin@globalpsychsolutions.com");
        //var response = client.SendAsync(sender, subject: subject, content: "GPS", to: recipients, parameters: parameters,
        //    templateId: templateId, isHtml: true).Result;
        //return new ActionResponse<bool> { IsSuccess = response, Entity = response };
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("betechtest42@gmail.com", "hqxwxvdbpgbjqrva"),
                EnableSsl = true,
            };

            // Load HTML template from Portal's Resource folder (copied to output at runtime)
            string bodyToSend = htmlContent ?? string.Empty;
            try
            {
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Resource", "Template", "SendAccounterRequestMailTemplate.html");
                if (File.Exists(templatePath))
                {
                    var template = File.ReadAllText(templatePath);
                    bodyToSend = template
                        .Replace("{{name}}", toName ?? string.Empty)
                        .Replace("{{customMessage}}", htmlContent ?? string.Empty)
                        .Replace("{{year}}", DateTime.UtcNow.Year.ToString());
                }
            }
            catch { /* fallback to provided htmlContent */ }

            var message = new MailMessage
            {
                From = new MailAddress("betechtest42@gmail.com", "Tades Yazılım"),
                Subject = subject,
                Body = bodyToSend,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);
            smtpClient.SendMailAsync(message);
            return response;
        }
        catch(Exception ex)
        {
            return response;
        }
    }

    public ActionResponse<bool> SendAccounterRequestMail(string subject, string messageText, string toEmail, string toName, string actionUrl, string senderName)
    {
        var response = new ActionResponse<bool>();
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("betechtest42@gmail.com", "hqxwxvdbpgbjqrva"),
                EnableSsl = true,
            };

            string bodyToSend = messageText ?? string.Empty;
            try
            {
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Resource", "Template", "SendAccounterRequestMailTemplate.html");
                if (File.Exists(templatePath))
                {
                    var template = File.ReadAllText(templatePath);
                    bodyToSend = template
                        .Replace("{{name}}", toName ?? string.Empty)
                        .Replace("{{customMessage}}", senderName ?? string.Empty)
                        .Replace("{{actionUrl}}", actionUrl ?? string.Empty)
                        .Replace("{{year}}", DateTime.UtcNow.Year.ToString());
                }
            }
            catch { }

            var message = new MailMessage
            {
                From = new MailAddress("betechtest42@gmail.com", "Mukellef.io"),
                Subject = subject,
                Body = bodyToSend,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);
            smtpClient.SendMailAsync(message);
            return response;
        }
        catch
        {
            return response;
        }
    }

    public ActionResponse<bool> SendTicketCreatedMail(CreatedTicketMailModel model)
    {   
        var response = new ActionResponse<bool>();
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("betechtest42@gmail.com", "hqxwxvdbpgbjqrva"),
                EnableSsl = true,
            };

            string bodyToSend =  string.Empty;
            try
            {
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Resource", "Template", "SendTicketCreatedMailToAdmins.html");
                if (File.Exists(templatePath))
                {
                    var template = File.ReadAllText(templatePath)
                        .Replace("{{TicketId}}", model.TicketId.ToString())
                        .Replace("{{Category}}", EnumExtensions.GetDisplayName(model.TicketCategory))
                        .Replace("{{Priority}}", EnumExtensions.GetDisplayName(model.TicketPriority))
                        .Replace("{{CustomerName}}", model.SenderName)
                        .Replace("{{Status}}", EnumExtensions.GetDisplayName(model.TicketStatus))
                        .Replace("{{CreatedAt}}", model.CreatedDate.ToString(DateFormatConst.dd_MM_yyyy_HH_mm_ss))
                        .Replace("{{Message}}", model.TicketMessage)
                        .Replace("{{TicketUrl}}", model.TicketUrl);
                    bodyToSend = template;
                    //.Replace("{{name}}", toName ?? string.Empty)
                    //.Replace("{{customMessage}}", senderName ?? string.Empty)
                    //.Replace("{{actionUrl}}", actionUrl ?? string.Empty)
                    //.Replace("{{year}}", DateTime.UtcNow.Year.ToString());
                }
            }
            catch { }

            var message = new MailMessage
            {
                From = new MailAddress("betechtest42@gmail.com", "Mukellef.io"),
                Subject = "Yeni Destek Talebi Bildirimi",
                Body = bodyToSend,
                IsBodyHtml = true
            };
            model.Recievers.ForEach(x=> message.To.Add(x));
            //message.To.Add(model.Recievers);
            smtpClient.SendMailAsync(message);
            return response;
        }
        catch
        {
            return response;
        }
    }

    public ActionResponse<bool> SendTicketMessageMailToClient(TicketMessageMailModel model)
    {
        var response = new ActionResponse<bool>();
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("betechtest42@gmail.com", "hqxwxvdbpgbjqrva"),
                EnableSsl = true,
            };

            string bodyToSend = string.Empty;
            try
            {
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Resource", "Template", "TicketMessageMailTemplate.html");
                if (File.Exists(templatePath))
                {
                    var template = File.ReadAllText(templatePath)
                        .Replace("{{TicketId}}", model.TicketId.ToString())
                        //.Replace("{{Category}}", EnumExtensions.GetDisplayName(model.TicketCategory))
                        //.Replace("{{Priority}}", EnumExtensions.GetDisplayName(model.TicketPriority))
                        .Replace("{{CustomerName}}", model.SenderName)
                        //.Replace("{{Status}}", EnumExtensions.GetDisplayName(model.TicketStatus))
                        .Replace("{{CreatedAt}}", model.CreatedDate.ToString(DateFormatConst.dd_MM_yyyy_HH_mm_ss))
                        .Replace("{{Message}}", model.TicketMessage)
                        .Replace("{{TicketUrl}}", model.TicketUrl)
                        .Replace("{{year}}", DateTime.Now.Year.ToString());
                    bodyToSend = template;
                    //.Replace("{{name}}", toName ?? string.Empty)
                    //.Replace("{{customMessage}}", senderName ?? string.Empty)
                    //.Replace("{{actionUrl}}", actionUrl ?? string.Empty)
                    //.Replace("{{year}}", DateTime.UtcNow.Year.ToString());
                }
            }
            catch { }

            var message = new MailMessage
            {
                From = new MailAddress("betechtest42@gmail.com", "Mukellef.io"),
                Subject = "Destek Talebinize Yanıt Geldi!",
                Body = bodyToSend,
                IsBodyHtml = true,
                //To = { model.Reciever }
            };
            model.Recievers.ForEach(x => message.To.Add(x));
            //message.To.Add(model.Recievers);
            smtpClient.SendMailAsync(message);
            return response;
        }
        catch
        {
            return response;
        }
    }

    public ActionResponse<bool> SendTicketMessageMailToAdmin(TicketMessageMailModel model)
    {
        var response = new ActionResponse<bool>();
        try
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("betechtest42@gmail.com", "hqxwxvdbpgbjqrva"),
                EnableSsl = true,
            };

            string bodyToSend = string.Empty;
            try
            {
                var templatePath = Path.Combine(AppContext.BaseDirectory, "Resource", "Template", "TicketMessageMailTemplateToAdmins.html");
                if (File.Exists(templatePath))
                {
                    var template = File.ReadAllText(templatePath)
                        .Replace("{{TicketId}}", model.TicketId.ToString())
                        .Replace("{{Category}}", EnumExtensions.GetDisplayName(model.TicketCategory))
                        .Replace("{{Priority}}", EnumExtensions.GetDisplayName(model.TicketPriority))
                        .Replace("{{CustomerName}}", model.SenderName)
                        .Replace("{{Status}}", EnumExtensions.GetDisplayName(model.TicketStatus))
                        .Replace("{{CreatedAt}}", model.CreatedDate.ToString(DateFormatConst.dd_MM_yyyy_HH_mm_ss))
                        .Replace("{{Message}}", model.TicketMessage)
                        .Replace("{{TicketUrl}}", model.TicketUrl)
                        .Replace("{{year}}", DateTime.Now.Year.ToString());
                    bodyToSend = template;
                    //.Replace("{{name}}", toName ?? string.Empty)
                    //.Replace("{{customMessage}}", senderName ?? string.Empty)
                    //.Replace("{{actionUrl}}", actionUrl ?? string.Empty)
                    //.Replace("{{year}}", DateTime.UtcNow.Year.ToString());
                }
            }
            catch { }

            var message = new MailMessage
            {
                From = new MailAddress("betechtest42@gmail.com", "Mukellef.io"),
                Subject = "Ticket Mesaj Bildirimi",
                Body = bodyToSend,
                IsBodyHtml = true,
                //To = { model.Reciever }
            };
            model.Recievers.ForEach(x => message.To.Add(x));
            //message.To.Add(model.Recievers);
            smtpClient.SendMailAsync(message);
            return response;
        }
        catch
        {
            return response;
        }
    }
}