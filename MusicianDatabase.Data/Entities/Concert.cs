namespace MusicianDatabase.Data.Entities
{
    public class Concert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; }

        public ICollection<ConcertBand> ConcertBands { get; set; }


    }
}
