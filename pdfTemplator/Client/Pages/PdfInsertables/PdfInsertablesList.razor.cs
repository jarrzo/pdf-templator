using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Insertables;
using System.Text.Json;

namespace pdfTemplator.Client.Pages.PdfInsertables
{
    public partial class PdfInsertablesList
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        public List<PdfInsertable> Insertables { get; set; } = new();

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
            }
        }

        private async Task CreateNewInsertable()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(EditPdfInsertable.Template), Template);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditPdfInsertable>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await GetPdfInsertables();
            }
        }
        private static string GetInsertableIcon(InsertableType type) => type switch
        {
            InsertableType.Text => Icons.Material.Filled.ShortText,
            InsertableType.Sequence => Icons.Material.Filled.DataArray,
            InsertableType.Table => Icons.Material.Filled.TableChart,
            InsertableType.Date => Icons.Material.Filled.AccessTime,
            _ => Icons.Material.Filled.Texture,
        };

        private async Task InsertIntoTextEditor(PdfInsertable insertable)
        {
            if (insertable.Type == InsertableType.Text) await InsertText(insertable);
            if (insertable.Type == InsertableType.Sequence) await InsertSequence(insertable);
            if (insertable.Type == InsertableType.Table) await InsertTable(insertable);
            if (insertable.Type == InsertableType.Date) await InsertText(insertable);
        }

        private async Task InsertText(PdfInsertable insertable)
        {
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", "{{" + insertable.Key + "}}");
        }

        private async Task InsertSequence(PdfInsertable insertable)
        {
            SequenceParams insertableParams = JsonSerializer.Deserialize<SequenceParams>(insertable.ParamsJSON)!;

            string data = $"<p>@start_{insertable.Key}</p>";
            foreach (var seqElement in insertableParams!.SequenceElements) data += "<p>{{" + seqElement.Key + "}}</p>";
            data += $"<p>@end_{insertable.Key}</p>";
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", data);
        }

        private async Task InsertTable(PdfInsertable insertable)
        {
            TableParams insertableParams = JsonSerializer.Deserialize<TableParams>(insertable.ParamsJSON)!;

            string data = $"<table style=\"border-collapse: collapse; width: 100%;\" border=\"1\"><colgroup><col style=\"width: 50%;\"><col style=\"width: 50%;\"></colgroup><tbody data-pdfinsertable=\"" + insertable.Key + "\"><tr>";
            foreach (var tableElement in insertableParams!.TableElements) data += "<td>{{" + tableElement.Key + "}}</td>";
            data += $"</tr></tbody></table>";
            Console.WriteLine(data);
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", data);
        }
    }
}