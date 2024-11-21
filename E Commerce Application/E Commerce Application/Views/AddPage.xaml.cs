using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace E_Commerce_Application.Views;

public partial class AddPage : ContentPage
{
    Uri baseAddress = new Uri("http://10.0.2.2:5135");
    public readonly HttpClient _httpClient;
    public ObservableCollection<ImageSource> _selectedImages = new ObservableCollection<ImageSource>();
    public List<FileResult> _selectedImageFiles = new List<FileResult>();

    public List<Models.Category> _categories = new List<Models.Category>();
    public List<SubCategory> _subCategories = new List<SubCategory>();
    public CategoriesViewModel CategoriesViewModel = new CategoriesViewModel();
    public List<ImageModel> _productImages = new List<ImageModel>();
    //public Command GoBackCommand { get; set; }

    public AddPage()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = baseAddress;

        //LoadCategoryList();
        InitializeComponent();
        BindingContext = this;


        //_categories = new List<Category>
        //	{
        //		new Category { intCategoryId = 1, strCategoryName = "Electronics" },
        //		new Category { intCategoryId = 2, strCategoryName = "Clothing" }
        //	};

        //_subCategories = new List<SubCategories>
        //	{
        //		new SubCategories { intSubCategoryId = 1, strSubCategoryName = "Mobile Phones", intCategoryId = 1 },
        //		new SubCategories { intSubCategoryId = 2, strSubCategoryName = "Laptops", intCategoryId = 1 },
        //		new SubCategories { intSubCategoryId = 3, strSubCategoryName = "Men's Wear", intCategoryId = 2 },
        //		new SubCategories { intSubCategoryId = 4, strSubCategoryName = "Women's Wear", intCategoryId = 2 }
        //	};

        LoadCategoriesData();

