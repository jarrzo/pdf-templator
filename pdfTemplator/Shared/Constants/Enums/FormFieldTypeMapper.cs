namespace pdfTemplator.Shared.Constants.Enums
{
    public static class FormFieldTypeMapper
    {
        public static FieldType ToFieldType(this FormFieldType formType) => formType switch
        {
            FormFieldType.Text => FieldType.Text,
            FormFieldType.Sequence => FieldType.Object,
            FormFieldType.Table => FieldType.Object,
            FormFieldType.Date => FieldType.Date,
            _ => FieldType.Text,
        };
    }
}
