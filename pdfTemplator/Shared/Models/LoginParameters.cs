using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class LoginParameters
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
