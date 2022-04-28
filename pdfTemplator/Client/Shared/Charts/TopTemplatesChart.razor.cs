using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared.Charts
{
    public partial class TopTemplatesChart
    {
        [Inject] private IChartService _chartManager { get; set; } = null!;
        private List<KeyValuePair<PdfTemplate, int>> _list = new();
        private bool _loading = true;

        public async Task GetSeries()
        {
            var response = await _chartManager.GetTopPdfTemplates();
            if (response != null)
            {
                _list = response.Data.ToList();
                _loading = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetSeries();
        }
    }
}
