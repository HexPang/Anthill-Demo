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
 * BaseObject
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    /// <summary>
    /// 游戏中所有对象的基类
    /// </summary>
    public abstract class BaseObject : Canvas
    {

        /// <summary>
        /// 获取或设置代号
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// 获取或设置中心
        /// </summary>
        public virtual Point Center { get; set; }
        /*
        /// <summary>
        /// 获取或设置X、Y坐标
        /// </summary>
        public virtual Point Coordinate
        {
            get { return new Point(Canvas.GetLeft(this) + Center.X, Canvas.GetTop(this) + Center.Y); }
            set { Canvas.SetLeft(this, value.X - Center.X); Canvas.SetTop(this, value.Y - Center.Y); }
        }
        */
        /// <summary>
        /// 获取或设置Z层次深度
        /// </summary>
        public int Z
        {
            get { return Canvas.GetZIndex(this); }
            set { Canvas.SetZIndex(this, value); }
        }

        public BaseObject()
        {

        }

    }
}
