using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class ConcertBandService : IConcertBandService
    {
        private readonly MusicianDbContext _context;

        public ConcertBandService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateConcertBand(ConcertBandCreateDto concertBandDto)
        {
            var concertBand = new ConcertBand
            {
                ConcertId = concertBandDto.ConcertId,
                BandId = concertBandDto.BandId
            };

            _context.ConcertBands.Add(concertBand);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteConcertBand(int concertId, int bandId)
        {
            var concertBand = await _context.ConcertBands.FindAsync(concertId, bandId);

            if (concertBand == null)
                return false;

            _context.ConcertBands.Remove(concertBand);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<ConcertBand> GetById(int concertId, int bandId)
        {
            var concertBand = await _context.ConcertBands.SingleOrDefaultAsync(cb => cb.ConcertId == concertId && cb.BandId == bandId);

            return concertBand;
        }

        public async Task<List<ConcertBand>> GetList()
        {
            var concertBands = await _context.ConcertBands.ToListAsync();

            return concertBands;
        }

        public async Task<bool> UpdateConcertBand(int concertId, int bandId, ConcertBandUpdateDto concertBandUpdateDto)
        {
            var concertBand = await _context.ConcertBands.SingleOrDefaultAsync(cb => cb.ConcertId == concertId && cb.BandId == bandId);

            if (concertBand == null)
                return false;

            // Update properties from the DTO
            concertBand.ConcertId = concertBandUpdateDto.ConcertId;
            concertBand.BandId = concertBandUpdateDto.BandId;

            _context.Entry(concertBand).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
