using System.Collections.Generic;

namespace PDFTemplator.Shared
{
    public class PdfTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<PdfField> Insertables { get; set; }
    }
}
