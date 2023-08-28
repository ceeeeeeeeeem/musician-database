using System.ComponentModel.DataAnnotations;

namespace MusicianDatabase.Service.DTOs
{
    public class ConcertCountDto
    {
        public int ConcertCount { get; set; }
        public string Band { get; set; }
    }
}
