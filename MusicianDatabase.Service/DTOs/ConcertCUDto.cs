using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ConcertCUDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
        public string Description { get; set; }
    }
}
