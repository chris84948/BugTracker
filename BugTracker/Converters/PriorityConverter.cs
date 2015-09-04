using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using BugTracker.Model;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public sealed class PriorityConverter : IValueConverter
    {
        private Dictionary<int, string> priorities = new Dictionary<int, string>()
        {
            {1, "High"},
            {2, "Medium"},
            {3, "Low"}
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int))
            {
                return (int)ePriority.Low;
            }

            int priority = (int)value;

            if (priorities.ContainsKey(priority))
                return priorities[priority];
            else
                return (int)ePriority.Low;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}