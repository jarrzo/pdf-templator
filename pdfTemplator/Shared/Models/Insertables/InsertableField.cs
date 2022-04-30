using pdfTemplator.Shared.Constants.Enums;

namespace pdfTemplator.Shared.Models.Insertables
{
    public class InsertableField
    {
        public string Key { get; set; } = null!;
        public InsertableType Type { get; set; }
        public string Value { get; set; } = null!;
        public bool HasElements { get; set; } = false;
        public List<List<InsertableField>> Elements { get; set; } = new();
        public PdfInsertable Insertable = null!;
    }
}
