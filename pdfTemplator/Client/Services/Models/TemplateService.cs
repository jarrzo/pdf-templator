using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class TemplateService : ITemplateService
    {
        private readonly HttpClient _httpClient;

        public TemplateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<Template>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(TemplateEndpoints.BaseUrl);
            return await response.ToResult<List<Template>>();
        }

        public async Task<IResult<List<Template>>> GetAllByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"{CategoryEndpoints.BaseUrl}/{categoryId}/{CategoryEndpoints.Templates}");
            return await response.ToResult<List<Template>>();
        }

        public async Task<IResult<Template>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{TemplateEndpoints.BaseUrl}/{id}");
            return await response.ToResult<Template>();
        }

        public async Task<IResult<Template>> SaveAsync(Template request)
        {
            HttpResponseMessage response;

            if (request.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{TemplateEndpoints.BaseUrl}/{request.Id}", request);
            else
                response = await _httpClient.PostAsJsonAsync(TemplateEndpoints.BaseUrl, request);

            return await response.ToResult<Template>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{TemplateEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<Conversion>>> GetConversionsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{TemplateEndpoints.BaseUrl}/{id}/{TemplateEndpoints.Conversions}");
            return await response.ToResult<List<Conversion>>();
        }
    }
}
