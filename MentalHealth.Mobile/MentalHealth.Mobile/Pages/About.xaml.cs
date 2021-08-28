using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MentalHealth.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class About : ContentPage
    {
        string paragraph1Text = "Mental health professions in many countries are not enough." +
            "Very few mental health patients are taken care of because of " +
            "the few number of psychiatrists and clinics available. " +
            "Patients with appointments wait for a long time, " +
            "and this can impact those patients who are in crisis. " +
            "Some patients also have to travel to travel long distances, " +
            "hence causing discouragng those who lack access to reliable " +
            "transportable or those who cannot afford to pay fare. " +
            "Some patients fear to access mental health care because they worry about what " +
            "others will think of them.Others also fail to seek therapy " +
            "because they are afraid to reveal personal things about themselves. \n\n" +
            "Mental health sessions are also costly and many people cannot afford to pay " +
            "for them since they are low income earners or solely depend on others to " +
            "provide for their needs.";
        public About()
        {
            InitializeComponent();
            Paragraph1Label.Text = paragraph1Text;
        }
    }
}