using AutoAdmin.Data;
using AutoAdmin.Models;
using AutoAdmin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class ClientService
{
    private readonly AutoAdminDbContext _db;

    public ClientService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public async Task<List<Client>> GetClientsAsync(
        Guid businessId,
        string? search = null,
        ClientStatus? status = null)
    {
        var query = _db.Clients
            .AsNoTracking()
            .Where(x => x.BusinessId == businessId && !x.IsArchived);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();

            query = query.Where(x =>
                x.Name.Contains(keyword) ||
                (x.CompanyName != null && x.CompanyName.Contains(keyword)) ||
                (x.Email != null && x.Email.Contains(keyword)) ||
                (x.Phone != null && x.Phone.Contains(keyword)));
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Client?> GetClientAsync(Guid businessId, Guid clientId)
    {
        return await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.BusinessId == businessId &&
                x.Id == clientId &&
                !x.IsArchived);
    }

    public async Task<ClientFormModel?> GetClientForEditAsync(Guid businessId, Guid clientId)
    {
        var client = await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.BusinessId == businessId &&
                x.Id == clientId &&
                !x.IsArchived);

        if (client is null)
        {
            return null;
        }

        return new ClientFormModel
        {
            Name = client.Name,
            CompanyName = client.CompanyName,
            Email = client.Email,
            Phone = client.Phone,
            Status = client.Status,
            EstimatedValue = client.EstimatedValue,
            Notes = client.Notes
        };
    }

    public async Task<Client> CreateClientAsync(Guid businessId, ClientFormModel model)
    {
        var client = new Client
        {
            BusinessId = businessId,
            Name = model.Name.Trim(),
            CompanyName = Clean(model.CompanyName),
            Email = Clean(model.Email),
            Phone = Clean(model.Phone),
            Status = model.Status,
            EstimatedValue = model.EstimatedValue,
            Notes = Clean(model.Notes),
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Clients.Add(client);
        await _db.SaveChangesAsync();

        return client;
    }

    public async Task<bool> UpdateClientAsync(Guid businessId, Guid clientId, ClientFormModel model)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(x =>
                x.BusinessId == businessId &&
                x.Id == clientId &&
                !x.IsArchived);

        if (client is null)
        {
            return false;
        }

        client.Name = model.Name.Trim();
        client.CompanyName = Clean(model.CompanyName);
        client.Email = Clean(model.Email);
        client.Phone = Clean(model.Phone);
        client.Status = model.Status;
        client.EstimatedValue = model.EstimatedValue;
        client.Notes = Clean(model.Notes);
        client.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ArchiveClientAsync(Guid businessId, Guid clientId)
    {
        var client = await _db.Clients
            .FirstOrDefaultAsync(x =>
                x.BusinessId == businessId &&
                x.Id == clientId &&
                !x.IsArchived);

        if (client is null)
        {
            return false;
        }

        client.IsArchived = true;
        client.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return true;
    }

    private static string? Clean(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}