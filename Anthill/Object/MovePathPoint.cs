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
using System.Collections.Generic;
/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill.Object
{
    public class MovePathPoint : StaticObject
    {
        public Image GraphicPoint = null;
        public Point Point = new Point();
        public int PathID = 0;
        public MovePathPoint(int pathID,Point p)
        {
            PathID = pathID;
            GraphicPoint = new Image() { Width = 4, Height = 4, Source = new BitmapImage(new Uri(string.Format(@"Images/path/{0}.png", PathID), UriKind.Relative)) };
            //this.Children.Add(GraphicPoint);
            Point = p;
            Canvas.SetLeft(GraphicPoint, p.X - 2);
            Canvas.SetTop(GraphicPoint, p.Y - 2);
        }
    }
    public class PathMenuEvent : EventArgs
    {
        public enum EventAction
        {
            SHOW_MENU = 0,
            HIDE_MENU = 1,
            REMOVE_PATH = 2,
            CANCEL = 3,
        }
        public EventAction ACTION = 0;
        public PathMenuEvent(EventAction ent = EventAction.SHOW_MENU)
        {
            ACTION = ent;
        }
    }
    public class MovePath : StaticObject
    {
        public List<MovePathPoint> Paths = null;
        public int PathID = 0;
        public event EventHandler PathMenuShow;
        //public event EventHandler RemovePath;
        public MovePath(List<MovePathPoint> p,int Pid)
        {
            PathID = Pid;
            Paths = p;
            if (Paths == null) return;
            ChangeID(Pid);
            foreach (MovePathPoint item in Paths)
            {
                item.GraphicPoint.MouseEnter += new MouseEventHandler(GraphicPoint_MouseEnter);
                item.GraphicPoint.MouseLeave += new MouseEventHandler(GraphicPoint_MouseLeave);
                //item.GraphicPoint.MouseRightButtonDown += new MouseButtonEventHandler(GraphicPoint_MouseRightButtonDown);
            }
            //this.MouseRightButtonDown += new MouseButtonEventHandler(MovePath_MouseRightButtonDown);
        }

        void GraphicPoint_MouseLeave(object sender, MouseEventArgs e)
        {
            if (PathMenuShow == null) return;
            PathMenuShow(this,new PathMenuEvent(PathMenuEvent.EventAction.HIDE_MENU));
            foreach (MovePathPoint item in Paths)
            {
                item.GraphicPoint.Opacity = 0.4;
            }
            //throw new NotImplementedException();
        }

        void GraphicPoint_MouseEnter(object sender, MouseEventArgs e)
        {
            if (PathMenuShow == null) return;
            PathMenuShow(this, new PathMenuEvent(PathMenuEvent.EventAction.SHOW_MENU));
            foreach (MovePathPoint item in Paths)
            {
                item.GraphicPoint.Opacity = 1;
            }
            //throw new NotImplementedException();
        }
        void GraphicPoint_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (MovePathPoint item in Paths)
            {
                item.GraphicPoint.MouseRightButtonDown -= GraphicPoint_MouseRightButtonDown;
            }
            //RemovePath(this, null);
            //throw new NotImplementedException();
        }
        public void ChangeID(int ID)
        {
            //PathLists
            if (Paths == null) return;
            PathID = ID;
            foreach (MovePathPoint item in Paths)
            {
                item.GraphicPoint.Opacity = 0.5;
                item.GraphicPoint.Source = new BitmapImage(new Uri(string.Format(@"Images/path/{0}.png", PathID), UriKind.Relative));
            }
        }
    }
}
