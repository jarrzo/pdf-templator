using Microsoft.AspNetCore.Components;
using pdfTemplator.Client.Services.Identity;

namespace pdfTemplator.Client.Shared
{
    public partial class MainBody
    {
        [Inject] private IdentityAuthenticationStateProvider authStateProvider { get; set; } = null!;
        [Parameter] public RenderFragment ChildContent { get; set; } = null!;
        private bool _drawerOpen = true;
        [Parameter] public EventCallback OnDarkModeToggle { get; set; }
        private char FirstLetterOfUsername { get; set; }

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
        }

        public async Task Logout()
        {
            await authStateProvider.Logout();
            _navigationManager.NavigateTo("/login");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            var state = await authStateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user != null && user.Identity != null && user.Identity.Name != null)
            {
                FirstLetterOfUsername = user.Identity.Name[0];
            }
        }
    }
}
