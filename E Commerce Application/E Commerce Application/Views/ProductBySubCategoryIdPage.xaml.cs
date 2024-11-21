using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class ProductBySubCategoryIdPage : ContentPage
{
    private readonly ProductBySubCategoryViewModel _viewModel;
    private List<Frame> selectedFrames = new List<Frame>();
    private readonly int _subCategoryId;

    public ProductBySubCategoryIdPage(int subCategoryId)
    {
        InitializeComponent();
        _subCategoryId = subCategoryId;
        _viewModel = new ProductBySubCategoryViewModel(subCategoryId);
        BindingContext = _viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadWishlistProductIds();
        await _viewModel.LoadProductsAsync(_subCategoryId);
        _viewModel.UpdateDistinctBrands();
    }

    private void OnMenuTapped(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }


    private async void OnHeartIconTapped(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.BindingContext is ProductResponse product)
        {
            if (button.Source is FileImageSource imageSource)
            {
                string selectedHeartImage = "heartselected";
                string deselectedHeartImage = "heart";

                if (imageSource.File == selectedHeartImage)
                {
                    button.Source = deselectedHeartImage;
                    _viewModel.ToggleWishlistCommand.Execute(product);
                }
                else
                {
                    button.Source = selectedHeartImage;
                    _viewModel.ToggleWishlistCommand.Execute(product);
                }                
            }
            //await Navigation.PushAsync(new ProductBySubCategoryIdPage(_subCategoryId));

        }
    }

    //private void OnHeartIconTapped(object sender, EventArgs e)
    //{
    //    if (sender is ImageButton button && button.BindingContext is Product product)
    //    {
    //        _viewModel.ToggleWishlistCommand.Execute(product);
    //    }
    //}


    private async void OnViewAllTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is ProductBySubCategoryViewModel viewModel)
        {
            await viewModel.LoadAllProductsAsync(_subCategoryId);
            ViewAllLabel.TextColor = Colors.Purple;
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ProductBySubCategoryViewModel viewModel)
        {
            viewModel.SearchProductsCommand.Execute(e.NewTextValue);
        }
    }

    private void Frame_Tapped(object sender, EventArgs e)
    {
        if (sender is Frame selectedFrame)
        {
            Label label = selectedFrame.FindByName<Label>("LabelFrame");

            if (selectedFrames.Contains(selectedFrame))
            {
                selectedFrame.BackgroundColor = Color.FromHex("#F5F4FA");
                label.TextColor = Color.FromHex("#000000");
                selectedFrames.Remove(selectedFrame);
            }
            else
            {
                selectedFrame.BackgroundColor = Color.FromHex("#30aaba");
                label.TextColor = Color.FromHex("#FFFFFF");
                selectedFrames.Add(selectedFrame);
            }
            
        }
    }

    private async void OnShoppingCartClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//CartView");
    }

    private async void OnWishlistClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//WishlistPage");
    }
}