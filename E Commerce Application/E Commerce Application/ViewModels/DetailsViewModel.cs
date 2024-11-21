using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public ObservableCollection<Product> product { get; set; }
        public DetailsViewModel()
        {
            product = new ObservableCollection<Product>();
            LoadData();
        }
        public void LoadData()
        {

        }

    }
}
