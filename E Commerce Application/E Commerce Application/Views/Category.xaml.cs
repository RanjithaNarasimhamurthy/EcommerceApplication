using E_Commerce_Application.Services;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class Category : ContentPage
{
	public Category(CategoryViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm.LoadCategoriesCommand.Execute(null);
    }
    private async void NavigateToSubCategoryShopPage(object sender, EventArgs e)
    {
        stackLayout1.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        var categoryId = 4;

        var subCategoryViewModel = new SubCategoryViewModel(categoryId);
        await Navigation.PushAsync(new SubCategoryShop(subCategoryViewModel));
        
    }
    private async void NavigateToSubCategoryGroceryPage(object sender, EventArgs e)
    {
        stackLayout2.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        var categoryId = 5;
        var subCategoryViewModel = new SubCategoryViewModel(categoryId);
        await Navigation.PushAsync(new SubCategoryGrocery(subCategoryViewModel));
        
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        stackLayout1.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        stackLayout2.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
    }
    private async void OnMenuClicked(object sender, EventArgs e)
    {
        OrderViewModel orderViewModel = new OrderViewModel();
        await Navigation.PushAsync(new OrderPage(orderViewModel));
    }
}