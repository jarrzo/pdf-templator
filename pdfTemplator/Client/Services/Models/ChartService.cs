using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
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
            var response = await _httpClient.GetAsync(ChartEndpoints.GetWeeklyConversionsCount);
            return await response.ToResult<List<double>>();
        }

        public async Task<IResult<List<KeyValuePair<Template, int>>>> GetTopTemplates()
        {
            var response = await _httpClient.GetAsync(ChartEndpoints.GetTopTemplates);
            return await response.ToResult<List<KeyValuePair<Template, int>>>();
        }
    }
}
