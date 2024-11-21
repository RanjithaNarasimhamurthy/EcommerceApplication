using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class VendorDetails : ContentPage
{
    public VendorDetails(AdminPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}