using Microsoft.AspNetCore.Mvc;
using OnlineVotingSystem.Data;
using OnlineVotingSystem.Models;
using System.Diagnostics;

namespace OnlineVotingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
         
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    
        // public IActionResult Vote(Voter voter)
        // {
        //      if (voter == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(voter);
        // }

        //GET
        public IActionResult Verification()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verification(int? id)
        {
            if (id == null || _context.Voter == null)
            {
                return NotFound();
            }
            
            var voter = await _context.Voter.FindAsync(id);
            if (voter == null)
            {
                return NotFound();
            }
            return RedirectToAction("Vote",voter);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}