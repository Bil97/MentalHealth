using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.UserAccount
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            MainPage.Tab.Navigation.PopToRootAsync();
            return base.OnBackButtonPressed();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = string.Empty;
                LoginButton.IsEnabled = false;
                var login = new MentalHealth.Models.UserAccount.Login()
                {
                    Email = EmailEntry.Text,
                    Password = PasswordEntry.Text,
                    RememberMe = RememberMeCheckBox.IsChecked
                };
                var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
                var result = await App.HttpClient.PostAsync($"api/ApplicationUsers/login", content);
                LoginButton.IsEnabled = true;
                var response = await result.Content.ReadAsStringAsync();

                if (!result.IsSuccessStatusCode)
                {
                    StateLabel.Text = response;
                    if (string.IsNullOrEmpty(response))
                        StateLabel.Text = result.StatusCode.ToString();
                }
                else
                {
                    LoginButton.IsEnabled = false;
                    var loginResult = await result.Content.ReadAsStringAsync();
                    Application.Current.Properties["authToken"] = loginResult;

                    App.HttpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("bearer", loginResult);
                    App.IsAuthenticated = true;

                    MainPage.Tab.Children.Insert(2, MainPage.Tab.InboxPage);

                    var authToken = Application.Current.Properties["authToken"].ToString();
                    var user = App.User.AuthenticationState(authToken);
                    if (user.IsInRole("Admin"))
                    {
                        MainPage.Tab.Children.Insert(2, MainPage.Tab.ApplicationsPage);
                    }
                    await App.GetUser();
                    MainPage.Tab.LoginToolBar.IsEnabled = false;
                    await MainPage.Tab.Navigation.PopToRootAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                LoginButton.IsEnabled = true;
                await DisplayAlert($"Error message", ex.Message, "OK");
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            StateLabel.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                LoginButton.IsEnabled = false;
                EmailLabel.IsVisible = true;
            }
            else EmailLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                LoginButton.IsEnabled = false;
                PasswordLabel.IsVisible = true;
            }
            else
            {
                PasswordLabel.IsVisible = false;
            }

            if (!string.IsNullOrWhiteSpace(EmailEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                StateLabel.Text = string.Empty;
                LoginButton.IsEnabled = true;
                EmailLabel.IsVisible = false;
                PasswordLabel.IsVisible = false;
            }
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new Register());
        }

        private async void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new ForgotPassword());
        }

    }
}