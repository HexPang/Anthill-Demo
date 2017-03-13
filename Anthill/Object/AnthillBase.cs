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
using System.Windows.Threading;

/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    public class AnthillBase : StaticObject
    {
        public event EventHandler BaseDestory;
        private int NomoreID = 0;
        private int DestoryID = 0;
        private int _health = 100;
        private int Health {
            get { return _health; }
            set { _health = value; if (PB != null) PB.Value = _health * 100 / MaxHealth; }
        }
        public int currHealth { get { return _health; } }
        private Image StaticSkin = null;
        private ProgressBar PB = null;
        private DispatcherTimer TM = null;
        private int MaxHealth = 100;
        public Point Position = new Point();
        public AnthillBase(int nomore,int destory,Point position,int hp = 100){
            NomoreID = nomore;
            MaxHealth = hp;
            DestoryID = destory;
            Health = hp;
            Position = position;
            Canvas.SetLeft(this, position.X - 147);
            Canvas.SetTop(this, position.Y - 147);
            Canvas.SetZIndex(this, 0);
            StaticSkin = new Image() { Width = 294, Height = 294, Source = new BitmapImage(new Uri(string.Format(@"Images/base/{0}.png", nomore), UriKind.Relative)), Stretch = Stretch.None, CacheMode = new BitmapCache() };
            this.Children.Add(StaticSkin);
            PB = new ProgressBar() { Width = 294,Height = 10,Value = 100};
            PB.Visibility = System.Windows.Visibility.Collapsed;
            this.Children.Add(PB);
            Canvas.SetTop(PB, -10);
            TM = new DispatcherTimer();
            TM.Tick += new EventHandler(TM_Tick);
        }

        void TM_Tick(object sender, EventArgs e)
        {
            PB.Visibility = System.Windows.Visibility.Collapsed;
            //throw new NotImplementedException();
        }
        public void Damaged(int dmg)
        {
            if (Health <= 0) return;
            PB.Visibility = System.Windows.Visibility.Visible;
            TM.Stop();
            Health -= dmg;
            TM.Interval = TimeSpan.FromMilliseconds(2000);
            TM.Start();
            if (Health <= 0) {
                Health = 0;
                if (BaseDestory != null)
                {
                    BaseDestory(this, null);
                }
                StaticSkin.Source = new BitmapImage(new Uri(string.Format(@"Images/base/{0}.png", DestoryID), UriKind.Relative));
            }
        }
    }
}
