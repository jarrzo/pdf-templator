using Microsoft.AspNetCore.Components;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Shared.Charts
{
    public partial class TopTemplatesChart
    {
        [Inject] private IChartService _chartManager { get; set; } = null!;
        private List<KeyValuePair<Template, int>> _list = new();
        private bool _loading = true;

        public async Task GetSeries()
        {
            var response = await _chartManager.GetTopTemplates();
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
