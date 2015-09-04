using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using BugTracker.Model;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public sealed class SeverityConverter : IValueConverter
    {
        private Dictionary<int, string> severities = new Dictionary<int, string>()
        {
            {1, "Critical"},
            {2, "High"},
            {3, "Medium"},
            {4, "Low"}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                return (int)eSeverity.Low;
            }

            int severity = (int)value;

            if (severities.ContainsKey(severity))
                return severities[severity];
            else
                return (int)eSeverity.Low;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}