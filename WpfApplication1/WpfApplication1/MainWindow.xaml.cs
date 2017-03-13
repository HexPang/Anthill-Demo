using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// 路径点
    /// </summary>
    public class PathPoints
    {
        public Point point;
        public Image DotPoint;
        public PathPoints(Point p, Image dp)
        {
            point = p;
            DotPoint = dp;
        }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int move = 0;
        Point lastPoint = new Point();
        private List<PathPoints> Paths = new List<PathPoints> { };
        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
        }

        public MainWindow()
        {
            InitializeComponent();

            this.MouseMove += new MouseEventHandler(MainWindow_MouseMove);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(MainWindow_MouseLeftButtonUp);
            sb.Completed += new EventHandler(sb_Completed);
            //button1.Click += new RoutedEventHandler(button1_Click);
        }

        
        void sb_Completed(object sender, EventArgs e)
        {
            WalkBack();

            //throw new NotImplementedException();
        }
        void WalkBack()
        {
            sb.Children.Clear();
            Canvas.SetZIndex(button1, 100);
            //throw new NotImplementedException();
            Point lastP = getPosition(button1);
            PointAnimationUsingKeyFrames DAUKF = new PointAnimationUsingKeyFrames() { Duration = new Duration(TimeSpan.FromMilliseconds(Paths.Count * 100)) };
            Storyboard.SetTarget(DAUKF, button1);
            Storyboard.SetTargetProperty(DAUKF, new PropertyPath("Coordinate"));
            //DAUKF.KeyFrames.Add(new LinearPointKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)), Value = lastPoint });
            int i = 0;
            foreach (PathPoints item in Paths)
            {
                item.DotPoint.Opacity = 0.5;
                DAUKF.KeyFrames.Add(new LinearPointKeyFrame() { KeyTime = TimeSpan.FromMilliseconds((Paths.Count - i - 1) * 100), Value = item.point });
                i++;
            }
            sb.Children.Add(DAUKF);
            sb.Begin();
        }
        void WalkTo()
        {
            sb.Children.Clear();
            Canvas.SetZIndex(button1, 100);
            //throw new NotImplementedException();
            Point lastP = getPosition(button1);
            PointAnimationUsingKeyFrames DAUKF = new PointAnimationUsingKeyFrames() { Duration = new Duration(TimeSpan.FromMilliseconds(Paths.Count * 100)) };
            Storyboard.SetTarget(DAUKF, button1);
            Storyboard.SetTargetProperty(DAUKF, new PropertyPath("Coordinate"));
            //DAUKF.KeyFrames.Add(new LinearPointKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)), Value = lastPoint });
            int i = 0;
            foreach (PathPoints item in Paths)
            {
                item.DotPoint.Opacity = 0.5;
                DAUKF.KeyFrames.Add(new LinearPointKeyFrame() { KeyTime = TimeSpan.FromMilliseconds((i + 1) * 100), Value = item.point });
                i++;
            }
            sb.Children.Add(DAUKF);
            sb.Begin();
        }
        void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WalkTo();
        }
        private Point getPosition(UIElement obj)
        {
            return new Point(Canvas.GetLeft(obj), Canvas.GetTop(obj));
        }
        Storyboard sb = new Storyboard();
        void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            for (int i = 0; i < Paths.Count; i++)
            {
                PathPoints pp = Paths[i];
                Layroot.Children.Remove(pp.DotPoint);
            }
            Paths.Clear();
            lastPoint = new Point(0, 0);
        }
        int FindPointByPoint(Point p)
        {
            int i = 0;
            foreach (PathPoints item in Paths)
            {
                if (GetDistance(item.point, p) <= 5) return i;
                i++;
            }
            return -1;
        }
        void RemoveLines(int offset)
        {
            while (Paths.Count > offset)
            {
                Layroot.Children.Remove(Paths[offset].DotPoint);
                Paths.RemoveAt(offset);
            }
        }
        void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            move++;
            if (move == 10)
            {
                move = 1;
                Point p = e.GetPosition(Layroot);
                int offset = FindPointByPoint(p);
                if (offset > -1)
                {
                    RemoveLines(offset);
                }
                if (lastPoint.X != 0 && lastPoint.Y != 0)
                {
                    double range = GetDistance(p, lastPoint);
                    this.Title = range.ToString();
                    if (range >= 30)
                    {
                        return;
                    }
                }
                else
                {
                    lastPoint = p;
                }
                lastPoint = p;
                Image point = new Image() { Width = 4, Height = 4, Source = new BitmapImage(new Uri(@"path_dot_spitter[4444]_low.png", UriKind.Relative)) };
                Paths.Add(new PathPoints(p, point));
                Layroot.Children.Add(point);
                Canvas.SetLeft(point, p.X - 2);
                Canvas.SetTop(point, p.Y - 2);
            }

            //throw new NotImplementedException();
        }
    }
}
