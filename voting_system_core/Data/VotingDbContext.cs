using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using voting_system_core.Models;
using Newtonsoft.Json;

namespace voting_system_core.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ULID <-> VARCHAR(26) Converter
            var ulidConverter = new ValueConverter<Ulid, string>(
                ulid => ulid.ToString(),  // Store as string (VARCHAR)
                str => Ulid.Parse(str)     // Convert back to Ulid in C#
            );

            // Apply ULID conversion
            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.PollId)
                      .HasConversion(ulidConverter)
                      .HasMaxLength(26);
            });

            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(e => e.PollId)
                      .HasConversion(ulidConverter)
                      .HasMaxLength(26);

                entity.Property(e => e.OptionId)
                      .HasConversion(ulidConverter)
                      .HasMaxLength(26);
            });

            modelBuilder.Entity<Vote>(entity =>
            {
                entity.Property(e => e.VoteId)
                      .HasConversion(ulidConverter)
                      .HasMaxLength(26);

                entity.Property(e => e.PollId)
                      .HasConversion(ulidConverter)
                      .HasMaxLength(26);

                entity.Property(e => e.OptionId)
                      .HasConversion(ulidConverter)
                      .HasMaxLength(26);
            });

            // Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Key);
                entity.Property(e => e.Value);
                entity.Property(e => e.Authority);
            });

            // Relationships
            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Options)
                .WithOne(p => p.Poll)
                .HasForeignKey(o => o.PollId);

            modelBuilder.Entity<Poll>()
                .HasMany(p => p.Votes)
                .WithOne(v => v.Poll)
                .HasForeignKey(v => v.PollId);

            modelBuilder.Entity<Option>()
                .HasMany(o => o.Votes)
                .WithOne(v => v.Option)
                .HasForeignKey(v => v.OptionId);
        }
    }

    // JSONB Converter (if you need it for other fields)
    public class JsonbValueConverter<T> : ValueConverter<T, string>
    {
        public JsonbValueConverter()
            : base(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<T>(v))
        {
        }
    }
}
