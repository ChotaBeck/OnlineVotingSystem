using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
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
            if (voter == null || image == null || image.Length == 0)
            {
                return NotFound();
            }

            // Authenticate with the Face API using your subscription key and endpoint
            var faceClient = new FaceClient(new ApiKeyServiceClientCredentials("c94b1dbdc745430c904a1f880bb6affd"))
            {
                Endpoint = "https://evotingsystem.cognitiveservices.azure.com/"
            };

            // Detect face in the uploaded image
            var faces = await faceClient.Face.DetectWithStreamAsync(image.OpenReadStream(), returnFaceId: true);

            if (faces.Count == 0)
            {
                // No face detected
                ViewBag.ErrorMessage = "No face detected in the provided image.";
                return View(voter);
            }

            // Assuming you have a face ID associated with the voter
            var voterFaceId = Guid.Parse(voter.FaceId); // Replace with your logic
            
            // Verify the detected face with the voter's face
            var verifyResult = await faceClient.Face.VerifyFaceToFaceAsync(faces[0].FaceId.Value, voterFaceId);

            if (verifyResult.IsIdentical)
            {
                // Face verification successful
                // Proceed with further actions or redirect
                return RedirectToAction("Index");
            }
            else
            {
                // Face verification failed
                ViewBag.ErrorMessage = "Face verification failed. The provided face does not match the voter's face.";
                return View(voter);
            }
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

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VoteCandidate(Dictionary<string, int> candidates)
        {
            if (candidates == null)
            {
                return NotFound();
            }
            
            List<Candidate> votedCandidates = new List<Candidate>();
            Candidate voteCandidate;
            foreach (var candidatesSIN in candidates)
            {   
                voteCandidate = _context.Candidate.Where(c => c.SIN == candidatesSIN.Value).FirstOrDefault();
                votedCandidates.Add(voteCandidate);
            }
            if (votedCandidates != null)
            {
                Vote vote = new Vote(1234,votedCandidates);
                
                    _context.Add(vote);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(VoteSuccess));
                
            }
            // ViewBag["error"] = "Voted Failed";
            return View(votedCandidates);
            
        }

        public IActionResult VoteSuccess()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}