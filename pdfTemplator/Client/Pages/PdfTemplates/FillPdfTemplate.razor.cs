using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class FillPdfTemplate
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        [Inject] private IPdfConversionService pdfConversionService { get; set; } = null!;
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        public List<PdfInsertable> Insertables { get; set; } = new();
        public List<PdfKeyValue> Fields { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetPdfInsertables();
        }

        private async Task GetPdfInsertables()
        {
            if (Template.Id == 0) return;

            var response = await pdfTemplateService.GetPdfInsertablesAsync(Template.Id);
            if (response != null)
            {
                Insertables = response.Data.ToList();
                GenerateFields();
            }
        }

        private void GenerateFields()
        {
            foreach (var insertable in Insertables)
            {
                Fields.Add(new PdfKeyValue
                {
                    Key = insertable.Key,
                    Value = ""
                });
            }
        }

        public async Task GenerateDocument()
        {
            var response = await pdfConversionService.ConvertAsync(Template.Id, Fields);
            await _jsRuntime.InvokeVoidAsync("downloadBase64File", "application/pdf", response.Data, $"{Template.Name}.pdf");
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}