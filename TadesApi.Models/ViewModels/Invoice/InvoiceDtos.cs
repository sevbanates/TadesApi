using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Models.ViewModels.Invoice
{
    public class InvoiceCreateDto
    {



        #region RecipientInfo


        [Required]
        [MaxLength(11)]
        [MinLength(10)]
        public int Vkn { get; set; }

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

        #endregion


        #region InvoiceHeader
        [Required]
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } = null!;
        public InvoiceTypes InvoiceType { get; set; }
        public Scenario Scenario { get; set; }
        public int Currency { get; set; }
        public string Note { get; set; }
        public DateTime? DeliveryDate { get; set; }

        #endregion
        [Required]
        public string DeliveryAddress { get; set; }

        public List<InvoiceItemCreateDto> Items { get; set; } = new();

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0m;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
      

    }

    public class InvoiceUpdateDto: IBaseUpdateModel
    {
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        public List<InvoiceItemCreateDto> Items { get; set; } = new();

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0m;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
        public Guid GuidId { get; set; }
    }

    public class InvoiceDto : BaseModel
    {
        public DateTime InvoiceDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string InvoiceNumber { get; set; } = null!;

        [Required]
        public int CustomerId { get; set; }

        public List<InvoiceItemCreateDto> Items { get; set; } = new();

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0m;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
    }
}
