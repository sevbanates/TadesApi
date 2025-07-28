using System;
using System.ComponentModel.DataAnnotations;
using TadesApi.Core;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Db.Entities
{
    public class UserRequest : BaseEntity
    {
        [Required]
        public long RequesterId { get; set; } // Accounter

        [Required]
        public long TargetUserId { get; set; } // Altýna alýnmak istenen kullanýcý
        public string TargetUserEmail { get; set; } // Altýna alýnmak istenen kullanýcý

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        // Navigation properties (opsiyonel)
        public virtual User? Requester { get; set; }
        public virtual User? TargetUser { get; set; }
    }
}