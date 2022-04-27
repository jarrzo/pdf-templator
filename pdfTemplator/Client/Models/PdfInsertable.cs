using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Client.Models
{
    public class PdfInsertable : BaseModel
    {
        [Required]
        public string Key { get; set; } = null!;
        public InsertableType Type { get; set; }
    }
}
