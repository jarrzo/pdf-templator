using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class PdfInsertableDto : BaseModelDto
    {
        [Required]
        public string Key { get; set; } = null!;
        public InsertableType Type { get; set; }
    }
}
