using Microsoft.EntityFrameworkCore;
using voting_system_core.Models;

namespace voting_system_core.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
