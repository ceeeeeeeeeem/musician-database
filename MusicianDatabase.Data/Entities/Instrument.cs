namespace MusicianDatabase.Data.Entities
{
    public class Instrument
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RoleInstruments> RoleInstruments { get; set; }
    }
}
