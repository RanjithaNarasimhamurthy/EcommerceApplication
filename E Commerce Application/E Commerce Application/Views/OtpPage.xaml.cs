using Newtonsoft.Json;
using System.Text;

namespace E_Commerce_Application.Views;

public partial class OtpPage : ContentPage
{
    private string _phone;

    public OtpPage(string phone)
    {
        InitializeComponent();
        _phone = phone;
    }


    private async void OnVerifyOtpClicked(object sender, EventArgs e)
    {
        string otp = $"{OtpDigit1.Text}{OtpDigit2.Text}{OtpDigit3.Text}{OtpDigit4.Text}{OtpDigit5.Text}{OtpDigit6.Text}";

        if (otp.Length != 6 || !otp.All(char.IsDigit))
        {
            await DisplayAlert("Error", "Please enter a valid 6-digit OTP.", "OK");
            return;
        }

        bool success = await VerifyOtpAsync(_phone, otp);
        if (success)
        {
            await DisplayAlert("Success", "OTP verified successfully.", "OK");
            await Navigation.PushAsync(new ResetPasswordPage(_phone));
        }
        else
        {
            await DisplayAlert("Error", "Invalid OTP. Please try again.", "OK");
        }
    }
    private void OnOtpDigitTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = (Entry)sender;
        if (!string.IsNullOrEmpty(entry.Text) && entry.Text.Length == 1)
        {
            switch (entry)
            {
                case Entry _ when entry == OtpDigit1:
                    OtpDigit2.Focus();
                    break;
                case Entry _ when entry == OtpDigit2:
                    OtpDigit3.Focus();
                    break;
                case Entry _ when entry == OtpDigit3:
                    OtpDigit4.Focus();
                    break;
                case Entry _ when entry == OtpDigit4:
                    OtpDigit5.Focus();
                    break;
                case Entry _ when entry == OtpDigit5:
                    OtpDigit6.Focus();
                    break;
                case Entry _ when entry == OtpDigit6:
                    OtpDigit6.Unfocus();
                    break;
            }
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }

    private async Task<bool> VerifyOtpAsync(string phone, string otp)
    {
        try
        {
            var httpClient = new HttpClient();
            var requestBody = new { PhoneNumber = phone, Otp = otp };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiUrl = "http://10.0.2.2:5135/api/otp/verifyotp";

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