using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class RoleCUDto
    {
        public int ArtistId { get; set; }
        public int BandId { get; set; }
        public string Description { get; set; }
    }
}
