using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;

namespace MusicianDatabase.Service.Interfaces
{
    public interface IVenueService
    {
        Task<bool> CreateVenue(VenueCreateDto venueDto);
        Task<bool> UpdateVenue(int id, VenueUpdateDto venueUpdateDto);
        Task<bool> DeleteVenue(int id);
        Task<List<Venue>> GetList();
        Task<Venue> GetById(int id);
        Task<List<VenueAvailable>> GetAvailable(DateTime date);
    }
}
