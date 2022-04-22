using pdfTemplator.Shared.Settings;
using pdfTemplator.Shared.Wrapper;

namespace pdfTemplator.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}