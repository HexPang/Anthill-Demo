using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    public class MousePointer : StaticObject
    {
        public ImageSource Source
        {
            get{return pointer.Source;}
            set { pointer.Source = value; }
        }
        private Image pointer = null;
        private Storyboard storyboard = null;
        public MousePointer()
        {

            pointer = new Image() { Width = 48,Height = 45,Stretch = Stretch.UniformToFill,CacheMode = new BitmapCache()};
            pointer.RenderTransform = new CompositeTransform();
            this.Children.Add(pointer);
            Canvas.SetZIndex(pointer, 99);
            pointer.RenderTransformOrigin = new Point(0.5, 0.5);
            this.Loaded += new RoutedEventHandler(MousePointer_Loaded);
        }

        void MousePointer_Loaded(object sender, RoutedEventArgs e)
        {
            Action();
            //throw new NotImplementedException();
        }
        public void Stop()
        {
            if (storyboard != null)
            {
                storyboard.Stop();
            }
        }
        public void Action()
        {
            try
            {
                storyboard.Stop();
            }
            catch (Exception)
            {

            }
            storyboard = null;
            storyboard = new Storyboard();

            DoubleAnimation x = new DoubleAnimation();
            x.From = 1;
            x.To = 0.5;
            x.Duration = TimeSpan.FromMilliseconds(200);
            x.AutoReverse = true;
            x.RepeatBehavior = RepeatBehavior.Forever;

            DoubleAnimation y = new DoubleAnimation();
            y.From = 1;
            y.To = 0.5;
            y.Duration = TimeSpan.FromMilliseconds(200);
            y.AutoReverse = true;
            y.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard.SetTarget(x, pointer);
            Storyboard.SetTargetProperty(x, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.ScaleX)"));

            Storyboard.SetTarget(y, pointer);
            Storyboard.SetTargetProperty(y, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.ScaleY)"));

            storyboard.Children.Add(x);
            storyboard.Children.Add(y);

            storyboard.Begin();
            //(UIElement.RenderTransform).(CompositeTransform.ScaleX)
        }
    }
}
