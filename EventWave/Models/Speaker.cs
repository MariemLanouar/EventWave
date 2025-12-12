namespace EventWave.Models
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Expertise { get; set; }
        public string Contact { get; set; }
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; } = false;
      

    }
}
