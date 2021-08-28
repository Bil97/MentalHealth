using System;

namespace MentalHealth.Services
{
    public class StateContainer
    {
        private string _title;
        private string _home;
        private string _therapists;
        private string _userProfile;

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyStateChanged();
            }
        }
        public string ActiveTab
        {
            get => _home;
            set
            {
                _home = value;
                NotifyStateChanged();
            }
        }
        public string Therapists
        {
            get => _therapists;
            set
            {
                _therapists = value;
                NotifyStateChanged();
            }
        }
        public string UserProfile
        {
            get => _userProfile;
            set
            {
                _userProfile = value;
                NotifyStateChanged();
            }
        }

    }
}
