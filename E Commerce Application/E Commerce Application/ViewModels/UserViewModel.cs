using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public partial class UserViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        [ObservableProperty]
        private User _user;


        public IRelayCommand LoadProfileCommand { get; }
        public IRelayCommand UpdateProfileCommand { get; }

        public UserViewModel()
        {
            _userService = new UserService();
            LoadProfileCommand = new RelayCommand(async () => await LoadUserDataAsync());
            UpdateProfileCommand = new RelayCommand(async () => await UpdateUserDataAsync());


            LoadProfileCommand.Execute(null);
        }

        private async Task LoadUserDataAsync()
        {

            User = await _userService.GetUserAsync();

        }
        private async Task UpdateUserDataAsync()
        {
            try
            {
                User = await _userService.UpdateUserAsync(User);

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed to update user data: {ex.Message}");
            }
        }

    }
}
