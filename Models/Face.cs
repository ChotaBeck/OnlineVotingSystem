using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineVotingSystem.Models
{
    public class Face
    {
        public int Id { get; set; }
        [ForeignKey("VoterId")]
        public int VoterId { get; set;}
        public string FaceId { get; set; }
        public string RecognizedIdentity { get; set; }
    }
}