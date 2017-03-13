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
using System.Xml.Linq;
/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    public class Kernel
    {
        /// <summary>
        /// 加载项目中XML文件
        /// </summary>
        /// <param name="uri">XML文件地址</param>
        /// <returns>XElement</returns>
        public static XElement LoadProjectXML(string uri)
        {
            return XElement.Load(uri);
        }
    }
}
