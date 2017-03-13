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

namespace Anthill.Object
{
    public class Button_BuyBug : StaticObject
    {
        Canvas Button = new Canvas() { Width = 75,Height = 75};
        Image ButtonImage = null;
        TextBlock BugCount = null;
        TextBlock BugCost = null;
        /// <summary>
        /// 购买虫子按钮
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <param name="count">当前数量</param>
        /// <param name="cost">生产价格</param>
        public Button_BuyBug(string name,int count,int cost)
        {            
            //this.CacheMode = new BitmapCache();
            BitmapImage BI = new BitmapImage(new Uri(string.Format(@"Images/button/{0}.png", name), UriKind.Relative));
            ButtonImage = new Image();
            ButtonImage.Source = BI;
            BugCost = new TextBlock();
            BugCost.Text = cost.ToString();
            BugCount = new TextBlock();
            BugCount.Text = count.ToString();
            Button.Children.Add(ButtonImage);
            BugCost.Foreground = new SolidColorBrush(Colors.White);
            BugCount.Foreground = new SolidColorBrush(Colors.White);
            Button.Children.Add(BugCost);
            Canvas.SetLeft(BugCost,64);
            Canvas.SetTop(BugCost, 64);
            Button.Children.Add(BugCount);
            Canvas.SetLeft(BugCount, 4);
            Canvas.SetTop(BugCount, 4);
            this.Children.Add(Button);
            this.Cursor = Cursors.Hand;
        }

        public void BugCounts(int count)
        {
            BugCount.Text = count.ToString();
        }
    }
}
