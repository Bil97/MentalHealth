using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Learn : ContentPage
    {
        public Learn()
        {
            InitializeComponent();
        }

        void Hide()
        {
            AnxietyDefPanel.IsVisible = false;
            AnxietyWhenPanel.IsVisible = false;
            AnxietyTriPanel.IsVisible = false;
            ResponsePanel.IsVisible = false;
            AnxietyProbPanel.IsVisible = false;
            AnxietyFromPanel.IsVisible = false;
            AnxietyDoPanel.IsVisible = false;
            AfraidPanel.IsVisible = false;
            FearPanel.IsVisible = false;
        }

        private void AnxietyDefTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AnxietyDefPanel.IsVisible;
            Hide();
            AnxietyDefPanel.IsVisible = !value;
        }

        private void AnxiousWhenTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AnxietyWhenPanel.IsVisible;
            Hide();
            AnxietyWhenPanel.IsVisible = !value;
        }

        private void AnxietyTriTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AnxietyTriPanel.IsVisible;
            Hide();
            AnxietyTriPanel.IsVisible = !value;
        }

        private void ResponseTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = ResponsePanel.IsVisible;
            Hide();
            ResponsePanel.IsVisible = !value;
        }

        private void AnxietyProbTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AnxietyProbPanel.IsVisible;
            Hide();
            AnxietyProbPanel.IsVisible = !value;
        }

        private void AnxietyFromTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AnxietyFromPanel.IsVisible;
            Hide();
            AnxietyFromPanel.IsVisible = !value;
        }

        private void AnxietyDoTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AnxietyDoPanel.IsVisible;
            Hide();
            AnxietyDoPanel.IsVisible = !value;
        }

        private void AfraidTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = AfraidPanel.IsVisible;
            Hide();
            AfraidPanel.IsVisible = !value;
        }

        private void FearTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var value = FearPanel.IsVisible;
            Hide();
            FearPanel.IsVisible = !value;
        }
    }
}