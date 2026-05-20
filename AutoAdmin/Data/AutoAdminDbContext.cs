using AutoAdmin.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Data;

public class AutoAdminDbContext : IdentityDbContext<AppUser>
{
    public AutoAdminDbContext(DbContextOptions<AutoAdminDbContext> options)
        : base(options)
    {
    }

    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<AiMessageDraft> AiMessageDrafts => Set<AiMessageDraft>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Business>(entity =>
        {
            entity.HasIndex(x => new { x.OwnerId, x.Name });

            entity.Property(x => x.Currency).HasDefaultValue("IDR");
            entity.Property(x => x.InvoicePrefix).HasDefaultValue("INV");

            entity.HasOne(x => x.Owner)
                .WithMany(x => x.Businesses)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Client>(entity =>
        {
            entity.HasIndex(x => new { x.BusinessId, x.Name });
            entity.HasIndex(x => new { x.BusinessId, x.Status });

            entity.Property(x => x.EstimatedValue).HasPrecision(18, 2);

            entity.HasOne(x => x.Business)
                .WithMany(x => x.Clients)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Invoice>(entity =>
        {
            entity.HasIndex(x => new { x.BusinessId, x.InvoiceNumber }).IsUnique();
            entity.HasIndex(x => new { x.BusinessId, x.Status });
            entity.HasIndex(x => x.DueDate);

            entity.Property(x => x.Subtotal).HasPrecision(18, 2);
            entity.Property(x => x.Discount).HasPrecision(18, 2);
            entity.Property(x => x.Tax).HasPrecision(18, 2);
            entity.Property(x => x.Total).HasPrecision(18, 2);

            entity.HasOne(x => x.Business)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Invoices)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<InvoiceItem>(entity =>
        {
            entity.Property(x => x.Quantity).HasPrecision(18, 2);
            entity.Property(x => x.UnitPrice).HasPrecision(18, 2);
            entity.Property(x => x.Total).HasPrecision(18, 2);

            entity.HasOne(x => x.Invoice)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Proposal>(entity =>
        {
            entity.HasIndex(x => new { x.BusinessId, x.Status });

            entity.Property(x => x.EstimatedPrice).HasPrecision(18, 2);

            entity.HasOne(x => x.Business)
                .WithMany(x => x.Proposals)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Proposals)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<TaskItem>(entity =>
        {
            entity.HasIndex(x => new { x.BusinessId, x.Status });
            entity.HasIndex(x => x.DueDate);

            entity.HasOne(x => x.Business)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Client)
                .WithMany(x => x.Tasks)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Invoice)
                .WithMany()
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<AiMessageDraft>(entity =>
        {
            entity.HasIndex(x => new { x.BusinessId, x.CreatedAtUtc });

            entity.HasOne(x => x.Business)
                .WithMany(x => x.AiMessageDrafts)
                .HasForeignKey(x => x.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Invoice)
                .WithMany()
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}