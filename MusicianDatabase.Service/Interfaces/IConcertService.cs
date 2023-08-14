using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IConcertService
    {
        Task<bool> CreateConcert(ConcertCreateDto concertDto);
        Task<bool> UpdateConcert(int id, ConcertUpdateDto concertUpdateDto);
        Task<bool> DeleteConcert(int id);
        Task<List<Concert>> GetList();
        Task<Concert> GetById(int id);
    }
}
