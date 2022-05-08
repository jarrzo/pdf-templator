using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Shared.Components.AutomatedTemplates;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.AutomatedTemplates
{
    public partial class AutomatedTemplateList
    {
        [Inject] private IAutomatedTemplateService automatedTemplateService { get; set; } = null!;
        private List<AutomatedTemplate> _list = new();
        private string _searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetTemplates();
        }

        private async Task GetTemplates()
        {
            var response = await automatedTemplateService.GetAllAsync();
            if (response != null)
            {
                _list = response.Data.ToList();
            }
        }

        private bool Search(AutomatedTemplate template)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return template.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                { nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await automatedTemplateService.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    _snackBar.Add(_localizer["Success"], Severity.Success);
                }
                else
                {
                    await Reset();
                    _snackBar.Add(_localizer["Error"], Severity.Error);
                }
            }
        }

        private async Task Reset()
        {
            await GetTemplates();
        }

        private async Task AddAutomatedTemplate()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditAutomatedTemplate>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task UpdateAutomatedTemplate(AutomatedTemplate template)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(EditAutomatedTemplate.AutomatedTemplate), template);
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditAutomatedTemplate>("Edit", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task GetFilled(AutomatedTemplate template)
        {
            var response = await automatedTemplateService.ConvertAsync(template.Id);

            if (response.Succeeded)
                await _jsRuntime.InvokeVoidAsync("downloadBase64File", "application/pdf", response.Data, $"{template.Name}.pdf");
            else
                foreach (var msg in response.Messages)
                    _snackBar.Add(msg, Severity.Error);
        }
    }
}
