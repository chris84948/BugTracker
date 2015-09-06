using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.MVVM;
using BugTracker.Properties;
using MahApps.Metro;
using System.Windows;
using System.Windows.Input;

namespace BugTracker.ViewModels
{
    class SettingsViewModel : ObservableObject
    {
        public ICommand MoveDBCommand { get { return new RelayCommand(MoveDatabase, () => true); } }

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

        private string _dbLocation;
        public string DBLocation
        {
            get { return _dbLocation; }
            set
            {
                _dbLocation = value;
                OnPropertyChanged(() => DBLocation);
            }
        }

        public SettingsViewModel()
        {
            LightTheme = Settings.Default.LightTheme;

            DBLocation = Settings.Default.DBLocation;
        }

        private void UpdateThemeAndSave(bool lightTheme)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                    ThemeManager.GetAccent(lightTheme ? "LightThemeAccent" : "DarkThemeAccent"),
                    ThemeManager.GetAppTheme(lightTheme ? "LightTheme" : "DarkTheme"));

            Settings.Default.LightTheme = lightTheme;
            Settings.Default.Save();
        }

        private void MoveDatabase()
        {

        }
    }
}
