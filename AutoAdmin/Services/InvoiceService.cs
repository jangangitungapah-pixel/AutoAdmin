using AutoAdmin.Data;
using AutoAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class InvoiceService
{
    private readonly AutoAdminDbContext _db;

    public InvoiceService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public decimal CalculateSubtotal(IEnumerable<InvoiceItem> items)
    {
        return items.Sum(x => x.Quantity * x.UnitPrice);
    }

    public decimal CalculateTotal(decimal subtotal, decimal discount, decimal tax)
    {
        return Math.Max(0, subtotal - discount + tax);
    }

    public async Task<string> GenerateNextInvoiceNumberAsync(Guid businessId)
    {
        var business = await _db.Businesses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == businessId);

        var prefix = business?.InvoicePrefix;

        if (string.IsNullOrWhiteSpace(prefix))
        {
            prefix = "INV";
        }

        var count = await _db.Invoices
            .Where(x => x.BusinessId == businessId)
            .CountAsync();

        return $"{prefix}-{count + 1:0000}";
    }

    public async Task MarkAsPaidAsync(Guid invoiceId)
    {
        var invoice = await _db.Invoices.FirstOrDefaultAsync(x => x.Id == invoiceId);

        if (invoice is null)
        {
            return;
        }

        invoice.Status = InvoiceStatus.Paid;
        invoice.PaidAtUtc = DateTime.UtcNow;
        invoice.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}