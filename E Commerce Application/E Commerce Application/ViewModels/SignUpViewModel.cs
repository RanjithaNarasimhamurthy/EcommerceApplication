using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public partial class SignUpViewModel : BaseViewModel
    {
        private readonly ISignUpService _signupService;
        [ObservableProperty]
        private string _name = string.Empty;
        [ObservableProperty]
        private string _email = string.Empty;
        [ObservableProperty]
        private string _phone = string.Empty;
        [ObservableProperty]
        private string _password = string.Empty;
        [ObservableProperty]
        private string _confirmPassword = string.Empty;
        [ObservableProperty]
        private string _eyeIconSource;
        [ObservableProperty]
        private string _confirmEyeIconSource;
        [ObservableProperty]
        private bool _isPassword = true;
        [ObservableProperty]
        private bool _isConfirmPassword = true;
        [ObservableProperty]
        private string _selectedRole="Customer";
        partial void OnSelectedRoleChanged(string value)
        {
            OnPropertyChanged(nameof(IsVendor));
        }
        [ObservableProperty]
        private List<string> _roles;
        [ObservableProperty]
        private string _BRNo = string.Empty;
        [ObservableProperty]
        private string _GSTNo = string.Empty;

        public SignUpViewModel()
        {
            _signupService = new SignUpService();
            Roles = new List<string> { "Customer", "Vendor" };
            SignUpCommand = new RelayCommand(async () => await SignUpAsync());
            TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
            ToggleConfirmPasswordVisibilityCommand = new RelayCommand(ToggleConfirmPasswordVisibility);
           
            EyeIconSource = "visible.png";
            ConfirmEyeIconSource = "visible.png";
            IsPassword = true;
        }

        
        public IRelayCommand SignUpCommand { get; }
        public IRelayCommand TogglePasswordVisibilityCommand { get; }
        public IRelayCommand ToggleConfirmPasswordVisibilityCommand { get; }
        public bool IsVendor => SelectedRole == "Vendor";

        private async Task SignUpAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(SelectedRole) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid email format", "OK");
                return;
            }
            
            if (await UserAlreadyExists())
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User already exists", "OK");
                return;
            }

            if (!IsPasswordStrong(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Password must be at least 8 characters long and contain upper case, lower case, digit, and special character, and should not contain any white space.", "OK"); 
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            //User newUser = new User
            //{
            //    strName = this.Name,
            //    strUserName = this.Email,
            //    strPassword = this.Password,
            //    strRole = this.SelectedRole,
            //    dtCreated_on = DateTime.Now,
            //    dtUpdated_on = DateTime.Now
            //};
            //if (long.TryParse(this.Phone, out long contactNumber) && Regex.IsMatch(this.Phone, @"^\d{10}$"))
            //{
            //    newUser.longContactNo = contactNumber;
            //}
            //else
            //{
            //    await Application.Current.MainPage.DisplayAlert("Invalid Phone Number", "Please enter a valid phone number.", "OK");
            //    return;
            //}

            if (SelectedRole == "Vendor")
            {
                Vendor newVendor = new Vendor
                {
                    strName = this.Name,
                    strUserName = this.Email,
                    strPassword = this.Password,
                    strBRNo = this.BRNo,
                    strGSTNo = this.GSTNo,
                    IsActive = true,
                    IsApproved = false,
                    dtCreated_on = DateTime.Now,
                    dtUpdated_on = DateTime.Now
                };

                if (long.TryParse(this.Phone, out long contactNumber) && Regex.IsMatch(this.Phone, @"^\d{10}$"))
                {
                    newVendor.longContactNo = contactNumber;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Phone Number", "Please enter a valid phone number.", "OK");
                    return;
                }

                try
                {
                    Vendor createdVendor = await _signupService.SignUpVendorAsync(newVendor);

                    if (createdVendor != null)
                    {
                        // Navigate to AdditionalInfoPage to collect extra details
                        //await Shell.Current.GoToAsync($"{nameof(AdditionalInfoPage)}?VendorId={createdVendor.intVendorId}");
                        await Application.Current.MainPage.DisplayAlert("Success", "Request sent successfully", "OK");
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Vendor signup failed. Please try again.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred during vendor signup: {ex.Message}", "OK");
                }
            }
            else
            {
                User newUser = new User
                {
                    strName = this.Name,
                    strUserName = this.Email,
                    strPassword = this.Password,
                    strRole = this.SelectedRole,
                    dtCreated_on = DateTime.Now,
                    dtUpdated_on = DateTime.Now
                };

                if (long.TryParse(this.Phone, out long contactNumber) && Regex.IsMatch(this.Phone, @"^\d{10}$"))
                {
                    newUser.longContactNo = contactNumber;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Invalid Phone Number", "Please enter a valid phone number.", "OK");
                    return;
                }

                try
                {
                    User createdUser = await _signupService.SignUpAsync(newUser);

                    if (createdUser != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "User created successfully", "OK");
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Signup failed. Please try again.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred during signup: {ex.Message}", "OK");
                }
            }
            //try
            //{
            //    User createdUser = await _signupService.SignUpAsync(newUser);

            //    if (createdUser != null)
            //    {
            //        await Application.Current.MainPage.DisplayAlert("Success", "User created successfully", "OK");
            //        await Shell.Current.GoToAsync(nameof(LoginPage));
            //    }
            //    else
            //    {
            //        await Application.Current.MainPage.DisplayAlert("Error", "Signup failed. Please try again.", "OK");
            //    }
            //}
            //catch(Exception ex) 
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred during signup: {ex.Message}", "OK");
            //}
        }

        private async Task<bool> UserAlreadyExists()
        {
            var existingUser = await _signupService.GetUserByEmailAndPhoneAsync(Email, Phone);
            return existingUser != null;
        }

        private bool IsPasswordStrong(string password)
        {
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");
            var hasDigits = new Regex(@"[0-9]+");
            var hasSpecialChar = new Regex(@"[\W]+");
            var hasWhiteSpace = new Regex(@"\s");

            return password.Length >= 8 &&
                   hasUpperCase.IsMatch(password) &&
                   hasLowerCase.IsMatch(password) &&
                   hasDigits.IsMatch(password) &&
                   hasSpecialChar.IsMatch(password) &&
                   !hasWhiteSpace.IsMatch(password);
        }

        private void TogglePasswordVisibility()
        {
            IsPassword = !IsPassword;
            EyeIconSource = IsPassword ? "visible.png" : "hide.png";
        }

        private void ToggleConfirmPasswordVisibility()
        {
            IsConfirmPassword = !IsConfirmPassword;
            ConfirmEyeIconSource = IsConfirmPassword ? "visible.png" : "hide.png";
        }

    }
}
