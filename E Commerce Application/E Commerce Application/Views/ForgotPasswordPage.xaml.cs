using E_Commerce_Application.ViewModels;
using Newtonsoft.Json;
using System.Text;
using System.Windows.Input;

namespace E_Commerce_Application.Views;

public partial class ForgotPasswordPage : ContentPage
{
    public Command GoBackCommand { get; set; }

    public ForgotPasswordPage()
    {
        InitializeComponent();
        GoBackCommand = new Command(async () => await GoBack());
    }
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
    private async void OnResetPasswordClicked(object sender, EventArgs e)
    {
        string phone = PhoneEntry.Text;

        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlert("Error", "Please enter your phone number.", "OK");
            return;
        }

        bool isOtpSent = await SendOtpAsync(phone);
        if (isOtpSent)
        {

            await Navigation.PushAsync(new OtpPage(phone));
        }
        else
        {
            await DisplayAlert("Error", "Phone number is not verified. Please try again.", "OK");
        }
    }

    private async Task<bool> SendOtpAsync(string phone)
    {
        try
        {
            var httpClient = new HttpClient();
            var requestBody = new { PhoneNumber = phone };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiUrl = "http://10.0.2.2:5135/api/OTP/sendotp";

            var response = await httpClient.PostAsync(apiUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    private async void OnSignInClicked(object sender, TappedEventArgs e)
    {
        //var vm = new LoginPageViewModel();
        //await Navigation.PushAsync(new LoginPage(vm));
        await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void SignIn(object sender, EventArgs e)
    {
        var vm = new SignUpViewModel();
        await Navigation.PushAsync(new SignUpPage(vm));
    }

}