using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostHubAPI.Models
{
    public class User : IdentityUser
    {
        public virtual List<Hub>? Hubs { get; set; }

        [InverseProperty("User")]
        public virtual List<Comment>? Comments { get; set; }

        [InverseProperty("Upvoters")]
        public virtual List<Comment>? Upvotes { get; set; }

        [InverseProperty("Downvoters")]
        public virtual List<Comment>? Downvotes { get; set; }

        // Avatar
        public string? FileName { get; set; }
        public string? MimeType { get; set; }
    }
}
