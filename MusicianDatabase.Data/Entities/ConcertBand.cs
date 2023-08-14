namespace MusicianDatabase.Data.Entities
{
    public class ConcertBand
    {
        public int ConcertId { get; set; }
        public Concert Concert { get; set; }

        public int BandId { get; set; }
        public Band Band { get; set; }
    }
}
