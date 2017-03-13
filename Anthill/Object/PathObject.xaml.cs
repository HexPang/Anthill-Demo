using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Anthill.Object
{
    public partial class PathObject : UserControl
    {
        Image LastIMG = null;
        public event EventHandler PathDone;
        public PathObject()
        {
            InitializeComponent();
            /*
            image8.MouseLeftButtonUp += new MouseButtonEventHandler((s,e)=>{
                this.Visibility = System.Windows.Visibility.Collapsed;
            });*/
            image1.MouseEnter += new MouseEventHandler((s,e)=>{
                this.Cursor = Cursors.Hand;
                image5.Visibility = System.Windows.Visibility.Visible;
                if (LastIMG != null) LastIMG.Visibility = System.Windows.Visibility.Collapsed;
                LastIMG = image5;
            });
            image2.MouseEnter += new MouseEventHandler((s, e) =>
            {
                this.Cursor = Cursors.Hand;
                image7.Visibility = System.Windows.Visibility.Visible;
                if (LastIMG != null) LastIMG.Visibility = System.Windows.Visibility.Collapsed;
                LastIMG = image7;
            });
            image3.MouseEnter += new MouseEventHandler((s, e) =>
            {
                this.Cursor = Cursors.Hand;
                image6.Visibility = System.Windows.Visibility.Visible;
                if (LastIMG != null) LastIMG.Visibility = System.Windows.Visibility.Collapsed;
                LastIMG = image6;
            });
            image4.MouseEnter += new MouseEventHandler((s, e) =>
            {
                this.Cursor = Cursors.Hand;
                image8.Visibility = System.Windows.Visibility.Visible;
                if (LastIMG != null) LastIMG.Visibility = System.Windows.Visibility.Collapsed;
                LastIMG = image8;
            });
            image5.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            image6.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            image7.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            image8.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            image4.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            /*
            image5.MouseEnter += new MouseEventHandler((s, e) =>
            {
                this.Cursor = Cursors.Hand;
                image8.Visibility = System.Windows.Visibility.Visible;
                if (LastIMG != null) LastIMG.Visibility = System.Windows.Visibility.Collapsed;
                LastIMG = image8;
            });*/
            image5.MouseLeave += new MouseEventHandler(image8_MouseLeave);
            image6.MouseLeave += new MouseEventHandler(image8_MouseLeave);
            image7.MouseLeave += new MouseEventHandler(image8_MouseLeave);
            image8.MouseLeave += new MouseEventHandler(image8_MouseLeave);
        }

        void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender == image8 || sender == image4)
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
                PathDone(sender, null);
            }
            else
            {                
                PathDone(sender, e);
            }
            //throw new NotImplementedException();
        }

        void image8_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (LastIMG != null) LastIMG.Visibility = System.Windows.Visibility.Collapsed;
            //throw new NotImplementedException();
            LastIMG.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
