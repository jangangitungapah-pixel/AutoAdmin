using System.ComponentModel.DataAnnotations;

namespace AutoAdmin.Models;

public enum TaskItemStatus
{
    Todo = 0,
    InProgress = 1,
    Done = 2,
    Cancelled = 3
}

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2,
    Urgent = 3
}

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BusinessId { get; set; }

    public Business? Business { get; set; }

    public Guid? ClientId { get; set; }

    public Client? Client { get; set; }

    public Guid? InvoiceId { get; set; }

    public Invoice? Invoice { get; set; }

    [Required]
    [MaxLength(180)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateOnly? DueDate { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAtUtc { get; set; }
}