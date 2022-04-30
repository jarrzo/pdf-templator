namespace pdfTemplator.Shared.Models.Insertables
{
    public class Sequence
    {
        public string Key = null!;
        public List<List<PdfKeyValue>> Elements { get; set; } = null!;
    }
}
