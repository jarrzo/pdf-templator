using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared.Components.Templates
{
    public partial class PreviewTemplate
    {
        [Parameter] public Template Template { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}