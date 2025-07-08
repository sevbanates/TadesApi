using System.ComponentModel.DataAnnotations.Schema;
using TadesApi.Core;
using TadesApi.Db.PartialEntites;

namespace TadesApi.Db.Entities
{
    public class Inquiry : BaseEntity, ISoftDeletable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }

        public long? ReplyMessageId { get; set; }

        [ForeignKey("ReplyMessageId")] public virtual ICollection<Inquiry> ReplyMessages { get; set; }
        public bool IsDeleted { get; set; }
    }
}