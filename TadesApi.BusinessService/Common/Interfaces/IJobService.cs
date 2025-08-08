
using SendWithBrevo;
using System.Collections.Generic;
using TadesApi.Core.Security;
using TadesApi.CoreHelper;

namespace TadesApi.BusinessService.CommonServices.interfaces
{
    public interface IJobService
    {
        void AddLog<T>(T entity, string message, SecurityModel securityModel);
        void SendAccounterRequestMail<T>(T entity, string subject, string messageText, string toEmail, string toName, string actionUrl, string senderName);

        //JobResponse UpdateAllActions();
        //JobResponse CheckPurchaseOrderItemExpire();
        //JobResponse CheckDealerOrderItemExpire();
        //JobResponse ExecuteCalculateCustomerSpend();
        //JobResponse ExecuteCalculateDealerSpend();
        //JobResponse TwoMonthsRemainingReport();
        //JobResponse PercentUsageReport();
    }
}
