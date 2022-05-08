using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Models
{
    public class FieldService : IFieldService
    {
        private readonly HttpClient _httpClient;

        public FieldService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<Field>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(FieldEndpoints.BaseUrl);
            return await response.ToResult<List<Field>>();
        }

        public async Task<IResult<Field>> GetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{FieldEndpoints.BaseUrl}/{id}");
            return await response.ToResult<Field>();
        }

        public async Task<IResult<Field>> SaveAsync(Field field)
        {
            HttpResponseMessage response;

            if (field.Id > 0)
                response = await _httpClient.PutAsJsonAsync($"{FieldEndpoints.BaseUrl}/{field.Id}", field);
            else
                response = await _httpClient.PostAsJsonAsync($"{FieldEndpoints.BaseUrl}", field);

            return await response.ToResult<Field>();
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{FieldEndpoints.BaseUrl}/{id}");
            return await response.ToResult<int>();
        }
    }
}
