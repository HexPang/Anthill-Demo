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
using System.Windows.Media.Imaging;
/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    public class FoodItem : StaticObject
    {
        public event EventHandler OutOfFood;
        public int FoodId = 0;
        private int MaxFood = 0;
        private int _food = 0;
        private ProgressBar Health;
        public Image FoodImage = null;
        public bool mustDrag = false;
        /// <summary>
        /// 是哪个龟孙在拖这个食物
        /// </summary>
        public Sprites.SpriteBase Drager = null;

        public int Food
        {
            get
            {
                return _food;
            }
            set
            {
                if (value > MaxFood) value = MaxFood;
                if (value <= 0) { value = 0; OutOfFood(this, null); }
                _food = value;
                Health.Value = (double)((double)Food / (double)MaxFood * 100);
            }
        }
        public FoodItem(int foodId, int Max, Point position, double w = 148,bool mustdrag = false)
        {
            mustDrag = mustdrag;
            Health = new ProgressBar() { Width = w, Height = 10, Foreground = new SolidColorBrush(Colors.Green), Background = new SolidColorBrush(Colors.Black) };
            FoodImage = new Image();
            MaxFood = Max;
            FoodId = foodId;
            Food = Max;
            Health.Visibility = System.Windows.Visibility.Collapsed;
            FoodImage.Source = new BitmapImage(new Uri(string.Format(@"Images/Foods/{0}.png", foodId), UriKind.Relative));
            this.Children.Add(FoodImage);
            this.Children.Add(Health);
            Canvas.SetLeft(Health, 0);
            Canvas.SetTop(Health, -10);
            Canvas.SetLeft(this, position.X);
            Canvas.SetTop(this, position.Y);
            Canvas.SetZIndex(this, 0);
        }
    }
}
