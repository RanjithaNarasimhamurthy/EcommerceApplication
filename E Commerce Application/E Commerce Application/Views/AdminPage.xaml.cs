using E_Commerce_Application.ViewModels;
using System.ComponentModel;

namespace E_Commerce_Application.Views;

public partial class AdminPage : ContentPage
{
    private readonly AdminPageViewModel _viewModel;

    public AdminPage(AdminPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        //_viewModel.LoadVendorsCommand.Execute(null);
        NoVendorsLabel.IsVisible = _viewModel.Vendors.Count == 0;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadVendorsAsync();        
    }

    private async void Logout_Tapped(object sender, TappedEventArgs e)
    {
        bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to logout?", "No", "Yes");
        if (!confirmLogout)
        {
           App.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }

    private async void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(AdminPageViewModel.Vendors))
        {
            NoVendorsLabel.IsVisible = _viewModel.Vendors.Count == 0;
        }
    }
}