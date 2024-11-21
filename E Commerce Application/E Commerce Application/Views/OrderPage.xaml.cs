using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class OrderPage : ContentPage
{
    public OrderPage(OrderViewModel vm)
    {
        InitializeComponent();
        BindingContext = new OrderViewModel();
        vm.LoadOrdersCommand.Execute(null);

    }
    //public OrderPage(OrderViewModel vm)
    //{
    //    //InitializeComponent();
    //    BindingContext = vm;
    //    vm.LoadOrdersCommand.Execute(null);

    //}




    protected override void OnAppearing()
    {
        base.OnAppearing();

        OrderViewModel orderViewModel = new OrderViewModel();
        BindingContext = orderViewModel;
        orderViewModel.LoadOrdersCommand.Execute(null);
    }

    private async void OnOrderTapped(object sender, EventArgs e)
    {
        var frame = sender as Frame;
        if (frame != null && frame.BindingContext is Order order)
        {
          
            var orderItemViewModel = new OrderItemViewModel(order.OrderId);
            await Navigation.PushAsync(new OrderItemPage(orderItemViewModel));
        }
    }
}