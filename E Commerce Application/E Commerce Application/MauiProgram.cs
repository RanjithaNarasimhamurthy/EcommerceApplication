using E_Commerce_Application.Services;
using E_Commerce_Application.ViewModels;
using E_Commerce_Application.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using CommunityToolkit.Maui;

namespace E_Commerce_Application
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).UseMauiCommunityToolkit();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<Category>();
            builder.Services.AddSingleton<SubCategoryGrocery>();
            builder.Services.AddSingleton<SubCategoryShop>();
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<OrderPage>();
            builder.Services.AddSingleton<OrderItemPage>();
            builder.Services.AddSingleton<OtpPage>();
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<ResetPasswordPage>();
            builder.Services.AddSingleton<ForgotPasswordPage>();
            builder.Services.AddSingleton<AdminPage>();
            builder.Services.AddSingleton<VendorDetails>();
            builder.Services.AddSingleton<ReviewPage>();
            builder.Services.AddSingleton<ProductPage>();
            builder.Services.AddSingleton<ProductBySubCategoryIdPage>();
            builder.Services.AddSingleton<WishlistPage>();
            builder.Services.AddSingleton<AddressPage>();
            builder.Services.AddSingleton<AddAddressPage>();
            builder.Services.AddSingleton<UpdateAddressPage>();
            builder.Services.AddSingleton<OrderTrackingPage>();
            builder.Services.AddSingleton<CartView>();
            builder.Services.AddSingleton<CheckoutView>();
            builder.Services.AddSingleton<AddPage>();
            builder.Services.AddSingleton<DetailsPage>();
            builder.Services.AddSingleton<ProductsPage>();
            builder.Services.AddSingleton<ProductViewPage>();
            builder.Services.AddSingleton<FeedbackListPage>();
            builder.Services.AddSingleton<ProductViewModel>();
            builder.Services.AddSingleton<CategoriesViewModel>();
            builder.Services.AddSingleton<DetailsViewModel>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<CategoryViewModel>();
            builder.Services.AddSingleton<SubCategoryViewModel>();
            builder.Services.AddSingleton<OrderViewModel>();
            builder.Services.AddSingleton<OrderItemViewModel>();
            builder.Services.AddSingleton<UserViewModel>();
            builder.Services.AddSingleton<AdminPageViewModel>();
            builder.Services.AddSingleton<ProductsViewModel>();
            builder.Services.AddSingleton<WishlistProductViewModel>();
            builder.Services.AddSingleton<AddressViewModel>();
            builder.Services.AddSingleton<CartViewModel>();
            builder.Services.AddSingleton<CheckoutViewModel>();
            builder.Services.AddSingleton<ProductBySubCategoryViewModel>();
            builder.Services.AddSingleton<FeedbackViewModel>();
            builder.Services.AddSingleton<ProductDetailViewModel>();
            builder.Services.AddSingleton<OrderTrackingViewModel>();
            //builder.Services.AddSingleton<CheckoutAppBar>();
            builder.Services.AddHttpClient<ICategoryService, CategoryService>();
            builder.Services.AddHttpClient<ISubCategoryService, SubCategoryService>();
            builder.Services.AddHttpClient<ILoginService, LoginService>();
            builder.Services.AddHttpClient<IOrderService, OrderService>();
            builder.Services.AddHttpClient<IOrderItemService, OrderItemService>();
            builder.Services.AddHttpClient<IUserService, UserService>();
            builder.Services.AddHttpClient<ISignUpService, SignUpService>();
            builder.Services.AddHttpClient<IFeedbackService, FeedbackService>();
            builder.Services.AddHttpClient<IProductsService, ProductsService>();
            builder.Services.AddHttpClient<IAddressService, AddressService>();
            builder.Services.AddHttpClient<ICartItemsService,CartItemsService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}