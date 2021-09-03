using MentalHealth.Services;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.UserAccount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        private MentalHealth.Models.UserAccount.Register register = new();
        public Register()
        {
            InitializeComponent();

            this.BindingContext = register;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            StateLabel.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(SurnameEntry.Text))
            {
                SurnameLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else SurnameLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
            {
                FirstNameLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else FirstNameLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                EmailLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else EmailLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(PhoneNumberEntry.Text))
            {
                PhoneNumberLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else PhoneNumberLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(IdNoEntry.Text))
            {
                IdNoLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else IdNoLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(LatitudeEntry.Text))
            {
                LatitudeLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else LatitudeLabel.IsVisible = false;
            if (string.IsNullOrWhiteSpace(LocationLongitudeEntry.Text))
            {
                LocationLongitudeLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else LocationLongitudeLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                PasswordLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else PasswordLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                ConfirmPasswordLabel.IsVisible = true;
                RegisterButton.IsEnabled = false;
            }
            else ConfirmPasswordLabel.IsVisible = false;

            if (!string.IsNullOrWhiteSpace(SurnameEntry.Text) && !string.IsNullOrWhiteSpace(FirstNameEntry.Text) &&
                !string.IsNullOrWhiteSpace(EmailEntry.Text) && !string.IsNullOrWhiteSpace(PhoneNumberEntry.Text) &&
                !string.IsNullOrWhiteSpace(IdNoEntry.Text) && !string.IsNullOrWhiteSpace(LatitudeEntry.Text) &&
                !string.IsNullOrWhiteSpace(IdNoEntry.Text) && !string.IsNullOrWhiteSpace(LocationLongitudeEntry.Text) &&
                !string.IsNullOrWhiteSpace(PasswordEntry.Text) && !string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                StateLabel.Text = string.Empty;
                RegisterButton.IsEnabled = true;
                SurnameLabel.IsVisible = false;
                FirstNameLabel.IsVisible = false;
                EmailLabel.IsVisible = false;
                PhoneNumberLabel.IsVisible = false;
                IdNoLabel.IsVisible = false;
                LatitudeLabel.IsVisible = false;
                LocationLongitudeLabel.IsVisible = false;
                PasswordLabel.IsVisible = false;
                ConfirmPasswordLabel.IsVisible = false;
            }

            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                RegisterButton.IsEnabled = false;
                StateLabel.Text = "Passwords do not match";
            }
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = string.Empty;
                RegisterButton.IsEnabled = false;
                var content = new StringContent(JsonSerializer.Serialize(register), Encoding.UTF8, "application/json");

                var result = await App.HttpClient.PostAsync($"api/ApplicationUsers/create-account", content);
                RegisterButton.IsEnabled = true;
                if (!result.IsSuccessStatusCode)
                {
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
                }
                else
                {
                    await Shell.Current.GoToAsync($"../{nameof(Login)}");
                }
            }
            catch (Exception ex)
            {
                RegisterButton.IsEnabled = true;
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"../{nameof(Login)}");
        }

        private async void LearnHowToButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var url = $"{BaseApi.Url}location-help";
                await Browser.OpenAsync(url, BrowserLaunchMode.External);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }
    }
}