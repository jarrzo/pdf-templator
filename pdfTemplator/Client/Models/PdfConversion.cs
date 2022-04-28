using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Client.Models
{
    public class PdfConversion : BaseModel
    {
        [Required]
        public string DataJSON { get; set; } = null!;
        [Required, MaxLength(128)]
        public string PdfPath { get; set; } = null!;
    }
}
