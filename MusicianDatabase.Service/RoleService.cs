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

        public RoleService(MusicianDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRole(RoleCreateDto roleDto)
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

        public async Task<bool> UpdateRole(int id, RoleUpdateDto roleUpdateDto)
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
