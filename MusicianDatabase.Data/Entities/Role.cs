using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Data.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key to Artist Table
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        // Foreign Key to Band Table
        public int BandId { get; set; }
        public Band Band { get; set; }

        // Additional properties related to the role, e.g., RoleDescription
        public string? Description { get; set; }

        // Collection of RoleInstruments associated with the role
        public ICollection<RoleInstruments> RoleInstruments { get; set; }
    }
}
