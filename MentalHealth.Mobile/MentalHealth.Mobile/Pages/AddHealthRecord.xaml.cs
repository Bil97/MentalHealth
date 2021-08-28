using MentalHealth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddHealthRecord : ContentPage
    {
        private string _sessionId;
        public AddHealthRecord(string sessionId)
        {
            InitializeComponent();
            _sessionId = sessionId;
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = "Submitting";
                SendButton.IsEnabled = false;

                var patientReport = new PatientHealthRecord
                {
                    SessionRecordId = _sessionId,
                    HealthRecord = ReportEntry.Text
                };

                var content = new StringContent(JsonSerializer.Serialize(patientReport), Encoding.UTF8, "application/json");

                var result = await App.HttpClient.PostAsync($"api/HealthRecords", content);
                ReportEntry.Text = string.Empty;
                StateLabel.Text = await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { await DisplayAlert("Error message", ex.Message, "OK"); }

        }

        private void ReportEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReportEntry.Text))
            {
                SendButton.IsEnabled = false;
            }
            else { SendButton.IsEnabled = true; }
        }
    }
}