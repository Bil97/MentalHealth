using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.Profession
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Apply : ContentPage
    {
        private decimal serviceFee = 0M;

        public Apply()
        {
            InitializeComponent();
            this.Appearing += Apply_Appearing;
        }

        private async void Apply_Appearing(object sender, EventArgs e)
        {
            await GetProfessions();
        }

        private List<Models.Profession> _professions;
        private IEnumerable<FileResult> _files;
        private string _professionId;

        private async Task GetProfessions()
        {
            try
            {
                using var stream = await App.HttpClient.GetStreamAsync("api/professions");
                _professions = await JsonSerializer.DeserializeAsync<List<Models.Profession>>(stream,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                _professionId = _professions!.FirstOrDefault()?.Id;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void ApplyButton_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            StateLabel.Text = "Submitting";
            ApplyButton.IsEnabled = false;
            var content = new MultipartFormDataContent {
                { new StringContent(_professionId), "professionId" },
                {new StringContent(serviceFee.ToString()), "serviceFee"}
            };

            foreach (var file in _files)
            {
                var fileStream = await file.OpenReadAsync();
                if (fileStream.Length > 5242880)
                {
                    StateLabel.Text = "Upload failed because some files are larger than 5Mb.";
                    ApplyButton.IsEnabled = false;
                    _files = null;
                    return;
                }

                content.Add(new StreamContent(fileStream), "formFile", file.FileName);
            }

            var result = await App.HttpClient.PostAsync($"api/professions/apply", content);
            _files = null;
            StateLabel.Text = await result.Content.ReadAsStringAsync();
            FilesLabel.Text = string.Empty;
            ApplyButton.IsEnabled = false;
            //}
            //catch (Exception ex)
            //{
            //    _files = null;
            //    ApplyButton.IsEnabled = true;
            //    StateLabel.Text = string.Empty;
            //    await DisplayAlert("Error message", ex.Message, "OK");
            //}
        }

        private async void ChooseFilesButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var files = await FilePicker.PickMultipleAsync();
                if (files != null && files?.Count() > 0)
                {
                    _files = files;
                    FilesLabel.Text = $"{_files.Count()} files selected";
                    if (!string.IsNullOrWhiteSpace(AmountEntry.Text))
                        ApplyButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                FilesLabel.Text = string.Empty;
                // The user canceled or something went wrong
                await DisplayAlert("Something went wrong", $"No files chosen\n{ex.Message}", "OK");
            }
        }

        private void AmountEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AmountEntry.Text) && _files != null)
            {
                serviceFee = Convert.ToDecimal(AmountEntry.Text);
                ApplyButton.IsEnabled = true;
            }
        }
    }
}