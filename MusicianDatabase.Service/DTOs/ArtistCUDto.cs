using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ArtistCUDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Genre { get; set; }

        public string Description { get; set; }
    }
}
