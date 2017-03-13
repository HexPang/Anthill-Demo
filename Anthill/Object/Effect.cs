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
using Anthill.Management;

/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    public class Effect : DynamicObject
    {
        public int StartFrame = 2;
        public int EndFrame = 102;
        int currFrame = 0;
        Image eff = null;
        public event EventHandler EffectOver;
        string EffectID = "";
        private double effWidth = 0;
        private double effHeight = 0;
        private Point effectPosition;
        private Image EffectBackground = null;
        /// <summary>
        /// 创建一个效果实例
        /// </summary>
        /// <param name="effID">效果ID</param>
        /// <param name="Position">位置</param>
        /// <param name="StartF">开始帧</param>
        /// <param name="EndF">结束帧</param>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        public Effect(string effID,Point Position,int StartF,int EndF,double Width,double Height,int background = 0) {
            this.CacheMode = new BitmapCache();
            EffectID = effID;
            StartFrame = StartF;
            EndFrame = EndF;
            effectPosition = Position;
            eff = new Image() { Stretch = Stretch.None,CacheMode = new BitmapCache() };
            //BitmapImage BI = new BitmapImage(new Uri(string.Format(@"Images/Effect/{0}/{1}.png", EffectID, StartFrame), UriKind.Relative));
            effWidth = Width;
            effHeight = Height;
            eff.Source = ResourceManage.GetImageResource(string.Format(@"Images/Effect/{0}/{1}.png", EffectID, StartFrame));//BI;
            currFrame = StartFrame;
            Canvas.SetLeft(eff, effectPosition.X - Width / 2);
            Canvas.SetTop(eff, effectPosition.Y - Height / 2);
            Canvas.SetZIndex(eff, 1);
            if (background > 0)
            {
                //Background
                EffectBackground = new Image();
                EffectBackground.Source = new BitmapImage(new Uri(string.Format(@"Images/Effect/{0}/{1}.png", EffectID, background), UriKind.Relative));
                Canvas.SetLeft(EffectBackground, effectPosition.X - Width / 2);
                Canvas.SetTop(EffectBackground, effectPosition.Y - Height / 2);
                Canvas.SetZIndex(EffectBackground, 2);
                //this.Children.Add(EffectBackground);
            }
            this.Loaded += new RoutedEventHandler(Effect_Loaded);
        }

        void Effect_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            //Dispatcher.BeginInvoke(() => {
                Heart.Interval = TimeSpan.FromMilliseconds(5);
                Heart.Tick += new EventHandler(Heart_Tick);
                Heart.Start();
                if (EffectBackground != null) this.Children.Add(EffectBackground);
                this.Children.Add(eff);
            //});
            this.Loaded -= Effect_Loaded;
        }

        void Heart_Tick(object sender, EventArgs e)
        {
            //BitmapImage BI = new BitmapImage(new Uri(string.Format(@"Images/Effect/{0}/{1}.png", EffectID, currFrame), UriKind.Relative));
            currFrame++;
            if (currFrame > EndFrame)
            {
                this.Children.Remove(eff);
                Heart.Stop();
                currFrame = StartFrame;
                EffectOver(this, null);
            }
            eff.Source = ResourceManage.GetImageResource(string.Format(@"Images/Effect/{0}/{1}.png", EffectID, currFrame));//BI;
            //throw new NotImplementedException();
        }
    }
}
