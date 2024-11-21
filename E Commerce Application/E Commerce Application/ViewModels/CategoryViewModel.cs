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
    public partial class CategoryViewModel : BaseViewModel
    {
        private readonly ICategoryService _categoryService;

        [ObservableProperty]
        private ObservableCollection<Category> _categories;

        public CategoryViewModel()
        {
            _categoryService = new CategoryService();            
            Categories = new ObservableCollection<Category>();
            LoadCategoriesCommand = new RelayCommand(async () => await LoadCategoriesAsync());
            LoadCategoriesCommand.Execute(null);
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

       

        public IRelayCommand LoadCategoriesCommand { get; }

        private async Task LoadCategoriesAsync()
        {
            
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                Categories = new ObservableCollection<Category>(categories);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            
        }
    }
}
