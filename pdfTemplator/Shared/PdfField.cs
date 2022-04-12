using System.ComponentModel.DataAnnotations;

namespace PDFTemplator.Shared
{
    public class PdfField
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(64)]
        public string Path { get; set; }

        public PdfFieldTypes Type { get; set; }
    }
}
