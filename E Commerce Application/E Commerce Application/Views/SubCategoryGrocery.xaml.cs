using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class SubCategoryGrocery : ContentPage
{
	public SubCategoryGrocery(SubCategoryViewModel vm)
	{
		InitializeComponent();
        BindingContext=vm;
        vm.LoadSubCategoriesCommand.Execute(null);
    }

    



    private async void NavigateToProductframe1Page(object sender, EventArgs e)
    {
        frame1.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        int subCategoryId = 3;
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        //await Shell.Current.GoToAsync("//ProductPage");
    }
    private async void NavigateToProductframe2Page(object sender, EventArgs e)
    {
        frame2.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        int subCategoryId = 4;
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        //await Shell.Current.GoToAsync("//ProductPage");
    }
    private async void NavigateToProductframe3Page(object sender, EventArgs e)
    {
        frame3.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        int subCategoryId = 5;
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        //await Shell.Current.GoToAsync("//ProductPage");
    }
    private async void NavigateToProductframe4Page(object sender, EventArgs e)
    {
        frame4.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        int subCategoryId = 6;
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        //await Shell.Current.GoToAsync("//ProductPage");
    }
    private async void NavigateToProductframe5Page(object sender, EventArgs e)
    {
        frame5.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        int subCategoryId = 7;
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        //await Shell.Current.GoToAsync("//ProductPage");
    }
    private async void NavigateToProductframe6Page(object sender, EventArgs e)
    {
        frame6.BackgroundColor = Color.FromRgba(224, 224, 224, 255);
        int subCategoryId = 8;
        await Navigation.PushAsync(new ProductBySubCategoryIdPage(subCategoryId));
        //await Shell.Current.GoToAsync("//ProductPage");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        frame1.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        frame2.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        frame3.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        frame4.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        frame5.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
        frame6.BackgroundColor = Color.FromRgba(255, 255, 255, 255);
    }
}