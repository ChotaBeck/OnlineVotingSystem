using System.ComponentModel.DataAnnotations;

namespace OnlineVotingSystem.Models
{
    public class Election
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Year { get; set; }
        public List<Candidate>? Candidates { get; set; }

    }
}
