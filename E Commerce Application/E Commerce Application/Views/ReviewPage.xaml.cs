using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class ReviewPage : ContentPage
{
    private OrderItem _orderItem;
    public ReviewPage(OrderItemViewModel vm, OrderItem orderItem)
    {
        InitializeComponent();

        BindingContext = vm;
        vm.LoadOrderItemsCommand.Execute(null);
        _orderItem = orderItem;

    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {

        string review = feedbackEditor.Text;
        if (string.IsNullOrWhiteSpace(review))
        {
            await DisplayAlert("Alert", "Review cannot be empty", "OK");
            return;
        }
        var feedback = new Feedback
        {
            strProductId = _orderItem.strProductId,
            intUserId = Preferences.Get("UserId", 0),
            intRating = null,
            strReview = review
        };

        var viewModel = BindingContext as OrderItemViewModel;
        if (viewModel != null)
        {
            await viewModel.AddOrUpdateReviewCommand.ExecuteAsync(feedback);
            await NavigateToOrderItemsPage(_orderItem.intOrderId, viewModel);
        }


    }


    private async Task NavigateToOrderItemsPage(int orderId, OrderItemViewModel viewModel)
    {
        var orderItemViewModel = new OrderItemViewModel(orderId);
        var orderItemsPage = new OrderItemPage(orderItemViewModel);
        await Shell.Current.Navigation.PushAsync(orderItemsPage);
    }
}