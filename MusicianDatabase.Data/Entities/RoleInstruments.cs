namespace MusicianDatabase.Data.Entities
{
    public class RoleInstruments
    {
        // Foreign Key to Role Table
        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Foreign Key to Instrument Table
        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
    }
}
