using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TadesApi.Core;

namespace TadesApi.Db.Entities
{
    public class Message: BaseEntity
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public bool IsDeletedBySender { get; set; }
        public bool IsDeletedByReceiver { get; set; }

        [MaxLength(250)] public string Subject { get; set; }

        [MaxLength(2500)] public string Body { get; set; }

        public bool IsRead { get; set; }

        public long? ReplyMessageId { get; set; }

        [ForeignKey("ReplyMessageId")] public virtual ICollection<Message> ReplyMessages { get; set; }

        [ForeignKey("SenderId")] public virtual User Sender { get; set; }

        [ForeignKey("ReceiverId")] public virtual User Receiver { get; set; }
    }
}