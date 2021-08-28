using MentalHealth.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthRecord : ContentPage
    {
        public HealthRecord()
        {
            InitializeComponent();
            this.Appearing += HealthRecord_Appearing;
        }

        private async void HealthRecord_Appearing(object sender, EventArgs e)
        {
            await GetSessions();
        }

        private async Task GetSessions()
        {
            try
            {
                StateLabel.Text = "Please wait...";
                var stream = await App.HttpClient.GetStreamAsync("api/HealthRecords");
                var sessions = await JsonSerializer.DeserializeAsync<List<SessionRecord>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                StateLabel.Text = $"{sessions.Count} sessions";
                HealthRecordsView.ItemsSource = sessions;
            }
            catch (Exception ex)
            {
                StateLabel.Text = string.Empty;
                await DisplayAlert("Error message ", ex.Message, "OK");
            }
        }

        private void HealthRecordsView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var session = e.Item as SessionRecord;
            if (session != null)
            {
                MainPage.Tab.Navigation.PushAsync(new SessionRecords(session.Id));
            }
        }
    }
}