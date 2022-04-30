using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Insertables;
using System.Text.Json;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class EditPdfInsertable
    {
        [Inject] private IPdfInsertableService pdfInsertableService { get; set; } = null!;
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        public PdfInsertable Insertable = new();
        public List<SequenceElement> SequenceElements = new();
        public List<TableElement> TableElements = new();
        public string DateFormat = "yyyy-MM-dd";
        public string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task AddInsertable()
        {
            PrepareInsertable();
            await pdfInsertableService.SaveAsync(Insertable);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }

        private void PrepareInsertable()
        {
            Insertable.PdfTemplateId = Template.Id;
            if (Insertable.Type == InsertableType.Text) SetupTextParams();
            if (Insertable.Type == InsertableType.Sequence) SetupSequenceParams();
            if (Insertable.Type == InsertableType.Table) SetupTableParams();
            if (Insertable.Type == InsertableType.Date) SetupDateParams();
        }

        private void SetupTextParams()
        {
            Insertable.ParamsJSON = "";
        }

        private void SetupSequenceParams()
        {
            SequenceParams insertableParams = new()
            {
                SequenceElements = SequenceElements,
            };

            Insertable.ParamsJSON = JsonSerializer.Serialize(insertableParams);
        }

        private void SetupTableParams()
        {
            TableParams insertableParams = new()
            {
                TableElements = TableElements,
            };

            Insertable.ParamsJSON = JsonSerializer.Serialize(insertableParams);
        }

        private void SetupDateParams()
        {
            DateParams insertableParams = new()
            {
                DateFormat = DateFormat,
            };

            Insertable.ParamsJSON = JsonSerializer.Serialize(insertableParams);
        }
    }
}