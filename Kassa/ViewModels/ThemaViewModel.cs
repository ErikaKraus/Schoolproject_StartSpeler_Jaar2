using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.ViewModels
{
    public partial class ThemaViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isToggled;

        public bool GetoggledDoorLaden = false;

        public ThemaViewModel()
        {
            Title = "Instellingen";
            var Theme = Preferences.Get("Theme", "");

            if (!IsToggled)
            {
                if (Theme == "Dark")
                {
                    GetoggledDoorLaden = true;
                    IsToggled = true;
                }
            }
        }

        [RelayCommand]
        public void ToggleTheme()
        {
            var CurrentTheme = Application.Current.RequestedTheme;

            Application.Current.UserAppTheme = CurrentTheme.ToString() == "Light" ? AppTheme.Dark : AppTheme.Light;

            Preferences.Set("Theme", Application.Current.UserAppTheme.ToString());

            MessagingCenter.Send(this, "ThemeChanged");
        }
    }
}
