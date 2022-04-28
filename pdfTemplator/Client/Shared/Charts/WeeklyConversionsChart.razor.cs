using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Services.Models;

namespace pdfTemplator.Client.Shared.Charts
{
    public partial class WeeklyConversionsChart
    {
        [Inject] private IChartService _chartManager { get; set; }
        private List<ChartSeries> _series = new();
        private string[] _dates = Array.Empty<string>();
        private readonly int _numberOfDays = 7;

        public async Task GetSeries()
        {
            var response = await _chartManager.GetWeeklyConversionsCount();
            if (response != null)
            {
                _series = new() { new() { Name = "Conversions", Data = response.Data.ToArray() } };
            }
        }

        public void GetWeekDates()
        {
            List<string> dates = new();
            DateTime today = DateTime.Today;

            for (int i = _numberOfDays; i >= 0; i--) dates.Add(today.AddDays(-1 * i).ToString("MM-dd"));

            _dates = dates.ToArray();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetSeries();
            GetWeekDates();
        }
    }
}
