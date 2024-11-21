using E_Commerce_Application.ViewModels;

namespace E_Commerce_Application.Views;

public partial class FeedbackListPage : ContentPage
{
    private readonly FeedbackViewModel _viewModel;
    public FeedbackListPage(FeedbackViewModel viewModel, string productId)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        _viewModel.ProductId = productId;
        _viewModel.LoadFeedbacksByProductIdCommand.Execute(null);
    }
}