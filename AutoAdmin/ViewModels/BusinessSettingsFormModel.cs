using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.ViewModels;

public class BusinessSettingsFormModel
{
    [Required(ErrorMessage = "Nama bisnis wajib diisi.")]
    [MaxLength(160, ErrorMessage = "Nama bisnis maksimal 160 karakter.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(80, ErrorMessage = "Industri maksimal 80 karakter.")]
    public string? Industry { get; set; }

    [Required(ErrorMessage = "Currency wajib diisi.")]
    [MaxLength(32, ErrorMessage = "Currency maksimal 32 karakter.")]
    public string Currency { get; set; } = "IDR";

    [Required(ErrorMessage = "Prefix invoice wajib diisi.")]
    [MaxLength(24, ErrorMessage = "Prefix invoice maksimal 24 karakter.")]
    public string InvoicePrefix { get; set; } = "INV";

    [MaxLength(255, ErrorMessage = "Alamat maksimal 255 karakter.")]
    public string? Address { get; set; }

    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [MaxLength(120, ErrorMessage = "Email maksimal 120 karakter.")]
    public string? Email { get; set; }

    [MaxLength(40, ErrorMessage = "Nomor telepon maksimal 40 karakter.")]
    public string? Phone { get; set; }

    [MaxLength(40, ErrorMessage = "Nomor WhatsApp maksimal 40 karakter.")]
    public string? WhatsAppNumber { get; set; }
}
