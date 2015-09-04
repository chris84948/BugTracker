using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace BugTracker.Resources
{
    public class MetroButton : Button
    {

        public Visual Icon
        {
            get { return (Visual)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Depedendency property backing variables
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Visual), typeof(MetroButton));
    }

}