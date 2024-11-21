using E_Commerce_Application.Models;
using E_Commerce_Application.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public class OrderTrackingViewModel : BaseViewModel
    {
        private readonly Uri BaseUrl = new Uri("http://10.0.2.2:5135/api/Cart/GetOrderItemById/GetOrderItemById");
        private System.Timers.Timer refreshTimer;

        public ObservableCollection<TrackingStep> TrackingSteps { get; set; }
        public ObservableCollection<OrderedItem> GetItemDetails { get; set; }

        private bool isLoaded;

        public bool IsLoaded
        {
            get => isLoaded;
            set => SetProperty(ref isLoaded, value);
        }

        public event Action OrderStatusChanged;
        private int _orderItemId;
        private int _orderId;
        public Command GoBackCommand { get; set; }

        public OrderTrackingViewModel(int orderItemId, int orderId)
        {
            _orderId = orderId;
            _orderItemId = orderItemId;
            TrackingSteps = new ObservableCollection<TrackingStep>();

            GetItemDetails = new ObservableCollection<OrderedItem>();
            LoadOrderDetails();
            GoBackCommand = new Command(async () => await GoBack(_orderId));

            refreshTimer = new System.Timers.Timer(30000);
            refreshTimer.Elapsed += (sender, e) => RefreshData();
            refreshTimer.Start();
        }

        private async Task GoBack(int orderId)
        {
            var orderItemViewModel = new OrderItemViewModel(orderId);
            var orderItemsPage = new OrderItemPage(orderItemViewModel);
            await Shell.Current.Navigation.PushAsync(orderItemsPage);
        }



        public async Task LoadOrderDetails()
        {
            IsLoaded = false;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{BaseUrl}/{_orderItemId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<OrderedItem>(jsonResponse);

                        PopulateTrackingSteps(responseObject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching order details: {ex.Message}");
            }

            IsLoaded = true;
        }


        private void PopulateTrackingSteps(OrderedItem orderItem)
        {

            TrackingSteps.Clear();


            bool isCompleted = false;

            TrackingSteps.Add(new TrackingStep
            {
                StepName = "Order Processing",
                StepDescription = "Your order is being processed.",
                IsCompleted = (isCompleted = orderItem.strOrderStatus == "Order Processing" || orderItem.strOrderStatus == "Order Processed" || orderItem.strOrderStatus == "Product Dispatched" || orderItem.strOrderStatus == "Out for Delivery" || orderItem.strOrderStatus == "Delivered")
            });

            TrackingSteps.Add(new TrackingStep
            {
                StepName = "Order Processed",
                StepDescription = "Your order has been processed.",
                IsCompleted = (isCompleted = orderItem.strOrderStatus == "Order Processed" || orderItem.strOrderStatus == "Product Dispatched" || orderItem.strOrderStatus == "Out for Delivery" || orderItem.strOrderStatus == "Delivered")
            });

            TrackingSteps.Add(new TrackingStep
            {
                StepName = "Product Dispatched",
                StepDescription = "Your product has been dispatched.",
                IsCompleted = (isCompleted = orderItem.strOrderStatus == "Product Dispatched" || orderItem.strOrderStatus == "Out for Delivery" || orderItem.strOrderStatus == "Delivered")
            });

            TrackingSteps.Add(new TrackingStep
            {
                StepName = "Out for Delivery",
                StepDescription = "Your product is out for delivery.",
                IsCompleted = (isCompleted = orderItem.strOrderStatus == "Out for Delivery" || orderItem.strOrderStatus == "Delivered")
            });

            TrackingSteps.Add(new TrackingStep
            {
                StepName = "Delivered",
                StepDescription = "Your product has been delivered.",
                IsCompleted = orderItem.strOrderStatus == "Delivered"
            });


            for (int i = 0; i < TrackingSteps.Count; i++)
            {
                TrackingSteps[i].IsLastStep = i == TrackingSteps.Count - 1;
            }
        }

        public async Task RefreshData()
        {
            await LoadOrderDetails();
        }
    }

    public class TrackingStep : BaseViewModel
    {
        private bool isCompleted;
        public string StepName { get; set; }
        public string StepDescription { get; set; }
        public bool IsCompleted
        {
            get => isCompleted;
            set => SetProperty(ref isCompleted, value);
        }

        public bool IsLastStep { get; set; }
    }
}
