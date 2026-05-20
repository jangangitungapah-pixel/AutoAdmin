using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.Models;

public enum AiMessageType
{
    FollowUp = 0,
    PaymentReminder = 1,
    ThankYou = 2,
    ProposalFollowUp = 3,
    MeetingReminder = 4,
    Offer = 5
}

public enum MessageTone
{
    Friendly = 0,
    Professional = 1,
    Firm = 2,
    Casual = 3
}

public class AiMessageDraft
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BusinessId { get; set; }

    public Business? Business { get; set; }

    public Guid? ClientId { get; set; }

    public Client? Client { get; set; }

    public Guid? InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    public AiMessageType Type { get; set; }

    public MessageTone Tone { get; set; } = MessageTone.Professional;

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}