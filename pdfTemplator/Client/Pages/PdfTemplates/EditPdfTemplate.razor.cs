using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class EditPdfTemplate
    {
        [Parameter] public PdfTemplate Template { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            Console.WriteLine(Template.Id);
            var response = await _pdfTemplateManager.SaveAsync(Template);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
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
            await Task.CompletedTask;
        }
    }
}
