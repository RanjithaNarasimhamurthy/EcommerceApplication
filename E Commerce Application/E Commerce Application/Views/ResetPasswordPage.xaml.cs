using E_Commerce_Application.ViewModels;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace E_Commerce_Application.Views;

public partial class ResetPasswordPage : ContentPage
{
    private string _phone;

    public ResetPasswordPage(string phone)
    {
        InitializeComponent();
        _phone = phone;
    }
    private async void OnResetClicked(object sender, EventArgs e)
    {
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        string validationMessage = ValidatePassword(password, confirmPassword);

        if (!string.IsNullOrEmpty(validationMessage))
        {
            await DisplayAlert("Error", validationMessage, "OK");
            return;
        }

        bool success = await ResetPasswordAsync(_phone, password);
        if (success)
        {
            await DisplayAlert("Success", "Password reset successfully.", "OK");

            var vm = new LoginPageViewModel();
            await Navigation.PushAsync(new LoginPage(vm));
        }
        else
        {
            await DisplayAlert("Error", "Failed to reset password. Please try again.", "OK");
        }
    }
    private string ValidatePassword(string newPassword, string confirmPassword)
    {
        if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
        {
            return "Password and Confirm Password fields cannot be empty.";
        }

        if (newPassword.Length < 8 || !Regex.IsMatch(newPassword, @"[A-Z]") || !Regex.IsMatch(newPassword, @"[a-z]") || !Regex.IsMatch(newPassword, @"[0-9]") || !Regex.IsMatch(newPassword, @"[\W_]"))
        {
            return "Password must meet the following criteria:\n- Be at least 8 characters long\n- Contain at least one uppercase letter\n- Contain at least one lowercase letter\n- Contain at least one digit\n- Contain at least one special character.";
        }

        if (newPassword != confirmPassword)
        {
            return "Password and Confirm Password fields does not match.";
        }

        return string.Empty;
    }


    private async Task<bool> ResetPasswordAsync(string phone, string password)
    {
        try
        {
            var httpClient = new HttpClient();
            var requestBody = new { longContactNo = phone, strNewPassword = password };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiUrl = "http://10.0.2.2:5135/api/Account/resetpassword";

            var response = await httpClient.PostAsync(apiUrl, content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
}