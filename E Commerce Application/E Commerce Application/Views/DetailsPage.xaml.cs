using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace E_Commerce_Application.Views;

public partial class DetailsPage : ContentPage
{
    Uri baseAddress = new Uri("http://10.0.2.2:5135");
    private readonly HttpClient _httpClient;

    private List<FileResult> _selectedImageFiles = new List<FileResult>();
    private ObservableCollection<ImageSource> _selectedImages = new ObservableCollection<ImageSource>();

    ProductViewModel viewModel = new ProductViewModel();

    private List<ImageModel> _productImages = new List<ImageModel>();

    public DetailsPage()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = baseAddress;
        InitializeComponent();
        ToolbarItems.Clear();
    }

    public DetailsPage(Product productDetails)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = baseAddress;

        viewModel.ProductDetails = productDetails;
        InitializeComponent();

        BindingContext = viewModel;

        LoadFilters();
    }

    private void LoadFilters()
    {
        var colors = viewModel.ProductDetails.ProductImagesobj.Select(p => p.strProductColor).Distinct().ToList();
        var sizes = viewModel.ProductDetails.ProductImagesobj.Select(p => p.strProductSize).Distinct().ToList();

        foreach (var color in colors)
        {
            viewModel.AvailableColors.Add(color);
        }

        foreach (var size in sizes)
        {
            viewModel.AvailableSizes.Add(size);
        }

        viewModel.SelectedColor = viewModel.AvailableColors.FirstOrDefault();
        viewModel.SelectedSize = viewModel.AvailableSizes.FirstOrDefault();
    }


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
                    var stream = await result.OpenReadAsync();

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
                ImagesCollectionView.ItemsSource = null;
                ImagesCollectionView.ItemsSource = _productImages;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to select images: " + ex.Message, "OK");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (SizeValidator.IsNotValid)
        {
            await DisplayAlert("Error", "Size is required", "OK");
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
        try
        {
            var x = viewModel.ProductDetails.strProductId;
            List<ProductImages> productImageDetails = _productImages.Select(img => new ProductImages
            {
                strProductId = viewModel.ProductDetails.strProductId,
                strImageName = img.FileName,
                vbImage = img.ImageBytes,
                strProductSize = SizeEntry.Text,
                strProductColor = ColorEntry.Text,
                dcProductPrice = decimal.TryParse(PriceEntry.Text, out var price) ? price : 0,
                intQuantityInStock = int.TryParse(QuantityEntry.Text, out var quantity) ? quantity : 0,
                dtCreated_On = DateTime.Now
            }).ToList();


            var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "product/AddSubProduct", productImageDetails);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Success", "Product Added successfully", "OK");
            }
            else if (response.ReasonPhrase == "Conflict")
            {
                await DisplayAlert("Error", "Product Already Exists", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to submit product", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to submit product: " + ex.Message, "OK");
        }
        await Navigation.PushAsync(new ProductsPage());

    }
    public async void onBackClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductsPage());
    }
    private void OnPreviousButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CarouselView carouselView)
        {
            if (carouselView.Position > 0)
            {
                carouselView.Position -= 1;
            }
        }
    }

    private void OnNextButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is CarouselView carouselView)
        {
            if (carouselView.Position < carouselView.ItemsSource.Cast<object>().Count() - 1)
            {
                carouselView.Position += 1;
            }
        }
    }


}
