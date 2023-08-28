using AutoMapper;
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
        private readonly IMapper _mapper;

        public VenueService(MusicianDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateVenue(VenueCUDto venueDto)
        {
            //var venue = new Venue
            //{
            //    Name = venueDto.Name,
            //    Genre = venueDto.Genre,
            //    Address = venueDto.Address,
            //    Description = venueDto.Description
            //};
            var venue = _mapper.Map<Venue>(venueDto);
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

        public async Task<List<VenueAvailableDto>> GetAvailable(DateTime date)
        {
            var venues = await _context.Venues
                .Where(v => !v.Concerts.Any(c => c.Date == date)) // Check for concerts on the specified date
                .ToListAsync();
            var availableVenues = _mapper.Map<List<VenueAvailableDto>>(venues);

            return availableVenues;
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

        public async Task<bool> UpdateVenue(int id, VenueCUDto venueUpdateDto)
        {
            var venue = await _context.Venues.SingleOrDefaultAsync(a => a.Id == id);

            if (venue == null)
                return false;

            //// Update properties from the DTO
            //venue.Name = venueUpdateDto.Name;
            //venue.Genre = venueUpdateDto.Genre;
            //venue.Address = venueUpdateDto.Address;
            //venue.Description = venueUpdateDto.Description;

            _mapper.Map(venueUpdateDto, venue);

            _context.Entry(venue).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
