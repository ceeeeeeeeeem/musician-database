using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IConcertBandService
    {
        Task<bool> CreateConcertBand(ConcertBandCreateDto concertBandDto);
        Task<bool> UpdateConcertBand(int concertId, int bandId, ConcertBandUpdateDto concertBandUpdateDto);
        Task<bool> DeleteConcertBand(int concertId, int bandId);
        Task<List<ConcertBand>> GetList();
        Task<ConcertBand> GetById(int concertId, int bandId);
    }
}
