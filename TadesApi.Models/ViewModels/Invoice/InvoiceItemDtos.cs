using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TadesApi.Models.ViewModels.Invoice;

public class InvoiceItemCreateDto
{
    public int InvoiceId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = null!;

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total => Quantity * UnitPrice;
}