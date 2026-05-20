using AutoAdmin.Data;
using AutoAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class BusinessService
{
    private readonly AutoAdminDbContext _db;

    public BusinessService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public async Task<List<Business>> GetBusinessesForUserAsync(string userId)
    {
        return await _db.Businesses
            .AsNoTracking()
            .Where(x => x.OwnerId == userId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<Business> CreateBusinessAsync(string userId, string name)
    {
        var business = new Business
        {
            OwnerId = userId,
            Name = name,
            Currency = "IDR",
            InvoicePrefix = "INV"
        };

        _db.Businesses.Add(business);
        await _db.SaveChangesAsync();

        return business;
    }
}