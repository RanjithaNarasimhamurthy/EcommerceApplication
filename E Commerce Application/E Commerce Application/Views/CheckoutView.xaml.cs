using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CheckoutView : ContentPage
{
    private readonly CheckoutViewModel _viewModel;

    public CheckoutView(List<CartResponseModel> selectedItems)
    {
        InitializeComponent();
        _viewModel = new CheckoutViewModel(selectedItems);
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

    }


}