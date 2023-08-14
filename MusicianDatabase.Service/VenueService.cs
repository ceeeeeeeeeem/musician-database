using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class VenueService : IVenueService
    {
        private readonly MusicianDbContext _context;

        public VenueService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateVenue(VenueCreateDto venueDto)
        {
            var venue = new Venue
            {
                Name = venueDto.Name,
                Genre = venueDto.Genre,
                Address = venueDto.Address,
                Description = venueDto.Description
            };

            _context.Venues.Add(venue);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteVenue(int id)
        {
            var venue = await _context.Venues.FindAsync(id);

            if (venue == null)
                return false;

            _context.Venues.Remove(venue);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<VenueAvailable>> GetAvailable(DateTime date)
        {
            var venues = await _context.Venues
                .Where(v => !v.Concerts.Any(c => c.Date == date)) // Check for concerts on the specified date
                .Select(venue => new VenueAvailable
                {
                    Id = venue.Id,
                    Name = venue.Name,
                    Genre = venue.Genre,
                    Address = venue.Address,
                    Description = venue.Description
                })
                .ToListAsync();

            return venues;
        }


        public async Task<Venue> GetById(int id)
        {
            var venue = await _context.Venues.SingleOrDefaultAsync(a => a.Id == id);

            return venue;
        }

        public async Task<List<Venue>> GetList()
        {
            var venues = await _context.Venues.ToListAsync();

            return venues;
        }

        public async Task<bool> UpdateVenue(int id, VenueUpdateDto venueUpdateDto)
        {
            var venue = await _context.Venues.SingleOrDefaultAsync(a => a.Id == id);

            if (venue == null)
                return false;

            // Update properties from the DTO
            venue.Name = venueUpdateDto.Name;
            venue.Genre = venueUpdateDto.Genre;
            venue.Address = venueUpdateDto.Address;
            venue.Description = venueUpdateDto.Description;

            _context.Entry(venue).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
