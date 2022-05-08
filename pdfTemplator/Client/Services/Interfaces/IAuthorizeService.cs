using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IAuthorizeService : IService
    {
        Task Login(LoginParameters loginParameters);
        Task Register(RegisterParameters registerParameters);
        Task Logout();
        Task<UserInfo> GetUserInfo();
    }
}
