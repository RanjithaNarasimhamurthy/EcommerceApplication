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
    public partial class WishlistProductViewModel : ObservableObject
    {
        private readonly IProductsService _productsService;

        private ObservableCollection<ProductResponse> _products;
        public ObservableCollection<ProductResponse> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }
        private bool _hasWishlistProducts;
        public bool HasWishlistProducts
        {
            get => _hasWishlistProducts;
            set => SetProperty(ref _hasWishlistProducts, value);
        }
        [ObservableProperty]
        private bool _isLoading;
        public IAsyncRelayCommand LoadWishlistProductsCommand { get; }
        public IAsyncRelayCommand SetWishlistTrueByIdCommand { get; }
        public IAsyncRelayCommand SetWishlistFalseByIdCommand { get; }
        public IRelayCommand<ProductResponse> ProductImageTappedCommand { get; }

        public WishlistProductViewModel()
        {
            _productsService = new ProductsService();
            Products = new ObservableCollection<ProductResponse>();
            LoadWishlistProductsCommand = new AsyncRelayCommand(LoadWishlistProductsAsync);
            SetWishlistTrueByIdCommand = new AsyncRelayCommand<string>(SetWishistTrueById);
            SetWishlistFalseByIdCommand = new AsyncRelayCommand<string>(SetWishistFalseById);
            ProductImageTappedCommand = new RelayCommand<ProductResponse>(OnProductImageTapped);
        }

        public async Task LoadWishlistProductsAsync()
        {
            IsLoading = true;
            try
            {
                int userId = Preferences.Get("UserId", 0);
                var wishlistProducts = await _productsService.GetWishlistProductsAsync(userId);
                //var wishlistProducts = products
                //    .OrderBy(p => p.strProductName).Where(p => p.IsWishlist == true)
                //    .ToList();

                foreach (var product in wishlistProducts)
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
                }

                Products = new ObservableCollection<ProductResponse>(wishlistProducts);
                HasWishlistProducts = Products.Any();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SetWishistTrueById(string id)
        {
            await _productsService.SetWishlistProductTrueByIdAsync(id);
        }

        private async Task SetWishistFalseById(string id)
        {
            await _productsService.SetWishlistProductFalseByIdAsync(id);
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
