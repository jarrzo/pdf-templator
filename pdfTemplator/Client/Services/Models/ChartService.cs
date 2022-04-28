using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public class ChartService : IChartService
    {
        private readonly HttpClient _httpClient;

        public ChartService(HttpClient httpClient)
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
