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
    public class ButtonClick : EventArgs
    {
        public byte Click = 0;
        public ButtonClick(byte c)
        {
            Click = c;
        }
    }
    public class HUD : StaticObject
    {
        public double HUDWidth = 76;
        public double HUDHeight = 76;
        Canvas BugsPanel = new Canvas() { Width = Application.Current.Host.Content.ActualWidth, Height = 76 };
        //StackPanel SP = new StackPanel() {Height = 76, Width = Application.Current.Host.Content.ActualWidth,Orientation = Orientation.Horizontal };
        Button_BuyBug BBB_1 = new Button_BuyBug("1", 10, 5);
        Button_BuyBug BBB_2 = new Button_BuyBug("2", 10, 5);
        Button_BuyBug BBB_3 = new Button_BuyBug("3", 10, 5);
        Button_BuyBug BBB_4 = new Button_BuyBug("4", 10, 5);
        public event EventHandler ButtonClicked;
        public void ReSize()
        {
            BugsPanel.Width = Application.Current.Host.Content.ActualWidth;
            Canvas.SetLeft(BugsPanel, 0);
            Canvas.SetTop(BugsPanel, Application.Current.Host.Content.ActualHeight - BugsPanel.Height);
            int i = 1;
            foreach (UIElement obj in BugsPanel.Children)
            {
                Canvas.SetLeft(obj, i * 76 + 600);
                i++;
            }
            //Canvas.SetLeft(BugsPanel, Application.Current.Host.Content.ActualWidth - HUDWidth);
            //Canvas.SetTop(BugsPanel, Application.Current.Host.Content.ActualHeight - HUDHeight);
        }
        public HUD()
        {
            this.Children.Add(BugsPanel);
            BugsPanel.Children.Add(BBB_1);
            BugsPanel.Children.Add(BBB_2);
            BugsPanel.Children.Add(BBB_3);
            BugsPanel.Children.Add(BBB_4);
            BBB_1.MouseLeftButtonUp += new MouseButtonEventHandler(BBB_1_MouseLeftButtonUp);
            BBB_2.MouseLeftButtonUp += new MouseButtonEventHandler(BBB_2_MouseLeftButtonUp);
            BBB_3.MouseLeftButtonUp += new MouseButtonEventHandler(BBB_3_MouseLeftButtonUp);
        }

        void BBB_3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
            ButtonClicked(this, new ButtonClick(3));
        }

        void BBB_2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonClicked(this, new ButtonClick(2));
        }

        void BBB_1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ButtonClicked(this, new ButtonClick(1));
        }
    }
}
