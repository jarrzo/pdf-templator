using pdfTemplator.Shared.Constants.Localization;

namespace pdfTemplator.Client.Settings
{
    public record ClientPreference
    {
        public bool IsDarkMode { get; set; }
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";
    }
}