using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IConcertBandService
    {
        Task<bool> CreateConcertBand(ConcertBandCUDto concertBandDto);
        Task<bool> UpdateConcertBand(int concertId, int bandId, ConcertBandCUDto concertBandCUDto);
        Task<bool> DeleteConcertBand(int concertId, int bandId);
        Task<List<ConcertBand>> GetList();
        Task<ConcertBand> GetById(int concertId, int bandId);
    }
}
