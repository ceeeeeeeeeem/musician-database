using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IBandService
    {
        Task<bool> CreateBand(BandCreateDto bandDto);
        Task<bool> UpdateBand(int id, BandUpdateDto bandUpdateDto);
        Task<bool> DeleteBand(int id);
        Task<List<Band>> GetList();
        Task<Band> GetById(int id);
        Task<List<BandAvailable>> GetAvailable(string genre, DateTime date);
        Task<List<ArtistRole>> GetMembers(int bandId);
        Task<bool> AddMember(int artistId, int bandId, int instrumentId, string? roleDescription = null);
    }
}
