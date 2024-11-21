using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(UserViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        //vm.LoadProfileCommand.Execute(null);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is UserViewModel viewModel)
        {
            viewModel.LoadProfileCommand.Execute(null);
        }
    }
    private async void LogOut(object sender, EventArgs e)
    {
        //var vm = new LoginPageViewModel();
        //await Navigation.PushAsync(new LoginPage(vm));
        bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to logout?", "No", "Yes");
        if (!confirmLogout)
        {
            App.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        //await Shell.Current.GoToAsync("//LoginPage");
    }

    private async void AddressButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AddressPage");
        //var viewModel = new AddressViewModel();
        //await Navigation.PushAsync(new AddressPage(viewModel));
    }
    private async void Logout_Tapped(object sender, TappedEventArgs e)
    {
        bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to logout?", "No", "Yes");
        if (!confirmLogout)
        {
            //App.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}