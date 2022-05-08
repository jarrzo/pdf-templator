using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Fields;
using System.Text.Json;

namespace pdfTemplator.Client.Shared.Components.DataSources
{
    public partial class EditDataSource
    {
        [Inject] private IDataSourceService dataSourceService { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public DataSource DataSource { get; set; } = new();
        public List<KeyValue> Headers { get; set; } = new();

        protected override Task OnInitializedAsync()
        {
            JsonToHeaders();
            return base.OnInitializedAsync();
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task AddDataSource()
        {
            HeadersToJson();
            await dataSourceService.SaveAsync(DataSource);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }

        private void HeadersToJson()
        {
            DataSource.HeadersJSON = JsonSerializer.Serialize(Headers);
        }

        private void JsonToHeaders()
        {
            if (DataSource.HeadersJSON != null) Headers = JsonSerializer.Deserialize<List<KeyValue>>(DataSource.HeadersJSON) ?? new();
        }
    }
}