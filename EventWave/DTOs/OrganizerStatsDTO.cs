namespace EventWave.DTOs
{
    public class OrganizerStatsDTO
    {
        public string OrganizerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public int TotalEvents { get; set; }
        public int SoldOutEvents { get; set; }

        public List<EventSalesDTO> Events { get; set; }
    }
}
