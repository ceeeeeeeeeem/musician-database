using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IRoleInstrumentService
    {
        Task<bool> CreateRoleInstrument(RoleInstrumentCUDto roleInstrumentDto);
        Task<bool> UpdateRoleInstrument(int roleId, int instrumentId);
        Task<bool> DeleteRoleInstrument(int roleId);
        Task<List<RoleInstruments>> GetList();
        Task<RoleInstruments> GetById(int roleId);
    }
}
