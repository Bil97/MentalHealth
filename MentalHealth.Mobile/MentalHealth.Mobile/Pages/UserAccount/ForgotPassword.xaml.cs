using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.UserAccount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = string.Empty;
                var password = new MentalHealth.Models.UserAccount.ForgotPassword()
                {
                    Email = EmailEntry.Text
                };
                var content = new StringContent(JsonSerializer.Serialize(password), Encoding.UTF8, "application/json");
                var result = await App.HttpClient.PostAsync($"api/ApplicationUsers/forgot-password", content);

                if (result.IsSuccessStatusCode)
                {

                }
                else
                {
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex) { await DisplayAlert("Error message", ex.Message, "OK"); }
        }

        private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                SubmitButton.IsEnabled = false;
                EmailLabel.IsEnabled = true;

                return;
            }
            EmailLabel.IsEnabled = false;
            SubmitButton.IsEnabled = true;
            StateLabel.Text = string.Empty;
        }
    }
}