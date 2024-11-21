using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.ViewModels
{
    public partial class AdminPageViewModel : BaseViewModel
    {
        private readonly ISignUpService _vendorService;

        //[ObservableProperty]
        //private ObservableCollection<Vendor> _vendors;
        public ObservableCollection<Vendor> Vendors { get; set; } = new ObservableCollection<Vendor>();

        [ObservableProperty]
        private Vendor _selectedVendor;

        public Command GoBackCommand { get; set; }

        public AdminPageViewModel()
        {
            _vendorService = new SignUpService();
            Vendors = new ObservableCollection<Vendor>();
            LoadVendorsCommand = new RelayCommand(async () => await LoadVendorsAsync());
            ApproveVendorCommand = new AsyncRelayCommand<Vendor>(ApproveVendor);
            RejectVendorCommand = new AsyncRelayCommand<Vendor>(RejectVendor);
            LoadVendorsCommand.Execute(null);
            GoBackCommand = new Command(async () => await GoBack());
        }

        public IRelayCommand LoadVendorsCommand { get; }
        public IAsyncRelayCommand<Vendor> ApproveVendorCommand { get; }
        public IAsyncRelayCommand<Vendor> RejectVendorCommand { get; }

        public async Task LoadVendorsAsync()
        {
            try
            {
                var vendors = await _vendorService.GetVendorsAsync();
                var filteredVendors = vendors.Where(v => !v.IsApproved && !v.IsRejected).ToList();
                //Vendors = new ObservableCollection<Vendor>(filteredVendors);
                //NoVendorsLabel.IsVisible = Vendors.Count == 0;
                Vendors.Clear();
                foreach (var vendor in filteredVendors)
                {
                    Vendors.Add(vendor);
                }
                OnPropertyChanged(nameof(Vendors));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return;
            }
        }

        [RelayCommand]
        private async Task OnItemTapped(Vendor vendor)
        {
            try
            {
                if (vendor == null)
                    return;
                SelectedVendor = vendor;
                await Shell.Current.GoToAsync(nameof(Views.VendorDetails), true, new Dictionary<string, object>
                {
                    { "SelectedVendor", vendor }
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
                return;
            }
        }

        private async Task ApproveVendor(Vendor vendor)
        {
            try
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to approve this vendor?", "No", "Yes");
                if (!confirm)
                {
                    await _vendorService.ApproveVendorAsync(vendor);

                    var user = new User
                    {
                        strName = vendor.strName,
                        strUserName = vendor.strUserName,
                        strPassword = vendor.strPassword,
                        strRole = "Vendor",
                        longContactNo = vendor.longContactNo,
                        dtCreated_on = DateTime.Now,
                        dtUpdated_on = DateTime.Now
                    };
                    
                    await _vendorService.SignUpAsync(user);
                    await LoadVendorsAsync();
                    DependencyService.Get<IToastService>().ShowToast("Vendor Approved Successfully", "Approve");
                    await Shell.Current.GoToAsync(nameof(Views.AdminPage));
                    
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }

        }

        private async Task RejectVendor(Vendor vendor)
        {
            try
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to reject this vendor?", "No", "Yes");
                if (!confirm)
                {
                    
                    string reason = await Application.Current.MainPage.DisplayPromptAsync("Rejection Reason", "Please provide a reason for rejection:");

                    if (reason != null)
                    {
                        if (!string.IsNullOrEmpty(reason))
                        {
                            vendor.strRejectionReason = reason;

                            await _vendorService.RejectVendorAsync(vendor);
                            await LoadVendorsAsync();
                            DependencyService.Get<IToastService>().ShowToast("Vendor Rejected Successfully", "Reject");
                            await Shell.Current.GoToAsync(nameof(Views.AdminPage));
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Rejection reason cannot be empty.", "Ok");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task GoBack()
        {
            await Shell.Current.GoToAsync(nameof(Views.AdminPage));
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
