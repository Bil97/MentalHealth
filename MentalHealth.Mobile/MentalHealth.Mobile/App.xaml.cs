using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using MentalHealth.Models.UserAccount;
using MentalHealth.Services;
using Xamarin.Forms;

namespace MentalHealth.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            //MainPage = new MainPage();
            App.Current.UserAppTheme = OSAppTheme.Light;
        }

        public static HttpClient HttpClient { get; set; }
        public static User User { get; set; }
        public static UserDetails UserDetails { get; set; }
        public static bool IsAuthenticated { get; set; } = false;

        protected override async void OnStart()
        {
            User = new User();
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            HttpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(BaseApi.Url)
            };

            if (!Application.Current.Properties.ContainsKey("authToken"))
                Application.Current.Properties.Add("authToken", null);

            var authToken = Application.Current.Properties["authToken"];
            if (authToken != null)
            {
                IsAuthenticated = true;
                HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", authToken.ToString());
                await GetUser();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static async Task GetUser()
        {
            try
            {
                var authToken = Application.Current.Properties["authToken"];
                if (authToken != null)
                {
                    var authState = User.AuthenticationState(authToken.ToString());

                    if (authState.Identity != null)
                    {
                        var result = await HttpClient.GetAsync($"api/ApplicationUsers/{authState.Identity.Name}");

                        if (result.IsSuccessStatusCode)
                        {
                            using (var stream = await result.Content.ReadAsStreamAsync())
                            {
                                UserDetails = await JsonSerializer.DeserializeAsync<UserDetails>(stream,
                                    new JsonSerializerOptions
                                    {
                                        PropertyNameCaseInsensitive = true,
                                    });
                            };
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error message", ex.Message, "OK");
            }
        }

    }
}