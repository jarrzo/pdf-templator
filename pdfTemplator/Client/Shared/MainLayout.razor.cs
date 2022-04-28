using Microsoft.AspNetCore.Components;
using MudBlazor;
using pdfTemplator.Client.Settings;

namespace pdfTemplator.Client.Shared
{
    public partial class MainLayout
    {
        private MudTheme _currentTheme;
        private bool _rightToLeft = false;
        private bool _drawerOpen = true;
        [Parameter] public EventCallback OnDarkModeToggle { get; set; }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = BlazorHeroTheme.DefaultTheme;
            _currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();
        }

        private async Task DarkMode()
        {
            bool isDarkMode = await _clientPreferenceManager.ToggleDarkModeAsync();
            _currentTheme = isDarkMode
                ? BlazorHeroTheme.DefaultTheme
                : BlazorHeroTheme.DarkTheme;
        }
    }
}
