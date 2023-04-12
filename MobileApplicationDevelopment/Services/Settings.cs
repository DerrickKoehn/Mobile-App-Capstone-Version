using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MobileApplicationDevelopment.Services
{
    public static class Settings
    {
        public static bool FirstRun
        {
            get
            {
                //If IsFirstRun does not yet exist, the app is being ran for the first time.
                //Otherwise, just return the value of FirstRun.
                return Preferences.Get(nameof(FirstRun), true);
            }
            set
            {
                Preferences.Set(nameof(FirstRun), value);
            }
        }
        public static void ClearSettings()
        {
            Preferences.Clear();
        }
    }
}
