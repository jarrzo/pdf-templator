using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class RegisterParameters
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
