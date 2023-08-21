using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineVotingSystem.Models
{
    public class Voter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SIN { get; set; }
        [Required]
        public string Email { get; set; }

        public int PhoneNumber { get; set; }
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public bool IsEligible { get; set; } = false;
        public bool HasVoted { get; set; } = false;
        public string FaceId { get; set; }
    }
}
