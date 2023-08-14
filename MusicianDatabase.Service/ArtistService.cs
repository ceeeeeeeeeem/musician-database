using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class ArtistService : IArtistService
    {
        private readonly MusicianDbContext _context;

        public ArtistService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateArtist(ArtistCreateDto artistDto)
        {
            var artist = new Artist
            {
                FirstName = artistDto.FirstName,
                LastName = artistDto.LastName,
                Genre = artistDto.Genre,
                Description = artistDto.Description
            };

            _context.Artists.Add(artist);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteArtist(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null)
                return false;

            _context.Artists.Remove(artist);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Artist> GetById(int id)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.Id == id);

            return artist;
        }

        public async Task<List<Artist>> GetList()
        {
            var artists = await _context.Artists.ToListAsync();

            return artists;
        }

        public async Task<List<ArtistRole>> GetRolesById(int id)
        {
            var artistRoles = await _context.Artists
                .Where(a => a.Id == id)
                .SelectMany(a => a.Roles.Select(r => new ArtistRole
                {
                    ArtistId = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    BandId = r.BandId,
                    BandName = r.Band.Name, // Assuming Band has a Name property
                    BandGenre = r.Band.Genre, // Assuming Band has a Genre property
                    Instrument = r.RoleInstruments.Single().Instrument.Name // Get the instrument's name directly
                }))
                .ToListAsync();

            var groupedArtistRoles = artistRoles
                .GroupBy(ar => new { ar.ArtistId, ar.BandId })
                .Select(group => new ArtistRole
                {
                    ArtistId = group.Key.ArtistId,
                    FirstName = group.First().FirstName,
                    LastName = group.First().LastName,
                    BandId = group.Key.BandId,
                    BandName = group.First().BandName,
                    BandGenre = group.First().BandGenre,
                    Instrument = string.Join(", ", group.Select(ar => ar.Instrument)) // Combine instruments
                })
                .ToList();

            return groupedArtistRoles;
        }



        public Task<List<ArtistRole>> GetRolesOfArtistByInstrument(int id, int instrumentId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateArtist(int id, ArtistUpdateDto artistUpdateDto)
        {
            var artist = await _context.Artists.SingleOrDefaultAsync(a => a.Id == id);

            if (artist == null)
                return false;

            // Update properties from the DTO
            artist.FirstName = artistUpdateDto.FirstName;
            artist.LastName = artistUpdateDto.LastName;
            artist.Genre = artistUpdateDto.Genre;
            artist.Description = artistUpdateDto.Description;

            _context.Entry(artist).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
