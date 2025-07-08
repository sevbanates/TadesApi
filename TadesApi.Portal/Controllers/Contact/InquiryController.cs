using TadesApi.Models.ActionsEnum;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using TadesApi.Core;
using TadesApi.BusinessService.InquiryServices.Interfaces;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ViewModels.Inquiry;

namespace TadesApi.Portal.Controllers.Inquiry
{
    [Route("api/inquiries")]
    [ApiController]
    public class InquiryController : BaseController
    {
        private readonly IInquiryService _inquiryService;

        public InquiryController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        [SecurityState((int)InquirySecurity.View)]
        [HttpGet]
        [Route("{id}/{guidId}")]
        public ActionResponse<InquiryDto> GetEntityById(long id, Guid guidId)
        {
            try
            {
                var response = _inquiryService.GetSingle(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<InquiryDto>(), "GetEntityById Error :" + ex.Message);
            }
        }


        [SecurityState((int)InquirySecurity.List)]
        [HttpGet]
        public PagedAndSortedResponse<InquiryDto> GetEntitiesPaged([FromQuery] PagedAndSortedInput input)
        {
            try
            {
                var response = _inquiryService.GetMulti(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<InquiryDto>(), "GetEntitiesPaged Error :" + ex.Message);
            }
        }

        [SecurityState((int)InquirySecurity.List)]
        [HttpGet]
        [Route("with-replies")]
        public PagedAndSortedResponse<InquiryWithReplyMessagesDto> GetContactsWithReplyMessages(
            [FromQuery] PagedAndSortedSearchInput input)
        {
            try
            {
                var response = _inquiryService.GetInquiriesWithReplyMessages(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<InquiryWithReplyMessagesDto>(),
                    "GetContactsWithReplyMessages Error :" + ex.Message);
            }
        }


        [SecurityState((int)InquirySecurity.Delete)]
        [HttpDelete]
        [Route("{id}/{guidId}")]
        public ActionResponse<bool> DeleteEntity(long id, Guid guidId)
        {
            try
            {
                var response = _inquiryService.Delete(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "DeleteEntity Error :" + ex.Message);
            }
        }


        [SecurityState((int)InquirySecurity.Save)]
        [HttpPost]
        [Route("{id}/reply")]
        public ActionResponse<bool> SendReplyMessage([FromBody] ReplyInquiryDto input, long id)
        {
            try
            {
                var response = _inquiryService.SendReplyMessage(id, input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "SendReplyMessage Error :" + ex.Message);
            }
        }
    }
}