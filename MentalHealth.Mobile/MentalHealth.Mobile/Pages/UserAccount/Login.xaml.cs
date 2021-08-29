using MentalHealth.Mobile.Pages.Communicate;
using MentalHealth.Mobile.Pages.Profession;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
            MainPage.NavPage.Navigation.PopToRootAsync();
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

                    var menuItems = MainPage.NavPage.FlyoutPage.ListView.ItemsSource as ObservableCollection<MainPageFlyoutMenuItem>;
                    var profile = new MainPageFlyoutMenuItem
                    {
                        Title = "Profile",
                        TargetType = typeof(Profile),
                        IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.person.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    };
                    menuItems.Insert(2, profile);
                    MainPage.NavPage.FlyoutPage.ListView.ItemsSource = menuItems;

                    var inbox = new MainPageFlyoutMenuItem
                    {
                        Title = "Inbox",
                        TargetType = typeof(Inbox),
                        IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.inbox.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    };
                    menuItems.Insert(2, inbox);

                    var authToken = Application.Current.Properties["authToken"].ToString();
                    var user = App.User.AuthenticationState(authToken);
                    if (user.IsInRole("Admin"))
                    {
                        var applications = new MainPageFlyoutMenuItem
                        {
                            Title = "Applications",
                            TargetType = typeof(Applications),
                            IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.folder.png",
                            typeof(MainPageFlyout).GetTypeInfo().Assembly)
                        };
                        menuItems.Insert(2, applications);
                    }
                    MainPage.NavPage.FlyoutPage.ListView.ItemsSource = menuItems;
                    await App.GetUser();
                    MainPage.NavPage.LoginToolBar.IsEnabled = false;
                    await MainPage.NavPage.Navigation.PopToRootAsync();
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
            await App.Current.MainPage.Navigation.PushModalAsync(new Register());
        }

        private async void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new ForgotPassword());
        }

    }
}