using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class AutomatedTemplateService : IAutomatedTemplateService
    {
        private readonly HttpClient _httpClient;

        public AutomatedTemplateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<AutomatedTemplate>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(AutomatedTemplateEndpoints.BaseUrl);
            return await response.ToResult<List<AutomatedTemplate>>();
        }

        public async Task<IResult<AutomatedTemplate>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{AutomatedTemplateEndpoints.BaseUrl}/{id}");
            return await response.ToResult<AutomatedTemplate>();
        }

        public async Task<IResult<AutomatedTemplate>> SaveAsync(AutomatedTemplate request)
        {
            HttpResponseMessage response;

            if (request.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{AutomatedTemplateEndpoints.BaseUrl}/{request.Id}", request);
            else
                response = await _httpClient.PostAsJsonAsync(AutomatedTemplateEndpoints.BaseUrl, request);

            return await response.ToResult<AutomatedTemplate>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{AutomatedTemplateEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ConvertAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{AutomatedTemplateEndpoints.BaseUrl}/{id}/convert");
            return await response.ToResult<string>();
        }
    }
}
