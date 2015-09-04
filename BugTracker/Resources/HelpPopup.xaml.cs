using System.Windows;
using System.Windows.Controls;

namespace BugTracker.Resources
{
	public partial class HelpPopup : UserControl
	{
		public HelpPopup()
		{
			InitializeComponent();
		}

		public FrameworkElement PopupContent {
			get { return (FrameworkElement)GetValue(PopupContentProperty); }
			set { SetValue(PopupContentProperty, value); }
		}

		public static readonly DependencyProperty PopupContentProperty = DependencyProperty.Register("PopupContent", typeof(FrameworkElement), typeof(HelpPopup));
	}
}