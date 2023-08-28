using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class BandAvailableDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }
    }
}
