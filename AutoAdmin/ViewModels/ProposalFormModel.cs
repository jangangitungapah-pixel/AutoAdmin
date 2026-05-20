using System.ComponentModel.DataAnnotations;
using AutoAdmin.Models;

namespace AutoAdmin.ViewModels;

public class ProposalFormModel
{
    public Guid? ClientId { get; set; }

    [Required(ErrorMessage = "Judul proposal wajib diisi.")]
    [MaxLength(180, ErrorMessage = "Judul proposal maksimal 180 karakter.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "Ringkasan maksimal 2000 karakter.")]
    public string? Summary { get; set; }

    [MaxLength(4000, ErrorMessage = "Scope kerja maksimal 4000 karakter.")]
    public string? ScopeOfWork { get; set; }

    [MaxLength(2000, ErrorMessage = "Deliverables maksimal 2000 karakter.")]
    public string? Deliverables { get; set; }

    [MaxLength(2000, ErrorMessage = "Timeline maksimal 2000 karakter.")]
    public string? Timeline { get; set; }

    [Range(0, 999999999999, ErrorMessage = "Nilai proposal tidak valid.")]
    public decimal EstimatedPrice { get; set; }

    public ProposalStatus Status { get; set; } = ProposalStatus.Draft;

    public DateOnly? ValidUntil { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));
}
