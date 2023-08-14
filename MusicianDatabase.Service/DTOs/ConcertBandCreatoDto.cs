using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ConcertBandCreateDto
    {
        [Required]
        public int ConcertId { get; set; }
        [Required]
        public int BandId { get; set; }
    }
}
