using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ViewModels.Inquiry;

namespace TadesApi.BusinessService.InquiryServices.Interfaces
{
    public interface IInquiryService : IBaseServiceNg<CreateInquiryDto, UpdateInquiryDto, InquiryDto, PagedAndSortedInput>
    {
        public ActionResponse<InquiryDto> GetSingle(long id, Guid guidId);
        public ActionResponse<bool> SendReplyMessage(long id, ReplyInquiryDto input);

        public PagedAndSortedResponse<InquiryWithReplyMessagesDto> GetInquiriesWithReplyMessages(PagedAndSortedSearchInput input);
        ActionResponse<InquiryDto> CreateInquiry(CreateInquiryDto input);
    }
}