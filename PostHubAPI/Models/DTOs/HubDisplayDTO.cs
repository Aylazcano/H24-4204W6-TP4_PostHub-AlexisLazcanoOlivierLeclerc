namespace PostHubAPI.Models.DTOs
{
    public class HubDisplayDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool? IsJoined { get; set; }
    }
}
