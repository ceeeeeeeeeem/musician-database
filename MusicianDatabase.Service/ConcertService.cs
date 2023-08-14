using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class ConcertService : IConcertService
    {
        private readonly MusicianDbContext _context;

        public ConcertService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateConcert(ConcertCreateDto concertDto)
        {
            var concert = new Concert
            {
                Name = concertDto.Name,
                Date = concertDto.Date,
                VenueId = concertDto.VenueId,
                Description = concertDto.Description
            };

            _context.Concerts.Add(concert);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteConcert(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);

            if (concert == null)
                return false;

            _context.Concerts.Remove(concert);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Concert> GetById(int id)
        {
            var concert = await _context.Concerts.SingleOrDefaultAsync(a => a.Id == id);

            return concert;
        }

        public async Task<List<Concert>> GetList()
        {
            var concerts = await _context.Concerts.ToListAsync();

            return concerts;
        }

        public async Task<bool> UpdateConcert(int id, ConcertUpdateDto concertUpdateDto)
        {
            var concert = await _context.Concerts.SingleOrDefaultAsync(a => a.Id == id);

            if (concert == null)
                return false;

            // Update properties from the DTO
            concert.Name = concertUpdateDto.Name;
            concert.Date = concertUpdateDto.Date;
            concert.VenueId = concertUpdateDto.VenueId;
            concert.Description = concertUpdateDto.Description;

            _context.Entry(concert).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