        ImagesCollectionView.ItemsSource = _productImages;
        //GoBackCommand = new Command(async () => await GoBack());
    }



    public async void LoadCategoriesData()
    {
        try
        {

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Product/GetCategoryList");
            CategoriesViewModel CategoriesList = new CategoriesViewModel();
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                CategoriesViewModel = JsonConvert.DeserializeObject<CategoriesViewModel>(data);
                _categories = CategoriesViewModel.Categories;
                _subCategories = CategoriesViewModel.SubCategories;
                CategoryPicker.ItemsSource = _categories;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alert", ex.Message, "ok");
        }
    }
    //public ObservableCollection<ImageSource> SelectedImages => _selectedImages;

    private bool IsImageFile(string fileName)
    {
        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
        string fileExtension = Path.GetExtension(fileName).ToLower();
        return imageExtensions.Contains(fileExtension);
    }

    private async void OnSelectImagesClicked(object sender, EventArgs e)
    {
        try
        {
            var results = await FilePicker.Default.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select Images"
            });

            if (results != null && results.Any())
            {
                _selectedImageFiles.Clear();
                _selectedImages.Clear();

                foreach (var result in results)
                {
                    if (!IsImageFile(result.FileName))
                    {
                        await DisplayAlert("Alert", $"The file {result.FileName} is not a valid image file.", "OK");
                        continue;
                    }
                    using (var stream = await result.OpenReadAsync())
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                        _productImages.Add(new ImageModel
                        {
                            ImageBytes = imageBytes,
                            ImageSource = imageSource,
                            FileName = result.FileName
                        });
                    }
                }
                ImagesCollectionView.ItemsSource = null;
                ImagesCollectionView.ItemsSource = _productImages;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to select images: " + ex.Message, "OK");
        }
    }



    //private async void OnSubmitClicked(object sender, EventArgs e)
    //{
    //    var VendorId = Preferences.Get("UserId",0).ToString();
    //    try
    //    {
    //        var selectedCategory = CategoryPicker.SelectedItem as Models.Category;
    //        var selectedSubCategory = SubCategoryPicker.SelectedItem as SubCategory;
    //        string Id = "VnProd_";
    //        Id += VendorId + "_";

    //        HttpResponseMessage httpResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Product/GetMaxProductId?Id=" + VendorId).Result;
    //        if (httpResponse.IsSuccessStatusCode)
    //        {
    //            string data = httpResponse.Content.ReadAsStringAsync().Result;
    //            if (data == "")
    //            {
    //                data = "0";
    //            }
    //            int intIdVal = int.Parse(data.Split('_').Last()) + 1;
    //            Id += "" + intIdVal;
    //            //MaxProductId = JsonConvert.DeserializeObject<>(data);

    //        }
    //        Product productDetails = new Product
    //        {

    //            strProductId = Id,
    //            intSubCategoryId = selectedSubCategory.intSubCategoryId,
    //            strProductVendorId = VendorId,
    //            strProductName = ProductNameEntry.Text,
    //            strProductDescription = DescriptionEditor.Text,
    //            strBrand = BrandEntry.Text,
    //            dcMRP = decimal.TryParse(MrpEntry.Text, out var price) ? price : 0,
    //            //intRating = 0,
    //            IsVisible = true,
    //            IsAvailable = true,
    //            dtCreated_On = DateTime.Now,
    //            dtUpdated_On = DateTime.Now,
    //            ProductImageDetails = _productImages.Select(img => new ProductImages
    //            {
    //                strProductId = Id,
    //                strImageName = img.FileName,
    //                vbImage = img.ImageBytes,
    //                strProductSize = SizeEntry.Text,
    //                strProductColor = ColorEntry.Text,
    //                dcProductPrice = decimal.TryParse(PriceEntry.Text, out var price) ? price : 0,
    //                intQuantityInStock = int.TryParse(QuantityEntry.Text, out var quantity) ? quantity : 0,
    //                dtCreated_On = DateTime.Now,
    //                dtUpdated_On = DateTime.Now
    //            }).ToList()
    //        };

    //        var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "product/AddProductList", productDetails);
    //        if (response.IsSuccessStatusCode)
    //        {
    //            await DisplayAlert("Success", "Product added successfully", "OK");
    //        }
    //        else if (response.ReasonPhrase == "Conflict")
    //        {
    //            await DisplayAlert("Error", "Product Already Exists", "OK");
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", "Failed to add product", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
    //    }

    //    await Navigation.PushAsync(new ProductsPage());
    //}



    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (SubCategoryPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "SubCategory is required", "OK");
            return;
        }
        if (ProductNameValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Product Name is required", "OK");
            return;
        }
        if (DescriptionValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Description is required", "OK");
            return;
        }
        if (BrandValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Brand is required", "OK");
            return;
        }
        if (SizeValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Size is required", "OK");
            return;
        }
        if (MRPValidator.IsNotValid)
        {
            await DisplayAlert("Error", "MRP is required", "OK");
            return;
        }
        if (PriceValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Price is required", "OK");
            return;
        }
        if (ColorValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Color is required", "OK");
            return;
        }
        if (QuantityValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Quantity is required", "OK");
            return;
        }
        if (_productImages.Count == 0)
        {
            await DisplayAlert("Error", "Images are required", "OK");
            return;
        }
        var VendorId = Preferences.Get("UserId", 0).ToString();
        try
        {
            var selectedCategory = CategoryPicker.SelectedItem as Models.Category;
            var selectedSubCategory = SubCategoryPicker.SelectedItem as SubCategory;
            string Id = "VnProd_";
            Id += VendorId + "_";

            HttpResponseMessage httpResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "Product/GetMaxProductId?Id=" + VendorId).Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                string data = httpResponse.Content.ReadAsStringAsync().Result;
                if (data == "")
                {
                    data = "0";
                }
                int intIdVal = int.Parse(data.Split('_').Last()) + 1;
                Id += "" + intIdVal;
                //MaxProductId = JsonConvert.DeserializeObject<>(data);

            }
            Product productDetails = new Product
            {

                strProductId = Id,
                intSubCategoryId = selectedSubCategory.intSubCategoryId,
                strProductVendorId = VendorId,
                strProductName = ProductNameEntry.Text,
                strProductDescription = DescriptionEditor.Text,
                strBrand = BrandEntry.Text,
                dcMRP = decimal.TryParse(MrpEntry.Text, out var price) ? price : 0,
                //intRating = 0,
                IsVisible = true,
                IsAvailable = true,
                dtCreated_On = DateTime.Now,
                dtUpdated_On = DateTime.Now,
                ProductImageDetails = _productImages.Select(img => new ProductImages
                {
                    strProductId = Id,
                    strImageName = img.FileName,
                    vbImage = img.ImageBytes,
                    strProductSize = SizeEntry.Text,
                    strProductColor = ColorEntry.Text,
                    dcProductPrice = decimal.TryParse(PriceEntry.Text, out var price) ? price : 0,
                    intQuantityInStock = int.TryParse(QuantityEntry.Text, out var quantity) ? quantity : 0,
                    dtCreated_On = DateTime.Now,
                    dtUpdated_On = DateTime.Now
                }).ToList()
            };

            var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "product/AddProductList", productDetails);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Product added successfully", "OK");
            }
            else if (response.ReasonPhrase == "Conflict")
            {
                await DisplayAlert("Error", "Product Already Exists", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to add product", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
        }

        await Navigation.PushAsync(new ProductsPage());
    }


    public void LoadCategoryList()
    {
        List<Models.Category> _categories = new List<Models.Category>();
        HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"Product/GetCategories").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            _categories = JsonConvert.DeserializeObject<List<Models.Category>>(data);

        }
        //CategoryPicker.ItemsSource = _categories;
    }
    private void OnCategorySelected(object sender, EventArgs e)
    {
        var selectedCategory = CategoryPicker.SelectedItem as Models.Category;
        if (selectedCategory != null)
        {
            SubCategoryPicker.ItemsSource = _subCategories
                .Where(sc => sc.intCategoryId == selectedCategory.intCategoryId)
                .ToList();
        }
    }
    public async void onBackClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductsPage());
    }

}