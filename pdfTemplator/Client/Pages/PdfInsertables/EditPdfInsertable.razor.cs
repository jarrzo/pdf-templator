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
        public List<ArrayElement> ArrayElements = new();
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
            if (Insertable.Type == InsertableType.Sequence) SetupArrayParams();
            if (Insertable.Type == InsertableType.Table) SetupArrayParams();
            if (Insertable.Type == InsertableType.Date) SetupDateParams();
        }

        private void SetupTextParams()
        {
            Insertable.ParamsJSON = "";
        }

        private void SetupArrayParams()
        {
            ArrayParams arrayParams = new()
            {
                ArrayElements = ArrayElements,
            };

            Insertable.ParamsJSON = JsonSerializer.Serialize(arrayParams);
        }

        private void SetupDateParams()
        {
            DateParams inserArrayParams = new()
            {
                DateFormat = DateFormat,
            };

            Insertable.ParamsJSON = JsonSerializer.Serialize(inserArrayParams);
        }
    }
}