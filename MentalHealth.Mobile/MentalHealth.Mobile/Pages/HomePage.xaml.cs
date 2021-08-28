using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void ReadMoreButton_Clicked(object sender, EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new About());
        }

        private void TherapistsButton_Clicked(object sender, EventArgs e)
        {
            MainPage.Tab.SelectedItem = MainPage.Tab.TherapistsPage;
        }

        private async void SignsButton_Clicked(object sender, EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new SignsOfAnxiety());
        }

        private async void TipsButton_Clicked(object sender, EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new QuickTips());
        }

        private async void ReliefButton_Clicked(object sender, EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new QuickRelief());
        }
    }
}