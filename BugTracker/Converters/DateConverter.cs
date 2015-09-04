using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public sealed class DateConverter : IValueConverter
    {
        private Dictionary<int, string> months = new Dictionary<int, string>()
        {
            {1, "Jan"},
            {2, "Feb"},
            {3, "Mar"},
            {4, "Apr"},
            {5, "May"},
            {6, "Jun"},
            {7, "Jul"},
            {8, "Aug"},
            {9, "Sep"},
            {10, "Oct"},
            {11, "Nov"},
            {12, "Dec"},
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                return "";
            }
            DateTime date = (DateTime) value;

            return String.Format("{0} {1}, {2}", date.Day.ToString(), months[date.Month], date.Year.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}