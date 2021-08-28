using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MentalHealth.Models.UserAccount;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.Profession
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Documents : ContentPage
    {
        private readonly string _userId;
        private readonly string _professionId;
        private List<UserProfession> _userProfessions;

        public Documents(string userId, string professionId)
        {
            InitializeComponent();
            _userId = userId;
            _professionId = professionId;
            this.Appearing += Documents_Appearing;
        }

        private async void Documents_Appearing(object sender, EventArgs e)
        {
            await GetDocuments();
        }

        private async Task GetDocuments()
        {
            try
            {
                using var result = await App.HttpClient.GetStreamAsync($"api/professions/documents/{_userId}");
                var userProfessions = await JsonSerializer.DeserializeAsync<List<UserProfession>>(result);
                _userProfessions = userProfessions?.Where(x => x.ProfessionId == _professionId).ToList();
                DocumentsView.ItemsSource = _userProfessions;
                if (_userProfessions == null || _userProfessions?.Count == 0)
                    StateLabel.Text = "There a no documents submitted";
                else
                {
                    StateLabel.Text = $"{_userProfessions.FirstOrDefault()?.User.FullName} \nTotal items: {_userProfessions.Count}";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void RefreshToolbarItem_Clicked(object sender, EventArgs e)
        {
            await GetDocuments();
        }

        private async void ApproveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var content = new MultipartFormDataContent
                    {{new StringContent(_professionId), "professionId"}, {new StringContent(_userId), "userId"}};
                var result = await App.HttpClient.PostAsync($"api/professions/approve", content);

                if (result.IsSuccessStatusCode)
                    await MainPage.Tab.Navigation.PopAsync();
                else
                    StateLabel.Text = await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void RejectButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                StateLabel.Text = "Submitting";
                var content = new MultipartFormDataContent
                 {
                     {new StringContent(_professionId), "professionId"}, {new StringContent(_userId), "userId"},
                     {new StringContent(RejectEditor.Text), "reason"}
                 };
                var result = await App.HttpClient.PostAsync($"api/professions/reject", content);
                if (result.IsSuccessStatusCode)
                    await MainPage.Tab.Navigation.PopAsync();
                StateLabel.Text = await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                StateLabel.Text = string.Empty;
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private void RejectEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RejectEditor.Text))
                RejectButton.IsEnabled = false;
            else RejectButton.IsEnabled = true;
        }
    }
}