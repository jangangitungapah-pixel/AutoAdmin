using AutoAdmin.Data;
using AutoAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class ClientService
{
    private readonly AutoAdminDbContext _db;

    public ClientService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public async Task<List<Client>> GetClientsAsync(Guid businessId, string? search = null)
    {
        var query = _db.Clients
            .AsNoTracking()
            .Where(x => x.BusinessId == businessId && !x.IsArchived);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Name.Contains(search) ||
                (x.CompanyName != null && x.CompanyName.Contains(search)) ||
                (x.Email != null && x.Email.Contains(search)));
        }

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Client?> GetClientAsync(Guid businessId, Guid clientId)
    {
        return await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == clientId);
    }

    public async Task ArchiveClientAsync(Guid businessId, Guid clientId)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == clientId);

        if (client is null)
        {
            return;
        }

        client.IsArchived = true;
        client.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}