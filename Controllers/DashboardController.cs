using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineVotingSystem.Data;
using OnlineVotingSystem.Models;

namespace OnlineVotingSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
         private readonly AggregatedDataService _aggregatedDataService;
         private readonly ApplicationDbContext _context;

        public DashboardController(AggregatedDataService aggregatedDataService, ApplicationDbContext context) 
        {
            _aggregatedDataService = aggregatedDataService;
            _context = context;
        }

        public IActionResult Index()
        {
            var aggregatedData = _aggregatedDataService.GetAggregatedData();

            var viewModel = new AggregatedDataViewModel
            {
                AggregatedData = aggregatedData
            };

            return View(viewModel);
        }
    
        public async Task<IActionResult> Results()
        {
            var candidates = await _context.Candidate.ToListAsync(); // Get candidates
            foreach (var candidate in candidates)
            {
                candidate.TotalVotes = _context.Votes.Count(v => v.CandidateId == candidate.SIN );
            }

            return View(candidates);
        }
    }
}
