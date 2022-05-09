using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Fields;
using System.Text;
using System.Text.Json;

namespace pdfTemplator.Client.Shared.Components.Fields
{
    public partial class TemplateFieldList
    {
        [Inject] private ITemplateService templateService { get; set; } = null!;
        [Parameter] public Template Template { get; set; } = null!;
        public List<Field> Fields { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetFields();
        }

        private async Task GetFields()
        {
            if (Template.Id > 0) Template = (await templateService.GetAsync(Template.Id)).Data;
            if (Template.Fields != null)
            {
                Fields = Template.Fields.ToList();
            }
        }

        private async Task SelectFromExistingFields()
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(FieldTable.Template), Template);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Large, FullWidth = true, DisableBackdropClick = true };
            _dialogService.Show<FieldTable>("Existing fields", parameters, options);
            await GetFields();
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

            StringBuilder str = new();
            str.Append($"<p>@start_{field.Key}</p>");
            foreach (var seqElement in arrayParams!.ArrayElements) str.Append("<p>{{" + seqElement.Key + "}}</p>");
            str.Append($"<p>@end_{field.Key}</p>");
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", str.ToString());
        }

        private async Task InsertTable(Field field)
        {
            ArrayParams arrayParams = JsonSerializer.Deserialize<ArrayParams>(field.ParamsJSON)!;

            StringBuilder str = new();
            str.Append($"<table style=\"border-collapse: collapse; width: 100%;\" border=\"1\"><tbody data-pdffield=\"" + field.Key + "\"><tr>");
            foreach (var tableElement in arrayParams!.ArrayElements) str.Append("<td>{{" + tableElement.Key + "}}</td>");
            str.Append($"</tr></tbody></table>");
            await _jsRuntime.InvokeVoidAsync("insertIntoEditor", str);
        }

        private bool TypeHasFields(FormFieldType type)
        {
            return Fields.Any(x => x.Type == type.ToFieldType());
        }
    }
}