using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;

namespace BugTracker.Common
{
    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry = Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");
        private static Geometry descGeometry = Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        private ListSortDirection _direction;
        public ListSortDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public SortAdorner(UIElement element, ListSortDirection dir) : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (AdornedElement.RenderSize.Width < 20) return;
        
            var transform = new TranslateTransform(AdornedElement.RenderSize.Width - 15, (AdornedElement.RenderSize.Height - 5) / 2);
            drawingContext.PushTransform(transform);

            Geometry geometry = (this.Direction == ListSortDirection.Descending ? descGeometry : ascGeometry);
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);
            drawingContext.Pop();
        }
    }
}
