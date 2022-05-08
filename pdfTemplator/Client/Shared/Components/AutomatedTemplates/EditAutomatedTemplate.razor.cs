using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared.Components.AutomatedTemplates
{
    public partial class EditAutomatedTemplate
    {
        [Inject] private ITemplateService templateService { get; set; } = null!;
        [Inject] private IDataSourceService dataSourceService { get; set; } = null!;
        [Inject] private IAutomatedTemplateService automatedTemplateService { get; set; } = null!;
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public AutomatedTemplate AutomatedTemplate { get; set; } = new();
        private List<Template> Templates { get; set; } = new();
        private List<DataSource> DataSources { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            Templates = (await templateService.GetAllAsync()).Data.ToList();
            DataSources = (await dataSourceService.GetAllAsync()).Data.ToList();
        }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task AddDataSource()
        {
            await automatedTemplateService.SaveAsync(AutomatedTemplate);
            _snackBar.Add("Created", Severity.Success);
            MudDialog.Close();
        }
    }
}