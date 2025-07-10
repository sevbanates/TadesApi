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
