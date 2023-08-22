using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineVotingSystem.Models;

namespace OnlineVotingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<OnlineVotingSystem.Models.Candidate>? Candidate { get; set; }
        public DbSet<OnlineVotingSystem.Models.Election>? Election { get; set; }
        public DbSet<OnlineVotingSystem.Models.Voter>? Voter { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}