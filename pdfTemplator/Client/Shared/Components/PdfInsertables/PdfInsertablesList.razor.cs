using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Insertables;
using System.Text.Json;

namespace pdfTemplator.Client.Shared.Components.PdfInsertables
{
    public partial class PdfInsertablesList
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        [Parameter] public PdfTemplate Template { get; set; } = null!;
        public List<PdfInsertable> Insertables { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetPdfInsertables();
            }
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
        private static string GetInsertableIcon(InsertableFormType type) => type switch
        {
            InsertableFormType.Text => Icons.Material.Filled.ShortText,
            InsertableFormType.Sequence => Icons.Material.Filled.DataArray,
            InsertableFormType.Table => Icons.Material.Filled.TableChart,
            InsertableFormType.Date => Icons.Material.Filled.AccessTime,
            _ => Icons.Material.Filled.Texture,
        };

        private async Task InsertIntoTextEditor(PdfInsertable insertable, InsertableFormType type)
        {
            if (type == InsertableFormType.Text) await InsertText(insertable);
            if (type == InsertableFormType.Sequence) await InsertSequence(insertable);
            if (type == InsertableFormType.Table) await InsertTable(insertable);
            if (type == InsertableFormType.Date) await InsertText(insertable);
        }

        private async Task InsertText(PdfInsertable insertable)
        {
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", "{{" + insertable.Key + "}}");
        }

        private async Task InsertSequence(PdfInsertable insertable)
        {
            ArrayParams arrayParams = JsonSerializer.Deserialize<ArrayParams>(insertable.ParamsJSON)!;

            string data = $"<p>@start_{insertable.Key}</p>";
            foreach (var seqElement in arrayParams!.ArrayElements) data += "<p>{{" + seqElement.Key + "}}</p>";
            data += $"<p>@end_{insertable.Key}</p>";
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", data);
        }

        private async Task InsertTable(PdfInsertable insertable)
        {
            ArrayParams arrayParams = JsonSerializer.Deserialize<ArrayParams>(insertable.ParamsJSON)!;

            string data = $"<table style=\"border-collapse: collapse; width: 100%;\" border=\"1\"><tbody data-pdfinsertable=\"" + insertable.Key + "\"><tr>";
            foreach (var tableElement in arrayParams!.ArrayElements) data += "<td>{{" + tableElement.Key + "}}</td>";
            data += $"</tr></tbody></table>";
            Console.WriteLine(data);
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", data);
        }
    }
}