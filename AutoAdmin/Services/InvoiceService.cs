using AutoAdmin.Data;
using AutoAdmin.Models;
using AutoAdmin.ViewModels;
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

    public async Task<List<Invoice>> GetInvoicesAsync(
        Guid businessId,
        string? search = null,
        InvoiceStatus? status = null)
    {
        var query = _db.Invoices
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.BusinessId == businessId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();

            query = query.Where(x =>
                x.InvoiceNumber.Contains(keyword) ||
                (x.Client != null && x.Client.Name.Contains(keyword)) ||
                (x.Client != null && x.Client.CompanyName != null && x.Client.CompanyName.Contains(keyword)));
        }

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query
            .OrderByDescending(x => x.IssueDate)
            .ThenByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceDetailAsync(Guid businessId, Guid invoiceId)
    {
        return await _db.Invoices
            .AsNoTracking()
            .Include(x => x.Client)
            .Include(x => x.Business)
            .Include(x => x.Items.OrderBy(item => item.SortOrder))
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == invoiceId);
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

    public async Task<Invoice> CreateInvoiceAsync(Guid businessId, InvoiceFormModel model)
    {
        var invoiceNumber = await GenerateNextInvoiceNumberAsync(businessId);
        var items = model.Items
            .Where(x => !string.IsNullOrWhiteSpace(x.Description))
            .ToList();

        if (items.Count == 0)
        {
            throw new InvalidOperationException("Invoice minimal punya satu item.");
        }

        var subtotal = items.Sum(x => x.Total);
        var invoice = new Invoice
        {
            BusinessId = businessId,
            ClientId = model.ClientId,
            InvoiceNumber = invoiceNumber,
            IssueDate = model.IssueDate,
            DueDate = model.DueDate,
            Status = model.Status,
            Subtotal = subtotal,
            Discount = model.Discount,
            Tax = model.Tax,
            Total = CalculateTotal(subtotal, model.Discount, model.Tax),
            Notes = Clean(model.Notes),
            PaidAtUtc = model.Status == InvoiceStatus.Paid ? DateTime.UtcNow : null,
            CreatedAtUtc = DateTime.UtcNow
        };

        for (var index = 0; index < items.Count; index++)
        {
            invoice.Items.Add(new InvoiceItem
            {
                Description = items[index].Description.Trim(),
                Quantity = items[index].Quantity,
                UnitPrice = items[index].UnitPrice,
                Total = items[index].Total,
                SortOrder = index + 1
            });
        }

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();

        return invoice;
    }

    public async Task<bool> UpdateInvoiceStatusAsync(Guid businessId, Guid invoiceId, InvoiceStatus status)
    {
        var invoice = await _db.Invoices
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == invoiceId);

        if (invoice is null)
        {
            return false;
        }

        invoice.Status = status;
        invoice.UpdatedAtUtc = DateTime.UtcNow;

        if (status == InvoiceStatus.Paid)
        {
            invoice.PaidAtUtc = DateTime.UtcNow;
        }
        else
        {
            invoice.PaidAtUtc = null;
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task MarkAsPaidAsync(Guid businessId, Guid invoiceId)
    {
        var invoice = await _db.Invoices
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == invoiceId);

        if (invoice is null)
        {
            return;
        }

        invoice.Status = InvoiceStatus.Paid;
        invoice.PaidAtUtc = DateTime.UtcNow;
        invoice.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    private static string? Clean(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
