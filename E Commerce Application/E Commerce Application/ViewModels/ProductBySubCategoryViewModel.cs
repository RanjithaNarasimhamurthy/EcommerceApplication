using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using E_Commerce_Application.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace E_Commerce_Application.ViewModels
{
    public partial class ProductBySubCategoryViewModel : ObservableObject 
    { 
    private readonly IProductsService _productsService;
    private readonly ISubCategoryService _subCategoryService;

        [ObservableProperty]
    private ObservableCollection<ProductResponse> _products = new ObservableCollection<ProductResponse>();
    private ObservableCollection<string> _distinctBrands = new ObservableCollection<string>();
    private ObservableCollection<string> _selectedBrands = new ObservableCollection<string>();
    public ObservableCollection<string> DistinctBrands
    {
        get => _distinctBrands;
        set => SetProperty(ref _distinctBrands, value);
    }
    public ObservableCollection<string> SelectedBrands
    {
        get => _selectedBrands;
        set => SetProperty(ref _selectedBrands, value);
    }

    [ObservableProperty]
    private ObservableCollection<string> _wishlistProductIds = new ObservableCollection<string>();

    public IAsyncRelayCommand SearchProductsCommand { get; }
    public IRelayCommand<ProductResponse> ProductImageTappedCommand { get; }
    public ICommand ViewAllBrandsCommand { get; }
    public ICommand BrandSelectedCommand { get; }
    public ICommand ToggleWishlistCommand { get; }
    public Command GoBackCommand { get; set; }


        private bool _hasProducts;
        public bool HasProducts
        {
            get => _hasProducts;
            set => SetProperty(ref _hasProducts, value);
        }

        [ObservableProperty]
    private bool _isLoading;

        private double _averageRating;
        public double AverageRating
        {
            get => _averageRating;
            set => SetProperty(ref _averageRating, value);
        }
        
        public ProductBySubCategoryViewModel(int subCategoryId)
        {
        _productsService = new ProductsService();
            _subCategoryService = new SubCategoryService();
            SearchProductsCommand = new AsyncRelayCommand<string>(async (query) => await SearchProductsAsync(query,subCategoryId));
        ViewAllBrandsCommand = new Command(UpdateDistinctBrands);
        BrandSelectedCommand = new Command<string>(async (brand) => await FilterBrandSelectionAsync(brand, subCategoryId));
        ProductImageTappedCommand = new RelayCommand<ProductResponse>(OnProductImageTapped);

        ToggleWishlistCommand = new Command<ProductResponse>(async (product) => await ToggleWishlistAsync(product));

        GoBackCommand = new Command(async () => await GoBack(subCategoryId));
        LoadWishlistProductIds();
        }



    public async Task LoadWishlistProductIds()
    {
        try
        {
            int userId = Preferences.Get("UserId", 0);
            var wishlistProducts = await _productsService.GetWishlistProductsAsync(userId);
            WishlistProductIds = new ObservableCollection<string>(wishlistProducts.Select(p => p.strProductId));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task ToggleWishlistAsync(ProductResponse product)
    {
        if (WishlistProductIds.Contains(product.strProductId))
        {
            await _productsService.SetWishlistProductFalseByIdAsync(product.strProductId);
            WishlistProductIds.Remove(product.strProductId);
            product.bitIsWishlist = false;
        }
        else
        {
            await _productsService.SetWishlistProductTrueByIdAsync(product.strProductId);
            WishlistProductIds.Add(product.strProductId);
            product.bitIsWishlist = true;
        }

            var productToUpdate = Products.FirstOrDefault(p => p.strProductId == product.strProductId);
            if (productToUpdate != null)
            {
                productToUpdate.bitIsWishlist = product.bitIsWishlist;
            }
            
        }






    public async Task LoadProductsAsync(int subCategoryId)
    {
        IsLoading = true;

        try
        {
            var products = await _productsService.GetProductsBySubCategoryId(subCategoryId);
            var randomProducts = products.OrderBy(p => p.strProductName).Take(6).ToList();

            foreach (var product in randomProducts)
            {
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    product.ProductImages = new List<ProductImages> { product.ProductImages.First() };
                }
                    if (product.Feedbacks != null && product.Feedbacks.Any())
                    {
                        if (product.Feedbacks.Count == 1)
                        {
                            product.AverageRating = (double)product.Feedbacks.First().intRating;
                        }
                        else
                        {
                            product.AverageRating = (double)product.Feedbacks.Average(f => f.intRating);
                        }
                    }
                    else
                    {
                        product.AverageRating = 0;
                    }

                    product.bitIsWishlist = WishlistProductIds.Contains(product.strProductId);
            }

            Products = new ObservableCollection<ProductResponse>(randomProducts);
            UpdateDistinctBrands();
            HasProducts = Products.Any();
            }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to load some products. Please try again later.", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }







    public async Task LoadAllProductsAsync(int subCategoryId)
    {
        IsLoading = true;
        var loadingTask = Task.Run(async () =>
        {
            var aProducts = await _productsService.GetProductsBySubCategoryId(subCategoryId);
            var allProducts = aProducts.OrderBy(p => p.strProductName).ToList();
            Parallel.ForEach(allProducts, product =>
            {
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    product.ProductImages = new List<ProductImages> { product.ProductImages.First() };
                }
                if (product.Feedbacks != null && product.Feedbacks.Any())
                {
                    if (product.Feedbacks.Count == 1)
                    {
                        product.AverageRating = (double)product.Feedbacks.First().intRating;
                    }
                    else
                    {
                        product.AverageRating = (double)product.Feedbacks.Average(f => f.intRating);
                    }
                }
                else
                {
                    product.AverageRating = 0;
                }
                product.bitIsWishlist = WishlistProductIds.Contains(product.strProductId);
            });
            return allProducts;
        });
        try
        {
            var allproducts = await loadingTask;
            Products = new ObservableCollection<ProductResponse>(allproducts);
            UpdateDistinctBrands();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to load all products. Please try again later.", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SearchProductsAsync(string query,int subCategoryId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                await LoadProductsAsync(subCategoryId);
                return;
            }

            var products = await _productsService.GetProductsBySubCategoryId(subCategoryId);
            var filteredProducts = products
            .Where(p => p.strProductName.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
                if (!filteredProducts.Any())
                {
                    await Shell.Current.DisplayAlert("No Products Found", "No products match your search query.", "OK");
                    return;
                }
                foreach (var product in filteredProducts)
            {
                if (product.ProductImages != null && product.ProductImages.Any())
                {
                    product.ProductImages = new List<ProductImages> { product.ProductImages.First() };
                }
                    if (product.Feedbacks != null && product.Feedbacks.Any())
                    {
                        if (product.Feedbacks.Count == 1)
                        {
                            product.AverageRating = (double)product.Feedbacks.First().intRating;
                        }
                        else
                        {
                            product.AverageRating = (double)product.Feedbacks.Average(f => f.intRating);
                        }
                    }
                    else
                    {
                        product.AverageRating = 0;
                    }
                    product.bitIsWishlist = WishlistProductIds.Contains(product.strProductId);
                }
            Products = new ObservableCollection<ProductResponse>(filteredProducts);
            UpdateDistinctBrands();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to search products. Please try again later.", "OK");
        }
    }

    public void UpdateDistinctBrands()
    {
        var distinctBrands = _products
            .Where(p => !string.IsNullOrEmpty(p.strBrand))
            .Select(p => p.strBrand)
            .Distinct()
            .ToList();

        DistinctBrands = new ObservableCollection<string>(distinctBrands);
    }

    private async Task FilterBrandSelectionAsync(string brand,int subCategoryId)
    {
        try
        {
            if (SelectedBrands == null)
            {
                SelectedBrands = new ObservableCollection<string>();
            }

            if (SelectedBrands.Contains(brand))
            {
                SelectedBrands.Remove(brand);
            }
            else
            {
                SelectedBrands.Add(brand);
            }

            if (SelectedBrands.Count == 0)
            {
                await LoadProductsAsync(subCategoryId);
            }
            else
            {
                var products = await _productsService.GetProductsBySubCategoryId(subCategoryId);
                var filteredProducts = products
                    .Where(p => SelectedBrands.Contains(p.strBrand, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                foreach (var product in filteredProducts)
                {
                    if (product.ProductImages != null && product.ProductImages.Any())
                    {
                        product.ProductImages = new List<ProductImages> { product.ProductImages.First() };
                    }
                        if (product.Feedbacks != null && product.Feedbacks.Any())
                        {
                            if (product.Feedbacks.Count == 1)
                            {
                                product.AverageRating = (double)product.Feedbacks.First().intRating;
                            }
                            else
                            {
                                product.AverageRating = (double)product.Feedbacks.Average(f => f.intRating);
                            }
                        }
                        else
                        {
                            product.AverageRating = 0;
                        }
                        product.bitIsWishlist = WishlistProductIds.Contains(product.strProductId);
                    }
                Products = new ObservableCollection<ProductResponse>(filteredProducts);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", "Failed to load products. Please try again later.", "OK");
        }
    }

    private async void OnProductImageTapped(ProductResponse product)
    {
        if (product != null)
        {
            string productId = product.strProductId;
            await Shell.Current.Navigation.PushAsync(new ProductViewPage(productId));
        }
    }
        private async Task GoBack(int subCategoryId)
        {
            var categoryId = await _subCategoryService.GetCategoryIdBySubCategoryIdAsync(subCategoryId);

            if (categoryId == 4)
            {
                var subCategoryViewModel = new SubCategoryViewModel(categoryId.Value);
                await Shell.Current.Navigation.PushAsync(new SubCategoryShop(subCategoryViewModel));
            }
            else if (categoryId == 5)
            {
                var subCategoryViewModel = new SubCategoryViewModel(categoryId.Value);
                await Shell.Current.Navigation.PushAsync(new SubCategoryGrocery(subCategoryViewModel));
            }
            else
            {
                await Shell.Current.GoToAsync("//Category");
            }
        }
    }
}
