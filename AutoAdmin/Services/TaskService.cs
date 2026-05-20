using AutoAdmin.Data;
using AutoAdmin.Models;
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
}