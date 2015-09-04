using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using BugTracker.Model;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public sealed class ListviewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double listviewWidth = (double)value;

            return listviewWidth - 575;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}