using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.MVVM;
using BugTracker.Properties;
using MahApps.Metro;
using System.Windows;

namespace BugTracker.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        private bool _lightTheme;
        public bool LightTheme
        {
            get { return _lightTheme; }
            set
            {
                _lightTheme = value;
                OnPropertyChanged(() => LightTheme);
                UpdateThemeAndSave(value);
            }
        }

        public SettingsViewModel()
        {
            LightTheme = Settings.Default.LightTheme;
        }

        private void UpdateThemeAndSave(bool lightTheme)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                    ThemeManager.GetAccent(lightTheme ? "LightThemeAccent" : "DarkThemeAccent"),
                    ThemeManager.GetAppTheme(lightTheme ? "LightTheme" : "DarkTheme"));

            Settings.Default.LightTheme = lightTheme;
            Settings.Default.Save();
        }
    }
}
