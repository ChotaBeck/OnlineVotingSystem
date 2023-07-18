using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineVotingSystem.Models
{
    public class CandidateViewModel
    {
        public int SIN { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PoliticalParty { get; set; }
        [Required]
        public string Position { get; set; }
        public bool IsEligible { get; set; }
        public IFormFile ImageUrl { get; set; }
        [ForeignKey("ElectionId")]
        public int ElectionId { get; set; }
        
        
    }
}