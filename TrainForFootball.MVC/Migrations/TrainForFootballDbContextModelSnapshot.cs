﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TrainForFootball.MVC.Data;

#nullable disable

namespace TrainForFootball.MVC.Migrations
{
    [DbContext(typeof(TrainForFootballDbContext))]
    partial class TrainForFootballDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("TrainForFootball.MVC.Models.League", b =>
                {
                    b.Property<int>("LeagueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LeagueId");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.Match", b =>
                {
                    b.Property<int>("MatchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AwayTeamId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HomeTeamId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("MatchDayNum")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MatchResultId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MatchId");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.MatchResult", b =>
                {
                    b.Property<int>("MatchResultId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AwayTeamScore")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HomeTeamScore")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MatchId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MatchResultId");

                    b.HasIndex("MatchId")
                        .IsUnique();

                    b.ToTable("MatchResults");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ColorOfClub")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FondationYear")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LeagueId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SquadName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StadiumName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.TeamStats", b =>
                {
                    b.Property<int>("TeamStatsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GoalsConceded")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GoalsScored")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MatchesDrawn")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MatchesLost")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MatchesWin")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeasonPoint")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TeamStatsId");

                    b.HasIndex("TeamId")
                        .IsUnique();

                    b.ToTable("TeamStats");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.Match", b =>
                {
                    b.HasOne("TrainForFootball.MVC.Models.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TrainForFootball.MVC.Models.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.MatchResult", b =>
                {
                    b.HasOne("TrainForFootball.MVC.Models.Match", "Match")
                        .WithOne("MatchResult")
                        .HasForeignKey("TrainForFootball.MVC.Models.MatchResult", "MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.Team", b =>
                {
                    b.HasOne("TrainForFootball.MVC.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("League");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.TeamStats", b =>
                {
                    b.HasOne("TrainForFootball.MVC.Models.Team", "Team")
                        .WithOne("TeamStats")
                        .HasForeignKey("TrainForFootball.MVC.Models.TeamStats", "TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Team");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.League", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.Match", b =>
                {
                    b.Navigation("MatchResult");
                });

            modelBuilder.Entity("TrainForFootball.MVC.Models.Team", b =>
                {
                    b.Navigation("TeamStats");
                });
#pragma warning restore 612, 618
        }
    }
}
