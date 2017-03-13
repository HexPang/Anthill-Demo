using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace WpfApplication1
{
    class ButtonEx : System.Windows.Controls.Button
    {
        public ButtonEx() { }

        public static readonly DependencyProperty CoordinateProperty = DependencyProperty.Register(
    "Coordinate",
    typeof(Point),
    typeof(ButtonEx), new PropertyMetadata(ChangeCoordinateProperty)
);

        public System.Windows.Point Coordinate
        {
            get
            {
                return (Point)GetValue(CoordinateProperty);
            }
            set
            {
                SetValue(CoordinateProperty, value);
            }
        }
        private static void ChangeCoordinateProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonEx sprite = (ButtonEx)d;
            if (sprite.IsVisible)
            {
                Point oldCoordinate = (Point)e.OldValue;
                Point newCoordinate = (Point)e.NewValue;
                Canvas.SetLeft(sprite, newCoordinate.X - 16);
                Canvas.SetTop(sprite, newCoordinate.Y - 16);
            }
        }
    }
}
