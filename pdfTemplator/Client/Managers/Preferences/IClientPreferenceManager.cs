using MudBlazor;
using pdfTemplator.Shared.Managers;

namespace pdfTemplator.Client.Managers.Preferences
{
    public interface IClientPreferenceManager : IPreferenceManager
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
}