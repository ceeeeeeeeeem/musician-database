using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateRole(RoleCreateDto roleDto);
        Task<bool> UpdateRole(int id, RoleUpdateDto roleUpdateDto);
        Task<bool> DeleteRole(int id);
        Task<List<Role>> GetList();
        Task<Role> GetById(int id);
    }
}
