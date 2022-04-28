﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class PdfTemplatesList
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        private List<PdfTemplate> _list = new();
        private PdfTemplate _template = new();
        private string _searchString = "";
        private bool _loaded = false;

        protected override async Task OnInitializedAsync()
        {
            await GetPdfTemplates();
        }

        private async Task GetPdfTemplates()
        {
            var response = await pdfTemplateService.GetAllAsync();
            if (response != null)
            {
                _list = response.Data.ToList();
                _loaded = true;
            }
        }

        private bool Search(PdfTemplate template)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (template.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return template.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
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
                var response = await pdfTemplateService.DeleteAsync(id);
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
            _template = new();
            await GetPdfTemplates();
        }
    }
}