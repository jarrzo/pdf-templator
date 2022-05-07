using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Client.Shared.Components.PdfTemplates;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.PdfTemplates
{
    public partial class PdfTemplatesList
    {
        [Inject] private IPdfTemplateService pdfTemplateService { get; set; } = null!;
        [Parameter] public int categoryId { get; set; }
        private List<PdfTemplate> _list = new();
        private string _searchString = "";

        protected override async Task OnParametersSetAsync()
        {
            await GetPdfTemplates();
        }

        private async Task GetPdfTemplates()
        {
            if(categoryId > 0)
                _list = (await pdfTemplateService.GetAllByCategoryAsync(categoryId)).Data.ToList();
            else
                _list = (await pdfTemplateService.GetAllAsync()).Data.ToList();
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

        private void Preview(PdfTemplate template)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(PreviewPdfTemplate.Template), template);

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraExtraLarge, FullWidth = true, DisableBackdropClick = true };
            _dialogService.Show<PreviewPdfTemplate>("Preview", parameters, options);
        }

        private async Task Reset()
        {
            await GetPdfTemplates();
        }
    }
}
