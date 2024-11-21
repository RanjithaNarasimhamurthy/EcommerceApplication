using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpViewModel vm)
	{
		InitializeComponent();
        BindingContext=vm;
	}
    public async void SignInButton_Clicked(object sender, EventArgs e)
    {

        var vm = new LoginPageViewModel();
        await Navigation.PushAsync(new LoginPage(vm));
    }
}