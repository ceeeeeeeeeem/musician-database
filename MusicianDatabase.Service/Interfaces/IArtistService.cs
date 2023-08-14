using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IArtistService
    {
        Task<bool> CreateArtist(ArtistCreateDto artistDto);
        Task<bool> UpdateArtist(int id, ArtistUpdateDto artistUpdateDto);
        Task<bool> DeleteArtist(int id);
        Task<List<Artist>> GetList();
        Task<Artist> GetById(int id);
        Task<List<ArtistRole>> GetRolesById(int id);
        Task<List<ArtistRole>> GetRolesOfArtistByInstrument(int id, int instrumentId);


    }
}
