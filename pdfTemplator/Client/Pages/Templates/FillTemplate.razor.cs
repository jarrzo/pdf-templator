using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Fields;
using System.Text.Json;

namespace pdfTemplator.Client.Pages.Templates
{
    public partial class FillTemplate
    {
        [Inject] private ITemplateService templateService { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        public Template Template { get; set; } = null!;
        public List<Field> Fields { get; set; } = new();
        public List<FormField> FormFields { get; set; } = new();
        public Dictionary<string, dynamic> PreparedData { get; set; } = new();
        public int SimpleFieldsCount = 0;

        protected override async Task OnInitializedAsync()
        {
            await GetTemplate();
        }

        private async Task GetTemplate()
        {
            var response = await templateService.GetAsync(Id);
            Template = response.Data;

            if (Template.Fields != null)
            {
                Fields = Template.Fields.ToList();
                SetupFields();

                SimpleFieldsCount = Fields.Where(x => x.Type != FieldType.Object).Count();
            }
        }

        public async Task GenerateDocument()
        {
            PrepareData();
            var response = await templateService.ConvertAsync(Template.Id, PreparedData);

            if (response.Succeeded)
                await _jsRuntime.InvokeVoidAsync("downloadBase64File", "application/pdf", response.Data, $"{Template.Name}.pdf");
            else
                foreach (var msg in response.Messages)
                    _snackBar.Add(msg, Severity.Error);
        }

        private void SetupFields()
        {
            foreach (var field in Fields)
            {
                FormFields.Add(GetField(field));
            }
        }

        private FormField GetField(Field field)
        {
            FormField formField = new()
            {
                Field = field,
                Key = field.Key,
                Type = field.Type,
                Value = "",
            };

            if (formField.Type == FieldType.Object) SetupObjectSubFields(formField);

            return formField;
        }

        private static void SetupObjectSubFields(FormField field)
        {
            field.HasElements = true;

            List<FormField> elements = new();
            foreach (var objField in JsonSerializer.Deserialize<ArrayParams>(field.Field.ParamsJSON)!.ArrayElements)
            {
                elements.Add(new() { Key = objField.Key, Value = "" });
            }
            field.Elements.Add(elements);
        }

        private static void AddElement(FormField field)
        {
            if (field.Type == FieldType.Object) SetupObjectSubFields(field);
        }

        private static int CountSize(int maxCount, int objectsCount)
        {
            if (objectsCount < maxCount)
                if (12 % objectsCount == 0)
                    return 12 / objectsCount;
                else
                    return maxCount;

            return 12 / maxCount;
        }

        private static string GetFieldDateFormat(FormField field)
        {
            return JsonSerializer.Deserialize<DateParams>(field.Field.ParamsJSON)!.DateFormat;
        }

        private void PrepareData()
        {
            PreparedData = new();
            GetTextFields();
            GetArrayFields();
        }

        private void GetTextFields()
        {
            foreach (var field in FormFields.Where(x => !x.HasElements))
            {
                PreparedData.Add(field.Key, field.Value);
            }
        }

        private void GetArrayFields()
        {
            foreach (var field in FormFields.Where(x => x.Type == FieldType.Object))
                GetObject(field);
        }

        private void GetObject(FormField field)
        {
            List<Dictionary<string, string>> objects = new();
            foreach (var element in field.Elements)
            {
                objects.Add(GetElement(element));
            }
            PreparedData.Add(field.Key, objects);
        }

        private Dictionary<string, string> GetElement(List<FormField> fields)
        {
            Dictionary<string, string> element = new();
            foreach (var field in fields)
            {
                element.Add(field.Key, field.Value);
            }
            return element;
        }
    }
}