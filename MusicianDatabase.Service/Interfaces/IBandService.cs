using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IBandService
    {
        Task<bool> CreateBand(BandCUDto bandDto);
        Task<bool> UpdateBand(int id, BandCUDto bandUpdateDto);
        Task<bool> DeleteBand(int id);
        Task<List<Band>> GetList();
        Task<Band> GetById(int id);
        Task<List<BandAvailableDto>> GetAvailable(string? genre, DateTime date);
        Task<List<ArtistRoleDto>> GetMembers(int bandId);
        Task<List<RoleInBandDto>> GetRoles(int bandId);
        Task<List<BandConcertsDto>> GetBandConcertsBetweenDates(int bandId, DateTime startDate, DateTime endDate); 
        Task<bool> AddMember(int artistId, int bandId, int instrumentId, string? roleDescription = null);
        Task<bool> RemoveMember(int artistId, int bankId);
    }
}
