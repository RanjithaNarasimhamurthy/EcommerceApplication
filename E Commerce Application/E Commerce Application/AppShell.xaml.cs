using E_Commerce_Application.Views;
using System.Linq;
using System.Windows.Input;

namespace E_Commerce_Application
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }
        public Command OpenFlyoutCommand { get; }
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(Category), typeof(Category));
            Routing.RegisterRoute(nameof(SubCategoryShop), typeof(SubCategoryShop));
            Routing.RegisterRoute(nameof(SubCategoryGrocery), typeof(SubCategoryGrocery));
            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(OrderPage), typeof(OrderPage));
            Routing.RegisterRoute(nameof(OrderItemPage), typeof(OrderItemPage));
            Routing.RegisterRoute(nameof(OtpPage), typeof(OtpPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
            Routing.RegisterRoute(nameof(VendorDetails), typeof(VendorDetails));
            Routing.RegisterRoute(nameof(ReviewPage), typeof(ReviewPage));
            Routing.RegisterRoute(nameof(WishlistPage), typeof(WishlistPage));
            Routing.RegisterRoute(nameof(AddressPage), typeof(AddressPage));
            Routing.RegisterRoute(nameof(AddAddressPage), typeof(AddAddressPage));
            Routing.RegisterRoute(nameof(UpdateAddressPage), typeof(UpdateAddressPage));
            Routing.RegisterRoute(nameof(ProductBySubCategoryIdPage), typeof(ProductBySubCategoryIdPage));
            Routing.RegisterRoute(nameof(ProductsPage), typeof(ProductsPage));
            Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
            Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            Routing.RegisterRoute(nameof(ProductViewPage), typeof(ProductViewPage));
            Routing.RegisterRoute(nameof(FeedbackListPage), typeof(FeedbackListPage));
            Routing.RegisterRoute(nameof(OrderTrackingPage), typeof(OrderTrackingPage));
            //OpenFlyoutCommand = new Command(() => FlyoutIsPresented = true);
            //BindingContext = this;
            LogoutCommand = new Command(async () => await LogoutAsync());
            BindingContext = this;


        }

        private async Task LogoutAsync()
        {
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync("//LoginPage");
        }
        //private void OnMenuClicked(object sender, EventArgs e)
        //{
        //    if (FlyoutIsPresented)
        //    {
        //        FlyoutIsPresented = false;
        //    }
        //    else
        //    {
        //        FlyoutIsPresented = true;
        //    }
        //}
    }
}