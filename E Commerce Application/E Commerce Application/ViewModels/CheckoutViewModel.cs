using E_Commerce_Application.Models;
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
    public class CheckoutViewModel : BaseViewModel
    {
        private readonly Uri BaseUrl = new Uri("http://10.0.2.2:5135/api/Cart");

        public ObservableCollection<CartResponseModel> SelectedProductDataList { get; private set; }
        public ObservableCollection<Order> OrderList { get; private set; }
        public ObservableCollection<OrderItem> OrderedItems { get; private set; }

        public decimal TotalPrice => SelectedProductDataList.Sum(p => ParsePrice(p.ProductPrice.ToString()) * p.QuantityInStock);

        private decimal ParsePrice(string priceString)
        {
            if (decimal.TryParse(priceString, out decimal price))
            {
                return price;
            }
            else
            {
                return 0;
            }
        }

        public decimal TaxAmount => TotalPrice * 0.10m;
        public decimal TotalAmount => TotalPrice + TaxAmount;

        public ICommand ProceedToPaymentCommand { get; private set; }

        private string _billingAddress;
        public string BillingAddress
        {
            get => _billingAddress;
            set => SetProperty(ref _billingAddress, value);
        }

        private string _shippingAddress;
        public string ShippingAddress
        {
            get => _shippingAddress;
            set => SetProperty(ref _shippingAddress, value);
        }

        private string _strPaymentMethod;
        public string strPaymentMethod
        {
            get => _strPaymentMethod;
            set => SetProperty(ref _strPaymentMethod, value);
        }

        public ObservableCollection<string> PaymentMethods { get; } = new ObservableCollection<string>
        {
            "Cash On Delivery",
            "UPI",
            "Card Payment"
        };

        //    public CheckoutViewModel(List<CartResponseModel> selectedItems)
        //    {
        //        //var availableItems = selectedItems.Where(item => item.IsAvailable).ToList();
        //        SelectedProductDataList = new ObservableCollection<CartResponseModel>(selectedItems);
        //        ProceedToPaymentCommand = new Command(async () => await ProceedToPayment());
        //        OrderList = new ObservableCollection<Order> { new Order() };
        //        OrderedItems = new ObservableCollection<OrderItem> { new OrderItem() };

        //        BillingAddress = "4th Floor, Anand Bhavan, Princess Street, Kalbadevi\nMumbai, Maharashtra\nPhone: 02222018558\nZip code: 400002\nCountry: India";
        //        ShippingAddress = "4th Floor, Anand Bhavan, Princess Street, Kalbadevi\nMumbai, Maharashtra\nPhone: 02222018558\nZip code: 400002\nCountry: India";
        //    }

        //    private async Task ProceedToPayment()
        //    {

        //        var orders = OrderList.Select(p => new Order
        //        {
        //            OrderId = 0,
        //            UserId = 101,
        //            //intAddressId = 1,
        //            strPaymentMethod = "Cash on delivery",
        //            dtOrderDate = DateTime.Now,
        //            decTotalAmount = TotalAmount,
        //            dtCreated_on = DateTime.Now,
        //            dtUpdated_on = DateTime.Now
        //        }).ToList();

        //        try
        //        {
        //            using (var client = new HttpClient())
        //            {
        //                var json = JsonConvert.SerializeObject(orders[0]);
        //                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //                var response = await client.PostAsync("http://10.0.2.2:5135/api/Cart/ProceedToOrder/ProceedToOrder", content);

        //                if (response.IsSuccessStatusCode)
        //                {

        //                    var recordResponse = await client.GetAsync($"{BaseUrl}/GetLatestOrderId");
        //                    if (recordResponse.IsSuccessStatusCode)
        //                    {
        //                        string data = await recordResponse.Content.ReadAsStringAsync();
        //                        int orderId = int.Parse(data);


        //                        var orderItems = SelectedProductDataList.Select(product => new OrderItem
        //                        {
        //                            ProductName = product.ProductName,
        //                            //BrandName = product.Brand,
        //                            decPrice = product.ProductPrice,
        //                            intQuantity = product.QuantityInStock,
        //                            strOrderStatus = "Pending",
        //                            strProductId = product.ProductId,
        //                            intOrderId = orderId
        //                        }).ToList();


        //                        json = JsonConvert.SerializeObject(orderItems);
        //                        content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //                        response = await client.PostAsync($"{BaseUrl}/PlaceOrder/PlaceOrder", content);

        //                        if (response.IsSuccessStatusCode)
        //                        {
        //                            await Shell.Current.DisplayAlert("Success", "Order placed successfully.", "OK");

        //                            await Shell.Current.Navigation.PopAsync();
        //                        }
        //                        else
        //                        {
        //                            await Shell.Current.DisplayAlert("Error", "Failed to place order items. Please try again later.", "OK");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        await Shell.Current.DisplayAlert("Error", "Failed to get the latest order ID.", "OK");
        //                    }
        //                }
        //                else
        //                {
        //                    await Shell.Current.DisplayAlert("Error", "Failed to create order. Please try again later.", "OK");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        //        }
        //    }
        //}

        public CheckoutViewModel(List<CartResponseModel> selectedItems)
        {
            var availableItems = selectedItems.Where(item => item.IsAvailable).ToList();
            SelectedProductDataList = new ObservableCollection<CartResponseModel>(availableItems);
            ProceedToPaymentCommand = new Command(async () => await ProceedToPayment());
            OrderList = new ObservableCollection<Order> { new Order() };
            OrderedItems = new ObservableCollection<OrderItem> { new OrderItem() };

            // Fetch the addresses
            _ = FetchAddresses();
        }

        private async Task<int?> FetchDefaultAddressId(int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"http://10.0.2.2:5135/api/Cart/GetAddressesByUserId/GetAddressesByUserId/{userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var addresses = JsonConvert.DeserializeObject<List<Address>>(json);
                        var defaultAddress = addresses.FirstOrDefault(a => a.IsDefault.GetValueOrDefault());

                        if (defaultAddress != null)
                        {
                            return defaultAddress.intAddressId;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to load addresses.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return null;
        }
        private async Task FetchAddresses()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    int userId = Preferences.Get("UserId", 0);
                    var response = await client.GetAsync($"http://10.0.2.2:5135/api/Cart/GetAddressesByUserId/GetAddressesByUserId/{userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var addresses = JsonConvert.DeserializeObject<List<Address>>(json);

                        var defaultAddress = addresses.FirstOrDefault(a => a.IsDefault.GetValueOrDefault());
                        if (defaultAddress != null)
                        {
                            BillingAddress = FormatAddress(defaultAddress);
                            ShippingAddress = FormatAddress(defaultAddress);
                        }
                    }
                    else
                    {
                        BillingAddress = "Failed to load address.";
                        ShippingAddress = "Failed to load address.";
                    }
                }
            }
            catch (Exception ex)
            {
                BillingAddress = $"Error: {ex.Message}";
                ShippingAddress = $"Error: {ex.Message}";
            }
        }

        private string FormatAddress(Address address)
        {
            return $"{address.strAddressLine1}, {address.strAddressLine2},{address.strCity}, {address.strState}\nPhone: {address.ContactNo}\nZip code: {address.intZipCode}\nCountry: India";
        }

        private async Task ProceedToPayment()
        {

            if (string.IsNullOrEmpty(strPaymentMethod))
            {
                await Shell.Current.DisplayAlert("Alert", "Please select a payment method.", "OK");
                return;
            }
            int userId = Preferences.Get("UserId", 0);
            int? defaultAddressId = await FetchDefaultAddressId(userId);

            var orders = OrderList.Select(p => new Order
            {
                OrderId = 0,
                intUserId = userId,
                intAddressId = defaultAddressId.Value,
                strPaymentMethod = strPaymentMethod,
                dtOrderDate = DateTime.Now,
                decTotalAmount = TotalAmount,
                dtCreated_on = DateTime.Now,
                dtUpdated_on = DateTime.Now
            }).ToList();

            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(orders[0]);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://10.0.2.2:5135/api/Cart/ProceedToOrder/ProceedToOrder", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var recordResponse = await client.GetAsync($"{BaseUrl}/GetLatestOrderId");
                        if (recordResponse.IsSuccessStatusCode)
                        {
                            string data = await recordResponse.Content.ReadAsStringAsync();
                            int orderId = int.Parse(data);

                            var orderItems = SelectedProductDataList.Select(product => new OrderedItem
                            { 
                                ProductName = product.ProductName,
                                Brand = product.Brand,
                                decPrice = product.ProductPrice,
                                intQuantity = product.QuantityInStock,
                                strSize = product.ProductSize,
                                strOrderStatus = "Pending",
                                strProductId = product.ProductId,
                                intOrderId = orderId,
                                dtCreated_on = DateTime.Now,
                                dtUpdated_on = DateTime.Now
                            }).ToList();

                            var orderRequest = new OrderRequest
                            {
                                UserId = userId,
                                OrderedItems = orderItems
                            };

                            json = JsonConvert.SerializeObject(orderRequest);

                            json = JsonConvert.SerializeObject(orderRequest);
                            content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                            response = await client.PostAsync($"{BaseUrl}/PlaceOrder/PlaceOrder/{userId}", content);

                            if (response.IsSuccessStatusCode)
                            {
                                await Shell.Current.DisplayAlert("Success", "Order placed successfully.", "OK");
                                await Shell.Current.Navigation.PopAsync();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", "Failed to place order items. Please try again later.", "OK");
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", "Failed to get the latest order ID.", "OK");
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Failed to create order. Please try again later.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
