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
using System.Windows.Threading;
/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    /// <summary>
    /// 动态物体
    /// </summary>
    public abstract class DynamicObject : BaseObject
    {

        /// <summary>
        /// 获取或设置生命计时器
        /// </summary>
        internal DispatcherTimer Heart = new DispatcherTimer();

    }
}
