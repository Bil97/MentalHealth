using System;
using Xamarin.Forms;

namespace MentalHealth.Mobile
{
    public class MainPageFlyoutMenuItem
    {
        public MainPageFlyoutMenuItem()
        {
            TargetType = typeof(MainPageFlyoutMenuItem);
        }
        public string Title { get; set; }
        public ImageSource IconSource { get; set; }

        public Type TargetType { get; set; }
    }
}