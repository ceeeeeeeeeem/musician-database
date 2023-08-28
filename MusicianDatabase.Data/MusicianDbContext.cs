using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicianDatabase.Data.Entities;

namespace MusicianDatabase.Data
{
    public class MusicianDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public MusicianDbContext(DbContextOptions<MusicianDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        // public DbQuery<ConcertCountDto> ConcertCountDtos { get; set; }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleInstruments> RoleInstruments { get; set; }
        public DbSet<Concert> Concerts { get; set; }
        public DbSet<ConcertBand> ConcertBands { get; set; }
        public DbSet<Venue> Venues { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Artist)
                .WithMany(a => a.Roles)
                .HasForeignKey(r => r.ArtistId);
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Band)
                .WithMany(b => b.Roles)
                .HasForeignKey(r => r.BandId);

            modelBuilder.Entity<RoleInstruments>()
                .HasKey(cb => new { cb.RoleId, cb.InstrumentId});
            modelBuilder.Entity<RoleInstruments>()
                .HasOne(ri => ri.Role)
                .WithMany(ai => ai.RoleInstruments)
                .HasForeignKey(r => r.RoleId);
            modelBuilder.Entity<RoleInstruments>()
                .HasOne(ri => ri.Instrument)
                .WithMany(ai => ai.RoleInstruments)
                .HasForeignKey(r => r.InstrumentId);

            modelBuilder.Entity<Concert>()
                .HasOne(c => c.Venue)
                .WithMany(v => v.Concerts)
                .HasForeignKey(c => c.VenueId);
            
            modelBuilder.Entity<ConcertBand>()
                .HasKey(cb => new { cb.ConcertId, cb.BandId });
            modelBuilder.Entity<ConcertBand>()
                .HasOne(cb => cb.Concert)
                .WithMany(c => c.ConcertBands)
                .HasForeignKey(cb => cb.ConcertId);
            modelBuilder.Entity<ConcertBand>()
                .HasOne(cb => cb.Band)
                .WithMany(b => b.ConcertBands)
                .HasForeignKey(cb => cb.BandId);

        }
    }
}
