using OnlineVotingSystem.Data;

namespace OnlineVotingSystem.Models;
public class AggregatedDataService
{
    private readonly ApplicationDbContext _context;

    public AggregatedDataService(ApplicationDbContext context)
    {
        _context = context;
    }

    public AggregatedData GetAggregatedData()
    {
        int totalCandidates = _context.Candidate.Count();
        int totalVoters = _context.Voter.Count();
        int totalVotes = _context.Votes.Count();

        return new AggregatedData
        {
            TotalCandidates = totalCandidates,
            TotalVoters = totalVoters,
            TotalVotes = totalVotes
        };
    }
}
