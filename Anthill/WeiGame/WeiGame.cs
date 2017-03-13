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
using System.Windows.Browser;
using System.IO;

namespace Anthill.WeiGame
{
    public class WeiGame
    {
        public static String auth = "";
        public static int totalScroll = 0;
        public static bool get(String url)
        {
            WebClientEx client = new WebClientEx();
            client.DownloadStringCompleted += new Action<string>(client_DownloadStringCompleted);
            CookieContainer cookie = new CookieContainer();
            String[] s = auth.Split (';');
            foreach (String item in s)
            {
                if (item != "")
                {
                    string[] ms = item.Split('=');
                    cookie.Add(new Uri(url), new Cookie(ms[0], ms[1]));
                }
            }
                try
                {
                    client.SendAsync(url,cookie);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error : " + ex.Message);
                }
                                   
            return true;
        }

        static void client_DownloadStringCompleted(string obj)
        {
            obj = obj;
           
        }
        public WeiGame(){
            
        }
    }
public class WebClientEx 
{
        private readonly static string No_Cache = "no-cache"; 
        private readonly static string Tricky_NeverIfNoneMatch = "2701-4c4585e8"; 
        private HttpWebRequest _request; 
        public WebClientEx() 
        { 
        } 
        public void SendAsync(string url, CookieContainer cookie) 
        { 
            _request = HttpWebRequest.CreateHttp(url); 
            _request.CookieContainer = cookie; 
            _request.Headers[HttpRequestHeader.CacheControl] = No_Cache; 
            _request.Headers[HttpRequestHeader.IfNoneMatch] = Tricky_NeverIfNoneMatch; 
            _request.BeginGetResponse(onResponseCallback, null); 
        } 
        private void onResponseCallback(IAsyncResult ar) 
        { 
            try 
            { 
                WebResponse response = _request.EndGetResponse(ar); 
                string result = null; 
                using (StreamReader sr = new StreamReader(response.GetResponseStream())) 
                { 
                    result = sr.ReadToEnd(); 
                } 
                if (DownloadStringCompleted != null) 
                    DownloadStringCompleted(result); 
            } 
            catch (Exception ) 
            { 
                if (DownloadError != null) 
                    DownloadError(); 
            } 
        } 
        public event Action<string> DownloadStringCompleted; 
        public event Action DownloadError; 
    }
}
