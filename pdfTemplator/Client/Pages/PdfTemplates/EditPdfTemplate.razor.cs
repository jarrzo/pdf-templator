using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class EditPdfTemplate
    {
        [Parameter] public int Id { get; set; }
        public PdfTemplate Template { get; set; } = new();

        private async Task SaveAsync()
        {
            Console.WriteLine(Template.Id);
            var response = await _pdfTemplateManager.SaveAsync(Template);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Id > 0)
            {
                var response = await _pdfTemplateManager.GetPdfTemplate(Id);
                Template = response.Data;
            }
            Console.WriteLine(Template.Id);
            await Task.CompletedTask;
        }
    }
}
