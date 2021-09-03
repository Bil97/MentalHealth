using MentalHealth.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SessionRecords : ContentPage, IQueryAttributable
    {
        private SessionRecord Session { get; set; }
        private string SessionId;

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            SessionId = HttpUtility.UrlEncode(query["sessionId"]);
        }

        public SessionRecords()
        {
            InitializeComponent();
            this.Appearing += SessionRecords_Appearing;

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