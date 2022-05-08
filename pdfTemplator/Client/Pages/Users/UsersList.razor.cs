using Microsoft.AspNetCore.Components;
using pdfTemplator.Client.Services.Models;
using pdfTemplator.Shared.Models;

namespace pdfTemplator.Client.Pages.Users
{
    public partial class UsersList
    {
        [Inject] private IUserService userService { get; set; } = null!;
        private List<UserInfo> _list = new();
        private string _searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetUsers();
        }

        private async Task GetUsers()
        {
            var response = await userService.GetAllAsync();
            if (response != null)
            {
                _list = response.Data.ToList();
            }
        }

        private bool Search(UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            return userInfo.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
