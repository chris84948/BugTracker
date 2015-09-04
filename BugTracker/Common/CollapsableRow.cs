using System;
using System.Windows;
using System.Windows.Controls;

namespace BugTracker.Common
{
    class CollapsableRow : RowDefinition
    {
        // Variables
        public static DependencyProperty VisibleProperty;

        // Properties
        public Boolean Visible { get { return (Boolean)GetValue(VisibleProperty); } set { SetValue(VisibleProperty, value); } }

        // Constructors
        static CollapsableRow()
        {
            VisibleProperty = DependencyProperty.Register("Visible", typeof(Boolean), typeof(CollapsableRow), new PropertyMetadata(true, new PropertyChangedCallback(OnVisibleChanged)));
            RowDefinition.HeightProperty.OverrideMetadata(typeof(CollapsableRow), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star), null, new CoerceValueCallback(CoerceHeight)));
            RowDefinition.MinHeightProperty.OverrideMetadata(typeof(CollapsableRow), new FrameworkPropertyMetadata((Double)0, null, new CoerceValueCallback(CoerceMinHeight)));
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
            obj.CoerceValue(RowDefinition.HeightProperty);
            obj.CoerceValue(RowDefinition.MinHeightProperty);
        }
        static Object CoerceHeight(DependencyObject obj, Object nValue)
        {
            return (((CollapsableRow)obj).Visible) ? nValue : new GridLength(0);
        }
        static Object CoerceMinHeight(DependencyObject obj, Object nValue)
        {
            return (((CollapsableRow)obj).Visible) ? nValue : (Double)0;
        }
    }
}