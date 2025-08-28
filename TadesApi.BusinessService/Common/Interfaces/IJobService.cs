
using SendWithBrevo;
using System.Collections.Generic;
using TadesApi.Core.Security;
using TadesApi.CoreHelper;
using TadesApi.Models.CustomModels;

namespace TadesApi.BusinessService.CommonServices.interfaces
{
    public interface IJobService
    {
        void AddLog<T>(T entity, string message, SecurityModel securityModel);
        void SendAccounterRequestMail<T>(T entity, string subject, string messageText, string toEmail, string toName, string actionUrl, string senderName);  
        void SendTicketCreatedMail(CreatedTicketMailModel model);
        void SendTicketMessageMailToClient(TicketMessageMailModel model);
        void SendTicketMessageMailToAdmin(TicketMessageMailModel model);

        //JobResponse UpdateAllActions();
        //JobResponse CheckPurchaseOrderItemExpire();
        //JobResponse CheckDealerOrderItemExpire();
        //JobResponse ExecuteCalculateCustomerSpend();
        //JobResponse ExecuteCalculateDealerSpend();
        //JobResponse TwoMonthsRemainingReport();
        //JobResponse PercentUsageReport();
    }
}
