using E_Commerce_Application.Models;
using E_Commerce_Application.Services;

namespace E_Commerce_Application
{
    public partial class App : Application
    {
        public static User user;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            DependencyService.Register<IToastService, ToastService>();
        }
    }
}