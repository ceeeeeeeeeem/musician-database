namespace MusicianDatabase.Data.Entities
{
    public class Band
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }



        // Collection of roles played by different artists in the band
        public ICollection<Role> Roles { get; set; }

        // Collection of lineups the band has been in concerts
        public ICollection<ConcertBand> ConcertBands { get; set; }

    }
}
