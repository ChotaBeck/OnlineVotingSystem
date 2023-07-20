using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
             return _context.Election != null ? 
                          View(await _context.Election.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Voter'  is null.");
            
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
        public async Task<IActionResult> VerifyFace(Voter voter, IFormFile image)
        {
            //autheticate face api client
            var client = FaceService.Authenticate(_configuration);
             
            //detect face
            var detectedFace =client.Face.DetectWithStreamWithHttpMessagesAsync(image.OpenReadStream(), true ,false);

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

        //GET
        public async Task<IActionResult> VoteCandidate(int? id)
        {
            if (id == null || _context.Election == null)
            {
                return NotFound();
            }
            //var applicationDbContext = _context.Candidate.Include(c => c.Election);
            var candidates = await _context.Candidate.Where(c => c.ElectionId == id).ToListAsync();
                    
           
            if (candidates == null)
            {
                return NotFound();
            }
            
            return View(candidates);
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}