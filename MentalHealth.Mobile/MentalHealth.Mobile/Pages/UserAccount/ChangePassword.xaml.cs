using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.UserAccount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePassword : ContentPage
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private MentalHealth.Models.UserAccount.ChangePassword _changePassword = new();

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = string.Empty;
                var password = new MentalHealth.Models.UserAccount.ChangePassword
                {
                    OldPassword = OldPasswordEntry.Text,
                    NewPassword = NewPasswordEntry.Text,
                    ConfirmNewPassword = ConfirmPasswordEntry.Text
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

            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            StateLabel.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(OldPasswordEntry.Text))
            {
                SubmitButton.IsEnabled = false;
                OldPasswordLabel.IsVisible = true;
            }
            else OldPasswordLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(NewPasswordEntry.Text))
            {
                SubmitButton.IsEnabled = false;
                NewPasswordLabel.IsVisible = true;
            }
            else NewPasswordLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                SubmitButton.IsEnabled = false;
                ConfirmPasswordLabel.IsEnabled = true;
            }
            else ConfirmPasswordLabel.IsVisible = false;

            if (!string.IsNullOrWhiteSpace(OldPasswordEntry.Text) && !string.IsNullOrWhiteSpace(NewPasswordEntry.Text) && !string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                StateLabel.Text = string.Empty;
                SubmitButton.IsEnabled = true;
                OldPasswordEntry.IsEnabled = true;
                NewPasswordLabel.IsEnabled = true;
                ConfirmPasswordLabel.IsEnabled = true;
            }
            if (NewPasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                SubmitButton.IsEnabled = false;
                StateLabel.Text = "Passwords do not match";
            }

        }
    }
}