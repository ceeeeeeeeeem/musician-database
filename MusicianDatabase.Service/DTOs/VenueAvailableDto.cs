using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class VenueAvailableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }
        public string Address { get; set; }

    }
}
