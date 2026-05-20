using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.Models;

public enum ClientStatus
{
    NewLead = 0,
    Contacted = 1,
    ProposalSent = 2,
    Negotiation = 3,
    Deal = 4,
    Completed = 5,
    Lost = 6
}

public class Client
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BusinessId { get; set; }

    public Business? Business { get; set; }

    [Required]
    [MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(160)]
    public string? CompanyName { get; set; }

    [MaxLength(120)]
    public string? Email { get; set; }

    [MaxLength(40)]
    public string? Phone { get; set; }

    public ClientStatus Status { get; set; } = ClientStatus.NewLead;

    public decimal EstimatedValue { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}