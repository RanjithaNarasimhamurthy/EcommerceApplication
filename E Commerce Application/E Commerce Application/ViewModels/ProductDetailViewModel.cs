using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace E_Commerce_Application.ViewModels
{
    public partial class ProductDetailViewModel : ObservableObject
    {
        private readonly IProductsService _productsService;
        private readonly IFeedbackService _feedbackService;
        private readonly ICartItemsService _cartItemsService;

        public IAsyncRelayCommand LoadProductByIdCommand { get; }
        //public ICommand SetWishlistTrueByIdCommand { get; }
        //public ICommand SetWishlistFalseByIdCommand { get; }
        public ICommand ToggleWishlistCommand { get; }
        public ICommand FilterImagesCommand { get; }
        public IRelayCommand LoadFeedbacksByProductIdCommand { get; }
        public IAsyncRelayCommand AddToCartCommand { get; }

        [ObservableProperty]
        private ObservableCollection<ProductResponse> _products = new ObservableCollection<ProductResponse>();

        private ProductResponse _productDetail;
        public ProductResponse ProductDetail
        {
            get => _productDetail;
            set => SetProperty(ref _productDetail, value);
        }
        private ObservableCollection<ProductImages> _dataImage;
        public ObservableCollection<ProductImages> DataImage
        {
            get => _dataImage;
            set => SetProperty(ref _dataImage, value);
        }
        private ObservableCollection<string> _productSizes;
        public ObservableCollection<string> ProductSizes
        {
            get => _productSizes;
            set => SetProperty(ref _productSizes, value);
        }
        private ObservableCollection<string> _productColors;
        public ObservableCollection<string> ProductColors
        {
            get => _productColors;
            set => SetProperty(ref _productColors, value);
        }
        public ObservableCollection<Feedback> Feedbacks { get; } = new ObservableCollection<Feedback>();
        private decimal _selectedProductPrice;
        public decimal SelectedProductPrice
        {
            get => _selectedProductPrice;
            set => SetProperty(ref _selectedProductPrice, value);
        }
        private string _selectedProductSize;
        public string SelectedProductSize
        {
            get => _selectedProductSize;
            set
            {
                if (SetProperty(ref _selectedProductSize, value))
                {
                    FilterImages(SelectedProductSize, SelectedProductColor);
                    UpdateSelectedProductPrice(DataImage);
                }
            }
        }
        private string _selectedProductColor;
        public string SelectedProductColor
        {
            get => _selectedProductColor;
            set
            {
                if (SetProperty(ref _selectedProductColor, value))
                {
                    FilterImages(SelectedProductSize, SelectedProductColor);
                    UpdateSelectedProductPrice(DataImage);

                    var sizesForSelectedColor = ProductDetail.ProductImages
                    .Where(pi => pi.strProductColor == SelectedProductColor)
                    .Select(pi => pi.strProductSize)
                    .Distinct()
                    .ToList();

                    ProductSizes = new ObservableCollection<string>(sizesForSelectedColor);
                }
            }
        }

        [ObservableProperty]
        private ObservableCollection<string> _wishlistProductIds = new ObservableCollection<string>();

        [ObservableProperty]
        private bool _isLoading;

        private double _averageRating;
        public double AverageRating
        {
            get => _averageRating;
            set => SetProperty(ref _averageRating, value);
        }
        private string _strReview;
        public string strReview
        {
            get => _strReview;
            set => SetProperty(ref _strReview, value);
        }

        private string _strUserName;
        public string strUserName
        {
            get => _strUserName;
            set => SetProperty(ref _strUserName, value);
        }
        private DateTime _dtUpdated_on;
        public DateTime dtUpdated_on
        {
            get => _dtUpdated_on;
            set => SetProperty(ref _dtUpdated_on, value);
        }
        private int _intRating;
        public int intRating
        {
            get => _intRating;
            set => SetProperty(ref _intRating, value);
        }
        private bool _isMRPVisible;
        public bool IsMRPVisible
        {
            get => _isMRPVisible;
            set => SetProperty(ref _isMRPVisible, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public ProductDetailViewModel(string ProductID)
        //{
        // _productsService = new ProductsService();
        // LoadProductByIdCommand = new AsyncRelayCommand(() => LoadProductByIdAsync(ProductID));
        // //SetWishlistTrueByIdCommand = new Command<string>(async (id) => await SetWishistTrueById(id));
        // //SetWishlistFalseByIdCommand = new Command<string>(async (id) => await SetWishistFalseById(id));
        // ToggleWishlistCommand = new Command<Product>(async (product) => await ToggleWishlistAsync(product));

        // LoadWishlistProductIds();

        // _feedbackService = new FeedbackService();
        // LoadFeedbacksByProductIdCommand = new RelayCommand(async () => await LoadFeedbacksByProductIdAsync(ProductID));

        // _cartItemsService = new CartItemsService();
        // AddToCartCommand = new AsyncRelayCommand(AddToCartAsync);

        // LoadProductByIdCommand.Execute(null);
        // LoadFeedbacksByProductIdCommand.Execute(null);
        //}
        public ProductDetailViewModel(string ProductID)
        {
            _productsService = new ProductsService();
            LoadProductByIdCommand = new AsyncRelayCommand(() => LoadProductByIdAsync(ProductID));
            ToggleWishlistCommand = new Command<ProductResponse>(async (product) => await ToggleWishlistAsync(product));

            _feedbackService = new FeedbackService();
            LoadFeedbacksByProductIdCommand = new AsyncRelayCommand(async () => await LoadFeedbacksByProductIdAsync(ProductID));

            _cartItemsService = new CartItemsService();
            AddToCartCommand = new AsyncRelayCommand(AddToCartAsync);

            Task.Run(async () => await Initialize(ProductID)).ConfigureAwait(false);
        }
        private async Task Initialize(string ProductID)
        {
            await LoadWishlistProductIds();
            //LoadProductByIdCommand.Execute(null);
            //LoadProductByIdCommand.Execute(ProductID);
            await LoadProductByIdAsync(ProductID);
            await LoadFeedbacksByProductIdAsync(ProductID);
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

        public async Task LoadProductByIdAsync(string productID)
        {
            IsLoading = true;
            try
            {
                var selectedProduct = await _productsService.GetProductByIdAsync(productID);

                if (selectedProduct != null)
                {
                    ProductDetail = selectedProduct;
                    DataImage = new ObservableCollection<ProductImages>(ProductDetail.ProductImages);

                    ExtractSizesAndColors(DataImage);
                    FilterImages(SelectedProductSize, SelectedProductColor);
                    CalculateAverageRating();
                    selectedProduct.bitIsWishlist = WishlistProductIds.Contains(selectedProduct.strProductId);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to load the selected product", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }


        private void UpdateSelectedProductPrice(ObservableCollection<ProductImages> productImages)
        {
            var selectedProductImage = productImages.FirstOrDefault(pi =>
            pi.strProductSize == SelectedProductSize &&
            pi.strProductColor == SelectedProductColor);

            if (selectedProductImage != null)
            {
                SelectedProductPrice = selectedProductImage.dcProductPrice;
                IsMRPVisible = true;
            }
            else if (ProductDetail != null)
            {
                SelectedProductPrice = ProductDetail.dcMRP;
                IsMRPVisible = false;
            }
        }

        private void ExtractSizesAndColors(ObservableCollection<ProductImages> productImages)
        {
            var distinctColors = productImages.Select(pi => pi.strProductColor).Distinct().ToList();

            if (distinctColors.Any())
            {
                ProductColors = new ObservableCollection<string>(distinctColors);
                if (SelectedProductColor == null || !ProductColors.Contains(SelectedProductColor))
                {
                    SelectedProductColor = ProductColors.First();
                }

                var sizesForSelectedColor = productImages
                .Where(pi => pi.strProductColor == SelectedProductColor)
                .Select(pi => pi.strProductSize)
                .Distinct()
                .ToList();

                ProductSizes = new ObservableCollection<string>(sizesForSelectedColor);
                FilterImages(null, SelectedProductColor);
            }
            else
            {
                ProductColors = new ObservableCollection<string>();
                ProductSizes = new ObservableCollection<string>();
                DataImage = new ObservableCollection<ProductImages>();
            }
        }

        private void FilterImages(string selectedSize, string selectedColor)
        {
            if (string.IsNullOrEmpty(selectedColor))
            {
                DataImage = new ObservableCollection<ProductImages>();
            }
            else if (string.IsNullOrEmpty(selectedSize))
            {
                DataImage = new ObservableCollection<ProductImages>(
                ProductDetail.ProductImages.Where(pi => pi.strProductColor == selectedColor));
            }
            else
            {
                DataImage = new ObservableCollection<ProductImages>(
                ProductDetail.ProductImages.Where(pi =>
                pi.strProductSize == selectedSize && pi.strProductColor == selectedColor));
            }
        }

        private void CalculateAverageRating()
        {
            if (ProductDetail.Feedbacks != null && ProductDetail.Feedbacks.Any())
            {
                if (ProductDetail.Feedbacks.Count == 1)
                {
                    AverageRating = (double)ProductDetail.Feedbacks.First().intRating;
                }
                else
                {
                    AverageRating = (double)ProductDetail.Feedbacks.Average(f => f.intRating);
                }
            }
            else
            {
                AverageRating = 0;
            }
        }

        private async Task LoadFeedbacksByProductIdAsync(string ProductID)
        {
            if (string.IsNullOrWhiteSpace(ProductID))
                return;
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByProductIdAsync(ProductID, true);
                Feedbacks.Clear();
                foreach (var feedback in feedbacks)
                {
                    Feedbacks.Add(feedback);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task AddToCartAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedProductSize) || string.IsNullOrEmpty(SelectedProductColor))
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a size and color.", "OK");
                    return;
                }
                if (ProductDetail == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Product details are missing.", "OK");
                    return;
                }

                var existingCartItem = await CartItemAlreadyExists();

                if (existingCartItem != null)
                {
                    existingCartItem.intQuantity += 1;
                    var updateResult = await _cartItemsService.UpdateCartItemQuantityAsync(existingCartItem);

                    if (updateResult)
                    {
                        await Shell.Current.DisplayAlert("Success", "Product quantity updated in cart", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Quantity is Out of Stock", "OK");
                    }
                }
                else
                {
                    var cartItem = new CartItem
                    {
                        strProductId = ProductDetail.strProductId,
                        intSubCategoryId = ProductDetail.intSubCategoryId,
                        strProductVendorId = ProductDetail.strProductVendorId,
                        strProductColor = SelectedProductColor,
                        strProductSize = SelectedProductSize,
                        dcProductPrice = SelectedProductPrice,
                        intQuantity = 1,
                        intUserId = Preferences.Get("UserId",0),
                        dtCreated_on = DateTime.Now,
                        dtUpdated_on = DateTime.Now
                    };

                    var res = await _cartItemsService.AddToCartAsync(cartItem);
                    if (res)
                    {
                        await Shell.Current.DisplayAlert("Success", "Product added to cart", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Out of Stock. Cannot add product to cart", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task<CartItem> CartItemAlreadyExists()
        {
            int userId = Preferences.Get("UserId",0);
            return await _cartItemsService.GetCartItemsAsync(ProductDetail.strProductId, SelectedProductColor, SelectedProductSize, userId);
        }

    }
}