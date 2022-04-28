using pdfTemplator.Shared.Models;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class PdfConversionService : IPdfConversionService
    {
        private readonly HttpClient _httpClient;

        public PdfConversionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<PdfConversion>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{PdfConversionEndpoints.BaseUrl}/{id}");
            return await response.ToResult<PdfConversion>();
        }

        public async Task<IResult<PdfConversion>> SaveAsync(PdfConversion pdfConversion)
        {
            HttpResponseMessage response;

            if (pdfConversion.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{PdfConversionEndpoints.BaseUrl}/{pdfConversion.Id}", pdfConversion);
            else
                response = await _httpClient.PostAsJsonAsync($"{PdfConversionEndpoints.BaseUrl}", pdfConversion);

            return await response.ToResult<PdfConversion>();
        }

        public async Task<IResult<string>> ConvertAsync(int id, List<PdfKeyValue> data)
        {
            var response = await _httpClient.PostAsJsonAsync($"{PdfConversionEndpoints.BaseUrl}/{id}/convert", data);
            return await response.ToResult<string>();
        }
    }
}
