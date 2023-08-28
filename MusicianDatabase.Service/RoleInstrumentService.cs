using AutoMapper;
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
        private readonly IMapper _mapper;

        public RoleInstrumentService(MusicianDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> CreateRoleInstrument(RoleInstrumentCUDto roleInstrumentDto)
        {
            var roleInstrument = _mapper.Map<RoleInstruments>(roleInstrumentDto);

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

        public async Task<bool> UpdateRoleInstrument(int concertId, int bandId, RoleInstrumentCUDto roleInstrumentCUDto)
        {
            var roleInstrument = await _context.RoleInstruments.SingleOrDefaultAsync(ri => ri.RoleId == concertId && ri.InstrumentId == bandId);

            if (roleInstrument == null)
                return false;

            _mapper.Map(roleInstrumentCUDto, roleInstrument);
            _context.Entry(roleInstrument).State = EntityState.Modified;

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
