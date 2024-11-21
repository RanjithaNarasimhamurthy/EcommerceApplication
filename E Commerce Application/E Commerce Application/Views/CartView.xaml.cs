using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class CartView : ContentPage
{

    private CartViewModel _viewModel;
    public CartView()
    {
        InitializeComponent();
        _viewModel = new CartViewModel();
        BindingContext = _viewModel;
    }
    private void SwipeItem_Invoked(object sender, EventArgs e)
    {

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshData();
    }



    private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        double value = e.NewValue;
    }
}