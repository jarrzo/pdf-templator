using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Models;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class EditPdfInsertable
    {
        public PdfInsertable Insertable = new();
        [Parameter] public PdfTemplate Template { get; set; } = null!;
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