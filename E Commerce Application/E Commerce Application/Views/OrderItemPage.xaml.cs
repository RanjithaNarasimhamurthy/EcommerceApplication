using E_Commerce_Application.Models;
using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class OrderItemPage : ContentPage
{
	public OrderItemPage(OrderItemViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        //vm.LoadOrderItemsCommand.Execute(null);
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
    //    var viewModel = this.BindingContext as OrderItemViewModel;

    //    if (viewModel != null)
    //    {
    //        foreach (var item in viewModel.OrderItems)
    //        {
    //            await viewModel.LoadRatingCommand.ExecuteAsync(item.strProductId);
    //        }
    //    }
    //}



    private async void OnStarTapped(object sender, EventArgs e)
    {
        if (sender is Label starLabel && starLabel.BindingContext is OrderItem orderItem)
        {
            var tapGestureRecognizer = starLabel.GestureRecognizers[0] as TapGestureRecognizer;
            if (tapGestureRecognizer != null)
            {
                int rating = int.Parse(tapGestureRecognizer.CommandParameter.ToString());
                FillStarsUpToRating(starLabel, rating);


                var feedback = new Feedback
                {
                    strProductId = orderItem.strProductId,
                    intUserId = Preferences.Get("UserId", 0),
                    intRating = rating,
                    strReview = null
                };

                var viewModel = BindingContext as OrderItemViewModel;
                if (viewModel != null)
                {
                    await viewModel.AddOrUpdateRatingCommand.ExecuteAsync(feedback);
                }
            }
        }
    }

    private void FillStarsUpToRating(Label clickedLabel, int rating)
    {
        StackLayout parentStackLayout = FindParentStackLayout(clickedLabel);
        if (parentStackLayout != null)
        {
            foreach (var child in parentStackLayout.Children)
            {
                if (child is Label starLabel)
                {
                    var tapGestureRecognizer = starLabel.GestureRecognizers[0] as TapGestureRecognizer;
                    if (tapGestureRecognizer != null)
                    {
                        int starRating = int.Parse(tapGestureRecognizer.CommandParameter.ToString());
                        if (starRating <= rating)
                        {
                            starLabel.Text = (string)Application.Current.Resources["FilledStarIcon"];
                        }
                        else
                        {
                            starLabel.Text = (string)Application.Current.Resources["EmptyStarIcon"];
                        }
                    }
                }
            }
        }
    }

    private StackLayout FindParentStackLayout(View view)
    {
        Element parent = view.Parent;
        while (parent != null)
        {
            if (parent is StackLayout stackLayout)
            {
                return stackLayout;
            }
            parent = parent.Parent;
        }
        return null;
    }

    private async void OnPostReviewTapped(object sender, EventArgs e)
    {
        if (sender is Label postReview && postReview.BindingContext is OrderItem orderItem)
        {
            var viewModel = this.BindingContext as OrderItemViewModel;

            if (viewModel != null)
            {
                var reviewPage = new ReviewPage(viewModel, orderItem);
                await Navigation.PushAsync(reviewPage);
            }
        }
    }

    private async void OnOrderItemTapped(object sender, EventArgs e)
    {
        if (sender is Label trackOrder && trackOrder.BindingContext is OrderItem orderItem)
        {
            var viewModel = this.BindingContext as OrderItemViewModel;

            if (viewModel != null)
            {
                var orderTrackingPage = new OrderTrackingPage(orderItem.intOrderItemId, orderItem.intOrderId);
                await Navigation.PushAsync(orderTrackingPage);
            }
        }
    }



}