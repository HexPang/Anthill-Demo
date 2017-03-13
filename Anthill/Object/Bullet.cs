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
using System.Windows.Media.Imaging;

/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */

namespace Anthill.Object
{
    public class Bullet : StaticObject
    {
        public Sprites.SpriteBase Target = null;
        public int BulletID = 0;
        Image bullet = null;
        public event EventHandler BulletIt;
        public int Speed = 2;
        public Bullet(int bulletID,double width,double height)
        {
            bullet = new Image() { Width = width, Height = height, Source = new BitmapImage(new Uri(string.Format(@"Images/Bullet/{0}.png", bulletID), UriKind.Relative)), Stretch = Stretch.None, CacheMode = new BitmapCache() };
            BulletID = bulletID;
        }
        public void Fire(Sprites.SpriteBase sb){
            Target = (Sprites.SpriteBase)sb.Target;
            //TurnAround(sb.Target.Position);
            this.Children.Add(bullet);
            Canvas.SetLeft(this, sb.Position.X + sb.SpriteWidth / 2);
            Canvas.SetTop(this, sb.Position.Y + sb.SpriteHeight / 2);
            double speed = Maths.GameMath.GetDistance(sb.Position, Target.Position) * Speed;
            Canvas.SetZIndex(bullet, 10);
            Storyboard s = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = Canvas.GetLeft(this);
            doubleAnimation.To = Target.Position.X + Target.SpriteWidth / 2;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(speed));
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            s.Children.Add(doubleAnimation);
            doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = Canvas.GetTop(this);
            doubleAnimation.To = Target.Position.Y + Target.SpriteHeight / 2;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(speed));
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            s.Children.Add(doubleAnimation);
            s.Completed += new EventHandler(s_Completed);
            s.Begin();
        }
        /*
        public void TurnAround(Point destination)
        {
            Point p1 = new Point(Canvas.GetLeft(this),Canvas.GetTop(this));
            double Angle = Maths.GameMath.GetAngle(destination.Y - p1.Y, destination.X - p1.X) - 90;
            PlaneProjection PP = new PlaneProjection();
            PP.RotationZ = -Angle;
            this.Projection = PP;
        }*/
        void s_Completed(object sender, EventArgs e)
        {
            Storyboard s = sender as Storyboard;
            BulletIt(this, null);
            this.Children.Remove(bullet);
            sender = null;
            s.Completed -= s_Completed;
            //throw new NotImplementedException();
        }
    }
}
