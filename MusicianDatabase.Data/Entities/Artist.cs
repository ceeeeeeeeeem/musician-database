namespace MusicianDatabase.Data.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Genre { get; set; }
        public string? Description { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}
