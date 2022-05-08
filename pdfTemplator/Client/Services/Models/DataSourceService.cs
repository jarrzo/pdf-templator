using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class DataSourceService : IDataSourceService
    {
        private readonly HttpClient _httpClient;

        public DataSourceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<DataSource>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(DataSourceEndpoints.BaseUrl);
            return await response.ToResult<List<DataSource>>();
        }

        public async Task<IResult<DataSource>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{DataSourceEndpoints.BaseUrl}/{id}");
            return await response.ToResult<DataSource>();
        }

        public async Task<IResult<DataSource>> SaveAsync(DataSource request)
        {
            HttpResponseMessage response;

            if (request.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{DataSourceEndpoints.BaseUrl}/{request.Id}", request);
            else
                response = await _httpClient.PostAsJsonAsync(DataSourceEndpoints.BaseUrl, request);

            return await response.ToResult<DataSource>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{DataSourceEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }
    }
}
