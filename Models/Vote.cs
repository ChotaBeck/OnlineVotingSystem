namespace OnlineVotingSystem.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int VoterId { get; set; }
        public int Candidates { get; set; }
        public DateTime TimeOfVote { get; set; }
    }
}
