using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class EditPdfTemplate
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        public PdfTemplate Template { get; set; } = new();

        private async Task SaveAsync()
        {
            var response = await pdfTemplateService.SaveAsync(Template);
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
                var response = await pdfTemplateService.GetAsync(Id);
                Template = response.Data;
            }
            else
            {
                var response = await pdfTemplateService.SaveAsync(new()
                {
                    Name = "New Template",
                    Description = "",
                    Content = "",
                });
                Template = response.Data;
                _navigationManager.NavigateTo($"/templates/{Template.Id}", false, true);
            }
            await Task.CompletedTask;
        }
    }
}
