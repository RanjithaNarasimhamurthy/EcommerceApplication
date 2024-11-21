using E_Commerce_Application.ViewModels;
using E_Commerce_Application.Models;
namespace E_Commerce_Application.Views;

public partial class AddressPage : ContentPage
{
    private readonly AddressViewModel _viewModel;

    public AddressPage(AddressViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _viewModel.LoadAddressesCommand.Execute(null); // Trigger the loading of addresses
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadAddressesCommand.Execute(null);
    }

    private async void AddAddress_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddAddressPage());
    }

    private async void EditAddress_OnClicked(object sender, EventArgs e)
    {
        var label = sender as Label;
        var address = label.BindingContext as Address;

        var updateAddressPage = new UpdateAddressPage();
        var viewModel = new AddressViewModel();
        await viewModel.InitializeForEditAsync(address.intAddressId);
        updateAddressPage.BindingContext = viewModel;

        await Navigation.PushAsync(updateAddressPage);
    }
    private async void RemoveAddress_OnClicked(object sender, EventArgs e)
    {
        var tappedLabel = (Label)sender;
        var address = (Address)tappedLabel.BindingContext;

        // Access the ViewModel
        var viewModel = (AddressViewModel)BindingContext;

        try
        {
            await viewModel.RemoveAddress(address);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to remove address: {ex.Message}");
        }
    }
    private async void SetDefaultAddress_OnClicked(object sender, EventArgs e)
    {
        var label = sender as Label;
        if (label != null && label.BindingContext is Address address)
        {
            var viewModel = BindingContext as AddressViewModel;
            if (viewModel != null)
            {
                await viewModel.SetDefaultAddressAsync(address);
            }
        }
    }
}