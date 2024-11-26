using Microsoft.EntityFrameworkCore;
using TrainForFootball.MVC.Models;

namespace TrainForFootball.MVC.Data
{
    public class TrainForFootballDbContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamStats> TeamStats { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchResult> MatchResults { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<User> Users { get; set; }

        public TrainForFootballDbContext(DbContextOptions<TrainForFootballDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=Data/TrainForFootball.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relazione tra Match e Team (HomeTeam e AwayTeam)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany() // Non è necessaria una proprietà specifica su Team
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany() // Non è necessaria una proprietà specifica su Team
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relazione tra Match e MatchResult (uno-a-uno)
            modelBuilder.Entity<Match>()
                .HasOne(m => m.MatchResult)
                .WithOne(mr => mr.Match)
                .HasForeignKey<MatchResult>(mr => mr.MatchId) // Definisce MatchId come chiave esterna
                .OnDelete(DeleteBehavior.Cascade); // Eliminazione in cascata per entrambi

            // Relazione tra Team e League
            modelBuilder.Entity<Team>()
                .HasOne(t => t.League)
                .WithMany(l => l.Teams)
                .HasForeignKey(t => t.LeagueId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relazione tra Team e TeamStats
            modelBuilder.Entity<Team>()
                .HasOne(t => t.TeamStats)
                .WithOne(ts => ts.Team)
                .HasForeignKey<TeamStats>(ts => ts.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
