
using TadesApi.Core.Security;
using TadesApi.CoreHelper;

namespace TadesApi.BusinessService.CommonServices.interfaces
{
    public interface IJobService
    {
        void AddLog<T>(T entity, string messag, SecurityModel securityModele);

        //JobResponse UpdateAllActions();
        //JobResponse CheckPurchaseOrderItemExpire();
        //JobResponse CheckDealerOrderItemExpire();
        //JobResponse ExecuteCalculateCustomerSpend();
        //JobResponse ExecuteCalculateDealerSpend();
        //JobResponse TwoMonthsRemainingReport();
        //JobResponse PercentUsageReport();
    }
}
