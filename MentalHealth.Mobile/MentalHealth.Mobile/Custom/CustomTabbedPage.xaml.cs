using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomTabbedPage : TabbedPage
    {
        public CustomTabbedPage()
        {
            InitializeComponent();
        }
    }
}