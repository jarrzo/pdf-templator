using pdfTemplator.Shared.Models;

namespace pdfTemplator.Server.Models
{
    public static class ApplicationUserMapper
    {
        public static UserInfo ToUserInfo(this ApplicationUser user)
        {
            return new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}