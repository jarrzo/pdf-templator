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

        public async Task<IResult<List<Conversion>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(ConversionEndpoints.BaseUrl);
            return await response.ToResult<List<Conversion>>();
        }

        public async Task<IResult<Conversion>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{ConversionEndpoints.BaseUrl}/{id}");
            return await response.ToResult<Conversion>();
        }
    }
}
