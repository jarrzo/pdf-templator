using pdfTemplator.Shared.Constants.Enums;

namespace pdfTemplator.Shared.Models.Fields
{
    public class FormField
    {
        public string Key { get; set; } = null!;
        public FieldType Type { get; set; }
        public string Value { get; set; } = null!;
        public bool HasElements { get; set; } = false;
        public List<List<FormField>> Elements { get; set; } = new();
        public Field Field = null!;
    }
}
