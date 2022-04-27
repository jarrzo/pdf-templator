using pdfTemplator.Shared.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Server.Models
{
    public class PdfInsertable : BaseModel
    {
        public int PdfTemplateId { get; set; }
        public PdfTemplate PdfTemplate { get; set; }

        [Required, MaxLength(64)]
        public string Key { get; set; } = null!;
        public InsertableType Type { get; set; }
    }
}
