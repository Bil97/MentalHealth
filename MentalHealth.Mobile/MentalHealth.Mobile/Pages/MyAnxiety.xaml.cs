using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyAnxiety : ContentPage
    {
        public MyAnxiety()
        {
            InitializeComponent();
        }

        private async void GeneralWorryTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new GeneralWorry());
        }

        private async void SocialAnxietyTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new SocialAnxiety());
        }

        private async void PerfectionismTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Perfectionism());
        }

        private async void PanicTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Panic());
        }
    }
}