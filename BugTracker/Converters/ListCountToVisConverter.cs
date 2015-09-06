using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using BugTracker.Model;
using BugTracker.ViewModels;

namespace BugTracker.Converters
{
    [ValueConversion(typeof(List<>), typeof(bool))]
    public sealed class ListCountToVisConverter : IValueConverter
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

        public ListCountToVisConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IList<IssueViewModel> list = (IList<IssueViewModel>)value;

            return list != null && list.Count > 0 ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}