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
namespace Anthill.Maths
{
    public class GameMath
    {
        /// <summary>
        /// 获取两点之间距离
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <returns>两点之间距离</returns>
        public static double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow((p1.X - p2.X), 2) + Math.Pow((p1.Y - p2.Y), 2));
        }

        /// <summary>
        /// 获取两点之间的角度(弧度制)
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <returns>角度(弧度制)</returns>
        public static double GetAngle(Point p1, Point p2)
        {
            return Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
        }

        /// <summary>
        /// 根据点商值获取角度(笛卡尔坐标系)
        /// </summary>
        /// <param name="y">y2-y1值</param>
        /// <param name="x">x2-x1值</param>
        /// <returns>直角坐标系角度</returns>
        public static double GetAngle(double y, double x)
        {
            return Math.Atan2(y, x) / Math.PI * 180;
        }
        /// <summary>
        /// 计算 targetPoint 之于 soucePoint 的相对角度 360{使用于轮盘}
        /// </summary>
        /// <param name="soucePoint">基準点</param>
        /// <param name="targetPoint">观测点</param>
        /// <returns></returns>
        public static double ConvertPositionAngel(Point soucePoint, Point targetPoint)
        {
            var res = (Math.Atan2(targetPoint.Y - soucePoint.Y, targetPoint.X - soucePoint.X)) / Math.PI * 180.0;
            return (res >= 0 && res <= 180) ? res += 90 : ((res < 0 && res >= -90) ? res += 90 : res += 450);
        }
    }
}
