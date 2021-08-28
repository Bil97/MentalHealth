using MentalHealth.Mobile.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickRelief : ContentPage
    {
        List<ImageSource> images = new()
        {
            ImageSource.FromResource("MentalHealth.Mobile.images.count.png", typeof(QuickRelief).GetTypeInfo().Assembly),
            ImageSource.FromResource("MentalHealth.Mobile.images.environment.png", typeof(QuickRelief).GetTypeInfo().Assembly),
            ImageSource.FromResource("MentalHealth.Mobile.images.notice.png", typeof(QuickRelief).GetTypeInfo().Assembly)
        };
        public QuickRelief()
        {
            InitializeComponent();
            this.BindingContext = images;
        }
    }
}