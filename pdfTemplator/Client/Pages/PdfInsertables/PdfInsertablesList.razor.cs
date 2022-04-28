using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Constants.Enums;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class PdfInsertablesList
    {
        [Parameter] public PdfTemplate Template { get; set; } = null!;

        private async Task CreateNewInsertable()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(CreatePdfInsertable.Template), Template);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<CreatePdfInsertable>("Create", parameters, options);
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