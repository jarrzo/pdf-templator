using pdfTemplator.Client.Managers.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Managers.Models
{
    public class ChartManager : IChartManager
    {
        private readonly HttpClient _httpClient;

        public ChartManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<double>>> GetWeeklyConversionsCount()
        {
            var response = await _httpClient.GetAsync(ChartsEndpoints.GetWeeklyConversionsCount);
            return await response.ToResult<List<double>>();
        }
    }
}
