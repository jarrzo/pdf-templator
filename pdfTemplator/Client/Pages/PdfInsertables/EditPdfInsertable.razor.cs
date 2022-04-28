using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class EditPdfInsertable
    {
        public PdfInsertable Insertable = new();
        [Inject] private IPdfInsertableService pdfInsertableService { get; set; } = null!;
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task AddInsertable()
        {
            Insertable.PdfTemplateId = Template.Id;
            await pdfInsertableService.SaveAsync(Insertable);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }
    }
}