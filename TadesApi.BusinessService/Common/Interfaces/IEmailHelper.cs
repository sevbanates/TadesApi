using System.Collections.Generic;
using SendWithBrevo;
using TadesApi.Core;

namespace TadesApi.BusinessService.Common.Interfaces;

public interface IEmailHelper
{
    //ActionResponse<bool> SendEmail(string subject, string htmlContent, string toEmail, string toName);
    ActionResponse<bool> SendAccounterRequestMail(string subject, string messageText, string toEmail, string toName, string actionUrl, string senderName); 
    ActionResponse<bool> SendTicketCreatedMail(string toName, string actionUrl, string senderName);
}