using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IArtistService
    {
        Task<bool> CreateArtist(ArtistCUDto artistDto);
        Task<bool> UpdateArtist(int id, ArtistCUDto artistUpdateDto);
        Task<bool> DeleteArtist(int id);
        Task<List<Artist>> GetList();
        Task<Artist> GetById(int id);
        Task<List<ArtistRoleDto>> GetRolesById(int id);
        //Task<List<ArtistRoleDto>> GetRolesOfArtistByInstrument(int id, int instrumentId);
        Task<List<Artist>> GetArtistsWithoutBands();

    }
}
