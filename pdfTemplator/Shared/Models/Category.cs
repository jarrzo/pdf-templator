using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class Category : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
    }
}
