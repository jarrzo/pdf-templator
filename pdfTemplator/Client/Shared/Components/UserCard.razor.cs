using Microsoft.AspNetCore.Components;
using pdfTemplator.Client.Services.Identity;

namespace pdfTemplator.Client.Shared.Components
{
    public partial class UserCard
    {
        [Inject] private IdentityAuthenticationStateProvider authStateProvider { get; set; } = null!;
        [Parameter] public string Class { get; set; }
        private string Username { get; set; }
        private char FirstLetterOfUsername { get; set; }

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
            if (user != null)
            {
                FirstLetterOfUsername = user.Identity.Name[0];
                Username = user.Identity.Name;
                StateHasChanged();
            }
        }
    }
}