using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class ConversionService : IConversionService
    {
        private readonly HttpClient _httpClient;

        public ConversionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<Conversion>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{ConversionEndpoints.BaseUrl}/{id}");
            return await response.ToResult<Conversion>();
        }

        public async Task<IResult<Conversion>> SaveAsync(Conversion conversion)
        {
            HttpResponseMessage response;

            if (conversion.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{ConversionEndpoints.BaseUrl}/{conversion.Id}", conversion);
            else
                response = await _httpClient.PostAsJsonAsync($"{ConversionEndpoints.BaseUrl}", conversion);

            return await response.ToResult<Conversion>();
        }

        public async Task<IResult<string>> ConvertAsync(int id, dynamic data)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ConversionEndpoints.BaseUrl}/{id}/convert", (object)data);
            return await response.ToResult<string>();
        }
    }
}
