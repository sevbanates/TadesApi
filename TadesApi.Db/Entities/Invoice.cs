using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TadesApi.Core;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Db.Entities
{
    public partial class Invoice : BaseEntity
    {
        public int Id { get; set; }

        #region RecipientInfo

        [Required]
        [MaxLength(11)]
        [MinLength(10)]
        public string Vkn { get; set; } // int -> string

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? Title { get; set; }

        [Required]
        public string TaxOffice { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string District { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; } // eklendi

        #endregion

        #region InvoiceHeader

        [Required]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } = null!;

        [MaxLength(10)]
        public string? Seri { get; set; } // eklendi

        [Required]
        public Guid Uuid { get; set; } = Guid.NewGuid(); // eklendi

        public InvoiceTypes InvoiceType { get; set; }
        public Scenario Scenario { get; set; }
        public int Currency { get; set; }
        public string Note { get; set; }
        public DateTime? DeliveryDate { get; set; }

        #endregion

        [Required]
        public string DeliveryAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0m;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        [MaxLength(100)]
        public string? GibStatus { get; set; } // eklendi

        [MaxLength(500)]
        public string? GibMessage { get; set; } // eklendi
    }
}
