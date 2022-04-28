using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class CreatePdfInsertable
    {
        public PdfInsertableDto Insertable = new();
        [Parameter] public PdfTemplateDto Template { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private void AddInsertable()
        {
            //Template.Insertables.Add(Insertable);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }
    }
}