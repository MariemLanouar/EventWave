namespace EventWave.DTOs
{
    public class SpeakerStatsDTO
    {
        public int SpeakerId { get; set; }
        public string FullName { get; set; }

        public int TotalEvents { get; set; }
        public int SoldOutEvents { get; set; }

        public List<SpeakerEventStatsDTO> Events { get; set; } = new();
    }
}
