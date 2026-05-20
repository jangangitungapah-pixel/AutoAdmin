using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.Models;

public class Business
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(160)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(80)]
    public string? Industry { get; set; }

    [MaxLength(32)]
    public string Currency { get; set; } = "IDR";

    [MaxLength(24)]
    public string InvoicePrefix { get; set; } = "INV";

    [MaxLength(255)]
    public string? Address { get; set; }

    [MaxLength(120)]
    public string? Email { get; set; }

    [MaxLength(40)]
    public string? Phone { get; set; }

    [MaxLength(40)]
    public string? WhatsAppNumber { get; set; }

    public string OwnerId { get; set; } = string.Empty;

    public AppUser? Owner { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>();

    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public ICollection<AiMessageDraft> AiMessageDrafts { get; set; } = new List<AiMessageDraft>();
}