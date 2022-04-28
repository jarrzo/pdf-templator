using System.ComponentModel.DataAnnotations;

namespace pdfTemplator.Shared.Models
{
    public class PdfKeyValue
    {
        [Required]
        public string Key { get; set; } = null!;
        [Required]
        public string Value { get; set; } = null!;
    }
}
