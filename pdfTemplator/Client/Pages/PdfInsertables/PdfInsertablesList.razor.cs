using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Client.Models;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class PdfInsertablesList
    {
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        public List<PdfInsertable> Insertables { get; set; } = new();

        private async Task CreateNewInsertable()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(EditPdfInsertable.Template), Template);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditPdfInsertable>("Create", parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
            }
        }
        private static string GetInsertableIcon(InsertableType type) => type switch
        {
            InsertableType.Text => Icons.Material.Filled.ShortText,
            InsertableType.Sequence => Icons.Material.Filled.DataArray,
            InsertableType.Table => Icons.Material.Filled.TableChart,
            InsertableType.Date => Icons.Material.Filled.DateRange,
            InsertableType.DateTime => Icons.Material.Filled.AccessTime,
            _ => Icons.Material.Filled.Texture,
        };

        private async Task InsertIntoTextEditor(string key)
        {
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", key);
        }
    }
}