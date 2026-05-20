using AutoAdmin.Data;
using AutoAdmin.Models;
using AutoAdmin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class TaskService
{
    private readonly AutoAdminDbContext _db;

    public TaskService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskItem>> GetOpenTasksAsync(Guid businessId)
    {
        return await _db.TaskItems
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Invoice)
            .Where(x =>
                x.BusinessId == businessId &&
                x.Status != TaskItemStatus.Done &&
                x.Status != TaskItemStatus.Cancelled)
            .OrderBy(x => x.DueDate)
            .ThenByDescending(x => x.Priority)
            .ToListAsync();
    }

    public async Task<List<TaskItem>> GetTasksAsync(
        Guid businessId,
        TaskItemStatus? status = null)
    {
        var query = _db.TaskItems
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Invoice)
            .Where(x => x.BusinessId == businessId);

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query
            .OrderBy(x => x.Status == TaskItemStatus.Done || x.Status == TaskItemStatus.Cancelled)
            .ThenBy(x => x.DueDate == null)
            .ThenBy(x => x.DueDate)
            .ThenByDescending(x => x.Priority)
            .ToListAsync();
    }

    public async Task<TaskItem> CreateTaskAsync(Guid businessId, TaskFormModel model)
    {
        var task = new TaskItem
        {
            BusinessId = businessId,
            ClientId = model.ClientId,
            InvoiceId = model.InvoiceId,
            Title = model.Title.Trim(),
            Description = Clean(model.Description),
            Status = model.Status,
            Priority = model.Priority,
            DueDate = model.DueDate,
            CreatedAtUtc = DateTime.UtcNow,
            CompletedAtUtc = model.Status == TaskItemStatus.Done ? DateTime.UtcNow : null
        };

        _db.TaskItems.Add(task);
        await _db.SaveChangesAsync();

        return task;
    }

    public async Task<bool> UpdateTaskStatusAsync(
        Guid businessId,
        Guid taskId,
        TaskItemStatus status)
    {
        var task = await _db.TaskItems
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == taskId);

        if (task is null)
        {
            return false;
        }

        task.Status = status;
        task.CompletedAtUtc = status == TaskItemStatus.Done ? DateTime.UtcNow : null;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task CompleteTaskAsync(Guid businessId, Guid taskId)
    {
        var task = await _db.TaskItems
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == taskId);

        if (task is null)
        {
            return;
        }

        task.Status = TaskItemStatus.Done;
        task.CompletedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    private static string? Clean(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
