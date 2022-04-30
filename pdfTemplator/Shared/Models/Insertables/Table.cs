namespace pdfTemplator.Shared.Models.Insertables
{
    public class Table
    {
        public string Key { get; set; } = null!;
        public List<List<PdfKeyValue>> Elements { get; set; } = null!;
    }
}
