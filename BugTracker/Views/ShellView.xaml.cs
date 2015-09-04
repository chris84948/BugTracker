using BugTracker.Properties;
using BugTracker.ViewModels;
using MahApps.Metro.Controls;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BugTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellView : MetroWindow
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void Button_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                ((ScreenBase)(sender as StackPanel).DataContext).CloseTab();
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Flyout.IsOpen = !Flyout.IsOpen;
        }
    }
}
