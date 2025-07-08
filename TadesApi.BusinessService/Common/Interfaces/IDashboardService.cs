using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Models.ViewModels.Inquiry;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.BusinessService.Common.Interfaces
{
    public interface IDashboardService
    {

        
        PagedAndSortedResponse<MessageWithSenderDto> GetMessages(PagedAndSortedInput input);
    }
}
