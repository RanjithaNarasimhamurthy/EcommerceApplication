using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using E_Commerce_Application.Models;
using E_Commerce_Application.Services;

namespace E_Commerce_Application.ViewModels
{
    public partial class AddressViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly AddressService _addressService;

        [ObservableProperty]
        private ObservableCollection<Address> addresses;
        [ObservableProperty]
        private Address addr;
        [ObservableProperty]
        private Address addre;

        public AddressViewModel()
        {
            _addressService = new AddressService();
            Addresses = new ObservableCollection<Address>();
            addr = new Address();

            UpdateAddressCommand = new RelayCommand(async () => await UpdateAddressAsync());
            SetDefaultAddressCommand = new AsyncRelayCommand<Address>(SetDefaultAddressAsync);
            LoadAddressesCommand = new RelayCommand(async () => await LoadAddresses());
            RemoveAddressCommand = new AsyncRelayCommand<Address>(RemoveAddress);
            AddAddressCommand = new AsyncRelayCommand(AddAddress);

            LoadAddressesCommand.Execute(null);
        }

        public IRelayCommand LoadAddressesCommand { get; }
        public IAsyncRelayCommand AddAddressCommand { get; }
        public IRelayCommand UpdateAddressCommand { get; }
        public IAsyncRelayCommand<Address> RemoveAddressCommand { get; }
        public IAsyncRelayCommand<Address> SetDefaultAddressCommand { get; }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(Addr.Name):
                        if (string.IsNullOrWhiteSpace(Addr.Name))
                            result = "Name is required.";
                        break;
                    case nameof(Addr.ContactNo):
                        if (!Addr.ContactNo.HasValue)
                            result = "Contact number is required.";
                        else if (Addr.ContactNo.ToString().Length != 10)
                            result = "Contact number must be 10 digits.";
                        break;
                    case nameof(Addr.strAddressLine1):
                        if (string.IsNullOrWhiteSpace(Addr.strAddressLine1))
                            result = "Address Line 1 is required.";
                        break;
                    case nameof(Addr.strCity):
                        if (string.IsNullOrWhiteSpace(Addr.strCity))
                            result = "City is required.";
                        break;
                    case nameof(Addr.strState):
                        if (string.IsNullOrWhiteSpace(Addr.strState))
                            result = "State is required.";
                        break;
                    case nameof(Addr.intZipCode):
                        if (Addr.intZipCode <= 0)
                            result = "Zip Code is required.";
                        break;
                }
                return result;
            }
        }

        public async Task RemoveAddress(Address address)
        {
            try
            {
                await _addressService.DeleteAddressAsync(address.intAddressId);
                Addresses.Remove(address);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove address: {ex.Message}");
            }
        }

        private async Task UpdateAddressAsync()
        {
            try
            {
                var updateAddress = await _addressService.UpdateAddressAsync(Addre);
                await Application.Current.MainPage.DisplayAlert("Success", "Address Updated Successfully.", "OK");
                await Shell.Current.GoToAsync("//AddressPage");
                await LoadAddresses();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update address data: {ex.Message}");
            }
        }

        public async Task InitializeForEditAsync(int addressId)
        {
            Addre = await _addressService.GetAddressByIdAsync(addressId);
        }

        private async Task LoadAddresses()
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);
                var addressList = await _addressService.GetAddressByUserId();
                if (addressList != null && addressList.Any())
                {
                    Addresses = new ObservableCollection<Address>(addressList);
                }
                else
                {
                    Addresses = new ObservableCollection<Address>();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                Addresses = new ObservableCollection<Address>(); // Ensure Addresses is initialized even on error
            }
        }

        private async Task AddAddress()
        {
            IsBusy = true;

            try
            {
                string validationErrors = ValidateAddress();
                if (!string.IsNullOrEmpty(validationErrors))
                {
                    await Application.Current.MainPage.DisplayAlert("Validation Errors", validationErrors, "OK");
                    return;
                }

                int userId = Preferences.Get("UserId", 0);
                Addr.intUserId = userId;
                Addr.dtUpdated_on = DateTime.Now;
                Addr.dtCreated_on = DateTime.Now;

                var addedAddress = await _addressService.AddAddressAsync(Addr);
                if (addedAddress != null)
                {
                    Addresses.Add(addedAddress);
                    await Shell.Current.GoToAsync("//AddressPage");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string ValidateAddress()
        {
            var errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Addr.Name))
                errors.AppendLine("Name is required.");
            if (!Addr.ContactNo.HasValue)
                errors.AppendLine("Contact number is required.");
            else if (Addr.ContactNo.ToString().Length != 10)
                errors.AppendLine("Contact number must be 10 digits.");

            if (string.IsNullOrWhiteSpace(Addr.strAddressLine1))
                errors.AppendLine("Address Line 1 is required.");

            if (string.IsNullOrWhiteSpace(Addr.strCity))
                errors.AppendLine("City is required.");

            if (string.IsNullOrWhiteSpace(Addr.strState))
                errors.AppendLine("State is required.");

            if (string.IsNullOrWhiteSpace(Addr.intZipCode.ToString()))
                errors.AppendLine("Zip code is required.");

            //if (Addr.intZipCode <= 0)
            //    errors.AppendLine("Zip Code is required.");

            return errors.ToString();
        }

        public async Task SetDefaultAddressAsync(Address address)
        {
            try
            {
                await _addressService.SetDefaultAddressAsync(address.intAddressId);
                await Application.Current.MainPage.DisplayAlert("Success", "Default Address Set Successfully.", "OK");
                await LoadAddresses();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to set default address: {ex.Message}");
            }
        }
    }
}
