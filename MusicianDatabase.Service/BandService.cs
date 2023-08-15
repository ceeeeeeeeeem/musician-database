using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class BandService : IBandService
    {
        private readonly MusicianDbContext _context;

        public BandService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddMember(int artistId, int bandId, int instrumentId, string? roleDescription = null)
        {
            var artist = await _context.Artists.FindAsync(artistId);
            var band = await _context.Artists.FindAsync(bandId);
            var instrument = await _context.Instruments.FindAsync(instrumentId);
            var newRole = new Role
            {
                ArtistId = artistId,
                BandId = bandId,
                Description = roleDescription,
            };
            _context.Roles.Add(newRole);
            int result1 = await _context.SaveChangesAsync();

            var newRoleInstrument = new RoleInstruments
            {
                RoleId = newRole.Id,
                InstrumentId = instrumentId
            };
            _context.RoleInstruments.Add(newRoleInstrument);
            int result2 = await _context.SaveChangesAsync();
            bool results = result1 > 0 && result2 > 0;
            return results;
        }

        public async Task<bool> CreateBand(BandCreateDto bandDto)
        {
            var band = new Band
            {
                Name = bandDto.Name,
                Genre = bandDto.Genre,
                Description = bandDto.Description
            };

            _context.Bands.Add(band);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteBand(int id)
        {
            var band = await _context.Bands.FindAsync(id);

            if (band == null)
                return false;

            _context.Bands.Remove(band);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<BandAvailable>> GetAvailable(string genre, DateTime date)
        {
            // önceki tarihlere istek atılmasını engellemek istersek burada yaparız.

            var bands = await _context.Bands.Include(b => b.ConcertBands).ThenInclude(cb => cb.Concert)
                .Where(b => b.Genre.Contains(genre) && !b.ConcertBands.Any(cb => cb.Concert.Date == date))
                .Select(b => new BandAvailable
                {
                    Id = b.Id,
                    Name = b.Name,
                    Genre = b.Genre,
                    Description = b.Description
                })
                .ToListAsync();

            return bands;
        }
        // TODO: Use DTOs
        public async Task<Band> GetById(int id)
        {
            var band = await _context.Bands.SingleOrDefaultAsync(a => a.Id == id);

            return band;
        }

        public async Task<List<Band>> GetList()
        {
            var bands = await _context.Bands.ToListAsync();

            return bands;
        }

        public async Task<List<ArtistRole>> GetMembers(int bandId)
        {
            var bandMembers = await _context.Bands
                .Where(b => b.Id == bandId)
                .SelectMany(b => b.Roles.Select(r => new
                {
                    BandId = b.Id,
                    BandName = b.Name,
                    ArtistId = r.Artist.Id,
                    FirstName = r.Artist.FirstName,
                    LastName = r.Artist.LastName,
                    ArtistGenre = r.Artist.Genre, // Added property
                    Instrument = r.RoleInstruments.Single().Instrument.Name
                }))
                .GroupBy(a => new { a.ArtistId, a.FirstName, a.LastName, a.ArtistGenre })
                .Select(g => new ArtistRole
                {
                    BandId = g.First().BandId,
                    BandName = g.First().BandName,
                    ArtistId = g.Key.ArtistId,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    ArtistGenre = g.Key.ArtistGenre,
                    Instrument = string.Join(", ", g.Select(a => a.Instrument))
                })
                .ToListAsync();

            return bandMembers;
        }

        public async Task<bool> RemoveMember(int artistId, int bandId)
        {
            var rolesToRemove = await _context.Roles
                .Include(r => r.RoleInstruments)
                .Where(r => r.ArtistId == artistId && r.BandId == bandId)
                .ToListAsync();

            if (!rolesToRemove.Any())
                return false; // Member not found in the specified band

            foreach (var role in rolesToRemove)
            {
                foreach (var roleInstrument in role.RoleInstruments.ToList())
                {
                    _context.RoleInstruments.Remove(roleInstrument);
                }

                _context.Roles.Remove(role);
            }

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }



        public async Task<bool> UpdateBand(int id, BandUpdateDto bandUpdateDto)
        {
            var band = await _context.Bands.SingleOrDefaultAsync(a => a.Id == id);

            if (band == null)
                return false;

            // Update properties from the DTO
            band.Name = bandUpdateDto.Name;
            band.Genre = bandUpdateDto.Genre;
            band.Description = bandUpdateDto.Description;

            _context.Entry(band).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }


    }
}
