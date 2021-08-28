using MentalHealth.Models.UserAccount;
using MentalHealth.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages.Communicate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Chat : ContentPage, IAsyncDisposable
    {
        private readonly string UserId;
        private readonly string SessionId;
        private readonly HubConnection hubConnection;
        private UserProfession Profession { get; set; }
        public static bool ChatBackNavigate = false;
        private List<MentalHealth.Models.Chat> messages;
        private MentalHealth.Models.Chat chat = new();
        private UserDetails user = new();
        bool toolbarAdded = false;
        public Chat(string userId, string sessionId)
        {
            InitializeComponent();
            ChatBackNavigate = false;
            this.Appearing += Chat_Appearing;
            UserId = userId;
            SessionId = sessionId;
            hubConnection = new HubConnectionBuilder().WithUrl(BaseApi.Url + "chathub").Build();
            hubConnection.On<string>("ReceiveMessage", async (chat) =>
            {
                await GetMessages();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            ChatBackNavigate = true;
            MainPage.Tab.Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }
        private async void Chat_Appearing(object sender, EventArgs e)
        {
            await GetProfession();

            if (Profession != null)
            {
                if (!Profession.ServiceFeePaid)
                {
                    await MainPage.Tab.Navigation.PushAsync(new Transaction(Profession.Id, UserId));
                }
                else
                {
                    if (hubConnection.State != HubConnectionState.Connected)
                    {
                        try
                        {
                            await hubConnection.StartAsync();
                        }
                        catch (HttpRequestException ex)
                        {
                            await DisplayAlert("Error message", ex.InnerException.Message, "OK");
                        }
                    }

                    if (!App.IsAuthenticated)
                    {
                        NotifyLabel.IsVisible = true;
                        MainFrame.IsVisible = false;
                    }
                    else
                    {
                        NotifyLabel.IsVisible = false;
                        MainFrame.IsVisible = true;

                        var authToken = App.Current.Properties["authToken"].ToString();
                        var user = App.User.AuthenticationState(authToken);
                        if (!toolbarAdded)
                            if (user.IsInRole("HealthOfficer"))
                            {
                                AddToolbar();
                                toolbarAdded = true;
                            }

                        this.BindingContext = App.UserDetails;
                        await GetUser();
                        await GetMessages();
                    }
                }
            }
        }

        private async void RefreshToolbarItem_Clicked(object sender, EventArgs e)
        {
            await GetUser();
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

        private async Task GetUser()
        {
            try
            {
                using var stream = await App.HttpClient.GetStreamAsync($"api/ApplicationUsers/userId/{UserId}");
                user = await JsonSerializer.DeserializeAsync<UserDetails>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }
        private async Task GetMessages()
        {
            try
            {
                var stream = await App.HttpClient.GetStreamAsync($"api/chats/{UserId}");
                messages = await JsonSerializer.DeserializeAsync<List<MentalHealth.Models.Chat>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                var session = messages?.FirstOrDefault()?.SessionRecord;
                if (session?.IsComplete == true)
                    StateLabel.Text = "Session Closed";
                else
                {
                    MessagesView.ItemsSource = messages;
                    StateLabel.Text = user.FullName;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private async void SendButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                SendButton.IsEnabled = false;

                if (string.IsNullOrWhiteSpace(MessageEntry.Text)) return;
                if (messages != null && messages.Count > 0)
                    chat = messages.FirstOrDefault();
                else chat = new()
                {
                    SenderBId = user.Id,
                };

                chat.Message = MessageEntry.Text;

                var content = new StringContent(JsonSerializer.Serialize(chat), Encoding.UTF8, "application/json");
                await App.HttpClient.PostAsync($"api/chats", content);

                await hubConnection.SendAsync("SendMessage", "message");

                SendButton.IsEnabled = true;
                MessageEntry.Text = string.Empty;
            }
            catch (InvalidOperationException ex)
            {
                SendButton.IsEnabled = true;
                await DisplayAlert("Error message", ex.Message, "OK");
            }
        }

        private void AddToolbar()
        {
            var addHealthRecord = new ToolbarItem { Text = "Add health reacord" };
            addHealthRecord.Clicked += async (sender, e) => { await MainPage.Tab.Navigation.PushAsync(new AddHealthRecord(SessionId)); };

            var closeSession = new ToolbarItem { Text = "Close this session" };
            closeSession.Clicked += async (sender, e) =>
            {
                try
                {
                    StateLabel.Text = "Please wait...";
                    await App.HttpClient.GetStringAsync($"api/HealthRecords/close-session/{SessionId}");
                    StateLabel.Text = string.Empty;
                    //MainPage.Tab.Navigation.PushAsync(new AddHealthRecord(SessionId));
                    ChatBackNavigate = true;
                    await MainPage.Tab.Navigation.PopAsync();
                }
                catch (Exception ex) { StateLabel.Text = string.Empty; await DisplayAlert("Send error", ex.Message, "OK"); }
            };

            this.ToolbarItems.Add(addHealthRecord);
            this.ToolbarItems.Add(closeSession);
        }

        private async Task GetProfession()
        {
            try
            {
                using var stream = await App.HttpClient.GetStreamAsync($"api/professions/recipient/{UserId}");
                Profession = await JsonSerializer.DeserializeAsync<UserProfession>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            }
            catch (HttpRequestException ex)
            {
                SendButton.IsEnabled = false;
                await DisplayAlert("GetProfession Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("GetProfession Error", ex.Message, "OK");
            }
        }

    }
}
