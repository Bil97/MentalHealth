using MentalHealth.Models.UserAccount;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.Profession
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Applications : ContentPage
    {
        public Applications()
        {
            InitializeComponent();
            this.Appearing += Therapists_AppearingAsync;
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
                var result = await App.HttpClient.GetAsync($"api/professions/applications");
                if (result.IsSuccessStatusCode)
                {
                    using var stream = await result.Content.ReadAsStreamAsync();
                    Professions = await JsonSerializer.DeserializeAsync<List<UserProfession>>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    ApplicationsView.ItemsSource = Professions;
                    if (Professions == null || Professions?.Count == 0)
                    {
                        StateLabel.Text = "There are currently no pending applications";
                    }
                    else
                    {
                        StateLabel.Text = $"Applications ({Professions.Count})";
                    }
                }
                else
                {
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex) { await DisplayAlert("Error message", ex.Message, "OK"); }
        }

        private async void RefreshToolbarItem_Clicked(object sender, EventArgs e)
        {
            await GetProfessions();
        }

        private async void DocumentsButton_Clicked(object sender, EventArgs e)
        {
            var profession = ((Button)sender).CommandParameter as UserProfession;
            await App.Current.MainPage.Navigation.PushModalAsync(new Documents(userId: profession?.UserId, professionId: profession?.ProfessionId));
        }
    }
}