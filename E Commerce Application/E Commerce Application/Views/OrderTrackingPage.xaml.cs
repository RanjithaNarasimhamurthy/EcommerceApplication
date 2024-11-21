using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class OrderTrackingPage : ContentPage
{
    private readonly OrderTrackingViewModel _viewModel;
    private int _orderItemId;
    private int _orderId;

    public OrderTrackingPage(int orderItemId, int orderId)
    {
        InitializeComponent();

        _orderItemId = orderItemId;
        _orderId = orderId;
        _viewModel = new OrderTrackingViewModel(orderItemId, orderId);
        BindingContext = _viewModel;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadOrderDetails();
    }
}