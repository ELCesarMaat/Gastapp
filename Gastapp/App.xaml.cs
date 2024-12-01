using Gastapp.Data;
using Gastapp.Models;
using Gastapp.Pages;
using Gastapp.Services.Navigation;
using Gastapp.Services.User;

namespace Gastapp
{
    public partial class App : Application
    {
        INavigationService _navigationService;
        UserService _userService;
        private GastappDbContext _dbContext;
        public App(INavigationService navigation, UserService userService, GastappDbContext dbContext)
        {
            
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzU2OTkzNkAzMjM3MmUzMDJlMzBTTGd3clVvZWIrTUpFd2kzSVVpTDk0clpyc0orMTdpRTdpSG10cnFzc3ZFPQ==");
            MainPage = new AppShell();
            _navigationService = navigation;
            _userService = userService;
            _dbContext = dbContext;
        }

        protected override async void OnStart()
        {
            //string token = UserSession.GetToken();
            //if(!string.IsNullOrEmpty(token))
            //{
            //    await ValidateSession(token);

            //}

            if (!IsFirstTime())
            {
                await _navigationService.NavigateToAsync($"//TodayPage");
            }

            base.OnStart();
        }

        private async Task ValidateSession(string token)
        {
            var response = await _userService.RefreshTokenAsync(token);
            if (response.Data != null)
            {
                var userData = response.Data;
                await _navigationService.NavigateToAsync("//TodayPage");
                UserSession.SetSession(userData.Token, userData.Email, userData.UserId);
            }
            else
            {
                UserSession.ClearSession();
            }
        }

        private bool IsFirstTime()
        {
            return !_dbContext.Categories.Any() && !UserSession.IsUserNameSaved();
        }
    }
}
