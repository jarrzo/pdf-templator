using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class PdfTemplate : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
    }
}
