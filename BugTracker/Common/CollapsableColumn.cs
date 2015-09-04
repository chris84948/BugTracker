using System;
using System.Windows;
using System.Windows.Controls;

namespace BugTracker.Common
{
    /// <summary>
    /// 
    /// How to easily hide columns and rows in a WPF Grid.
    /// 
    /// http://www.codeproject.com/Articles/437237/WPF-Grid-Column-and-Row-Hiding
    /// 
    /// </summary>
    public class CollapsableColumn : ColumnDefinition
    {
        // Variables
        public static DependencyProperty VisibleProperty;

        // Properties
        public Boolean Visible { get { return (Boolean)GetValue(VisibleProperty); } set { SetValue(VisibleProperty, value); } }

        // Constructors
        static CollapsableColumn()
        {
            VisibleProperty = DependencyProperty.Register("Visible", typeof(Boolean), typeof(CollapsableColumn), new PropertyMetadata(true, new PropertyChangedCallback(OnVisibleChanged)));
            ColumnDefinition.WidthProperty.OverrideMetadata(typeof(CollapsableColumn), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null, new CoerceValueCallback(CoerceWidth)));
            ColumnDefinition.MinWidthProperty.OverrideMetadata(typeof(CollapsableColumn), new FrameworkPropertyMetadata((Double)0, null, new CoerceValueCallback(CoerceMinWidth)));
        }

        // Get/Set
        public static void SetVisible(DependencyObject obj, Boolean nVisible)
        {
            obj.SetValue(VisibleProperty, nVisible);
        }
        public static Boolean GetVisible(DependencyObject obj)
        {
            return (Boolean)obj.GetValue(VisibleProperty);
        }

        static void OnVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            obj.CoerceValue(ColumnDefinition.WidthProperty);
            obj.CoerceValue(ColumnDefinition.MinWidthProperty);
        }
        static Object CoerceWidth(DependencyObject obj, Object nValue)
        {
            return (((CollapsableColumn)obj).Visible) ? nValue : new GridLength(0);
        }
        static Object CoerceMinWidth(DependencyObject obj, Object nValue)
        {
            return (((CollapsableColumn)obj).Visible) ? nValue : (Double)0;
        }
    }
}