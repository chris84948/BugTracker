using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using BugTracker.Model;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public sealed class IssueStateConverter : IValueConverter
    {
        private Dictionary<int, string> states = new Dictionary<int, string>()
        {
            {1, "Open"},
            {2, "Closed"}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                return (int)eIssueState.Open;
            }

            int state = (int)value;

            if (states.ContainsKey(state))
                return states[state];
            else
                return (int)eIssueState.Open;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}