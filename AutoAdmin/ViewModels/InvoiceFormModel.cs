using System.ComponentModel.DataAnnotations;
using AutoAdmin.Models;

namespace AutoAdmin.ViewModels;

public class InvoiceFormModel
{
    public Guid? ClientId { get; set; }

    public DateOnly IssueDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public DateOnly DueDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));

    public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

    [Range(0, 999999999999, ErrorMessage = "Diskon tidak valid.")]
    public decimal Discount { get; set; }

    [Range(0, 999999999999, ErrorMessage = "Pajak tidak valid.")]
    public decimal Tax { get; set; }

    [MaxLength(1000, ErrorMessage = "Catatan maksimal 1000 karakter.")]
    public string? Notes { get; set; }

    [MinLength(1, ErrorMessage = "Invoice minimal punya satu item.")]
    public List<InvoiceItemFormModel> Items { get; set; } =
    [
        new InvoiceItemFormModel()
    ];
}
