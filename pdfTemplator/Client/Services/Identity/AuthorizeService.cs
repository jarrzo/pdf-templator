using pdfTemplator.Client.Services.Interfaces;
using pdfTemplator.Shared.Models;
using System.Net.Http.Json;

namespace pdfTemplator.Client.Services.Identity
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly HttpClient _httpClient;

        public AuthorizeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Login(LoginParameters loginParameters)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Authorize/Login", loginParameters);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("api/Authorize/Logout", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task Register(RegisterParameters registerParameters)
        {
            var result = await _httpClient.PostAsJsonAsync("api/Authorize/Register", registerParameters);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
        }

        public async Task<UserInfo> GetUserInfo()
        {
            var result = await _httpClient.GetFromJsonAsync<UserInfo>("api/Authorize/UserInfo");
            return result;
        }
    }
}
