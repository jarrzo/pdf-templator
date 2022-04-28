using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Client.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class PdfTemplateService : IPdfTemplateService
    {
        private readonly HttpClient _httpClient;

        public PdfTemplateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<PdfTemplate>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(PdfTemplateEndpoints.BaseUrl);
            return await response.ToResult<List<PdfTemplate>>();
        }

        public async Task<IResult<PdfTemplate>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{PdfTemplateEndpoints.BaseUrl}/{id}");
            return await response.ToResult<PdfTemplate>();
        }

        public async Task<IResult<PdfTemplate>> SaveAsync(PdfTemplate request)
        {
            HttpResponseMessage response;

            if (request.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{PdfTemplateEndpoints.BaseUrl}/{request.Id}", request);
            else
                response = await _httpClient.PostAsJsonAsync(PdfTemplateEndpoints.BaseUrl, request);

            return await response.ToResult<PdfTemplate>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{PdfTemplateEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }
    }
}
