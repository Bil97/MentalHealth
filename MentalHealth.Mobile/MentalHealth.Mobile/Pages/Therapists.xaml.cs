using MentalHealth.Mobile.Pages.Communicate;
using MentalHealth.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Therapists : ContentPage
    {
        public Therapists()
        {
            InitializeComponent();
            this.Appearing += Therapists_AppearingAsync;
            if (Device.RuntimePlatform == Device.UWP)
                RefreshToolbar.IconImageSource = ImageSource.FromResource("MentalHealth.Mobile.images.reload.png", typeof(MainPage).GetTypeInfo().Assembly);

        }

        private async void Therapists_AppearingAsync(object sender, EventArgs e)
        {
            await GetProfessions();
        }

        private List<UserProfession> Professions { get; set; }

        private async Task GetProfessions()
        {
            try
            {
                StateLabel.Text = "Loading";
                var result = await App.HttpClient.GetAsync($"api/professions/profession/therapist");
                if (result.IsSuccessStatusCode)
                {
                    using var stream = await result.Content.ReadAsStreamAsync();
                    Professions = await JsonSerializer.DeserializeAsync<List<UserProfession>>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    ProfessionsView.ItemsSource = Professions;
                    if (Professions == null || Professions?.Count == 0)
                    {
                        StateLabel.Text = "There are currently no registered Therapists";
                    }
                    else
                    {
                        StateLabel.Text = $"Therapists currently unoccupied ({Professions.Count})";
                    }
                }
                else
                {
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex) { await DisplayAlert("Error message", ex.Message, "OK"); }
        }

        private async void RefreshToolbar_Clicked(object sender, EventArgs e)
        {
            await GetProfessions();
        }

        private async void ChatButton_Clicked(object sender, EventArgs e)
        {
            var profession = (sender as Button).CommandParameter as UserProfession;
            Chat.ChatBackNavigate = false;
            await MainPage.Tab.Navigation.PushAsync(new Transaction(profession.Id));
        }
    }
}