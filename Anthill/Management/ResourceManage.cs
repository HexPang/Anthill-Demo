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
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Anthill.Management
{
    public class ResourceManage
    {
        //private Hashtable Resources;
        //private HashSet<ImageSource> hs = new HashSet<ImageSource>() { };
        //Hashtable 
        private static Dictionary<string, ImageSource> Resources = new Dictionary<string, ImageSource>() { };
        public static ImageSource GetImageResource(string uri)
        {
            if (Resources.ContainsKey(uri))
            {
                return Resources[uri];
            }
            else
            {
                BitmapImage bi = new BitmapImage(new Uri(uri,UriKind.Relative));
                Resources.Add(uri, bi);
                return bi;
            }
        }
    }
}
