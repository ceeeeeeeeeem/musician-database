using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{ // for get methods
    public class RoleInBandDto
    {
        public string BandName { get; set; }
        public string ArtistFirstName { get; set; }
        public string ArtistLastName { get; set; }
        public List<string> Instrument { get; set; }
    }
}
