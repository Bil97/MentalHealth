using System;
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
            await Shell.Current.GoToAsync($"Pages/{nameof(GeneralWorry)}");
        }

        private async void SocialAnxietyTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"Pages/{nameof(SocialAnxiety)}");
        }

        private async void PerfectionismTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"Pages/{nameof(Perfectionism)}");
        }

        private async void PanicTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"Pages/{nameof(Panic)}");
        }
    }
}