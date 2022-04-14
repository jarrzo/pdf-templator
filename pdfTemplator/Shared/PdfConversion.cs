using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared
{
    public class PdfConversion
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public PdfTemplate PdfTemplate { get; set; } = null!;
        [Required]
        public string DataJSON { get; set; } = null!;
        [Required, MaxLength(128)]
        public string PdfPath { get; set; } = null!;
    }
}
