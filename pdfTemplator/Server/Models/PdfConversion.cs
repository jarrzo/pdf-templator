using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Server.Models
{
    public class PdfConversion : BaseModel
    {
        public int PdfTemplateId { get; set; }
        [Required]
        public PdfTemplate PdfTemplate { get; set; } = null!;
        [Required]
        public string DataJSON { get; set; } = null!;
        [Required, MaxLength(128)]
        public string PdfPath { get; set; } = null!;
    }
}
