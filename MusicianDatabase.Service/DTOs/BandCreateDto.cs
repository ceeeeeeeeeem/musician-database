using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class BandCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        public string Description { get; set; }
    }
}
