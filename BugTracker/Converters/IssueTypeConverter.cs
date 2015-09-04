using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using BugTracker.Model;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public sealed class IssueTypeConverter : IValueConverter
    {
        private Dictionary<int, string> types = new Dictionary<int, string>()
        {
            {1, "Bug"},
            {2, "Change Request"}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                return (int)eIssueType.Bug;
            }

            int issueType = (int)value;

            if (types.ContainsKey(issueType))
                return types[issueType];
            else
                return (int)eIssueType.Bug;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}