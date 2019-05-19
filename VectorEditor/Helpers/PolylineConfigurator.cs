using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace VectorEditor
{
    public static class PolylineConfigurator
    {
        public static readonly DependencyProperty NodesProperty = DependencyProperty.RegisterAttached(
        "Nodes", typeof(List<Shape>), typeof(PolylineConfigurator), new PropertyMetadata(new List<Shape>()));

        public static void SetNodes(Polyline element, List<Shape> value)
        {
            element.SetValue(NodesProperty, value);
        }

        public static List<Shape> GetNodes(Polyline element)
        {
            return (List<Shape>)element.GetValue(NodesProperty);
        }

        public static readonly DependencyProperty PolylineProperty = DependencyProperty.RegisterAttached(
            "Polyline", typeof(Polyline), typeof(PolylineConfigurator), new PropertyMetadata());

        public static void SetPolyline(DependencyObject element, Polyline value)
        {
            element.SetValue(PolylineProperty, value);
        }

        public static Polyline GetPolyline(DependencyObject element)
        {
            return (Polyline)element.GetValue(PolylineProperty);
        }
    }
}
