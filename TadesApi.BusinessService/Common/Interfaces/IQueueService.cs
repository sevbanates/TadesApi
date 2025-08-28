using SendWithBrevo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Security;
using TadesApi.Models.CustomModels;

namespace TadesApi.BusinessService.CommonServices.interfaces
{
    public interface IQueueService
    {
        void AddLog<T>(T entity, string message, SecurityModel securityModel);
        //void CheckImzager();
        //void CalculateDelaerOrder();
        void SendAccounterRequestMail<T>(T entity, string subject, string messageText, string toEmail, string toName, string actionUrl, string senderName);
        void SendTicketCreatedMail(CreatedTicketMailModel model);
        void SendTicketMessageMailToClient(TicketMessageMailModel model);
        void SendTicketMessageMailToAdmin(TicketMessageMailModel model);
    }
}
