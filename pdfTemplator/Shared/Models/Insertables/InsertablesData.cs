namespace pdfTemplator.Shared.Models.Insertables
{
    public class InsertablesData
    {
        public List<PdfKeyValue> TextFields { get; set; } = null!;
        public List<Sequence> SequenceFields { get; set; } = null!;
        public List<Table> TableFields { get; set; } = null!;
    }
}
