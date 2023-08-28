using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ArtistRoleDto
    {
        public int ArtistId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ArtistGenre { get; set; }
        public int BandId { get; set; }
        public string BandName { get; set; }
        public string? BandGenre { get; set; }
        public string Instrument { get; set; }
    }
}
