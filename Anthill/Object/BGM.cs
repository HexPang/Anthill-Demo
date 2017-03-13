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
    public class BGM : StaticObject
    {
        MediaElement Media = null;
        bool isLoop = false;
        public BGM(string name)
        {
            Media = new MediaElement();
            this.Children.Add(Media);
            Media.Source = new Uri(string.Format(@"Resources/bgm/{0}.mp3",name), UriKind.Relative);
            Media.MediaEnded += new RoutedEventHandler(Media_MediaEnded);
            Media.CacheMode = new BitmapCache();
        }
        public void Play(bool loop = true)
        {
            Media.Play();
            isLoop = loop;
        }

        void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            Media.Position = TimeSpan.FromMilliseconds(0);
            Media.Play();
        }
    }
}
