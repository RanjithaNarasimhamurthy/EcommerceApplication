using E_Commerce_Application.ViewModels;
using E_Commerce_Application.Views;

namespace E_Commerce_Application;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm)
	{
		InitializeComponent();
		BindingContext=vm;
	}
    private async void OnForgotPasswordClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new ForgotPasswordPage());
    }
    private async void SignUp(object sender, TappedEventArgs e)
    {
        var vm = new SignUpViewModel();
        await Navigation.PushAsync(new SignUpPage(vm));
    }
}