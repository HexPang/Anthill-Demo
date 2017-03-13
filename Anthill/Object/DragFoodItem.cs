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

namespace Anthill.Object
{
    public class DragFoodItem : FoodItem
    {
        private Image _DragImage = null;

        /// <summary>
        /// 食物质量(决定蚂蚁拖动时的速度)
        /// </summary>
        public double Quality = 0;
        /// <summary>
        /// 食物是否需要蚂蚁拖动
        /// </summary>
        public bool needDrag = false;
        /// <summary>
        /// 食物拖动时所使用的效果图
        /// </summary>
        public Image DragImage
        {
            get { return _DragImage == null ? FoodImage : _DragImage; }
            set { _DragImage = value; }
        }
        public DragFoodItem(int foodId,int Max,Point position,double w = 148)
            : base(foodId,Max,position,w)
        { 
            
        }
        

    }
}
