using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ConcertQuickCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public int BandId { get; set; }

        public string Description { get; set; }
    }
}
