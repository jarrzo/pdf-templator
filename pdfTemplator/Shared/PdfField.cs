using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared
{
    public class PdfField
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required, MaxLength(64)]
        public string Path { get; set; } = null!;
        [Required]
        public PdfFieldTypes Type { get; set; }
    }
}
