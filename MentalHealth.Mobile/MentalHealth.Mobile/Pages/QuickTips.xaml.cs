using System;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickTips : ContentPage
    {
        public QuickTips()
        {
            InitializeComponent();
            this.BindingContext = images;
        }

        List<ImageSource> images = new List<ImageSource>
        {
           ImageSource.FromResource("MentalHealth.Mobile.images.count.png", typeof(MainPage).GetTypeInfo().Assembly),
           ImageSource.FromResource("MentalHealth.Mobile.images.environment.png", typeof(MainPage).GetTypeInfo().Assembly),
           ImageSource.FromResource("MentalHealth.Mobile.images.notice.png", typeof(MainPage).GetTypeInfo().Assembly),
        };
    }
}