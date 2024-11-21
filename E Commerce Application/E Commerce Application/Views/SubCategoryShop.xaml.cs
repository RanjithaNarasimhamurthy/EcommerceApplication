using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class SubCategoryShop : ContentPage
{
	public SubCategoryShop(SubCategoryViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        vm.LoadSubCategoriesCommand.Execute(null);
    }
    private async void NavigateToProductPage(object sender, EventArgs e)
    {
        int subCategoryId = 1; 
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
    }
    private async void NavigateToProductPage1(object sender, EventArgs e)
    {
        int subCategoryId = 2; 
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
    }
    private async void NavigateToAllProductPage(object sender, EventArgs e)
    {
        //int subCategoryId = 2;
        //await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        await Shell.Current.GoToAsync("//ProductPage");
    }


}