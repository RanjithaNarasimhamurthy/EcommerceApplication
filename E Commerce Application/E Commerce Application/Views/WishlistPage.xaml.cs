using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class WishlistPage : ContentPage
{
    private readonly WishlistProductViewModel _viewModel = new WishlistProductViewModel();
    public WishlistPage(WishlistProductViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    private async void OnHomeTapped(object sender, TappedEventArgs e)
    {
        try
        {
            //ProductsViewModel productsViewModel = new ProductsViewModel();
            //await Shell.Current.Navigation.PushAsync(new ProductPage(productsViewModel));
            ProductsViewModel productsViewModel = new ProductsViewModel();
            await Shell.Current.GoToAsync(nameof(ProductPage), true, new Dictionary<string, object>
            {
                ["BindingContext"] = productsViewModel
            });
        }
        catch
        {
            await Shell.Current.DisplayAlert("Error", "Navigation Failed", "OK");
        }
    }

    private async void OnHeartIconTapped(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ProductResponse product)
        {
            if (button.Source is FileImageSource fileImageSource && fileImageSource.File == "heartselected")
            {
                button.Source = (FileImageSource)ImageSource.FromFile("heart.png");
                await _viewModel.SetWishlistFalseByIdCommand.ExecuteAsync(product.strProductId);
            }
            else
            {
                button.Source = (FileImageSource)ImageSource.FromFile("heartselected.png");
                await _viewModel.SetWishlistTrueByIdCommand.ExecuteAsync(product.strProductId);
            }
            ((WishlistProductViewModel)BindingContext).LoadWishlistProductsCommand.ExecuteAsync(null);
        }
    }
}