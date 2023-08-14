using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class RoleInstrumentCreateDto
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int InstrumentId { get; set; }
    }
}
