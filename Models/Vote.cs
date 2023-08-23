namespace OnlineVotingSystem.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int VoterId { get; set; }
        public int CandidateId { get; set; }
        public int ElectionId { get; set; }
        

        public Vote(){  }

        public Vote(int voterId, int candidateId, int electionId)
        {
        VoterId = voterId;
        CandidateId = candidateId;
        ElectionId = electionId;
        }
    }
    

    
}
