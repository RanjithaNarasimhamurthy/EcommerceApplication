using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class ProductViewPage : ContentPage
{
    private readonly ProductDetailViewModel _viewModel;
    private readonly string _productId;
    public ProductViewPage(string productID)
    {
        InitializeComponent();
        _productId = productID;
        _viewModel = new ProductDetailViewModel(productID);
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadWishlistProductIds();
        
    }

    private async void OnRightArrowTapped(object sender, TappedEventArgs e)
    {
        try
        {
            //ProductsViewModel productsViewModel = new ProductsViewModel();
            //await Shell.Current.Navigation.PushAsync(new ProductPage(productsViewModel));
            await Shell.Current.GoToAsync("//ProductPage");
        }
        catch
        {
            await Shell.Current.DisplayAlert("Error", "Navigation Failed", "OK");
        }
    }
    private void SizePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedSizePicker = (Picker)sender;
        if (selectedSizePicker.SelectedIndex != -1)
        {
            string size = selectedSizePicker.Items[selectedSizePicker.SelectedIndex];
            _viewModel.SelectedProductSize = size;
        }
    }
    private void ColorPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedColorPicker = (Picker)sender;
        if (selectedColorPicker.SelectedIndex != -1)
        {
            string color = selectedColorPicker.Items[selectedColorPicker.SelectedIndex];
            _viewModel.SelectedProductColor = color;
        }
    }
    private void OnHeartClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ProductDetailViewModel viewModel)
        {
            var product = viewModel.ProductDetail;
            if (product == null)
            {
                System.Diagnostics.Debug.WriteLine("ProductDetail is null.");
                return;
            }

            if (button.Source is FileImageSource imageSource)
            {
                string selectedHeartImage = "heartselected";
                string deselectedHeartImage = "heart";

                if (imageSource.File == selectedHeartImage)
                {
                    button.Source = deselectedHeartImage;
                }
                else
                {
                    button.Source = selectedHeartImage;
                }

                viewModel.ToggleWishlistCommand.Execute(product);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ImageButton Source is not a FileImageSource.");
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Sender is not an ImageButton or BindingContext is not ProductDetailViewModel.");
        }
    }
    private async void OnFeedbackClicked(object sender, TappedEventArgs e)
    {
        var vm = new FeedbackViewModel();
        await Navigation.PushAsync(new FeedbackListPage(vm, _viewModel.ProductDetail.strProductId));
    }
}