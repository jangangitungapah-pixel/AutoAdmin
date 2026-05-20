using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.Models;

public enum ProposalStatus
{
    Draft = 0,
    Sent = 1,
    Accepted = 2,
    Rejected = 3,
    ConvertedToInvoice = 4
}

public class Proposal
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BusinessId { get; set; }

    public Business? Business { get; set; }

    public Guid? ClientId { get; set; }

    public Client? Client { get; set; }

    [Required]
    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Summary { get; set; }

    [MaxLength(4000)]
    public string? ScopeOfWork { get; set; }

    [MaxLength(2000)]
    public string? Deliverables { get; set; }

    [MaxLength(2000)]
    public string? Timeline { get; set; }

    public decimal EstimatedPrice { get; set; }

    public ProposalStatus Status { get; set; } = ProposalStatus.Draft;

    public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    public DateOnly? ValidUntil { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }
}