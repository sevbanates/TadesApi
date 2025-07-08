using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.BusinessService.MessageServices.Interfaces
{
    public interface IMessageService
    {
        ActionResponse<MessageDto> GetMessageById(long messageId, Guid guidId);
        ActionResponse<MessageWithSenderAndRepliesDto> GetMessageWithReplies(long messageId, Guid guidId);
        PagedAndSortedResponse<MessageWithSenderDto> GetReceivedMessages(PagedAndSortedInput input);
        PagedAndSortedResponse<MessageWithReceiverDto> GetSentMessages(PagedAndSortedInput input);
        ActionResponse<int> GetUnreadMessagesCount();
        ActionResponse<CreateMessageDto> CreateMessage(CreateMessageDto message);
        ActionResponse<MessageDto> DeleteMessageForSender(long messageId, Guid guidId);
        ActionResponse<MessageDto> DeleteMessageForReceiver(long messageId, Guid guidId);
    }
}