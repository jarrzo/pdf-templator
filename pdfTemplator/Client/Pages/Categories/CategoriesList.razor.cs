using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Client.Shared.Components.Categories;
using pdfTemplator.Client.Shared.Components.PdfTemplates;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.Categories
{
    public partial class CategoriesList
    {
        [Inject] private ICategoryService categoryService { get; set; } = null!;
        private List<Category> _list = new();
        private string _searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetCategories();
        }

        private async Task GetCategories()
        {
            var response = await categoryService.GetAllAsync();
            if (response != null)
            {
                _list = response.Data.ToList();
            }
        }

        private bool Search(Category category)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return category.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
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
                var response = await categoryService.DeleteAsync(id);
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
            await GetCategories();
        }

        private async Task AddCategory()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditCategory>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task UpdateCategory(Category category)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(EditCategory.CategoryParams), category.ToCategoryParameters());
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditCategory>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }
    }
}
