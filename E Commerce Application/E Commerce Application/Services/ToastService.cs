//using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Widget;
using Microsoft.Maui.Controls.PlatformConfiguration;

[assembly: Dependency(typeof(E_Commerce_Application.Services.ToastService))]
namespace E_Commerce_Application.Services
{
    public class ToastService : IToastService
    {
        public void ShowToast(string message,string action)
        {
            //Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
            var layout = new LinearLayout(Android.App.Application.Context)
            {
                Orientation = Orientation.Horizontal,
                DividerPadding = 10
            };

            var textView = new TextView(Android.App.Application.Context)
            {
                Text = message,
                TextSize = 16
            };

            if (action == "Approve")
            {
                textView.SetTextColor(Android.Graphics.Color.DarkGreen);
            }
            else if (action == "Reject")
            {
                textView.SetTextColor(Android.Graphics.Color.Red);
            }
            layout.AddView(textView);

            // Create the toast and set the custom layout
            var toast = new Android.Widget.Toast(Android.App.Application.Context);
            toast.SetGravity(Android.Views.GravityFlags.Top | Android.Views.GravityFlags.CenterHorizontal, 0, 150);
            toast.Duration = ToastLength.Short;
            toast.View = layout;
            toast.Show();
        }
    }
}
