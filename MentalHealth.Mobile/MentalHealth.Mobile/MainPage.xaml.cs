using MentalHealth.Mobile.Custom;
using MentalHealth.Mobile.Pages;
using MentalHealth.Mobile.Pages.Communicate;
using MentalHealth.Mobile.Pages.Profession;
using MentalHealth.Mobile.Pages.UserAccount;
using System.Reflection;
using Xamarin.Forms;

namespace MentalHealth.Mobile
{
    public partial class MainPage : CustomTabbedPage
    {
        public Applications ApplicationsPage;
        public Inbox InboxPage;
        bool isLoaded = false;
        public MainPage()
        {
            InitializeComponent();
            Tab = this;
            this.Appearing += MainPage_Appearing;
            if (Device.RuntimePlatform == Device.UWP)
                LoginToolBar.IconImageSource = ImageSource.FromResource("MentalHealth.Mobile.images.account_login.png", typeof(MainPage).GetTypeInfo().Assembly);

            ApplicationsPage = new Applications();
            InboxPage = new Inbox();
        }
        private void MainPage_Appearing(object sender, System.EventArgs e)
        {
            if (isLoaded) return;

            LoginToolBar.IsEnabled = true;
            if (App.IsAuthenticated)
            {
                this.Children.Insert(2, InboxPage);

                var authToken = Application.Current.Properties["authToken"].ToString();
                var user = App.User.AuthenticationState(authToken);
                if (user.IsInRole("Admin"))
                {
                    this.Children.Insert(2, ApplicationsPage);
                }
                LoginToolBar.IsEnabled = false;
            }
            isLoaded = true;
        }

        public static MainPage Tab { get; private set; }

        private async void LoginToolBar_Clicked(object sender, System.EventArgs e)
        {
            if (!App.IsAuthenticated)
            {
                await this.Navigation.PushAsync(new Login());
            }
        }

        private async void SignsToolBar_Clicked(object sender, System.EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new SignsOfAnxiety());
        }

        private async void TipsToolBar_Clicked(object sender, System.EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new QuickTips());
        }

        private async void ReliefToolBar_Clicked(object sender, System.EventArgs e)
        {
            await MainPage.Tab.Navigation.PushAsync(new QuickRelief());
        }
    }
}
