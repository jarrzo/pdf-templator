namespace pdfTemplator.Shared.Constants.Enums
{
    public static class InsertableFormTypeMapper
    {
        public static InsertableType ToInsertableType(this InsertableFormType formType) => formType switch
        {
            InsertableFormType.Text => InsertableType.Text,
            InsertableFormType.Sequence => InsertableType.Object,
            InsertableFormType.Table => InsertableType.Object,
            InsertableFormType.Date => InsertableType.Date,
            _ => InsertableType.Text,
        };
    }
}
