using System.ComponentModel.DataAnnotations;

namespace PostHubAPI.Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
