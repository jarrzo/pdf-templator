using pdfTemplator.Client.Settings;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Preferences
{
    public interface IPreferenceManager
    {
        Task SetPreference(ClientPreference preference);

        Task<ClientPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}