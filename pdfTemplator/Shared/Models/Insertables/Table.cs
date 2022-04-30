namespace pdfTemplator.Shared.Models.Insertables
{
    public class Table
    {
        public string Key = null!;
        public List<List<PdfKeyValue>> Elements { get; set; } = null!;
    }
}
