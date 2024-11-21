using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Product _productDetails;
        public Product ProductDetails
        {
            get => _productDetails;
            set
            {
                _productDetails = value;
                OnPropertyChanged(nameof(ProductDetails));
            }
        }

        private ObservableCollection<string> _availableColors = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableColors
        {
            get => _availableColors;
            set
            {
                _availableColors = value;
                OnPropertyChanged(nameof(AvailableColors));
            }
        }

        private ObservableCollection<string> _availableSizes = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableSizes
        {
            get => _availableSizes;
            set
            {
                _availableSizes = value;
                OnPropertyChanged(nameof(AvailableSizes));
            }
        }

        private string _selectedColor;
        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));
                FilterProductImages();
            }
        }

        private string _selectedSize;
        public string SelectedSize
        {
            get => _selectedSize;
            set
            {
                _selectedSize = value;
                OnPropertyChanged(nameof(SelectedSize));
                FilterProductImages();
            }
        }

        private ObservableCollection<ProductImages> _filteredProductImages = new ObservableCollection<ProductImages>();
        public ObservableCollection<ProductImages> FilteredProductImages
        {
            get => _filteredProductImages;
            set
            {
                _filteredProductImages = value;
                OnPropertyChanged(nameof(FilteredProductImages));
            }
        }

        private void FilterProductImages()
        {
            if (ProductDetails != null && ProductDetails.ProductImagesobj != null)
            {
                FilteredProductImages = new ObservableCollection<ProductImages>(
                    ProductDetails.ProductImagesobj
                    .Where(e => e.strProductColor == SelectedColor && e.strProductSize == SelectedSize)
                );
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
