using pdfTemplator.Shared.Models;
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

        public async Task<IResult<PdfInsertable>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{PdfInsertableEndpoints.BaseUrl}/{id}");
            return await response.ToResult<PdfInsertable>();
        }

        public async Task<IResult<PdfInsertable>> SaveAsync(PdfInsertable pdfInsertable)
        {
            HttpResponseMessage response;

            if (pdfInsertable.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{PdfInsertableEndpoints.BaseUrl}/{pdfInsertable.Id}", pdfInsertable);
            else
                response = await _httpClient.PostAsJsonAsync($"{PdfInsertableEndpoints.BaseUrl}", pdfInsertable);

            return await response.ToResult<PdfInsertable>();
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{PdfInsertableEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }
    }
}
