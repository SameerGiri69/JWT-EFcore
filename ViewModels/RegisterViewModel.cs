using System.ComponentModel.DataAnnotations;

namespace JWT_EF_Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        
        public string? Password { get; set; }
        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? UserName { get; set; }
    }
}
