using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Perfectionism : ContentPage
    {
        public Perfectionism()
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
            IntroButton.BackgroundColor = Color.DodgerBlue;
            SignsButton.BackgroundColor = Color.LightGray;
            TipsButton.BackgroundColor = Color.LightGray;
            Hide();
            IntroPanel.IsVisible = true;
        }

        private void SignsButton_Clicked(object sender, EventArgs e)
        {
            IntroButton.BackgroundColor = Color.LightGray;
            SignsButton.BackgroundColor = Color.DodgerBlue;
            TipsButton.BackgroundColor = Color.LightGray;
            Hide();
            SignsPanel.IsVisible = true;
        }

        private void TipsButton_Clicked(object sender, EventArgs e)
        {
            IntroButton.BackgroundColor = Color.LightGray;
            SignsButton.BackgroundColor = Color.LightGray;
            TipsButton.BackgroundColor = Color.DodgerBlue;
            Hide();
            TipsPanel.IsVisible = true;
        }

        private async void TherapistsButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(Therapists)}");
        }

    }
}