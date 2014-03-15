using System.Data.Entity;

namespace Leaguerly.Api.Models
{
    public class LeaguerlyDbContext : DbContext
    {
        public DbSet<League> Leagues { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Location> Locations { get; set; }

        public LeaguerlyDbContext() : base("LeaguerlyDb") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            RegisterLeagueModel(modelBuilder);
            RegisterDivisionModel(modelBuilder);
            RegisterTeamModel(modelBuilder);
            RegisterPlayerModel(modelBuilder);
        }

        private void RegisterLeagueModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<League>()
                .Property(league => league.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<League>()
                .Property(league => league.Alias)
                .HasMaxLength(30)
                .IsRequired()
                .IsUnicode(false);
        }

        private void RegisterDivisionModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Division>()
                .Property(division => division.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Division>()
                .Property(division => division.Alias)
                .HasMaxLength(30)
                .IsRequired()
                .IsUnicode(false);
        }

        private void RegisterTeamModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Team>()
                .Property(team => team.Name)
                .HasMaxLength(100)
                .IsRequired();
        }

        private void RegisterPlayerModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Player>()
                .Property(player => player.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Player>()
                .Property(player => player.LastName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}