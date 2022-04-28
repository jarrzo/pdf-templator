using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class PdfConversionDto : BaseModelDto
    {
        [Required]
        public string DataJSON { get; set; } = null!;
        [Required, MaxLength(128)]
        public string PdfPath { get; set; } = null!;
    }
}
