using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IConcertService
    {
        Task<bool> CreateConcert(ConcertCUDto concertDto);
        Task<bool> QuickCreateConcert(ConcertQuickCreateDto concertQCDto);
        Task<bool> UpdateConcert(int id, ConcertCUDto concertUpdateDto);
        Task<bool> DeleteConcert(int id);
        Task<List<Concert>> GetList();
        Task<List<ConcertCountDto>> GetConcertCountsBetweenDates(DateTime startDate, DateTime endDate);
        Task<Concert> GetById(int id);
    }
}
