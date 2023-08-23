using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineVotingSystem.Data;
using OnlineVotingSystem.Models;
using System.Diagnostics;
using OpenCvSharp;

namespace OnlineVotingSystem.Controllers
{
    public class VoteController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly FaceService _faceService;
        private readonly IWebHostEnvironment  _HostingEnvironment;

        private readonly string SubscriptionKey = "c94b1dbdc745430c904a1f880bb6affd";
        private readonly string Endpoint = "https://evotingsystem.cognitiveservices.azure.com/";
      

        

        public VoteController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration, FaceService faceService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _faceService = faceService;
            _HostingEnvironment = webHostEnvironment;
           
        }

        public async Task<IActionResult> Index()
        {
            var voter = HttpContext.Session.GetObject<Voter>("Voter");

            if(voter.HasVoted==true)
                return RedirectToAction("AlreadyVoted");

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
        public async Task<IActionResult> VerifyFaces([FromBody]byte[] inputImage)
        {
            
            // //Assuming you have a byte array (byteArray) to convert
            // string fileName = "yourfile.jpg"; // Provide the desired file name

            // // Convert byte array to IFormFile using the helper method
            // IFormFile image = ConvertByteArrayToIFormFile(inputImage, fileName);

            // // Now you can use the IFormFile for uploading, processing, etc.
            // // For example:
            // // _yourFileUploadService.Upload(formFile); // Replace with your file upload service
            // if (voter == null || image == null || image.Length == 0)
            // {
            //     return NotFound();
            // }

            // // Authenticate with the Face API using your subscription key and endpoint
            // var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(SubscriptionKey))
            // {
            //     Endpoint = Endpoint
            // };

            // // Detect face in the uploaded image
            // var faces = await faceClient.Face.DetectWithStreamAsync(image.OpenReadStream(), returnFaceId: true);

            // if (faces.Count == 0)
            // {
            //     // No face detected
            //     ViewBag.ErrorMessage = "No face detected in the provided image.";
            //     return View(voter);
            // }

            // // Assuming you have a face ID associated with the voter
            // string FaceId = "";
            // var voterFaceId = Guid.Parse(FaceId); // Replace with your logic
            
            // // Verify the detected face with the voter's face
            // var verifyResult = await faceClient.Face.VerifyFaceToFaceAsync(faces[0].FaceId.Value, voterFaceId);

            // if (verifyResult.IsIdentical)
            // {
            //     // Face verification successful
            //     // Proceed with further actions or redirect
            //     return RedirectToAction("Index");
            // }
            // else
            // {
            //     // Face verification failed
            //     ViewBag.ErrorMessage = "Face verification failed. The provided face does not match the voter's face.";
                return View();
            // }
        }

        [HttpPost]
        public JsonResult ProcessFrame([FromBody] byte[] imageData)
        {
            string cascadeFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "haarcascade_frontalface_default.xml");

            using (var image = Cv2.ImDecode(imageData, ImreadModes.Color))
            using (var cascadeClassifier = new CascadeClassifier(cascadeFilePath))
            {
                Rect[] faces = cascadeClassifier.DetectMultiScale(image);

                var faceCoordinates = faces.Select(face => new
                {
                    X = face.X,
                    Y = face.Y,
                    Width = face.Width,
                    Height = face.Height
                });

                return Json(faceCoordinates);
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
            Voter voterSession = new Voter(); // Create the voter object
            voterSession = voter;
            HttpContext.Session.SetObject("Voter", voterSession);
           
            return RedirectToAction("Index",voter);
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
            var voterSession = HttpContext.Session.GetObject<Voter>("Voter");

            if (votedCandidates != null)
            {
                foreach (var candidate in votedCandidates)
                {
                    if(candidate==null)
                        break;
                    Vote vote = new Vote(voterSession.SIN,candidate.SIN,candidate.ElectionId);
                
                    _context.Add(vote);
                    await _context.SaveChangesAsync();
                }
                    
                    var votedVoter = _context.Voter.Where(v=> v.SIN == voterSession.SIN).FirstOrDefault();
                    if(votedVoter==null){
                         return NotFound();
                     }
                    votedVoter.HasVoted = true;
                    _context.Update(votedVoter);
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
        public IActionResult AlreadyVoted()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult CaptureImage(string imageData)
        {
            // Process the captured image data here
            // imageData will contain the base64-encoded image data
            // Convert it to byte[] or save it as needed

            return Json(new { success = true });
        }


        public static IFormFile ConvertByteArrayToIFormFile(byte[] byteArray, string fileName)
{
        // Create a stream from the byte array
        using (var stream = new MemoryStream(byteArray))
        {
            // Create an IFormFile instance
            var formFile = new FormFile(stream, 0, byteArray.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };

            return formFile;
        }
}
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}