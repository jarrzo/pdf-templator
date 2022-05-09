namespace pdfTemplator.Server.Models
{
    public class SmtpOptions
    {
        public const string Smtp = "Smtp";

        public string Host { get; set; } = null!;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
