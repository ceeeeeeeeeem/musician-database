using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class InstrumentCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
