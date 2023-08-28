using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class InstrumentService : IInstrumentService
    {
        private readonly MusicianDbContext _context;

        public InstrumentService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateInstrument(InstrumentCUDto instrumentDto)
        {
            var instrument = new Instrument
            {
                Name = instrumentDto.Name,
            };

            _context.Instruments.Add(instrument);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteInstrument(int id)
        {
            var instrument = await _context.Instruments.FindAsync(id);

            if (instrument == null)
                return false;

            _context.Instruments.Remove(instrument);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Instrument> GetById(int id)
        {
            var instrument = await _context.Instruments.SingleOrDefaultAsync(a => a.Id == id);

            return instrument;
        }

        // Couldn't implement to controller, Swagger gives API error? -- Fixed!
        public async Task<List<ArtistRoleDto>> GetInstrumentalists(int instrumentId)
        {
            var instrumentalists = await _context.RoleInstruments
                .Where(ri => ri.InstrumentId == instrumentId)
                .Include(ri => ri.Role.Artist)
                .Select(ri => new ArtistRoleDto
                {
                    ArtistId = ri.Role.Artist.Id,
                    FirstName = ri.Role.Artist.FirstName,
                    LastName = ri.Role.Artist.LastName,
                    ArtistGenre = ri.Role.Artist.Genre,
                    BandId = ri.Role.BandId,
                    BandName = ri.Role.Band.Name,
                    Instrument = ri.Instrument.Name
                })
                .ToListAsync();

            return instrumentalists;
        }



        public async Task<List<Instrument>> GetList()
        {
            var instruments = await _context.Instruments.ToListAsync();

            return instruments;
        }

        public async Task<bool> UpdateInstrument(int id, InstrumentCUDto instrumentUpdateDto)
        {
            var instrument = await _context.Instruments.SingleOrDefaultAsync(a => a.Id == id);

            if (instrument == null)
                return false;

            // Update properties from the DTO
            instrument.Name = instrumentUpdateDto.Name;

            _context.Entry(instrument).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
