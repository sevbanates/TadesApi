
using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.Models.ViewModels.Message
{
    public class MessageDto
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreDate { get; set; }
        public bool IsRead { get; set; }
    }
    
    public class CreateMessageDto
    {
        [Required]
        public long ReceiverId { get; set; }
        
        [StringLength(250)]
        public string Subject { get; set; }
        
        [Required]
        [StringLength(2500)]
        public string Body { get; set; }
        
        public long AppJobId { get; set; }
        
        public long? ReplyMessageId { get; set; }
    }

    public class UpdateMessageDto : IBaseUpdateModel
    {
        public Guid GuidId { get; set; }
    }
    
    public class MessageWithSenderDto : MessageDto
    {
        public UserBasicDto Sender { get; set; }
    }
    
    public class MessageWithReceiverDto : MessageDto
    {
        public UserBasicDto Receiver { get; set; }
    }
    
    public class MessageWithSenderAndRepliesDto : MessageWithSenderDto
    {
        public List<MessageWithSenderDto> ReplyMessages { get; set; }
    }
}
