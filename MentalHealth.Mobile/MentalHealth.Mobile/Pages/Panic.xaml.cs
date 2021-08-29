using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Panic : ContentPage
    {
        public Panic()
        {
            InitializeComponent();
        }

        void Hide()
        {
            IntroPanel.IsVisible = false;
            SignsPanel.IsVisible = false;
            TipsPanel.IsVisible = false;
        }
        private void IntroButton_Clicked(object sender, EventArgs e)
        {
            var value = IntroPanel.IsVisible;
            IntroButton.BackgroundColor = Color.DodgerBlue;
            SignsButton.BackgroundColor = Color.LightGray;
            TipsButton.BackgroundColor = Color.LightGray;
            Hide();
            IntroPanel.IsVisible = !value;
        }

        private void SignsButton_Clicked(object sender, EventArgs e)
        {
            var value = SignsPanel.IsVisible;
            IntroButton.BackgroundColor = Color.DodgerBlue;
            SignsButton.BackgroundColor = Color.LightGray;
            TipsButton.BackgroundColor = Color.LightGray;
            Hide();
            SignsPanel.IsVisible = !value;
        }

        private void TipsButton_Clicked(object sender, EventArgs e)
        {
            var value = TipsPanel.IsVisible;
            IntroButton.BackgroundColor = Color.DodgerBlue;
            SignsButton.BackgroundColor = Color.LightGray;
            TipsButton.BackgroundColor = Color.LightGray;
            Hide();
            TipsPanel.IsVisible = !value;
        }

        private async void TherapistsButton_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Therapists());
        }

    }
}