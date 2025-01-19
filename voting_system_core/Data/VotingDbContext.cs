using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using voting_system_core.Models;
using AutoMapper.Execution;
using System;
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

            var ulidConverter = new ValueConverter<Ulid, string>(
                ulid => ulid.ToString(),
                str => Ulid.Parse(str));

            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(e => e.PollId).HasConversion(ulidConverter);
            });
            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(e => e.OptionId).HasConversion(ulidConverter);
            });
            modelBuilder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.PollId).HasConversion(ulidConverter);
            });
            modelBuilder.Entity<Vote>(entity =>
            {
                entity.Property(e => e.VoteId).HasConversion(ulidConverter);
            });


            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Key).HasColumnType("int2");
                entity.Property(e => e.Value).HasColumnType("varchar");
                entity.Property(e => e.Authority).HasColumnType("text");
            });

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
