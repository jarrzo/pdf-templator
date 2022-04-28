namespace pdfTemplator.Shared.Models
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = null!;
        public Dictionary<string, string>? ExposedClaims { get; set; }
    }
}
