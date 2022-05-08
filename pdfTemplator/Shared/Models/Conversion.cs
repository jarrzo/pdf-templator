using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class Conversion : BaseModel
    {
        [Required]
        public int TemplateId { get; set; }
        [Required]
        public string DataJSON { get; set; } = null!;
        [Required, MaxLength(128)]
        public string PdfPath { get; set; } = null!;

        public virtual Template? Template { get; set; }
    }
}
