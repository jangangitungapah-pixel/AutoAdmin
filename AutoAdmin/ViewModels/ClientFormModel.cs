using AutoAdmin.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.ViewModels;

public class ClientFormModel
{
    [Required(ErrorMessage = "Nama client wajib diisi.")]
    [MaxLength(160, ErrorMessage = "Nama client maksimal 160 karakter.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160, ErrorMessage = "Nama perusahaan maksimal 160 karakter.")]
    public string? CompanyName { get; set; }

    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    [MaxLength(120, ErrorMessage = "Email maksimal 120 karakter.")]
    public string? Email { get; set; }

    [MaxLength(40, ErrorMessage = "Nomor telepon maksimal 40 karakter.")]
    public string? Phone { get; set; }

    public ClientStatus Status { get; set; } = ClientStatus.NewLead;

    [Range(0, 999_999_999_999, ErrorMessage = "Estimasi nilai project tidak valid.")]
    public decimal EstimatedValue { get; set; }

    [MaxLength(1000, ErrorMessage = "Catatan maksimal 1000 karakter.")]
    public string? Notes { get; set; }
}