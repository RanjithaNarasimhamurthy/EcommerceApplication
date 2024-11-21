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
    public partial class FeedbackViewModel : BaseViewModel
    {
        private readonly IFeedbackService _feedbackService;

        [ObservableProperty]
        private string productId;

        public ObservableCollection<Feedback> Feedbacks { get; } = new ObservableCollection<Feedback>();

        public FeedbackViewModel()
        {
            _feedbackService = new FeedbackService();
            LoadFeedbacksByProductIdCommand = new RelayCommand(async () => await LoadFeedbacksByProductIdAsync());
            BackButtonTappedCommand = new RelayCommand(OnBackButtonTapped);
        }

        public IRelayCommand LoadFeedbacksByProductIdCommand { get; }
        public IRelayCommand BackButtonTappedCommand { get; }
        private async Task LoadFeedbacksByProductIdAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductId))
                return;
            try
            {
                var feedbacks = await _feedbackService.GetFeedbacksByProductIdAsync(ProductId, false);
                Feedbacks.Clear();
                foreach (var feedback in feedbacks)
                {
                    Feedbacks.Add(feedback);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private async void OnBackButtonTapped()
        {
            await Shell.Current.Navigation.PushAsync(new ProductViewPage(productId));
        }
    }
}
