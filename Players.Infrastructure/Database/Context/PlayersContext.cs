using Microsoft.EntityFrameworkCore;
using Players.Domain.Entities;

namespace Players.Infrastructure.Database.Context
{
    public class PlayersContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public PlayersContext(DbContextOptions<PlayersContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .Property(e => e.Sport).HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
