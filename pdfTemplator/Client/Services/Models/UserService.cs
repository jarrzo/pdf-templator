using pdfTemplator.Client.Services.Routes;
using pdfTemplator.Shared.Extensions;
using pdfTemplator.Shared.Models;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Models
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<UserInfo>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(UserEndpoints.BaseUrl);
            return await response.ToResult<List<UserInfo>>();
        }
    }
}
