using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IInstrumentService
    {
        Task<bool> CreateInstrument(InstrumentCreateDto instrumentDto);
        Task<bool> UpdateInstrument(int id, InstrumentUpdateDto instrumentUpdateDto);
        Task<bool> DeleteInstrument(int id);
        Task<List<Instrument>> GetList();
        Task<Instrument> GetById(int id);
        Task<List<ArtistRole>> GetInstrumentalists(int instrumentId);
    }
}
