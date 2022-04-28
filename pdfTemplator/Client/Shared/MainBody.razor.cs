using Microsoft.AspNetCore.Components;

namespace pdfTemplator.Client.Shared
{
    public partial class MainBody
    {
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
    }
}
