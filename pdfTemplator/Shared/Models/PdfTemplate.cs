using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class PdfTemplate : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required, MaxLength(512)]
        public string Description { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        public List<PdfInsertable> Insertables { get; set; } = new();
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
