using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using pdfTemplator.Shared.Extensions;
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

        public async Task<IResult<string>> ConvertAsync(int id)
        {
            var tempData = new object();
            var response = await _httpClient.PostAsJsonAsync($"{PdfTemplateEndpoints.Convert}/{id}", tempData);
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{PdfTemplateEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<PdfTemplate>> GetPdfTemplate(int id)
        {
            var response = await _httpClient.GetAsync($"{PdfTemplateEndpoints.Get}/{id}");
            return await response.ToResult<PdfTemplate>();
        }

        public async Task<IResult<List<PdfTemplate>>> GetPdfTemplates()
        {
            var response = await _httpClient.GetAsync(PdfTemplateEndpoints.GetList);
            return await response.ToResult<List<PdfTemplate>>();
        }

        public async Task<IResult<int>> SaveAsync(PdfTemplate request)
        {
            HttpResponseMessage response;

            if(request.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{PdfTemplateEndpoints.Put}/{request.Id}", request);
            else
                response = await _httpClient.PostAsJsonAsync(PdfTemplateEndpoints.Post, request);

            return await response.ToResult<int>();
        }
    }
}
