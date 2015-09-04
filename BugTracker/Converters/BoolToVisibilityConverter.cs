using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace BugTracker.Converters
{
    /// <summary>
    /// Boolean visibility converter, can be used for hidden gone or visible
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue
        {
            get { return m_TrueValue; }
            set { m_TrueValue = value; }
        }

        private Visibility m_TrueValue;
        public Visibility FalseValue
        {
            get { return m_FalseValue; }
            set { m_FalseValue = value; }
        }

        private Visibility m_FalseValue;
        public BoolToVisibilityConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return null;
            }
            return System.Convert.ToBoolean(value) ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, TrueValue))
            {
                return true;
            }
            if (Equals(value, FalseValue))
            {
                return false;
            }
            return null;
        }
    }
}