using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ConcertBandCUDto
    {
        public int ConcertId { get; set; }
        public int BandId { get; set; }
    }
}
