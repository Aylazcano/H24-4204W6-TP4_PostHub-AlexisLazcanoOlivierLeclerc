using System.Text.Json.Serialization;

namespace PostHubAPI.Models
{
    public class Hub
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public virtual List<Post>? Posts { get; set; }

        [JsonIgnore]
        public virtual List<User>? Users { get; set; }
    }
}
