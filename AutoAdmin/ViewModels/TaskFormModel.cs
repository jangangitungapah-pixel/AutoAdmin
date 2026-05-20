using System.ComponentModel.DataAnnotations;
using AutoAdmin.Models;

namespace AutoAdmin.ViewModels;

public class TaskFormModel
{
    public Guid? ClientId { get; set; }

    public Guid? InvoiceId { get; set; }

    [Required(ErrorMessage = "Judul task wajib diisi.")]
    [MaxLength(180, ErrorMessage = "Judul task maksimal 180 karakter.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Deskripsi maksimal 1000 karakter.")]
    public string? Description { get; set; }

    public TaskItemStatus Status { get; set; } = TaskItemStatus.Todo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateOnly? DueDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
}
