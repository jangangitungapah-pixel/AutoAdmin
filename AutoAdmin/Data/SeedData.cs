using AutoAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AutoAdminDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        await db.Database.MigrateAsync();

        const string demoEmail = "demo@autoadmin.local";
        const string demoPassword = "Demo123!";

        var demoUser = await userManager.FindByEmailAsync(demoEmail);

        if (demoUser is null)
        {
            demoUser = new AppUser
            {
                UserName = demoEmail,
                Email = demoEmail,
                EmailConfirmed = true,
                FullName = "AutoAdmin Demo User"
            };

            var result = await userManager.CreateAsync(demoUser, demoPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Failed to create demo user: {errors}");
            }
        }

        var hasBusiness = await db.Businesses.AnyAsync(x => x.OwnerId == demoUser.Id);

        if (hasBusiness)
        {
            return;
        }

        var business = new Business
        {
            OwnerId = demoUser.Id,
            Name = "AutoAdmin Demo Business",
            Industry = "Freelancer & UMKM",
            Email = demoEmail,
            Phone = "+62 812-0000-0000",
            WhatsAppNumber = "+62 812-0000-0000",
            Address = "Jakarta, Indonesia",
            Currency = "IDR",
            InvoicePrefix = "AAD"
        };

        var clientA = new Client
        {
            Business = business,
            Name = "Budi Santoso",
            CompanyName = "37 Music Studio",
            Email = "budi@example.com",
            Phone = "+62 812-1111-2222",
            Status = ClientStatus.Deal,
            EstimatedValue = 6_500_000,
            Notes = "Butuh sistem booking studio dan invoice bulanan."
        };

        var clientB = new Client
        {
            Business = business,
            Name = "Nadia Putri",
            CompanyName = "Rumah PC",
            Email = "nadia@example.com",
            Phone = "+62 813-3333-4444",
            Status = ClientStatus.ProposalSent,
            EstimatedValue = 8_500_000,
            Notes = "Interested dengan website company profile dan invoice digital."
        };

        var invoice = new Invoice
        {
            Business = business,
            Client = clientA,
            InvoiceNumber = "AAD-0001",
            IssueDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
            Status = InvoiceStatus.Sent,
            Subtotal = 6_500_000,
            Discount = 0,
            Tax = 0,
            Total = 6_500_000,
            Notes = "Pembayaran tahap pertama untuk pembuatan sistem booking."
        };

        invoice.Items.Add(new InvoiceItem
        {
            Description = "Web app booking studio",
            Quantity = 1,
            UnitPrice = 4_500_000,
            Total = 4_500_000,
            SortOrder = 1
        });

        invoice.Items.Add(new InvoiceItem
        {
            Description = "Dashboard invoice dan laporan",
            Quantity = 1,
            UnitPrice = 2_000_000,
            Total = 2_000_000,
            SortOrder = 2
        });

        var proposal = new Proposal
        {
            Business = business,
            Client = clientB,
            Title = "Proposal Website Rumah PC",
            Summary = "Pembuatan website modern untuk jasa rakit PC, katalog layanan, dan invoice digital.",
            ScopeOfWork = "Landing page, halaman layanan, halaman portfolio, dashboard admin sederhana.",
            Deliverables = "Source code, deployment guide, desain responsive, dokumentasi admin.",
            Timeline = "14 hari kerja",
            EstimatedPrice = 8_500_000,
            Status = ProposalStatus.Sent,
            ValidUntil = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14))
        };

        var taskA = new TaskItem
        {
            Business = business,
            Client = clientA,
            Invoice = invoice,
            Title = "Follow-up pembayaran invoice AAD-0001",
            Description = "Kirim reminder sopan via WhatsApp.",
            Priority = TaskPriority.High,
            Status = TaskItemStatus.Todo,
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        var taskB = new TaskItem
        {
            Business = business,
            Client = clientB,
            Title = "Follow-up proposal Rumah PC",
            Description = "Tanyakan feedback proposal dan jadwal meeting singkat.",
            Priority = TaskPriority.Medium,
            Status = TaskItemStatus.Todo,
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        };

        db.Businesses.Add(business);
        db.Clients.AddRange(clientA, clientB);
        db.Invoices.Add(invoice);
        db.Proposals.Add(proposal);
        db.TaskItems.AddRange(taskA, taskB);

        await db.SaveChangesAsync();
    }
}