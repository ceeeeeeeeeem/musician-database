using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class RoleCreateDto
    {
        [Required]
        public int ArtistId { get; set; }

        [Required]
        public int BandId { get; set; }

        public string Description { get; set; }
    }
}
