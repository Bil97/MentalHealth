using MentalHealth.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.Communicate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inbox : ContentPage, IAsyncDisposable
    {
        private HubConnection hubConnection;
        public Inbox()
        {
            InitializeComponent();
            this.Appearing += Inbox_Appearing;
            hubConnection = new HubConnectionBuilder().WithUrl(BaseApi.Url + "chathub").Build();
            hubConnection.On<string>("ReceiveMessage", async (chat) =>
            {
                await GetMessages();
            });
        }

        private async void Inbox_Appearing(object sender, EventArgs e)
        {
            if (hubConnection.State != HubConnectionState.Connected)
            {
                try
                {
                    await hubConnection.StartAsync();
                }
                catch (HttpRequestException ex)
                {
                    await DisplayAlert("Error message", ex.Message, "OK");
                }
            }

            await GetMessages();
        }

        private List<MentalHealth.Models.Chat> messages;

        private async void RefreshToolbarItem_Clicked(object sender, EventArgs e)
        {
            await GetMessages();
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await hubConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("DisposeAsync Error", ex.Message, "OK");
            }
        }

        private async Task GetMessages()
        {
            try
            {
                using var stream = await App.HttpClient.GetStreamAsync($"api/chats/inbox");
                messages = await JsonSerializer.DeserializeAsync<List<MentalHealth.Models.Chat>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                MessagesView.ItemsSource = messages;
                StateLabel.Text = $"Inbox ({messages.Count})";
            }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void MessagesView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var chat = e.Item as MentalHealth.Models.Chat;
            string userId;
            Chat.ChatBackNavigate = false;

            if (chat.CurrentUserId != chat.SenderAId)
            {
                userId = chat.SenderA.Id;
            }
            else
            {
                userId = chat.SenderB.Id;
            }

            await App.Current.MainPage.Navigation.PushModalAsync(new Chat(userId, chat.SessionId));
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var chat = (sender as Button).CommandParameter as MentalHealth.Models.Chat;

            try
            {
                await App.HttpClient.DeleteAsync($"api/chats/clear/{chat.ConversationId}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }


    }
}