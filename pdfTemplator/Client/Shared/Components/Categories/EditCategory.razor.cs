using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared.Components.Categories
{
    public partial class EditCategory
    {
        [Inject] private ICategoryService categoryService{ get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public Category CategoryParams { get; set; } = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task AddCategory()
        {
            await categoryService.SaveAsync(CategoryParams);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }
    }
}