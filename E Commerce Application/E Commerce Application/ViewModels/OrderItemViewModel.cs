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

namespace E_Commerce_Application.ViewModels
{
    public partial class OrderItemViewModel : BaseViewModel
    {
        private readonly IOrderItemService _orderItemService;
        private readonly IFeedbackService _feedbackService;

        [ObservableProperty]
        private ObservableCollection<OrderItem> _orderItems;

        private decimal _subtotal;
        public decimal Subtotal
        {
            get => _subtotal;
            set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _tax;
        public decimal Tax
        {
            get => _tax;
            set
            {
                if (_tax != value)
                {
                    _tax = value;
                    OnPropertyChanged();
                }
            }
        }

        //private int _rating;
        //public int Rating
        //{
        //    get => _rating;
        //    set
        //    {
        //        if (_rating != value)
        //        {
        //            _rating = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        public Command GoBackCommand { get; set; }
        public Command GoBackToOrderListCommand { get; set; }

        public OrderItemViewModel(int orderId)
        {
            _orderItemService = new OrderItemService();
            _feedbackService = new FeedbackService();

            _orderItems = new ObservableCollection<OrderItem>();
            LoadOrderItemsCommand = new RelayCommand(async () => await LoadOrderItemsAsync(orderId));
            LoadOrderItemsCommand.Execute(null);
            

            AddOrUpdateRatingCommand = new AsyncRelayCommand<Feedback>(AddOrUpdateRatingAsync);
            AddOrUpdateReviewCommand = new AsyncRelayCommand<Feedback>(AddOrUpdateReviewAsync);

            //LoadRatingCommand = new AsyncRelayCommand<string>(LoadRatingAsync);
            GoBackCommand = new Command(async () => await GoBack(orderId));
            GoBackToOrderListCommand = new Command(async () => await GoBackToOrderList());


        }

        public IRelayCommand LoadOrderItemsCommand { get; }
       
        public IAsyncRelayCommand<Feedback> AddOrUpdateRatingCommand { get; }
        public IAsyncRelayCommand<Feedback> AddOrUpdateReviewCommand { get; }
        //public IAsyncRelayCommand<string> LoadRatingCommand { get; }


        private async Task LoadOrderItemsAsync(int orderId)
        {
            try
            {
                var orderItems = await _orderItemService.GetOrderItemsByOrderIdAsync(orderId);
                OrderItems = new ObservableCollection<OrderItem>(orderItems);
                CalculateSubtotal();
                CalculateTax();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void CalculateSubtotal()
        {
            Subtotal = OrderItems.Sum(item => item.decItemAmount) + OrderItems.Sum(item => item.decItemAmount) * 0.10m;
        }
        private void CalculateTax()
        {
            Tax = OrderItems.Sum(item => item.decItemAmount) * 0.10m;
        }

        private async Task AddOrUpdateRatingAsync(Feedback feedback)
        {
            try
            {
                var updatedFeedback = await _feedbackService.AddOrUpdateRatingAsync(feedback);

                await Application.Current.MainPage.DisplayAlert("Success", "Rating updated successfully", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task AddOrUpdateReviewAsync(Feedback feedback)
        {
            try
            {
                var updatedFeedback = await _feedbackService.AddOrUpdateReviewAsync(feedback);

                await Application.Current.MainPage.DisplayAlert("Success", "Review updated successfully", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task GoBack(int orderId)
        {
            //await Shell.Current.GoToAsync("..");
            //await Shell.Current.Navigation.PushAsync(new OrderItemPage(orderId));
            var orderItemViewModel = new OrderItemViewModel(orderId);
            var orderItemsPage = new OrderItemPage(orderItemViewModel);
            await Shell.Current.Navigation.PushAsync(orderItemsPage);
        }

        private async Task GoBackToOrderList()
        {
            //var orderViewModel = new OrderViewModel();
            //var orderPage = new OrderPage(orderViewModel);
            //await Shell.Current.Navigation.PushAsync(orderPage);
            await Shell.Current.GoToAsync("//OrderPage");
        }

        //private async Task LoadRatingAsync(string productId)
        //{
        //    try
        //    {
        //        var userId = Preferences.Get("UserId", 0);
        //        var feedback = await _feedbackService.GetFeedbackAsync(productId, userId);

        //        if (feedback != null && feedback.intRating.HasValue)
        //        {
        //            Rating = feedback.intRating.Value;
        //        }
        //        else
        //        {
        //            Rating = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}

    }
}
