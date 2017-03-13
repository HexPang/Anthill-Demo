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
    public class SFX : StaticObject
    {
        MediaElement Media = null;
        public event EventHandler SFXOver;
        public SFX()
        {
            Media = new MediaElement() { CacheMode = new BitmapCache()};
            this.Children.Add(Media);
            //Media.MediaEnded += new RoutedEventHandler(Media_MediaEnded);
        }
        public void Play(string name)
        {
            Media.Source = new Uri(string.Format(@"Resources/sfx/{0}.mp3", name), UriKind.Relative);
            Media.Play();
            if (SFXOver != null)
            {
                Media.MediaEnded +=new RoutedEventHandler(Media_MediaEnded);
            }
        }

        void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            //this.Children.Remove(Media);
            //Media = null;
            SFXOver(this, e);
        }
    }
}
