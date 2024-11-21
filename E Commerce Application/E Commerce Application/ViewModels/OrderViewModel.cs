using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public partial class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;

        [ObservableProperty]
        private ObservableCollection<Order> _orders;

        [ObservableProperty]
        private bool _hasOrderItems;

        [ObservableProperty]
        private bool _isLoading;

        public OrderViewModel()
        {
            _orderService = new OrderService();
            Orders = new ObservableCollection<Order>();
            Orders.CollectionChanged += Orders_CollectionChanged;
            LoadOrdersCommand = new RelayCommand(async () => await LoadOrdersAsync());
            //OrderTappedCommand = new RelayCommand<Order>(async (order) => await OnOrderTappedAsync(order));
            LoadOrdersCommand.Execute(null);
        }
        private void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasOrderItems = Orders.Count > 0;
        }
        //public ObservableCollection<Category> Categories
        //{
        //    get => _categories;
        //    set
        //    {
        //        _categories = value;
        //        OnPropertyChanged();
        //    }
        //}



        public IRelayCommand LoadOrdersCommand { get; }
        public IRelayCommand<Order> OrderTappedCommand { get; }

        private async Task LoadOrdersAsync()
        {
            IsLoading = true;
            try
            {
                var orders = await _orderService.GetOrdersSummaryByUserIdAsync();
                Orders = new ObservableCollection<Order>(orders);
                HasOrderItems = Orders.Count > 0;
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
        //private async Task OnOrderTappedAsync(Order order)
        //{
        //    if (order != null)
        //    {
        //        await Shell.Current.GoToAsync($"OrderDetailPage?OrderId={order.OrderId}");
        //    }
        //}
    }
}
