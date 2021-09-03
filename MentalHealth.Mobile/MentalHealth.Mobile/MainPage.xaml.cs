using MentalHealth.Mobile.Pages;
using MentalHealth.Mobile.Pages.Communicate;
using MentalHealth.Mobile.Pages.Profession;
using MentalHealth.Mobile.Pages.UserAccount;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : Shell
    {
        bool isLoaded = false;
        public static MainPage page;
        public MainPage()
        {
            InitializeComponent();
            RegisterPages();
            page = this;
            this.Appearing += MainPage_Appearing;
        }

        private void RegisterPages()
        {
            Routing.RegisterRoute($"Pages/{nameof(AddHealthRecord)}", typeof(AddHealthRecord));
            Routing.RegisterRoute($"Pages/{nameof(GeneralWorry)}", typeof(GeneralWorry));
            Routing.RegisterRoute($"Pages/{nameof(HealthRecord)}", typeof(HealthRecord));
            Routing.RegisterRoute($"Pages/{nameof(HealthRecord)}", typeof(HealthRecord));
            Routing.RegisterRoute($"Pages/{nameof(Panic)}", typeof(Panic));
            Routing.RegisterRoute($"Pages/{nameof(Perfectionism)}", typeof(Perfectionism));
            Routing.RegisterRoute($"Pages/{nameof(SessionRecords)}", typeof(SessionRecords));
            Routing.RegisterRoute($"Pages/{nameof(SocialAnxiety)}", typeof(SocialAnxiety));
            Routing.RegisterRoute($"Pages/{nameof(Transaction)}", typeof(Transaction));
            Routing.RegisterRoute($"Pages/UserAccount/{nameof(ChangePassword)}", typeof(ChangePassword));
            Routing.RegisterRoute($"Pages/UserAccount/{nameof(ForgotPassword)}", typeof(ForgotPassword));
            Routing.RegisterRoute($"Pages/UserAccount/{nameof(Register)}", typeof(Register));
            Routing.RegisterRoute($"Pages/Profession/{nameof(Apply)}", typeof(Apply));
            Routing.RegisterRoute($"Pages/Profession/{nameof(Documents)}", typeof(Documents));
            Routing.RegisterRoute($"Pages/Communicate/{nameof(Chat)}", typeof(Chat));
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            if (isLoaded) return;

            if (App.IsAuthenticated)
            {
                ProfileMenuItem.IsVisible = true;
                InboxMenuItem.IsVisible = true;

                var authToken = Application.Current.Properties["authToken"]?.ToString();
                var user = App.User.AuthenticationState(authToken);
                if (user.IsInRole("Admin"))
                {
                    ApplicationsMenuItem.IsVisible = true;
                }
            }
            else
            {
                LoginMenuItem.IsVisible = true;
            }
            isLoaded = true;
        }

        private async void LoginToolBar_Clicked(object sender, EventArgs e)
        {
            if (!App.IsAuthenticated)
            {
                await Shell.Current.GoToAsync($"//{nameof(Login)}");
            }
        }

        private async void AboutToolBar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(About)}");
        }

        private async void LearnToolBar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(Learn)}");
        }

        private async void MyAnxietyToolBar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(MyAnxiety)}");
        }

        private async void QuickTipsToolBar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(QuickTips)}");
        }
    }
}