using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlineVotingSystem.Enum;

namespace OnlineVotingSystem.Models
{
    public class Candidate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        public string ImageUrl { get; set; }
        public int ElectionId { get; set; }
        [ForeignKey("ElectionId")]
        public Election Election { get; set; }
        public int TotalVotes { get; set; }

    }
}
