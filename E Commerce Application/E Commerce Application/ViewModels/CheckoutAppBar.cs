using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        private bool _isCheckoutEnabled;
        public bool IsCheckoutEnabled
        {
            get => _isCheckoutEnabled;
            set => SetProperty(ref _isCheckoutEnabled, value);
        }

        public CartViewModel CartViewModel { get; }

        public AppShellViewModel()
        {
            IsCheckoutEnabled = false;
            CartViewModel = new CartViewModel();
        }
    }
}
