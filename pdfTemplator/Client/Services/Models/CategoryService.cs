using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<Category>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(CategoryEndpoints.BaseUrl);
            return await response.ToResult<List<Category>>();
        }

        public async Task<IResult<Category>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{CategoryEndpoints.BaseUrl}/{id}");
            return await response.ToResult<Category>();
        }

        public async Task<IResult<Category>> SaveAsync(CategoryParameters request)
        {
            HttpResponseMessage response;

            if (request.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{CategoryEndpoints.BaseUrl}/{request.Id}", request);
            else
                response = await _httpClient.PostAsJsonAsync(CategoryEndpoints.BaseUrl, request);

            return await response.ToResult<Category>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{CategoryEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<PdfTemplate>>> GetPdfTemplates(int id)
        {
            var response = await _httpClient.GetAsync($"{CategoryEndpoints.BaseUrl}/{id}/{CategoryEndpoints.PdfTemplates}");
            return await response.ToResult<List<PdfTemplate>>();
        }
    }
}
