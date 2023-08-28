using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using MusicianDatabase.Service.Interfaces;

namespace MusicianDatabase.Service
{
    public class RoleService : IRoleService
    {
        private readonly MusicianDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(MusicianDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateRole(RoleCUDto roleDto)
        {
            var role = new Role
            {
                ArtistId = roleDto.ArtistId,
                BandId = roleDto.BandId,
                Description = roleDto.Description
            };

            _context.Roles.Add(role);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return false;

            _context.Roles.Remove(role);

            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Role> GetById(int id)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(a => a.Id == id);

            return role;
        }

        public async Task<List<Role>> GetList()
        {
            var roles = await _context.Roles.ToListAsync();

            return roles;
        }

        public async Task<List<RoleDto>> GetDetailedList()
        {
            var roles = await _context.Roles.Include(r => r.Artist).Include(r => r.Band)
                .Include(r => r.RoleInstruments).ThenInclude(ri => ri.Instrument).ToListAsync();
            var detailedRoles = _mapper.Map<List<RoleDto>>(roles);

            return detailedRoles;
        }

        public async Task<bool> UpdateRole(int id, RoleCUDto roleUpdateDto)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(a => a.Id == id);

            if (role == null)
                return false;

            // Update properties from the DTO
            role.ArtistId = roleUpdateDto.ArtistId;
            role.BandId = roleUpdateDto.BandId;
            role.Description = roleUpdateDto.Description;

            _context.Entry(role).State = EntityState.Modified;

            // A try-catch block necessary?
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
