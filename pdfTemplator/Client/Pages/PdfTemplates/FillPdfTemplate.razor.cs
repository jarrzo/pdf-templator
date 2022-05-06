using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Constants.Enums;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Models.Insertables;
using System.Text.Json;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class FillPdfTemplate
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        [Inject] private IPdfConversionService pdfConversionService { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        public PdfTemplate Template { get; set; } = null!;
        public List<PdfInsertable> Insertables { get; set; } = new();
        public List<InsertableField> Fields { get; set; } = new();
        public Dictionary<string, dynamic> PreparedData { get; set; } = new();
        public int SimpleFieldsCount = 0;

        protected override async Task OnInitializedAsync()
        {
            await GetPdfTemplate();
            await GetPdfInsertables();
        }

        private async Task GetPdfTemplate()
        {
            var response = await pdfTemplateService.GetAsync(Id);
            Template = response.Data;
        }

        private async Task GetPdfInsertables()
        {
            if (Template.Id == 0) return;

            var response = await pdfTemplateService.GetPdfInsertablesAsync(Template.Id);
            if (response != null)
            {
                Insertables = response.Data.ToList();
                SetupFields();
            }

            SimpleFieldsCount = Fields.Where(x => x.Type != InsertableType.Sequence && x.Type != InsertableType.Table).Count();
        }

        public async Task GenerateDocument()
        {
            PrepareData();
            var response = await pdfConversionService.ConvertAsync(Template.Id, PreparedData);
            await _jsRuntime.InvokeVoidAsync("downloadBase64File", "application/pdf", (string)response.Data, $"{Template.Name}.pdf");
        }

        private void SetupFields()
        {
            foreach (var insertable in Insertables)
            {
                Fields.Add(GetField(insertable));
            }
        }

        private InsertableField GetField(PdfInsertable insertable)
        {
            InsertableField field = new()
            {
                Insertable = insertable,
                Key = insertable.Key,
                Type = insertable.Type,
                Value = "",
            };

            if (insertable.Type == InsertableType.Sequence) SetupSequenceSubFields(field);
            if (insertable.Type == InsertableType.Table) SetupTableSubFields(field);

            return field;
        }

        private static void SetupSequenceSubFields(InsertableField field)
        {
            field.HasElements = true;

            List<InsertableField> elements = new();
            foreach (var seqElement in JsonSerializer.Deserialize<SequenceParams>(field.Insertable.ParamsJSON)!.SequenceElements)
            {
                elements.Add(new() { Key = seqElement.Key, Value = "" });
            }
            field.Elements.Add(elements);
        }

        private static void SetupTableSubFields(InsertableField field)
        {
            field.HasElements = true;

            List<InsertableField> elements = new();
            foreach (var tableElement in JsonSerializer.Deserialize<TableParams>(field.Insertable.ParamsJSON)!.TableElements)
            {
                elements.Add(new() { Key = tableElement.Key, Value = "" });
            }
            field.Elements.Add(elements);
        }

        private static void AddElement(InsertableField field)
        {
            if (field.Type == InsertableType.Sequence) SetupSequenceSubFields(field);
            if (field.Type == InsertableType.Table) SetupTableSubFields(field);
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

        private static string GetInsertableDateFormat(InsertableField field)
        {
            return JsonSerializer.Deserialize<DateParams>(field.Insertable.ParamsJSON)!.DateFormat;
        }

        private void PrepareData()
        {
            PreparedData = new();
            GetTextFields();
            GetArrayFields();
        }

        private void GetTextFields()
        {
            foreach (var field in Fields.Where(x => !x.HasElements))
            {
                PreparedData.Add(field.Key, field.Value);
            }
        }

        private void GetArrayFields()
        {
            foreach (var field in Fields.Where(x => x.Type == InsertableType.Sequence || x.Type == InsertableType.Table))
                GetObject(field);
        }

        private void GetObject(InsertableField field)
        {
            List<Dictionary<string, string>> objects = new();
            foreach (var element in field.Elements)
            {
                objects.Add(GetElement(element));
            }
            PreparedData.Add(field.Key, objects);
        }

        private Dictionary<string, string> GetElement(List<InsertableField> fields)
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