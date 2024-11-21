using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;
using Newtonsoft.Json;

namespace E_Commerce_Application.Views;

public partial class ProductsPage : ContentPage
{
    Uri baseAddress = new Uri("http://10.0.2.2:5135");
    private readonly HttpClient _httpClient;
    private List<Models.Category> _categories;
    private List<SubCategory> _subCategories;
    private List<Product> _productList;

    public ProductsPage()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = baseAddress;
        InitializeComponent();
        LoadCategoriesData();
        LoadProducts();
    }

    public async Task LoadProducts(string categoryId = null, string subCategoryId = null)
    {
        try
        {
            overlay.IsVisible = true;
            ProductListView.IsEnabled = false;
            _productList = new List<Product>();
            var vendorId = Preferences.Get("UserId",0).ToString();
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Product/GetProductList?vendorId=" + vendorId);
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                _productList = JsonConvert.DeserializeObject<List<Product>>(data);
            }

            //if (string.IsNullOrEmpty(categoryId))
            //{
            //	await DisplayAlert("Alert", "Select Filter", "Ok");
            //	_productList = _productList.Where(p => p.CategoryId == categoryId).ToList();
            //}

            if (!string.IsNullOrEmpty(subCategoryId))
            {
                int subcategoryId = int.Parse(subCategoryId);
                _productList = _productList.Where(p => p.intSubCategoryId == subcategoryId).ToList();
            }

            ProductListView.ItemsSource = _productList;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
        }
        finally
        {
            overlay.IsVisible = false;
            ProductListView.IsEnabled = true;
        }
    }

    public async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            var productId = (button?.CommandParameter as Product).strProductId;
            Product product = new Product();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"Product/GetProductDetailsById?id={productId}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(data);
            }

            await Navigation.PushAsync(new DetailsPage(product));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
        }
    }

    public async void Delete_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var productId = (button?.CommandParameter as Product)?.strProductId;

        if (productId == null)
        {
            await DisplayAlert("Error", "Product not found", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this product?", "Yes", "No");
        if (confirm)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"Product/DeleteProduct?ProductId={productId}");
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Product deleted successfully", "OK");
                await Navigation.PushAsync(new ProductsPage());
            }
            else
            {
                await DisplayAlert("Error", "Error deleting product", "OK");
            }
        }
    }


    public async void Add_Product(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPage());
    }

    public async void LoadCategoriesData()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Product/GetCategoryList");
            CategoriesViewModel categoriesViewModel = new CategoriesViewModel();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                categoriesViewModel = JsonConvert.DeserializeObject<CategoriesViewModel>(data);
                _categories = categoriesViewModel.Categories;
                _subCategories = categoriesViewModel.SubCategories;
                CategoryPicker.ItemsSource = _categories;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
        }
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var selectedCategory = CategoryPicker.SelectedItem as Models.Category;
        if (selectedCategory != null)
        {
            SubCategoryPicker.ItemsSource = _subCategories.Where(sc => sc.intCategoryId == selectedCategory.intCategoryId).ToList();
        }
    }

    private async void OnApplyFilter(object sender, EventArgs e)
    {
        try
        {
            var selectedCategory = CategoryPicker.SelectedItem as Models.Category;
            var selectedSubCategory = SubCategoryPicker.SelectedItem as SubCategory;

            string categoryId = selectedCategory.intCategoryId + "";
            string subCategoryId = selectedSubCategory.intSubCategoryId + "";

            await LoadProducts(categoryId, subCategoryId);
        }
        catch (NullReferenceException ex)
        {
            await DisplayAlert("Alert", "Filters cannot be null", "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "OK");
        }
    }
    public async void onRefreshClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductsPage());
    }

    private async void Logout_Tapped(object sender, TappedEventArgs e)
    {
        bool confirmLogout = await DisplayAlert("Logout", "Are you sure you want to logout?", "No", "Yes");
        if (!confirmLogout)
        {
            //App.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
