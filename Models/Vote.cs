namespace OnlineVotingSystem.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int VoterId { get; set; }
        public List<Candidate> VotedCandidate { get; set; }

        public Vote(){  }

        public Vote(int voterId, List<Candidate> votedCandidate)
        {
        voterId = voterId;
        votedCandidate = votedCandidate;
        }
    }
    

    
}
