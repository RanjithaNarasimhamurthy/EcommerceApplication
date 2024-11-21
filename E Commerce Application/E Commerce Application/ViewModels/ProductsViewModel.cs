using CommunityToolkit.Mvvm.ComponentModel;
using E_Commerce_Application.Services;
using E_Commerce_Application.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using E_Commerce_Application.Views;

namespace E_Commerce_Application.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductsService _productsService;

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
        private double _avgRating;
        public double AverageRating
        {
            get => _avgRating;
            set => SetProperty(ref _avgRating, value);
        }

        [ObservableProperty]
        private ObservableCollection<string> _wishlistProductIds = new ObservableCollection<string>();

        public IAsyncRelayCommand SearchProductsCommand { get; }
        //public IAsyncRelayCommand LoadProductsCommand { get; }
        public IRelayCommand<ProductResponse> ProductImageTappedCommand { get; }
        public ICommand ViewAllBrandsCommand { get; }
        public ICommand BrandSelectedCommand { get; }
        //public ICommand SetWishlistTrueByIdCommand { get; }
        //public ICommand SetWishlistFalseByIdCommand { get; }
        public ICommand ToggleWishlistCommand { get; }

        private bool _hasProducts;
        public bool HasProducts
        {
            get => _hasProducts;
            set => SetProperty(ref _hasProducts, value);
        }

        [ObservableProperty]
        private bool _isLoading;

        public ProductsViewModel()
        {
            _productsService = new ProductsService();
            //LoadProductsCommand = new AsyncRelayCommand(LoadProductsAsync);
            SearchProductsCommand = new AsyncRelayCommand<string>(SearchProductsAsync);
            ViewAllBrandsCommand = new Command(UpdateDistinctBrands);
            BrandSelectedCommand = new Command<string>(async (brand) => await FilterBrandSelectionAsync(brand));
            ProductImageTappedCommand = new RelayCommand<ProductResponse>(OnProductImageTapped);
            //SetWishlistTrueByIdCommand = new Command<string>(async (id) => await SetWishistTrueById(id));
            //SetWishlistFalseByIdCommand = new Command<string>(async (id) => await SetWishistFalseById(id));

            ToggleWishlistCommand = new Command<ProductResponse>(async (product) => await ToggleWishlistAsync(product));

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
        }



        public async Task LoadProductsAsync()
        {
            IsLoading = true;

            try
            {
                var products = await _productsService.GetProductsAsync();
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







        public async Task LoadAllProductsAsync()
        {
            IsLoading = true;
            var loadingTask = Task.Run(async () =>
            {
                var aProducts = await _productsService.GetProductsAsync();
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

        private async Task SearchProductsAsync(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    await LoadProductsAsync();
                    return;
                }

                var products = await _productsService.GetProductsAsync();
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

        private async Task FilterBrandSelectionAsync(string brand)
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
                    await LoadProductsAsync();
                }
                else
                {
                    var products = await _productsService.GetProductsAsync();
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
    }
}