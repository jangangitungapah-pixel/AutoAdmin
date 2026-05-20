using AutoAdmin.Data;
using AutoAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class CurrentBusinessService
{
    private readonly AutoAdminDbContext _db;

    public CurrentBusinessService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public async Task<Business?> GetFirstBusinessForUserAsync(string userId)
    {
        return await _db.Businesses
            .AsNoTracking()
            .Where(x => x.OwnerId == userId)
            .OrderBy(x => x.CreatedAtUtc)
            .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetFirstBusinessIdForUserAsync(string userId)
    {
        return await _db.Businesses
            .AsNoTracking()
            .Where(x => x.OwnerId == userId)
            .OrderBy(x => x.CreatedAtUtc)
            .Select(x => (Guid?)x.Id)
            .FirstOrDefaultAsync();
    }
}