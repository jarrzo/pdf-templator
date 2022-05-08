using MudBlazor;

namespace pdfTemplator.Client.Services.Interfaces
{
    public interface IClientPreferenceService : IPreferenceService
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
}