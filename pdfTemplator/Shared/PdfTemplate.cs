using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared
{
    public class PdfTemplate
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        public List<PdfField> Insertables { get; set; } = new();
    }
}
