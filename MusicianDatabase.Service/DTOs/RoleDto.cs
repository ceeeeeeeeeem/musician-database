using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{ // for get methods
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string ArtistFirstName { get; set; }
        public string ArtistLastName { get; set; }

        public string BandName { get; set; }
        public string Description { get; set; }
        public string Instrument { get; set; }
    }
}
