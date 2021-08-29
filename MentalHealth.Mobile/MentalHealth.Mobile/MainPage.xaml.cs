using MentalHealth.Mobile.Pages;
using MentalHealth.Mobile.Pages.Communicate;
using MentalHealth.Mobile.Pages.Profession;
using MentalHealth.Mobile.Pages.UserAccount;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        bool isLoaded = false;
        public static MainPage NavPage { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            NavPage = this;

            this.Appearing += MainPage_Appearing;
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
                LoginToolBar.IconImageSource = ImageSource.FromResource("MentalHealth.Mobile.images.account_login.png", typeof(MainPage).GetTypeInfo().Assembly);
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
            if (isLoaded) return;

            LoginToolBar.IsEnabled = true;
            var menuItems = FlyoutPage.ListView.ItemsSource as ObservableCollection<MainPageFlyoutMenuItem>;
            if (App.IsAuthenticated)
            {
                var profile = new MainPageFlyoutMenuItem
            {
                Title = "Profile",
                TargetType = typeof(Profile),
                IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.person.png",
              typeof(MainPageFlyout).GetTypeInfo().Assembly)
            };
            menuItems.Insert(2, profile);

            var inbox = new MainPageFlyoutMenuItem
            {
                Title = "Inbox",
                TargetType = typeof(Inbox),
                IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.inbox.png",
                 typeof(MainPageFlyout).GetTypeInfo().Assembly)
            };
            menuItems.Insert(2, inbox);

            var authToken = Application.Current.Properties["authToken"]?.ToString();
            var user = App.User.AuthenticationState(authToken);
            if (user.IsInRole("Admin"))
            {
                var applications = new MainPageFlyoutMenuItem
                {
                    Title = "Applications",
                    TargetType = typeof(Applications),
                    IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.folder.png",
                    typeof(MainPageFlyout).GetTypeInfo().Assembly)
                };
                menuItems.Insert(2, applications);
            }
            LoginToolBar.IsEnabled = false;
            }
            else
            {
                var login = new MainPageFlyoutMenuItem
            {
                Title = "Login",
                TargetType = typeof(Login),
                IconSource = ImageSource.FromResource("MentalHealth.Mobile.images.account_login.png",
                typeof(MainPageFlyout).GetTypeInfo().Assembly)
            };
            menuItems.Insert(2, login);
            }
            FlyoutPage.ListView.ItemsSource = menuItems;
            isLoaded = true;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainPageFlyoutMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            FlyoutPage.ListView.SelectedItem = null;
        }

        private void LoginToolBar_Clicked(object sender, EventArgs e)
        {
            if (!App.IsAuthenticated)
            {
                NavPage.Detail = new NavigationPage(new Login());
            }
        }

        private void AboutToolBar_Clicked(object sender, EventArgs e)
        {
            NavPage.Detail = new NavigationPage(new About());
        }

        private void LearnToolBar_Clicked(object sender, EventArgs e)
        {
            NavPage.Detail = new NavigationPage(new Learn());
        }

        private void MyAnxietyToolBar_Clicked(object sender, EventArgs e)
        {
            NavPage.Detail = new NavigationPage(new MyAnxiety());
        }

        private void QuickTipsToolBar_Clicked(object sender, EventArgs e)
        {
            NavPage.Detail = new NavigationPage(new QuickTips());
        }
    }
}