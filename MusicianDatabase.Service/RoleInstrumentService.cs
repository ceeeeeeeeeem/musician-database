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

        public async Task<bool> DeleteRoleInstrument(int roleId)
        {
            var roleInstrument = await _context.RoleInstruments.FindAsync(roleId);

            if (roleInstrument == null)
                return false;

            _context.RoleInstruments.Remove(roleInstrument);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<RoleInstruments> GetById(int roleId)
        {
            var roleInstrument = await _context.RoleInstruments.SingleOrDefaultAsync(ri => ri.RoleId == roleId);

            return roleInstrument;
        }

        public async Task<List<RoleInstruments>> GetList()
        {
            var roleInstruments = await _context.RoleInstruments.ToListAsync();

            return roleInstruments;
        }

        public async Task<bool> UpdateRoleInstrument(int roleId, int instrumentId)
        {
            var roleInstrument = await _context.RoleInstruments.SingleOrDefaultAsync(ri => ri.RoleId == roleId);

            if (roleInstrument == null)
                return false;

            // Delete the existing entity with the old key values
            _context.RoleInstruments.Remove(roleInstrument);

            // Create a new entity with the updated key values
            var newRoleInstrument = new RoleInstruments
            {
                // Set the new key values
                RoleId = roleId,
                InstrumentId = instrumentId,
                // Map other properties from the DTO as needed
            };

            // Add the new entity to the context
            _context.RoleInstruments.Add(newRoleInstrument);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

    }
}
