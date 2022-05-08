using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared.Components.Fields
{
    public partial class FieldTable
    {
        [Inject] private IFieldService fieldService { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public Template? Template { get; set; }
        private List<Field> _list = new();
        private string _searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetFields();
        }

        private async Task GetFields()
        {
            var response = await fieldService.GetAllAsync();
            if (response != null)
            {
                _list = response.Data.ToList();
            }
        }

        private bool Search(Field field)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return field.Key?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
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
                var response = await fieldService.DeleteAsync(id);
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
            await GetFields();
        }

        private async Task AddField()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditField>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task UpdateField(Field field)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(EditField.Field), field);
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditField>("Edit", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task AddToTemplate(Field field)
        {
            if (Template != null)
            {
                if (field.Templates == null) field.Templates = new List<Template>();
                field.Templates.Add(Template);
                await fieldService.SaveAsync(field);
                Cancel();
            }
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}
