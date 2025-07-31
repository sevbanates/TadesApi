using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TadesApi.Core;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Db.Entities
{
    public class TicketMessage : BaseEntity
    {
        [Required]
        public long TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        [Required]
        public long SenderId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SenderName { get; set; }

        [Required]
        [MaxLength(100)]
        public string SenderEmail { get; set; }

        [Required]
        [MaxLength(20)]
        public string SenderType { get; set; } // "user" | "admin"

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }

        [MaxLength(1000)]
        public string? Attachments { get; set; } // Dosya yollarý virgül ile ayrýlmýþ

        [Required]
        public DateTime CreatedAt { get; set; }

        public bool IsInternal { get; set; } // Admin'ler arasý notlar için
    }
}