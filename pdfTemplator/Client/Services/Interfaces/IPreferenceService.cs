using pdfTemplator.Client.Settings;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IPreferenceService : IService
    {
        Task SetPreference(ClientPreference preference);

        Task<ClientPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}