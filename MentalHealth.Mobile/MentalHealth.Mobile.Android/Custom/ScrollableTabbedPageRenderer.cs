using Android.Content;
using Google.Android.Material.Tabs;
using MentalHealth.Mobile.Custom;
using MentalHealth.Mobile.Droid.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(ScrollableTabbedPageRenderer))]
namespace MentalHealth.Mobile.Droid.Custom
{
    public class ScrollableTabbedPageRenderer : TabbedPageRenderer
    {
        public ScrollableTabbedPageRenderer(Context context) : base(context)
        {

        }

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);
            var tabLayout = child as TabLayout;
            if (tabLayout != null)
            {
                tabLayout.TabMode = TabLayout.ModeScrollable;
            }
        }
    }
}