using AutoAdmin.Data;
using AutoAdmin.Models;
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

    public string GenerateProposalSummary(string clientName, string serviceName, decimal estimatedPrice)
    {
        return
            $"Proposal untuk {clientName} terkait layanan {serviceName}. " +
            $"Estimasi nilai project adalah Rp {estimatedPrice:N0}. " +
            "Proposal ini mencakup scope kerja, timeline, deliverables, dan rincian biaya secara profesional.";
    }
}