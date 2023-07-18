using Microsoft.AspNetCore.Mvc;
using OnlineVotingSystem.Data;
using OnlineVotingSystem.Models;
using System.Diagnostics;

namespace OnlineVotingSystem.Controllers
{
    public class VoteController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
         private readonly IConfiguration _configuration;
        private readonly FaceService _faceService;

        public VoteController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration, FaceService faceService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _faceService = faceService;
        }

        public IActionResult Index()
        {
            return View();
        }

    
        //Get
        public IActionResult VerifyFace(Voter voter)
        {
             if (voter == null)
            {
                return NotFound();
            }
            return View(voter);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyFace(Voter voter, string faceDetails)
        {
             if (voter == null)
            {
                return NotFound();
            }
            return View(voter);
        }
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
            return RedirectToAction("VerifyFace",voter);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}