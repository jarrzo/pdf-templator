using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class CategoryParameters
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
    }
}
