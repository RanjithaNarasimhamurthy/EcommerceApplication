using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using E_Commerce_Application.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace E_Commerce_Application.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _userName = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private bool _rememberMe;
        [ObservableProperty]
        private bool _isPassword = true;
        [ObservableProperty]
        private string _eyeIconSource;
        //readonly ILoginService loginService = new LoginService();

        private readonly ILoginService _loginService;

        public LoginPageViewModel()
        {
            _loginService = new LoginService();
            LoadCredentials();
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            EyeIconSource = "visible.png";
        }
        public IRelayCommand TogglePasswordVisibilityCommand { get; }

        [RelayCommand]
        public async void Login()
        {
            try
            {
                //if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                //{
                if (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password))
                {
                    User user = await _loginService.Login(UserName, Password);
                    if (user == null)
                    {
                        await Shell.Current.DisplayAlert("Error", "Username/Password is incorrect", "Ok");
                        return;
                    }
                    if (RememberMe)
                    {
                        SaveCredentials();
                    }
                    else
                    {
                        ClearCredentials();
                    }
                    if (Preferences.ContainsKey(nameof(App.user)))
                    {
                        Preferences.Remove(nameof(App.user));
                    }
                    string userDetails = JsonConvert.SerializeObject(user);
                    Preferences.Set(nameof(App.user), userDetails);
                    App.user = user;
                    //AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
                    if (user.strRole == "Admin")
                    {
                        await Shell.Current.GoToAsync(nameof(Views.AdminPage));
                        //var adminPageViewModel = new AdminPageViewModel();
                        //await Shell.Current.Navigation.PushAsync(new AdminPage(adminPageViewModel));
                    }
                    else if (user.strRole == "Vendor")
                    {
                        await Shell.Current.GoToAsync(nameof(Views.ProductsPage));

                    }
                    //await Shell.Current.GoToAsync(nameof(Views.Category));
                    else 
                    { 
                        await Shell.Current.GoToAsync("//Category"); 
                    }
                    
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "All fields required", "Ok");
                    return;
                }

                //}
                //else
                //{
                //    await Shell.Current.DisplayAlert("Error", "No Internet Access", "Ok");
                //    return;
                //}

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return;
            }
        }

        private void SaveCredentials()
        {
            SecureStorage.SetAsync(nameof(UserName), UserName);
            SecureStorage.SetAsync(nameof(Password), Password);
        }

        private void ClearCredentials()
        {
            SecureStorage.Remove(nameof(UserName));
            SecureStorage.Remove(nameof(Password));
        }

        private async void LoadCredentials()
        {
            UserName = await SecureStorage.GetAsync(nameof(UserName));
            Password = await SecureStorage.GetAsync(nameof(Password));
            RememberMe = !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        private void TogglePasswordVisibility()
        {
            IsPassword = !IsPassword;
            EyeIconSource = IsPassword ? "visible.png" : "hide.png";
        }

    }
}
