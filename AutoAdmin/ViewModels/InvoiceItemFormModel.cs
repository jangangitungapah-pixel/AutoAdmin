using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.ViewModels;

public class InvoiceItemFormModel
{
    [Required(ErrorMessage = "Deskripsi item wajib diisi.")]
    [MaxLength(255, ErrorMessage = "Deskripsi item maksimal 255 karakter.")]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, 999999, ErrorMessage = "Quantity harus lebih dari 0.")]
    public decimal Quantity { get; set; } = 1;

    [Range(0, 999999999999, ErrorMessage = "Harga tidak valid.")]
    public decimal UnitPrice { get; set; }

    public decimal Total => Quantity * UnitPrice;
}
