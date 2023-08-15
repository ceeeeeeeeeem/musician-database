using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class RoleInstrumentService : IRoleInstrumentService
    {
        private readonly MusicianDbContext _context;

        public RoleInstrumentService(MusicianDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateRoleInstrument(RoleInstrumentCreateDto roleInstrumentDto)
        {
            var roleInstrument = new RoleInstruments
            {
                RoleId = roleInstrumentDto.RoleId,
                InstrumentId = roleInstrumentDto.InstrumentId
            };

            _context.RoleInstruments.Add(roleInstrument);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteRoleInstrument(int concertId, int bandId)
        {
            var roleInstrument = await _context.RoleInstruments.FindAsync(concertId, bandId);

            if (roleInstrument == null)
                return false;

            _context.RoleInstruments.Remove(roleInstrument);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<RoleInstruments> GetById(int concertId, int bandId)
        {
            var roleInstrument = await _context.RoleInstruments.SingleOrDefaultAsync(ri => ri.RoleId == concertId && ri.InstrumentId == bandId);

            return roleInstrument;
        }

        public async Task<List<RoleInstruments>> GetList()
        {
            var roleInstruments = await _context.RoleInstruments.ToListAsync();

            return roleInstruments;
        }

        public async Task<bool> UpdateRoleInstrument(int concertId, int bandId, RoleInstrumentUpdateDto roleInstrumentUpdateDto)
        {
            var roleInstrument = await _context.RoleInstruments.SingleOrDefaultAsync(ri => ri.RoleId == concertId && ri.InstrumentId == bandId);

            if (roleInstrument == null)
                return false;

            // Update properties from the DTO
            roleInstrument.RoleId = roleInstrumentUpdateDto.RoleId;
            roleInstrument.InstrumentId = roleInstrumentUpdateDto.InstrumentId;

            _context.Entry(roleInstrument).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
