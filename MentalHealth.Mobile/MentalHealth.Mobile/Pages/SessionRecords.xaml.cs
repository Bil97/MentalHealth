using MentalHealth.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SessionRecords : ContentPage
    {
        private SessionRecord Session { get; set; }
        private string SessionId;
        public SessionRecords(string sessionId)
        {
            InitializeComponent();
            this.Appearing += SessionRecords_Appearing;
            SessionId = sessionId;
            var authToken = Application.Current.Properties["authToken"]?.ToString();

            if (App.User.AuthenticationState(authToken).IsInRole("HealthOfficer"))
                PatientPanel.IsVisible = false;
            else if (App.User.AuthenticationState(authToken).IsInRole("User"))
                HealthOfficerPanel.IsVisible = false;
        }

        private async void SessionRecords_Appearing(object sender, EventArgs e)
        {
            await GetSession();
        }

        private async Task GetSession()
        {
            try
            {
                var stream = await App.HttpClient.GetStreamAsync($"api/HealthRecords/{SessionId}");
                Session = await JsonSerializer.DeserializeAsync<SessionRecord>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                if (Session != null && Session?.PatientHealthRecords != null) this.BindingContext = Session;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message ", ex.Message, "OK");
            }
        }

    }
}