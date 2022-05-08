using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Fields;
using System.Text.Json;

namespace pdfTemplator.Client.Shared.Components.Fields
{
    public partial class FieldList
    {
        [Parameter] public Template Template { get; set; } = null!;
        public List<Field> Fields { get; set; } = new();

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                GetFields();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        private void GetFields()
        {
            if (Template.Fields != null)
            {
                Fields = Template.Fields.ToList();
            }
        }

        private async Task CreateNewField()
        {
            var parameters = new DialogParameters();

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditField>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                GetFields();
            }
        }
        private static string GetFieldIcon(FormFieldType type) => type switch
        {
            FormFieldType.Text => Icons.Material.Filled.ShortText,
            FormFieldType.Sequence => Icons.Material.Filled.DataArray,
            FormFieldType.Table => Icons.Material.Filled.TableChart,
            FormFieldType.Date => Icons.Material.Filled.AccessTime,
            _ => Icons.Material.Filled.Texture,
        };

        private async Task InsertIntoTextEditor(Field field, FormFieldType type)
        {
            if (type == FormFieldType.Text) await InsertText(field);
            if (type == FormFieldType.Sequence) await InsertSequence(field);
            if (type == FormFieldType.Table) await InsertTable(field);
            if (type == FormFieldType.Date) await InsertText(field);
        }

        private async Task InsertText(Field field)
        {
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", "{{" + field.Key + "}}");
        }

        private async Task InsertSequence(Field field)
        {
            ArrayParams arrayParams = JsonSerializer.Deserialize<ArrayParams>(field.ParamsJSON)!;

            string data = $"<p>@start_{field.Key}</p>";
            foreach (var seqElement in arrayParams!.ArrayElements) data += "<p>{{" + seqElement.Key + "}}</p>";
            data += $"<p>@end_{field.Key}</p>";
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", data);
        }

        private async Task InsertTable(Field field)
        {
            ArrayParams arrayParams = JsonSerializer.Deserialize<ArrayParams>(field.ParamsJSON)!;

            string data = $"<table style=\"border-collapse: collapse; width: 100%;\" border=\"1\"><tbody data-pdffield=\"" + field.Key + "\"><tr>";
            foreach (var tableElement in arrayParams!.ArrayElements) data += "<td>{{" + tableElement.Key + "}}</td>";
            data += $"</tr></tbody></table>";
            Console.WriteLine(data);
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", data);
        }
    }
}