using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.Models;

public class InvoiceItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    public decimal Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }

    public int SortOrder { get; set; }
}