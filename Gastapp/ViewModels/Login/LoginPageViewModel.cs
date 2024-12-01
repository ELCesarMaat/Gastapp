using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Gastapp.Pages;
using Gastapp.Services.Navigation;
using Gastapp.Services.User;

namespace Gastapp.ViewModels.Login
{
    public partial class LoginPageViewModel(INavigationService navigationService, UserService userService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly UserService _userService = userService;

        [ObservableProperty] private string _email = string.Empty;
        [ObservableProperty] private string _password = string.Empty;

        [RelayCommand]
        async void Login()
        {
            var loginResponse = await _userService.LoginAsync(Email, Password);
            var loginData = loginResponse.Data;
            if (loginResponse.Code == 101 && loginData != null)
            {
                UserSession.SetSession(loginData.Token, loginData.Email, loginData.UserId);
                await _navigationService.NavigateToAsync($"//TodayPage");
            }
        }
    }
}
