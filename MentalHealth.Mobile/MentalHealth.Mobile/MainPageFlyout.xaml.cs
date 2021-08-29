using MentalHealth.Mobile.Pages;
using MentalHealth.Mobile.Pages.Communicate;
using MentalHealth.Mobile.Pages.Profession;
using MentalHealth.Mobile.Pages.UserAccount;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageFlyout : ContentPage
    {
        public ListView ListView;

        public MainPageFlyout()
        {
            InitializeComponent();

            BindingContext = new MainPageFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class MainPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageFlyoutMenuItem> MenuItems { get; set; }

            public MainPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<MainPageFlyoutMenuItem>(new[]
                {
                    new MainPageFlyoutMenuItem {
                        Title = "Home",
                        TargetType=typeof(HomePage),
                        IconSource=ImageSource.FromResource("MentalHealth.Mobile.images.home.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    },
                    new MainPageFlyoutMenuItem {
                        Title = "Therapists",
                        TargetType=typeof(Therapists),
                        IconSource=ImageSource.FromResource("MentalHealth.Mobile.images.people.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    },

                    new MainPageFlyoutMenuItem {
                        Title = "Learn",
                        TargetType=typeof(Learn),
                        IconSource=ImageSource.FromResource("MentalHealth.Mobile.images.pencil.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    },
                    new MainPageFlyoutMenuItem {
                        Title = "My Anxiety",
                        TargetType=typeof(MyAnxiety),
                        IconSource=ImageSource.FromResource("MentalHealth.Mobile.images.bug.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    },
                    new MainPageFlyoutMenuItem {
                        Title = "Quick Tips",
                        TargetType=typeof(QuickTips),
                        IconSource=ImageSource.FromResource("MentalHealth.Mobile.images.excerpt.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    },
                    new MainPageFlyoutMenuItem {
                        Title = "About",
                        TargetType=typeof(About),
                        IconSource=ImageSource.FromResource("MentalHealth.Mobile.images.info.png",
                        typeof(MainPageFlyout).GetTypeInfo().Assembly)
                    },
                });

            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}