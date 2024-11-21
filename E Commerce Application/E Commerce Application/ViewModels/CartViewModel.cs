using E_Commerce_Application.Models;
using E_Commerce_Application.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace E_Commerce_Application.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        public event EventHandler ProceedToCheckoutRequested;
        private readonly Uri BaseUrl = new Uri("http://10.0.2.2:5135/api/Cart");

        public ICommand DeleteCommand { get; private set; }
        public ICommand FavoriteCommand { get; private set; }
        public ICommand ClearCartCommand { get; private set; }
        public ICommand ProceedToCheckoutCommand { get; private set; }

        private ObservableCollection<CartResponseModel> _allProductDataList;
        public ObservableCollection<CartResponseModel> AllProductDataList
        {
            get { return _allProductDataList; }
            set
            {
                _allProductDataList = value;
                OnPropertyChanged(nameof(AllProductDataList));
                OnPropertyChanged(nameof(IsCartEmpty));
            }
        }

        private bool _isLoaded = false;
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set
            {
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }

        public bool IsCartEmpty => _allProductDataList == null || _allProductDataList.Count == 0;

        public CartViewModel()
        {
            _httpClient = new HttpClient();
            DeleteCommand = new Command<CartResponseModel>(DeleteProduct);
            //FavoriteCommand = new Command<CartResponseModel>(FavoriteProduct);
            ClearCartCommand = new Command(ClearCart);
            ProceedToCheckoutCommand = new Command(ProceedToCheckout);
            _httpClient.BaseAddress = BaseUrl;
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await PopulateData();
        }

        private async Task PopulateData()
        {
            try
            {
                var userId = Preferences.Get("UserId", 0);
                HttpResponseMessage response = await _httpClient.GetAsync($"{BaseUrl}/GetCombinedProductDetails/GetCombinedProductDetails/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();


                    var responseObject = JsonConvert.DeserializeObject<CombinedProductDetailsResponse>(data);


                    if (responseObject?.Value != null)
                    {
                        AllProductDataList = new ObservableCollection<CartResponseModel>(responseObject.Value);
                        IsLoaded = true;
                        OnPropertyChanged(nameof(IsCartEmpty));
                    }
                    else
                    {

                        await Shell.Current.DisplayAlert("Error", "No product details found.", "OK");
                    }
                }
                else
                {

                    await Shell.Current.DisplayAlert("Error", "Failed to load product details.", "OK");
                }
            }
            catch (Exception ex)
            {

                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }







        private async void DeleteProduct(CartResponseModel product)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}/Delete/{product.IntCartItemId}");

                if (response.IsSuccessStatusCode)
                {
                    AllProductDataList.Remove(product);
                    OnPropertyChanged(nameof(IsCartEmpty));
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }



        private async void ClearCart()
        {
            try
            {
                var userId = Preferences.Get("UserId", 0);
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}/ClearCart/ClearAll/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    AllProductDataList.Clear();
                    OnPropertyChanged(nameof(IsCartEmpty));
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void ProceedToCheckout()
        {
            try
            {
                var availableProducts = AllProductDataList.Where(p => p.QuantityInStock > 0).ToList();

                var checkoutPage = new CheckoutView(availableProducts);

                ProceedToCheckoutRequested?.Invoke(this, EventArgs.Empty);

                await Shell.Current.Navigation.PushAsync(checkoutPage);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task RefreshData()
        {
            await PopulateData();
        }
    }
}
