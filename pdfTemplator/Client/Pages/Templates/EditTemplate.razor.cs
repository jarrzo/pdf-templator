using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.Templates
{
    public partial class EditTemplate
    {
        [Inject] private ITemplateService templateService { get; set; } = null!;
        [Inject] private ICategoryService categoryService { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        public Template Template { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        private async Task SaveAsync()
        {
            var response = await templateService.SaveAsync(Template);
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
            Categories = (await categoryService.GetAllAsync()).Data.ToList();

            if (Id > 0)
            {
                var response = await templateService.GetAsync(Id);
                Template = response.Data;
                _navigationManager.NavigateTo($"/templates/edit/{Template.Id}", false, true);
            }
            else
            {
                Template = new()
                {
                    Name = "New Template",
                    Description = "",
                    Content = "",
                    CategoryId = Categories.First().Id,
                };
                var response = await templateService.SaveAsync(Template);
                Template = response.Data;
                _navigationManager.NavigateTo($"/templates/edit/{Template.Id}", false, true);
            }
            await Task.CompletedTask;
        }
    }
}
