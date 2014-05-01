using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace Leaguerly.Api.Models
{
    public class LeaguerlyDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<League> Leagues { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public LeaguerlyDbContext()
            : base("LeaguerlyDb") {

            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            RegisterLeagueModel(modelBuilder);
            RegisterDivisionModel(modelBuilder);
            RegisterTeamModel(modelBuilder);
            RegisterProfileModel(modelBuilder);
            RegisterPlayerModel(modelBuilder);
            RegisterManagerModel(modelBuilder);
            RegisterIdentityModel(modelBuilder);
        }

        private void RegisterLeagueModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<League>()
                .Property(league => league.Name)
                .HasMaxLength(128)
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
                .HasMaxLength(128)
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
                .HasMaxLength(128)
                .IsRequired();
        }

        private void RegisterProfileModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Profile>()
                .Property(profile => profile.UserId)
                .HasMaxLength(128);

            modelBuilder.Entity<Profile>()
                .Property(profile => profile.FirstName)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(profile => profile.LastName)
                .HasMaxLength(128)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(profile => profile.Email)
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute { IsUnique = true })
                );

        }

        private void RegisterPlayerModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Player>().ToTable("Players");
        }

        private void RegisterManagerModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Manager>().ToTable("Managers");
        }

        private void RegisterIdentityModel(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}