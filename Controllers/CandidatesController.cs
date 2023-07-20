using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineVotingSystem.Data;
using OnlineVotingSystem.Models;

namespace OnlineVotingSystem.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; 

        public CandidatesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Candidate.Include(c => c.Election);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Candidate == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate
                .Include(c => c.Election)
                .FirstOrDefaultAsync(m => m.SIN == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // GET: Candidates/Create
        public IActionResult Create()
        {
            ViewData["ElectionId"] = new SelectList(_context.Set<Election>(), "Id", "Id");
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SIN,FirstName,LastName,PoliticalParty,Position,IsEligible,ImageUrl,ElectionId")] CandidateViewModel candidateView)
        {
            Candidate candidate = new Candidate();

            if (ModelState.IsValid)
            {
                if (candidateView.ImageUrl != null && candidateView.ImageUrl.Length > 0)
                {
                    
                    // Save the profile picture to a specific location on the server
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "CandidatePictures", candidateView.ImageUrl.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await candidateView.ImageUrl.CopyToAsync(stream);
                    }

                    // Set the ProfilePicturePath property of the employee
                    
                    
                    candidate.SIN = candidateView.SIN;
                    candidate.FirstName = candidateView.FirstName;
                    candidate.LastName = candidateView.LastName;
                    candidate.PoliticalParty = candidateView.PoliticalParty;
                    candidate.Position = candidateView.Position.ToString();
                    candidate.IsEligible = candidateView.IsEligible;
                    candidate.ElectionId = candidateView.ElectionId;
                    candidate.ImageUrl = filePath;
                    
                    
                }
                

                _context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElectionId"] = new SelectList(_context.Set<Election>(), "Id", "Id", candidate.ElectionId);
            return View(candidateView);
        }

        

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Candidate == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            ViewData["ElectionId"] = new SelectList(_context.Set<Election>(), "Id", "Id", candidate.ElectionId);
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SIN,FirstName,LastName,PoliticalParty,Position,IsEligible,Image,ElectionId")] Candidate candidate)
        {
            if (id != candidate.SIN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(candidate.SIN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElectionId"] = new SelectList(_context.Set<Election>(), "Id", "Id", candidate.ElectionId);
            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Candidate == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate
                .Include(c => c.Election)
                .FirstOrDefaultAsync(m => m.SIN == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Candidate == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Candidate'  is null.");
            }
            var candidate = await _context.Candidate.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidate.Remove(candidate);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int id)
        {
          return (_context.Candidate?.Any(e => e.SIN == id)).GetValueOrDefault();
        }
    }
}
