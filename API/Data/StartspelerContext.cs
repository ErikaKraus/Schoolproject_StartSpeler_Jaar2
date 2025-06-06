using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Data
{
    //public class StartspelerContext : IdentityDbContext<Gebruiker>
    //Code voor kortere stringwaarde ID    
    public class StartspelerContext : IdentityDbContext<Gebruiker, IdentityRole<string>, string>
    {
        public StartspelerContext(DbContextOptions<StartspelerContext> options) : base(options) { }

        public DbSet<Artikel> Artikels { get; set; } = default!;
        public DbSet<BestellingLijn> Bestellijnen { get; set; } = default!;
        public DbSet<Bestelling> Bestellingen { get; set; } = default!;
        public DbSet<Gebruiker> Gebruikers { get; set; } = default!;
        public DbSet<Evenement> Evenementen { get; set; } = default!;
        public DbSet<Community> Communities { get; set; } = default!;
        public DbSet<EventGebruiker> EventGebruikers { get; set; } = default!;
        



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //code voor kortere stringwaarde ID
            modelBuilder.Entity<Gebruiker>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(8);
                entity.HasKey(e => e.Id);
            });

            //one to many
            modelBuilder.Entity<Evenement>()
                .HasOne(h => h.Community)
                .WithMany(i => i.Evenementen)
                .HasForeignKey(j => j.CommunityId)
                .IsRequired();

            //many to many
            modelBuilder.Entity<EventGebruiker>()
                .HasOne(eg => eg.Evenement)
                .WithMany(e => e.EventGebruikers)
                .HasForeignKey(eg => eg.EvenementId)
                .IsRequired();

            modelBuilder.Entity<EventGebruiker>()
                .HasOne(eg => eg.Gebruiker)
                .WithMany(e => e.EventGebruikers)
                .HasForeignKey(eg => eg.GebruikerId)
                .IsRequired();


            //// Many to many
            modelBuilder.Entity<BestellingLijn>()
                .HasOne(b => b.Bestelling)
                .WithMany(c => c.BestellingLijnen)
                .HasForeignKey(d => d.BestellingId)
                .IsRequired();

            modelBuilder.Entity<BestellingLijn>()
                .HasOne(e => e.Artikel)
                .WithMany(f => f.Bestellinglijnen)
                .HasForeignKey(g => g.ArtikelId)
                .IsRequired();
        }
    }
}
