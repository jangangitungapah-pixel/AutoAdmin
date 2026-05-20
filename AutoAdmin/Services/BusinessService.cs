using AutoAdmin.Data;
using AutoAdmin.Models;
using AutoAdmin.ViewModels;
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

    public async Task<Business?> GetBusinessForUserAsync(string userId, Guid businessId)
    {
        return await _db.Businesses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OwnerId == userId && x.Id == businessId);
    }

    public async Task<BusinessSettingsFormModel?> GetBusinessSettingsAsync(string userId, Guid businessId)
    {
        var business = await GetBusinessForUserAsync(userId, businessId);

        if (business is null)
        {
            return null;
        }

        return new BusinessSettingsFormModel
        {
            Name = business.Name,
            Industry = business.Industry,
            Currency = business.Currency,
            InvoicePrefix = business.InvoicePrefix,
            Address = business.Address,
            Email = business.Email,
            Phone = business.Phone,
            WhatsAppNumber = business.WhatsAppNumber
        };
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

    public async Task<bool> UpdateBusinessSettingsAsync(
        string userId,
        Guid businessId,
        BusinessSettingsFormModel model)
    {
        var business = await _db.Businesses
            .FirstOrDefaultAsync(x => x.OwnerId == userId && x.Id == businessId);

        if (business is null)
        {
            return false;
        }

        business.Name = model.Name.Trim();
        business.Industry = Clean(model.Industry);
        business.Currency = model.Currency.Trim().ToUpperInvariant();
        business.InvoicePrefix = model.InvoicePrefix.Trim().ToUpperInvariant();
        business.Address = Clean(model.Address);
        business.Email = Clean(model.Email);
        business.Phone = Clean(model.Phone);
        business.WhatsAppNumber = Clean(model.WhatsAppNumber);
        business.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    private static string? Clean(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
