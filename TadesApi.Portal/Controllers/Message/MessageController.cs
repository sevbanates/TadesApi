using TadesApi.Models.ActionsEnum;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using TadesApi.Core;
using TadesApi.BusinessService.MessageServices.Interfaces;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.Portal.Controllers.Message
{
    [Route("api/messages")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [SecurityState((int)MessageSecurity.View)]
        [HttpGet]
        [Route("{id}/{guidId}")]
        public ActionResponse<MessageDto> GetMessageById(long id, Guid guidId)
        {
            try
            {
                var response = _messageService.GetMessageById(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<MessageDto>(), "GetMessageById Error :" + ex.Message);
            }
        }

        [SecurityState((int)MessageSecurity.View)]
        [HttpGet]
        [Route("{id}/with-replies")]
        public ActionResponse<MessageWithSenderAndRepliesDto> GetMessageWithReplies(long id, Guid guidId)
        {
            try
            {
                var response = _messageService.GetMessageWithReplies(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<MessageWithSenderAndRepliesDto>(), "GetMessageWithReplies Error :" + ex.Message);
            }
        }

        [SecurityState((int)MessageSecurity.List)]
        [HttpGet]
        [Route("sent")]
        public PagedAndSortedResponse<MessageWithReceiverDto> GetSentMessages([FromQuery] PagedAndSortedInput input)
        {
            try
            {
                var response = _messageService.GetSentMessages(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<MessageWithReceiverDto>(), "GetSentMessages Error :" + ex.Message);
            }
        }

        [SecurityState((int)MessageSecurity.List)]
        [HttpGet]
        [Route("received")]
        public PagedAndSortedResponse<MessageWithSenderDto> GetReceivedMessages([FromQuery] PagedAndSortedInput input)
        {
            try
            {
                var response = _messageService.GetReceivedMessages(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<MessageWithSenderDto>(), "GetReceivedMessages Error :" + ex.Message);
            }
        }

        [SecurityState((int)MessageSecurity.List)]
        [HttpGet]
        [Route("unread-count")]
        public ActionResponse<int> GetUnreadMessagesCount()
        {
            try
            {
                var response = _messageService.GetUnreadMessagesCount();
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<int>(), "GetUnreadMessagesCount Error :" + ex.Message);
            }
        }


        [SecurityState((int)MessageSecurity.Save)]
        [HttpPost]
        [Route("create")]
        public ActionResponse<CreateMessageDto> CreateMessage(CreateMessageDto input)
        {
            try
            {
                var response = _messageService.CreateMessage(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<CreateMessageDto>(), "CreateMessage Error :" + ex.Message);
            }
        }

        [SecurityState((int)MessageSecurity.Delete)]
        [HttpDelete]
        [Route("{id}/from-sender")]
        public ActionResponse<MessageDto> DeleteMessageForSender(long id, Guid guidId)
        {
            try
            {
                var response = _messageService.DeleteMessageForSender(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<MessageDto>(), "DeleteMessageForSender Error :" + ex.Message);
            }
        }

        [SecurityState((int)MessageSecurity.Delete)]
        [HttpDelete]
        [Route("{id}/from-receiver")]
        public ActionResponse<MessageDto> DeleteMessageForReceiver(long id, Guid guidId)
        {
            try
            {
                var response = _messageService.DeleteMessageForReceiver(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<MessageDto>(), "DeleteMessageForReceiver Error :" + ex.Message);
            }
        }

       
    }
}