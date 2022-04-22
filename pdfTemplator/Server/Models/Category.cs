using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Server.Models
{
    public class Category : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required, MaxLength(512)]
        public string Description { get; set; } = null!;
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
        public ICollection<Category> Children { get; set; } = null!;
        public ICollection<PdfTemplate> PdfTemplates { get; set; } = null!;

        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
