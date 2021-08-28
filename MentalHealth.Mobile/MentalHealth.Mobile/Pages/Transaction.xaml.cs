using MentalHealth.Models;
using MentalHealth.Models.UserAccount;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Chat = MentalHealth.Mobile.Pages.Communicate.Chat;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Transaction : ContentPage
    {
        private string _professionId;
        private string _userId;
        SessionRecord _sessionRecord = new();
        private UserProfession Profession { get; set; }
        UserDetails User { get; set; }
        public Transaction(string professionId, string userId = null)
        {
            InitializeComponent();
            _professionId = professionId;
            _userId = userId;

            this.Appearing += Transaction_Appearing;
        }

        private async void Transaction_Appearing(object sender, EventArgs e)
        {
            if (Chat.ChatBackNavigate)
            {
                await MainPage.Tab.Navigation.PopAsync();
                return;
            }
            var authToken = Application.Current.Properties["authToken"];
            if (authToken == null)
            {
                await MainPage.Tab.Navigation.PushAsync(new UserAccount.Login());
                return;
            }


            await GetProfession();

            if (Profession?.ServiceFee == 0 && Profession?.ServiceFeePaid == false)
                await Pay();
            if (Profession?.ServiceFeePaid == true)
                await GetSessionId();
            else
                await GetUser();

            string amount = Profession.ServiceFee.ToString("C2", CultureInfo.CreateSpecificCulture("sw-KE"));
            TitleLabel.Text = $"To consult {Profession.User.FullName} you must pay pay a service fee of {amount}";

            if (User != null && User?.Phonenumber.Length >= 9)
            {
                var phonenumber = User.Phonenumber;
                if (phonenumber.StartsWith("0"))
                    phonenumber = "254" + phonenumber.Remove(0, 1);
                if (phonenumber.StartsWith("+254"))
                    phonenumber = "254" + phonenumber.Remove(0, 1);
                else if (!phonenumber.StartsWith("254"))
                    phonenumber = "254" + phonenumber;

                PhonenumberLabel.Text = $"Your number: {phonenumber}";
            }
            else
            {
                PayButton.IsEnabled = false;
                PhonenumberLabel.Text = "Your phone number is invalid";
            }
        }

        private async Task GetProfession()
        {
            try
            {
                var authToken = Application.Current.Properties["authToken"];
                var user = App.User.AuthenticationState(authToken.ToString());
                if (user.Identity != null)
                {
                    var stream = await App.HttpClient.GetStreamAsync($"api/professions/professionid/{_professionId}");
                    Profession = await JsonSerializer.DeserializeAsync<UserProfession>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async Task GetUser()
        {
            try
            {
                var authToken = App.Current.Properties["authToken"].ToString();
                var user = App.User.AuthenticationState(authToken);

                if (user.Identity != null)
                {
                    var result = await App.HttpClient.GetAsync($"api/ApplicationUsers/{user.Identity.Name}");

                    if (!result.IsSuccessStatusCode)
                    {
                        StateLabel.Text = await result.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        using var stream = await result.Content.ReadAsStreamAsync();
                        User = await JsonSerializer.DeserializeAsync<UserDetails>(stream, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                        });
                    }
                };
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }

        }

        private async Task GetSessionId()
        {
            try
            {
                string userId;

                var content = new MultipartFormDataContent { { new StringContent(_professionId, Encoding.UTF8, "application/json"), "professionId" } };
                if (string.IsNullOrWhiteSpace(_userId))
                {
                    userId = Profession.UserId;
                    content.Add(new StringContent(Profession.UserId, Encoding.UTF8, "application/json"), "userId");
                }
                else
                {
                    userId = _userId;
                    content.Add(new StringContent(_userId, Encoding.UTF8, "application/json"), "userId");
                }
                var result = await App.HttpClient.PostAsync($"api/HealthRecord", content);

                if (!result.IsSuccessStatusCode)
                {
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
                }
                else
                {
                    var sessionId = await result.Content.ReadAsStringAsync();
                    await MainPage.Tab.Navigation.PushAsync(new Chat(Profession.User.Id, sessionId));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async Task Pay()
        {
            try
            {
                StateLabel.Text = "Please wait...";
                _sessionRecord = new SessionRecord
                {
                    ProfessionId = Profession.Id
                };
                var content = new StringContent(JsonSerializer.Serialize(_sessionRecord), Encoding.UTF8, "application/json");
                var result = await App.HttpClient.PostAsync("api/HealthRecords/pay", content);

                if (result.IsSuccessStatusCode)
                {
                    var sessionId = await result.Content.ReadAsStringAsync();
                    await MainPage.Tab.Navigation.PushAsync(new Chat(Profession.User.Id, sessionId));
                }
                else
                {
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                StateLabel.Text = string.Empty;
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void PayButton_Clicked(object sender, EventArgs e)
        {
            await Pay();
        }
    }
}