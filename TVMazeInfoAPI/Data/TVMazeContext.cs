using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TVMazeInfoAPI.Domain.Entities;

namespace TVMazeInfoAPI.Data
{
    public class TVMazeContext : DbContext
    {
        public TVMazeContext(DbContextOptions<TVMazeContext> options)
            : base(options)
        {
        }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Show>()
                .HasMany(s => s.Episodes)
                .WithOne(e => e.Show)
                .HasForeignKey(e => e.ShowId);

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Network)
                .WithOne()
                .HasForeignKey<Show>(s => s.NetworkId);

            modelBuilder.Entity<Cast>()
                .HasOne(c => c.Person)
                .WithMany()
                .HasForeignKey(c => c.PersonId);

            modelBuilder.Entity<Cast>()
                .HasOne(c => c.Character)
                .WithMany()
                .HasForeignKey(c => c.CharacterId);

            modelBuilder.Entity<Crew>()
                .HasOne(c => c.Person)
                .WithMany()
                .HasForeignKey(c => c.PersonId);

            modelBuilder.Entity<Network>()
                .HasOne(n => n.Country)
                .WithOne()
                .HasForeignKey<Network>(n => n.CountryCode);

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Rating)
                .WithOne(r => r.Show)
                .HasForeignKey<Rating>(r => r.ShowId);

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Externals)
                .WithOne(e => e.Show)
                .HasForeignKey<Externals>(e => e.ShowId);

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Image)
                .WithOne(i => i.Show)
                .HasForeignKey<Image>(i => i.ShowId);

            modelBuilder.Entity<Show>()
                .HasOne(s => s._links)
                .WithOne(l => l.Show)
                .HasForeignKey<Links>(l => l.ShowId);

            modelBuilder.Entity<Show>()
                .HasMany(s => s.Cast)
                .WithOne(c => c.Show)
                .HasForeignKey(c => c.ShowId);

            modelBuilder.Entity<Show>()
                .HasMany(s => s.Crew)
                .WithOne(cr => cr.Show)
                .HasForeignKey(cr => cr.ShowId);
        }
    }
}
