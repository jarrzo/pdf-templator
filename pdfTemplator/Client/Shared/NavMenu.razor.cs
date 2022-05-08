using Microsoft.AspNetCore.Components;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] private ICategoryService categoryService { get; set; } = null!;
        public List<Category> Categories { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GetCategories();
        }

        private async Task GetCategories()
        {
            Categories = (await categoryService.GetAllAsync()).Data.ToList();
        }

        public static string GetUrl(Category category)
        {
            return $"/category/{category.Id}";
        }
    }
}
