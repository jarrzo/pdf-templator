using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Shared.Components.DataSources;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.DataSources
{
    public partial class DataSourceList
    {
        [Inject] private IDataSourceService dataSourceService { get; set; } = null!;
        private List<DataSource> _list = new();
        private string _searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetDataSources();
        }

        private async Task GetDataSources()
        {
            var response = await dataSourceService.GetAllAsync();
            if (response != null)
            {
                _list = response.Data.ToList();
            }
        }

        private bool Search(DataSource dataSource)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return dataSource.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
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
                var response = await dataSourceService.DeleteAsync(id);
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
            await GetDataSources();
        }

        private async Task AddDataSource()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditDataSource>("Create", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task UpdateDataSource(DataSource dataSource)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(EditDataSource.DataSource), dataSource);
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<EditDataSource>("Edit", parameters, options);
            var response = await dialog.Result;
            if (!response.Cancelled)
            {
                await Reset();
            }
        }

        private async Task GetData(DataSource dataSource)
        {
            var response = await dataSourceService.GetDataAsync(dataSource.Id);
            if (response.Succeeded)
                await _jsRuntime.InvokeVoidAsync("downloadBase64File", "application/json", response.Data, $"{dataSource.Name}.json");
            else
                foreach (var msg in response.Messages)
                    _snackBar.Add(msg, Severity.Error);
        }
    }
}
