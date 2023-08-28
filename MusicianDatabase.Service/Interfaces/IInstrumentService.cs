using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IInstrumentService
    {
        Task<bool> CreateInstrument(InstrumentCUDto instrumentDto);
        Task<bool> UpdateInstrument(int id, InstrumentCUDto instrumentUpdateDto);
        Task<bool> DeleteInstrument(int id);
        Task<List<Instrument>> GetList();
        Task<Instrument> GetById(int id);
        Task<List<ArtistRoleDto>> GetInstrumentalists(int instrumentId);
    }
}
