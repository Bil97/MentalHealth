using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MentalHealth.Mobile.Pages.Profession;
using MentalHealth.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.UserAccount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
            this.Appearing += Profile_Appearing;
        }

        private async void Profile_Appearing(object sender, EventArgs e)
        {
            if (!App.IsAuthenticated)
            {
                NotifyLabel.IsVisible = true;
                MainFrame.IsVisible = false;
            }
            else
            {
                var authToken = Application.Current.Properties["authToken"]?.ToString();
                if (App.User.AuthenticationState(authToken).IsInRole("Admin"))
                {
                    ApplyForTherapistButton.IsVisible = false;
                    HealthRecordButton.IsVisible = false;
                }
                NotifyLabel.IsVisible = false;
                MainFrame.IsVisible = true;
                if (App.UserDetails == null)
                    await App.GetUser();
                this.BindingContext = App.UserDetails;
            }
        }

        private async void UpdateProfileButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = string.Empty;
                UpdateProfileButton.IsEnabled = false;

                var content = new StringContent(JsonSerializer.Serialize(App.UserDetails), Encoding.UTF8,
                    "application/json");
                var result =
                    await App.HttpClient.PutAsync($"api/ApplicationUsers/update-account/{App.UserDetails.Id}", content);

                UpdateProfileButton.IsEnabled = true;
                StateLabel.Text = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    await App.GetUser();
                    this.BindingContext = App.UserDetails;
                }
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
                UpdateProfileButton.IsEnabled = true;
            }
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            var authToken = Application.Current.Properties["authToken"]?.ToString();
            var menuItems = MainPage.NavPage.FlyoutPage.ListView.ItemsSource as ObservableCollection<MainPageFlyoutMenuItem>;

            menuItems.RemoveAt(2);
            if (App.User.AuthenticationState(authToken).IsInRole("Admin"))
                menuItems.RemoveAt(2);

            MainPage.NavPage.FlyoutPage.ListView.ItemsSource = menuItems;

            Application.Current.Properties["authToken"] = null;

            App.HttpClient.DefaultRequestHeaders.Authorization = null;
            App.IsAuthenticated = false;
            App.UserDetails = new Models.UserAccount.UserDetails();
            MainPage.NavPage.LoginToolBar.IsEnabled = true;

            NotifyLabel.IsVisible = true;
            MainFrame.IsVisible = false;
            await App.Current.MainPage.Navigation.PushModalAsync(new HomePage());
        }

        private async void ChangePasswordButton_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new ChangePassword());
        }

        private async void ApplyButton_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new Apply());
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            StateLabel.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(SurnameEntry.Text))
            {
                SurnameLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            else SurnameLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
            {
                FirstNameLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            FirstNameLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                EmailLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            else EmailLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(PhoneNumberEntry.Text))
            {
                PhoneNumberLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            else PhoneNumberLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(IdNoEntry.Text))
            {
                IdNoLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            else IdNoLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(LatitudeEntry.Text))
            {
                LatitudeLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            else LatitudeLabel.IsVisible = false;
            if (string.IsNullOrWhiteSpace(LocationLongitudeEntry.Text))
            {
                LocationLongitudeLabel.IsVisible = true;
                UpdateProfileButton.IsEnabled = false;
            }
            else LocationLongitudeLabel.IsVisible = false;

            if (!string.IsNullOrWhiteSpace(SurnameEntry.Text) && !string.IsNullOrWhiteSpace(FirstNameEntry.Text) &&
                !string.IsNullOrWhiteSpace(EmailEntry.Text) && !string.IsNullOrWhiteSpace(PhoneNumberEntry.Text) &&
                !string.IsNullOrWhiteSpace(IdNoEntry.Text) && !string.IsNullOrWhiteSpace(LatitudeEntry.Text) &&
                !string.IsNullOrWhiteSpace(IdNoEntry.Text) && !string.IsNullOrWhiteSpace(LocationLongitudeEntry.Text))
            {
                StateLabel.Text = string.Empty;
                SurnameLabel.IsVisible = false;
                FirstNameLabel.IsVisible = false;
                EmailLabel.IsVisible = false;
                PhoneNumberLabel.IsVisible = false;
                IdNoLabel.IsVisible = false;
                LatitudeLabel.IsVisible = false;
                LocationLongitudeLabel.IsVisible = false;
                UpdateProfileButton.IsEnabled = true;
            }
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

        private async void HealthRecordButton_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new HealthRecord());
        }
    }
}