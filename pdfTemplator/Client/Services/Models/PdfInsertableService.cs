using pdfTemplator.Client.Models;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class PdfInsertableService : IPdfInsertableService
    {
        private readonly HttpClient _httpClient;

        public PdfInsertableService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<PdfInsertable>>> GetAllAsync(PdfTemplate pdfTemplate)
        {
            var response = await _httpClient.GetAsync($"{GetBaseUrl(pdfTemplate)}");
            return await response.ToResult<List<PdfInsertable>>();
        }
        public async Task<IResult<PdfInsertable>> GetAsync(PdfTemplate pdfTemplate, int pdfInsertableId)
        {
            var response = await _httpClient.GetAsync($"{GetBaseUrl(pdfTemplate)}/{pdfInsertableId}");
            return await response.ToResult<PdfInsertable>();
        }

        public async Task<IResult<PdfInsertable>> SaveAsync(PdfTemplate pdfTemplate, PdfInsertable pdfInsertable)
        {
            HttpResponseMessage response;

            if (pdfInsertable.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{GetBaseUrl(pdfTemplate)}/{pdfInsertable.Id}", pdfInsertable);
            else
                response = await _httpClient.PostAsJsonAsync($"{GetBaseUrl(pdfTemplate)}", pdfInsertable);

            return await response.ToResult<PdfInsertable>();
        }
        public async Task<IResult<int>> DeleteAsync(PdfTemplate pdfTemplate, int pdfInsertableId)
        {
            var response = await _httpClient.DeleteAsync($"{GetBaseUrl(pdfTemplate)}/{pdfInsertableId}");
            return await response.ToResult<int>();
        }

        private static string GetBaseUrl(PdfTemplate pdfTemplate)
        {
            return $"{PdfTemplateEndpoints.BaseUrl}/{pdfTemplate.Id}/{PdfInsertableEndPoints.BaseUrl}";
        }
    }
}
