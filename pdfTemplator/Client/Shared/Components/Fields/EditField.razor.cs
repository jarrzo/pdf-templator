using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Fields;
using System.Text.Json;

namespace pdfTemplator.Client.Shared.Components.Fields
{
    public partial class EditField
    {
        [Inject] private IFieldService fieldService { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public Field Field { get; set; } = new();
        public List<ArrayElement> ArrayElements { get; set; } = new();
        public string DateFormat { get; set; } = "yyyy-MM-dd";
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

        protected override Task OnInitializedAsync()
        {
            GetArrayElements();
            return base.OnInitializedAsync();
        }

        public void GetArrayElements()
        {
            if (Field.Type == FieldType.Object) ArrayElements = JsonSerializer.Deserialize<ArrayParams>(Field.ParamsJSON)!.ArrayElements;
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task AddField()
        {
            PrepareField();
            await fieldService.SaveAsync(Field);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }

        private void PrepareField()
        {
            if (Field.Type == FieldType.Text) SetupTextParams();
            if (Field.Type == FieldType.Object) SetupObjectParams();
            if (Field.Type == FieldType.Date) SetupDateParams();
        }

        private void SetupTextParams()
        {
            Field.ParamsJSON = "";
        }

        private void SetupObjectParams()
        {
            ArrayParams arrayParams = new()
            {
                ArrayElements = ArrayElements,
            };

            Field.ParamsJSON = JsonSerializer.Serialize(arrayParams);
        }

        private void SetupDateParams()
        {
            DateParams inserArrayParams = new()
            {
                DateFormat = DateFormat,
            };

            Field.ParamsJSON = JsonSerializer.Serialize(inserArrayParams);
        }
    }
}