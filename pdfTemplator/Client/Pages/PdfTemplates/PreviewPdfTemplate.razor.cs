using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class PreviewPdfTemplate
    {
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}