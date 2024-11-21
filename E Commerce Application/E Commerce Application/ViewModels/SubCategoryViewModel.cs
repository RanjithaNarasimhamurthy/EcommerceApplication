using E_Commerce_Application.Services;
using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace E_Commerce_Application.ViewModels
{
    public partial class SubCategoryViewModel : BaseViewModel
    {
        private readonly ISubCategoryService _subcategoryService;

        [ObservableProperty]
        private ObservableCollection<SubCategory> _subCategories;
        public Command GoBackCommand { get; set; }
        public SubCategoryViewModel(int categoryId)
        {
            _subcategoryService = new SubCategoryService(); 
            SubCategories = new ObservableCollection<SubCategory>();
            LoadSubCategoriesCommand = new RelayCommand(async () => await LoadSubCategoriesAsync(categoryId));
            LoadSubCategoriesCommand.Execute(null);
            GoBackCommand = new Command(async () => await GoBack());
        }
       

        public IRelayCommand LoadSubCategoriesCommand { get; }

        private async Task LoadSubCategoriesAsync(int categoryId)
        {
            try
            {
                var subcategories = await _subcategoryService.GetSubCategoriesByCategoryIdAsync(categoryId);
                SubCategories = new ObservableCollection<SubCategory>(subcategories);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task GoBack()
        {
            await Shell.Current.GoToAsync("//Category");
        }
    }

}
