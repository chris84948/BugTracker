using BugTracker.Properties;
using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BugTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // add custom accent and theme resource dictionaries
            ThemeManager.AddAppTheme("LightTheme", new Uri(@"pack://application:,,,/Resources\Themes\LightTheme.xaml"));
            ThemeManager.AddAppTheme("DarkTheme", new Uri(@"pack://application:,,,/Resources\Themes\DarkTheme.xaml"));
            ThemeManager.AddAccent("LightThemeAccent", new Uri(@"pack://application:,,,/Resources\Themes\LightThemeAccent.xaml"));
            ThemeManager.AddAccent("DarkThemeAccent", new Uri(@"pack://application:,,,/Resources\Themes\DarkThemeAccent.xaml"));
     
            ThemeManager.ChangeAppStyle(Application.Current,
                    ThemeManager.GetAccent(Settings.Default.LightTheme ? "LightThemeAccent" : "DarkThemeAccent"),
                    ThemeManager.GetAppTheme(Settings.Default.LightTheme ? "LightTheme" : "DarkTheme"));
            
            base.OnStartup(e);
        }
    }
}
