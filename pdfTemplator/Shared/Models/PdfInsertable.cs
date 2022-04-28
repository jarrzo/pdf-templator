using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class PdfInsertable : BaseModel
    {
        [Required]
        public int PdfTemplateId { get; set; }

        [Required, MaxLength(64)]
        public string Key { get; set; } = null!;
        public InsertableType Type { get; set; }
    }
}
