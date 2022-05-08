using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class Template : BaseModel
    {
        [Required, MaxLength(64)]
        public string Name { get; set; } = null!;
        [Required(AllowEmptyStrings = true), MaxLength(512)]
        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        public string Content { get; set; } = null!;

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public virtual ICollection<Field>? Fields { get; set; }
    }
}
