using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TadesApi.Models.ViewModels.Invoice;

public class InvoiceItemCreateDto
{
   
        public int InvoiceId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = null!;
        
        [Required]
        [MaxLength(200)]
        public string ProductCode { get; set; } = null!;
        
        [MaxLength(200)]
        public string? Description { get; set; } = null!;
        
        [Required]
        public int UnitType { get; set; }

        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public int VatRate { get; set; }
        
        public int? DiscountRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total => Quantity * UnitPrice;
}