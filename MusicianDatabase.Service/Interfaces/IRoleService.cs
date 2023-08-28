using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateRole(RoleCUDto roleDto);
        Task<bool> UpdateRole(int id, RoleCUDto roleUpdateDto);
        Task<bool> DeleteRole(int id);
        Task<List<Role>> GetList();
        Task<List<RoleDto>> GetDetailedList();
        Task<Role> GetById(int id);
    }
}
