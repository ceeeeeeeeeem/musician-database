using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IRoleInstrumentService
    {
        Task<bool> CreateRoleInstrument(RoleInstrumentCreateDto roleInstrumentDto);
        Task<bool> UpdateRoleInstrument(int roleId, int instrumentId, RoleInstrumentUpdateDto roleInstrumentUpdateDto);
        Task<bool> DeleteRoleInstrument(int roleId, int instrumentId);
        Task<List<RoleInstruments>> GetList();
        Task<RoleInstruments> GetById(int roleId, int instrumentId);
    }
}
