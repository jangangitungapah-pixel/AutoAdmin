using AutoAdmin.Data;
using AutoAdmin.Models;
using AutoAdmin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AutoAdmin.Services;

public class ProposalService
{
    private readonly AutoAdminDbContext _db;

    public ProposalService(AutoAdminDbContext db)
    {
        _db = db;
    }

    public async Task<List<Proposal>> GetProposalsAsync(Guid businessId)
    {
        return await _db.Proposals
            .AsNoTracking()
            .Include(x => x.Client)
            .Where(x => x.BusinessId == businessId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<Proposal> CreateProposalAsync(Guid businessId, ProposalFormModel model)
    {
        var proposal = new Proposal
        {
            BusinessId = businessId,
            ClientId = model.ClientId,
            Title = model.Title.Trim(),
            Summary = Clean(model.Summary),
            ScopeOfWork = Clean(model.ScopeOfWork),
            Deliverables = Clean(model.Deliverables),
            Timeline = Clean(model.Timeline),
            EstimatedPrice = model.EstimatedPrice,
            Status = model.Status,
            ValidUntil = model.ValidUntil,
            CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Proposals.Add(proposal);
        await _db.SaveChangesAsync();

        return proposal;
    }

    public async Task<bool> UpdateProposalStatusAsync(
        Guid businessId,
        Guid proposalId,
        ProposalStatus status)
    {
        var proposal = await _db.Proposals
            .FirstOrDefaultAsync(x => x.BusinessId == businessId && x.Id == proposalId);

        if (proposal is null)
        {
            return false;
        }

        proposal.Status = status;
        proposal.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public string GenerateProposalSummary(string clientName, string serviceName, decimal estimatedPrice)
    {
        return
            $"Proposal untuk {clientName} terkait layanan {serviceName}. " +
            $"Estimasi nilai project adalah Rp {estimatedPrice:N0}. " +
            "Proposal ini mencakup scope kerja, timeline, deliverables, dan rincian biaya secara profesional.";
    }

    private static string? Clean(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
