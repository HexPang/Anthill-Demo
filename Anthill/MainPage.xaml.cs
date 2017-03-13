using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Anthill.Sprites;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using Anthill.Object;
using System.Threading;
using System.Windows.Browser;
/**
 * @author HexPang
 * @website http://github.com/hexpang/
 */
namespace Anthill
{
    public partial class MainPage : UserControl
    {
        int TotalSprite = 0;
        Point StartPoint = new Point(517, 383);
        //Point EndPoint = new Point(853, 427);
        //List<Point> EndPoint = new List<Point> { new Point(853, 427) };
        Image Food = new Image();
        HUD hud = new HUD();
        DispatcherTimer TM = new DispatcherTimer();
        private int FoodCount = 100;
        PathObject PO = new PathObject();
        private List<SpriteBase> Sprites = new List<SpriteBase> { };
        BGM bgm = null;
        bool NoBugs = false;

        void RemoveItem(UIElement obj)
        {
            if (obj == null) return;
            foreach (UIElement item in Map.Children)
            {
                if (item.GetHashCode() == obj.GetHashCode())
                {
                    Map.Children.Remove(obj);
                    return;
                }
            }
        }
        UIElement GetNearby(SpriteBase sb, bool spriteOnly = false, bool evilOnly = false,bool incBase = false)
        {
            foreach (object item in Map.Children)
            {
                if ((item is FoodItem || item is SpriteBase || item is AnthillBase) && sb.Range((UIElement)item) <= sb.ScanRange && item != sb)
                {
                    if (spriteOnly && item is SpriteBase || (item is AnthillBase && incBase))
                    {
                        if (evilOnly && ((SpriteBase)item).Camp == SpriteCampation.EVIL || !evilOnly)
                        {
                            return (UIElement)item;
                        }
                    }
                    else if (!spriteOnly)
                    {
                        return (UIElement)item;
                    }
                }
            }
            return null;
        }
        List<SpriteBase> GetNearBy(Point position, double Range)
        {
            List<SpriteBase> LSB = new List<SpriteBase> { };
            foreach (SpriteBase item in Sprites)
            {
                if (Maths.GameMath.GetDistance(item.Position, position) <= Range)
                {
                    LSB.Add(item);
                }
            }
            return LSB;
        }
        FoodItem GetNearByFood(Point position, double Range)
        {
            foreach (object item in Map.Children)
            {
                if (item is FoodItem)
                {
                    Point p = new Point(Canvas.GetLeft((FoodItem)item),Canvas.GetTop((FoodItem)item));
                    if(Maths.GameMath.GetDistance(position,p) <= Range){
                        return (FoodItem)item;
                    }
                }
            }
            return null;
        }
        private void DoAISearch()
        {
            //Dispatcher.BeginInvoke(() =>
            //{
            for (int i = 0; i < Sprites.Count; i++)
            {
                SpriteBase sb = Sprites[i];
                UIElement Near = null;//GetNearby(sb);
                if (sb.Camp == SpriteCampation.JUSTICE)
                {
                    Near = GetNearby(sb, true, true);
                }
                else if (sb.Camp == SpriteCampation.EVIL)
                {
                    if (sb.SpriteID == 6)
                    {
                        Near = GetNearby(sb);
                    }
                    else
                    {
                        Near = GetNearby(sb,true,false,true);
                    }
                }
                else
                {
                    break;
                }
                if (Near != null && sb.SpriteID != 3)
                {
                    if (Near is FoodItem)
                    {
                        if (sb.SpriteID == 6)
                        {
                            if (sb.Tag == null || (int)sb.Tag != sb.GetHashCode() && ((FoodItem)Near).Food > 0)
                            {
                                sb.Tag = sb.GetHashCode();
                                sb.MoveOver -= BugMoveOver;
                                //sb.StopAll();
                                sb.MoveOver += (ss, ee) =>
                                {
                                    if (((FoodItem)Near).Food > 0)
                                    {
                                        sb.StopAll();
                                        sb.Attacks += sb_Attacks;
                                        sb.StandFrameInterval = 100;
                                        sb.SpriteState = SpriteState.State.ATTACK;
                                        sb.TurnAround(new Point(Canvas.GetLeft(Near) + 74, Canvas.GetTop(Near) + 67));
                                        sb.Attack(null, 217, 220);
                                        sb.Target = Near;
                                    }
                                    else
                                    {
                                        sb.Attacks -= sb_Attacks;
                                        RemoveItem(Near);
                                        sb.SpriteState = SpriteState.State.WALK;
                                        sb.StartPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                                        sb.EndPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                                        sb.Move(sb.EndPoint);
                                        sb.MoveOver += BugMoveOver;
                                    }
                                };
                                sb.Attacks += sb_Attacks;
                                Point dst = new Point(Canvas.GetLeft(Near) + 74 - 50, Canvas.GetTop(Near) + 67 - 50);
                                //dst = new Point((double)RND.Next((int)dst.X - 30, (int)dst.X + 30), (double)RND.Next((int)dst.Y - 30, (int)dst.Y + 30));
                                sb.Move(dst);
                            }
                        }
                    }
                    else if (Near is SpriteBase)
                    {
                        if (sb.Camp == SpriteCampation.JUSTICE)
                        {
                            if (((SpriteBase)Near).Camp == SpriteCampation.EVIL)
                            {
                                if (sb.Target == null)
                                {
                                    sb.Tag = sb.GetHashCode();
                                    Point dst = new Point(Canvas.GetLeft(Near) + ((SpriteBase)Near).SpriteWidth - 10, Canvas.GetTop(Near) + ((SpriteBase)Near).SpriteHeight - 10);
                                    sb.Target = (SpriteBase)Near;
                                    sb.Attacks += new EventHandler(sb_Attacks);

                                    //sb.SpriteState = SpriteState.State.ATTACK;
                                    //if(sb.Paths!=null) sb.Paths.Clear();
                                    if (sb.SpriteID != 5)
                                    {
                                        sb.MoveOver -= BugMoveOver;
                                        sb.MoveOver -= sb_MoveOver;
                                        sb.EndPoint = dst;
                                        sb.MoveOver += SpriteMoveATK;
                                        sb.Move(dst);
                                    }
                                    else
                                    {
                                        sb.StopAll();
                                        sb.SpriteState = SpriteState.State.ATTACK;
                                        sb.TurnAround (((SpriteBase)sb.Target).Position);
                                        sb.Attack((SpriteBase)sb.Target, 315, 317);
                                    }
                                }
                            }
                        }
                        else if (sb.Camp == SpriteCampation.EVIL)
                        {
                            if (((SpriteBase)Near).Camp == SpriteCampation.JUSTICE)
                            {
                                if (sb.Target == null && sb.SpriteID != 6)
                                {
                                    sb.Tag = sb.GetHashCode();
                                    sb.MoveOver -= BugMoveOver;
                                    sb.MoveOver -= sb_MoveOver;
                                    Point dst = new Point(Canvas.GetLeft(Near) + ((SpriteBase)Near).SpriteWidth - 10, Canvas.GetTop(Near) + ((SpriteBase)Near).SpriteHeight - 10);
                                    sb.MoveOver += SpriteMoveATK;
                                    sb.Target = (SpriteBase)Near;
                                    sb.Attacks += new EventHandler(sb_Attacks);
                                    sb.EndPoint = dst;
                                    //sb.SpriteState = SpriteState.State.ATTACK;
                                    //if(sb.Paths!=null) sb.Paths.Clear();
                                }
                            }
                        }
                    }
                    else if (Near is AnthillBase)
                    {
                        //基地阿
                        if (sb.Target == null && sb.SpriteID != 6)
                        {
                            sb.Tag = sb.GetHashCode();
                            sb.MoveOver -= BugMoveOver;
                            sb.MoveOver -= sb_MoveOver;
                            Point dst = new Point();
                            if (Near is SpriteBase)
                            {
                                dst = new Point(Canvas.GetLeft(Near) + ((SpriteBase)Near).SpriteWidth - 10, Canvas.GetTop(Near) + ((SpriteBase)Near).SpriteHeight - 10);
                            }
                            else if (Near is AnthillBase)
                            {
                                dst = ((AnthillBase)Near).Position;
                            }
                            sb.MoveOver += SpriteMoveATK;
                            sb.Target = Near;
                            sb.Attacks += new EventHandler(sb_Attacks);
                            sb.EndPoint = dst;
                            //sb.SpriteState = SpriteState.State.ATTACK;
                            //if(sb.Paths!=null) sb.Paths.Clear();
                        }
                    }
                }
                else if (sb.SpriteID == 3 && !sb.HasFood)
                {
                    int offset = sb.GetNearPathPoint();
                    //Near = GetNearby(sb);
                    Near = GetNearByFood(sb.Position, 50);
                    if (Near != null)
                    {
                        sb.Move(new Point(Canvas.GetLeft(Near) + 74, Canvas.GetTop(Near) + 67));
                        //GotFood(sb,null);
                    }
                }
            }
            //});
        }
        void DoAI()
        {
            while (true)
            {
                Dispatcher.BeginInvoke(DoAISearch);
                //DoAISearch();
                System.Threading.Thread.Sleep(200);
            }
        }
        void SpriteMoveATK(object sb, EventArgs e)
        {
            SpriteBase s = sb as SpriteBase;
            s.StopAll();
            //sb.StandFrameInterval = 100;
            s.SpriteState = SpriteState.State.ATTACK;
            if (s.Target is SpriteBase)
            {
                s.TurnAround(((SpriteBase)s.Target).Position);
            }
            else if(s.Target is AnthillBase)
            {
                s.TurnAround(((AnthillBase)s.Target).Position);
            }
            if (s.SpriteID == 5)
            {
                s.Attack(s.Target, 315, 317);
            }
            else if (s.SpriteID == 9)
            {
                s.Attack(s.Target, 2, 5);
            }
            else if (s.SpriteID == 10)
            {
                s.Attack(s.Target, 43, 46);
            }
            else
            {
                s.Attack(s.Target, 325, 327);
            }
            //s.Attack((SpriteBase)s.Target, 325, 327);
            //SpriteMoveATK
            s.MoveOver -= SpriteMoveATK;
        }
        private bool SpiritOnMap(SpriteBase sb)
        {
            foreach (object obj in Map.Children)
            {
                if (sb == obj)
                {
                    return true;
                }
            }
            return false;
        }
        private Point getPoint(UIElement ui)
        {
            double w = (double)ui.GetValue(ActualWidthProperty) / 2;
            double h = (double)ui.GetValue(ActualHeightProperty) / 2;
            return new Point(Canvas.GetLeft(ui) + w ,Canvas.GetTop(ui) + h);
        }
        void sb_Attacks(object sender, EventArgs e1)
        {
            SpriteAttack e = e1 as SpriteAttack;
            double range = Maths.GameMath.GetDistance(getPoint(e.Damager), e.Attacker.Position);
            /*if (range > e.Attacker.AttackRange)
            {
                e.Attacker.StopAll();
                e.Attacker.Attacks -= sb_Attacks;
                e.Attacker.MoveOver += BugMoveOver;
                e.Attacker.SpriteState = SpriteState.State.WALK;
                if (e.Attacker.Camp == SpriteCampation.JUSTICE)
                {
                    e.Attacker.MoveWithPath(false, false, true);
                }
                else
                {
                    e.Attacker.StartPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                    e.Attacker.EndPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                    e.Attacker.Move(e.Attacker.EndPoint);
                }
                e.Attacker.Target = null;
                e.Attacker.Tag = null;
                //RemoveItem(e.Attacker.Target);
                return;
            }*/
            if (e.Attacker.Target is FoodItem || e.Attacker.Target == null)
            {
                if (e.Attacker.Target != null)
                {
                    ((FoodItem)e.Attacker.Target).Food -= 1;
                    e.Attacker.Health++;
                }
                if (e.Attacker.Target == null || ((FoodItem)e.Attacker.Target).Food <= 0)
                {
                    e.Attacker.StopAll();
                    e.Attacker.Attacks -= sb_Attacks;
                    e.Attacker.MoveOver += BugMoveOver;
                    e.Attacker.StartPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                    e.Attacker.EndPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                    e.Attacker.Move(e.Attacker.EndPoint);
                    e.Attacker.SpriteState = SpriteState.State.WALK;
                    RemoveItem(e.Attacker.Target);
                    e.Attacker.Target = null;
                    e.Attacker.Tag = null;
                    return;
                }
            }
            else if(e.Damager is SpriteBase)
            {
                if (e.Damager == null || !(SpiritOnMap((SpriteBase)e.Damager)))
                {
                    e.Attacker.Attacks -= sb_Attacks;
                    RemoveItem(e.Attacker.Target);
                    e.Attacker.MoveOver += BugMoveOver;
                    e.Attacker.SpriteState = SpriteState.State.WALK;
                    if (e.Attacker.Camp == SpriteCampation.JUSTICE)
                    {
                        e.Attacker.MoveWithPath(false,false,true);
                    }
                    else
                    {
                        e.Attacker.StartPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                        e.Attacker.EndPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                        e.Attacker.Move(e.Attacker.EndPoint);
                    }

                    e.Attacker.Target = null;
                    e.Attacker.Tag = null;
                    //return;
                }
                //e.Attacker.TurnAround(((SpriteBase)e.Damager).Position);
                if (((SpriteBase)e.Damager).Health == 0)
                {
                    e.Attacker.Target = null;
                    e.Attacker.Tag = null;
                    e.Attacker.Attacks -= sb_Attacks;
                    e.Attacker.SpriteState = SpriteState.State.WALK;
                    RemoveItem(e.Damager);
                    if (e.Attacker.Paths != null && e.Attacker.Paths.Count > 0)
                    {
                        double range1 = Maths.GameMath.GetDistance(e.Attacker.Paths[0].Point, e.Attacker.Position);
                        double range2 = Maths.GameMath.GetDistance(e.Attacker.Paths[e.Attacker.Paths.Count - 1].Point, e.Attacker.Position);
                        e.Attacker.MoveOver += sb_MoveOver;
                        if (range1 > range2)
                        {
                            e.Attacker.Move(e.Attacker.Paths[e.Attacker.Paths.Count - 1].Point);
                        }
                        else
                        {
                            e.Attacker.Move(e.Attacker.Paths[0].Point);
                        }
                    }
                    else
                    {
                        e.Attacker.Move(StartPoint);
                    }
                }
                else
                {
                    double atkRange = e.Attacker.AttackRange;
                    if (e.Attacker.SpriteID == 5)
                    {
                        Bullet b = new Bullet(114, 22, 22);
                        Map.Children.Add(b);
                        b.BulletIt += new EventHandler(b_BulletIt);
                        b.Fire(e.Attacker);
                    }
                    else
                    {
                        ((SpriteBase)e.Attacker).sfx.Play("spitter_impact");
                        ((SpriteBase)e.Damager).Damage(e.Attacker, 10);
                    }
                    double Range = Maths.GameMath.GetDistance(e.Attacker.Position, ((SpriteBase)e.Damager).Position);
                    if (Range >= atkRange && e.Attacker.SpriteID != 5)//追击
                    {
                        Point p = ((SpriteBase)e.Damager).Position;
                        p.X += atkRange;
                        p.Y += atkRange;
                        e.Attacker.Move(p);
                        e.Attacker.MoveOver += SpriteMoveATK;
                    }
                }
            }
            else if (e.Damager is AnthillBase)
            {
                AnthillBase ab = e.Damager as AnthillBase;
                if (ab.currHealth > 0)
                {
                    ab.Damaged(1);
                }
                else
                {
                    e.Attacker.StopAll();
                    e.Attacker.Attacks -= sb_Attacks;
                    e.Attacker.MoveOver += BugMoveOver;
                    e.Attacker.StartPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                    e.Attacker.EndPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                    e.Attacker.Move(e.Attacker.EndPoint);
                    e.Attacker.SpriteState = SpriteState.State.WALK;
                    e.Attacker.Target = null;
                    e.Attacker.Tag = null;
                    return;

                }
            }
            //throw new NotImplementedException();
        }

        void b_BulletIt(object sender, EventArgs e)
        {
            ((Bullet)sender).Target.Damage(null, 6);
            Map.Children.Remove((UIElement)sender);
            //throw new NotImplementedException();
        }
        private void resetPointer(bool clear = true)
        {
            if (clear) RemoveLines(0);
            Paths.Clear();
            Canvas.SetLeft(Pointer, StartPoint.X - 24);
            Canvas.SetTop(Pointer, StartPoint.Y - 22.5);
            Pointer.Action();
        }
        public MainPage()
        {
            InitializeComponent();
        }

        void hud_ButtonClicked(object sender, EventArgs e)
        {
            byte c = ((ButtonClick)e).Click;
            if (FoodCount >= 10)
            {
                FoodCount -= 10;
                if (c == 1)
                {
                    SpriteBase sb = new SpriteBase(304, 311, 3, 56, 48);
                    SpriteGoHome(sb);
                }
                else if (c == 2)
                {
                    SpriteBase sb = new SpriteBase(328, 334, 4, 84, 61);
                    SpriteGoHome(sb);
                }
                else if (c == 3)
                {
                    SpriteBase sb = new SpriteBase(318, 324, 5, 73, 52);
                    SpriteGoHome(sb);
                }
            }
            //throw new NotImplementedException();
        }
        void RemovePathButton_Click(object sender, RoutedEventArgs e)
        {
            if (RemovePathButton.Tag != null)
            {
                MovePath MP = RemovePathButton.Tag as MovePath;
                foreach (SpriteBase sb in Sprites)
                {
                    if (sb.Paths != null)
                    {
                        if (sb.Paths.Equals(MP.Paths))
                        {
                            //sb.Paths.Clear();
                            sb.Move(StartPoint);
                        }
                    }
                }
                RemovePaths(MP);
            }
            RemovePathButton.Tag = null;
            RemovePathButton.Visibility = System.Windows.Visibility.Collapsed;
            //throw new NotImplementedException();
        }
        private bool inRemovePathButton = false;
        void RemovePathButton_MouseLeave(object sender, MouseEventArgs e)
        {
            inRemovePathButton = false;
            RemovePathButton.Visibility = System.Windows.Visibility.Collapsed;
            RemovePathButton.Tag = null;
            //throw new NotImplementedException();
        }

        void RemovePathButton_MouseEnter(object sender, MouseEventArgs e)
        {
            inRemovePathButton = true;
            //throw new NotImplementedException();
        }

        void HomeTimer_Tick(object sender, EventArgs e)
        {
            SpriteHomme();
            //throw new NotImplementedException();
        }

        void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            hud.ReSize();
            //throw new NotImplementedException();
        }
        void BugMoveOver(object sb, EventArgs e)
        {
            if (((SpriteBase)sb).Health == 0) { ((SpriteBase)sb).MoveOver -= BugMoveOver; return; }
            if (((SpriteBase)sb).SpriteState == SpriteState.State.ATTACK) return;
            int sID = ((SpriteBase)sb).SpriteID;
            MovePath MPP = getWay(sID);
            if (MPP.PathID != 0)
            {
                ((SpriteBase)sb).Paths = MPP.Paths;
            }
            else
            {
                ((SpriteBase)sb).StartPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
                ((SpriteBase)sb).EndPoint = new Point(RND.Next(0, 800), RND.Next(0, 800));
            }
        }
        void InitGameBugs()
        {
            //AddRandBug_2();
            //AddRandBug_1();
            if (NoBugs) return;
            for (int i = 0; i < 2; i++)
            {
                AddRandBug_1();
            }
            AddRandBug_2();
            AddRandBug_2();
            AddRandBug_3();
            AddRandBug_3();
        }
        private void AddRandBug_3()
        {
            SpriteBase sb = new SpriteBase(47, 55, 10, 106, 75);
            sb.Dead += new EventHandler(sb_Dead);
            sb.MaxHP = 100;
            sb.Health = 100;
            sb.BaseSpeed = 20;
            sb.Position = new Point(-RND.Next(10, 200) * RND.Next(0, 10), -RND.Next(10, 200) * RND.Next(0, 10));
            sb.MoveOver += BugMoveOver;
            sb.Camp = SpriteCampation.EVIL;
            this.Map.Children.Add(sb);
            sb.StartPoint = sb.Position;
            sb.EndPoint = StartPoint;
            sb.Move(sb.EndPoint);
            sb.Damaged += new EventHandler(sb_Damaged);
            Sprites.Add(sb);
            TotalSprite += 1;
        }
        private void AddRandBug_2()
        {
            SpriteBase sb = new SpriteBase(6, 14, 9, 106, 75);
            sb.Dead += new EventHandler(sb_Dead);
            sb.MaxHP = 100;
            sb.Health = 100;
            sb.BaseSpeed = 20;
            sb.Position = new Point(-RND.Next(10, 200) * RND.Next(0, 10), -RND.Next(10, 200) * RND.Next(0, 10));
            sb.MoveOver += BugMoveOver;
            sb.Camp = SpriteCampation.EVIL;
            this.Map.Children.Add(sb);
            sb.StartPoint = sb.Position;
            sb.EndPoint = StartPoint;
            sb.Move(sb.EndPoint);
            sb.Damaged += new EventHandler(sb_Damaged);
            Sprites.Add(sb);
            TotalSprite += 1;
        }
        private void AddRandBug_1()
        {
            SpriteBase sb = new SpriteBase(221, 228, 6, 95, 88);
            sb.Dead += new EventHandler(sb_Dead);
            sb.MaxHP = 100;
            sb.Health = 100;
            sb.BaseSpeed = 30;
            sb.Position = new Point(-RND.Next(10, 200) * RND.Next(0, 10), -RND.Next(10, 200) * RND.Next(0, 10));
            sb.MoveOver += BugMoveOver;
            sb.Camp = SpriteCampation.EVIL;
            this.Map.Children.Add(sb);
            sb.StartPoint = sb.Position;
            sb.EndPoint = StartPoint;
            sb.Move(sb.EndPoint);
            sb.Damaged += new EventHandler(sb_Damaged);
            Sprites.Add(sb);
            TotalSprite += 1;
        }
        void RemoveSprite(SpriteBase sb)
        {
            foreach (SpriteBase item in Sprites)
            {
                if (item.GetHashCode() == sb.GetHashCode())
                {
                    Sprites.Remove(item);
                    return;
                }
            }
        }
        int lastSubmit = 0;
        void sb_Dead(object sender, EventArgs e)
        {
            RemoveSprite((SpriteBase)sender);
            TotalSprite -= 1;
            //((SpriteBase)sender).sfx.Play("death5");
            ((SpriteBase)sender).Attacks -= sb_Attacks;
            ((SpriteBase)sender).StopAll();
            ((SpriteBase)sender).Damaged -= sb_Damaged;
            ((SpriteBase)sender).MoveOver -= sb_MoveOver;
            ((SpriteBase)sender).MoveOver -= BugMoveOver;
            if(((SpriteBase)sender).Camp == SpriteCampation.EVIL){
                WeiGame.WeiGame.totalScroll += 1;
                int[] locks = {10,100,200,500,800,1000,5000,10000};
                for (int i = 0; i < locks.Length - 1; i++)
                {
                    if (WeiGame.WeiGame.totalScroll >= locks[i] && WeiGame.WeiGame.totalScroll < locks[i + 1])
                    {
                        if (locks[i] > lastSubmit)
                        {
                            lastSubmit = WeiGame.WeiGame.totalScroll;
                            WeiGame.WeiGame.get("http://tl.fkmz.net/app/anthill/api/api.php?action=unlock&scroll=" + WeiGame.WeiGame.totalScroll);
                            break;
                        }
                    }
                }
            }
            PlaySFX("death5");
            FoodItem FI = null;
            if (((SpriteBase)sender).SpriteID == 6)
            {
                FI = new FoodItem(216, 10, ((SpriteBase)sender).Position, 79, true);
                AddRandBug_1();
            }
            else if (((SpriteBase)sender).SpriteID == 9)
            {
                FI = new FoodItem(1, 10, ((SpriteBase)sender).Position, 84, true);
                AddRandBug_2();
            }
            else if (((SpriteBase)sender).SpriteID == 10)
            {
                FI = new FoodItem(42, 10, ((SpriteBase)sender).Position, 83, true);
                AddRandBug_3();
            }
            if (FI != null)
            {
                FI.FoodImage.Projection = ((SpriteBase)sender).BugPanel.Projection;
                FI.OutOfFood += OutofFood;
                Map.Children.Add(FI);
            }
            ((SpriteBase)sender).Dead -= sb_Dead;
            Sprites.Remove((SpriteBase)sender);
            RemoveItem((UIElement)sender);
            ((SpriteBase)sender).Dispose();
            sender = null;
            //GC.Collect();
            //throw new NotImplementedException();
        }

        void sb_Damaged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        private Point dest = new Point();
        void GotFood(object ss, EventArgs e)
        {
            if (((SpriteBase)ss).HasFood)
            {
                FoodCount += 2;
                //((SpriteBase)ss).ChangeSpeed(1);
                ((SpriteBase)ss).sfx.Play("collect_resource");
                label1.Content = string.Format("食物:{0}", FoodCount);
                ((SpriteBase)ss).resetfoodImage();
                ((SpriteBase)ss).HasFood = false;
            }
            else
            {
                FoodItem FI = FindFood((SpriteBase)ss);
                if (FI != null)
                {
                    if (FI.mustDrag)
                    {
                        ((SpriteBase)ss).foodImage.Source = FI.FoodImage.Source;
                        ((SpriteBase)ss).HasFood = true;
                        Canvas.SetLeft(((SpriteBase)ss).foodImage, 0);
                        Canvas.SetTop(((SpriteBase)ss).foodImage, 0);
                        FI.Food = 0;
                    }
                    else
                    {
                        FI.Food -= 2;
                        ((SpriteBase)ss).HasFood = true;

                        //((SpriteBase)ss).MoveWithPath(true);
                    }
                }
            }
        }
        void RemovePath(Point endPoint)
        {
            foreach (object o in Map.Children)
            {
                if (o is PathLine)
                {
                    double Range = Maths.GameMath.GetDistance(endPoint, ((PathLine)o)._end);
                    if (Range <= 50)
                    {
                        endPoint = ((PathLine)o)._end;
                        Map.Children.Remove((PathLine)o);
                        break;
                    }
                }
            }
        }
        private bool PODone = false;
        void PO_PathDone(object sender, EventArgs e)
        {
            PODone = true;
            Point destination = dest;
            if (e == null)
            {
                PO.Visibility = System.Windows.Visibility.Collapsed;
                /*if (Pointer != null)
                {
                    Pointer.Visibility = System.Windows.Visibility.Collapsed;
                }*/
                resetPointer();
                leftDown = false;
                return;
            }
            if (e != null)
            {
                Image img = sender as Image;
                Uri u = ((BitmapImage)img.Source).UriSource;
                string s = u.OriginalString;
                s = s.Substring(s.LastIndexOf('/') + 1);
                s = s.Replace(".png", "");
                if (s == "Ring_1")
                {
                    List<MovePathPoint> MPP = new List<MovePathPoint>() { };
                    MPP.AddRange(Paths);
                    MovePath MP = new MovePath(MPP, 3);
                    MP.PathMenuShow += new EventHandler(MP_PathMenuShow);
                    PathLists.Add(MP);
                }
                else if (s == "Ring_3")
                {
                    List<MovePathPoint> MPP = new List<MovePathPoint>() { };
                    MPP.AddRange(Paths);
                    MovePath MP = new MovePath(MPP, 4);
                    MP.PathMenuShow += new EventHandler(MP_PathMenuShow);
                    PathLists.Add(MP);
                }
                else if (s == "Ring_2")
                {
                    List<MovePathPoint> MPP = new List<MovePathPoint>() { };
                    MPP.AddRange(Paths);
                    MovePath MP = new MovePath(MPP, 5);
                    MP.PathMenuShow += new EventHandler(MP_PathMenuShow);
                    PathLists.Add(MP);
                    /*
                    SpriteBase sb = new SpriteBase(318, 324, 5, 73, 52);
                    sb.Position = StartPoint;
                    sb.MoveOver += new EventHandler(sb_MoveOver);
                    this.Map.Children.Add(sb);
                    sb.StartPoint = StartPoint;
                    sb.AttackRange = 300;
                    sb.ScanRange = 500;
                    sb.StartPoint = StartPoint;
                    //sb.EndPoint = EndPoint[0];
                    // sb.Move(sb.EndPoint);
                    Sprites.Add(sb);
                    TotalSprite += 1;
                    Canvas.SetZIndex(sb, 10);
                    // PathLine pl = new PathLine(StartPoint, destination);
                    //Map.Children.Add(pl);
                    //Canvas.SetZIndex(pl, 0);
                    // pl.Start("path_dot_spitter[4444]_low");

                    List<MovePathPoint> MPP = new List<MovePathPoint>() { };
                    MPP.AddRange(Paths);
                    //Paths.Clear();
                    sb.Paths = MPP;
                    MovePath MP = new MovePath(MPP, 5);
                    MP.PathMenuShow += new EventHandler(MP_PathMenuShow);
                    //MP.RemovePath += new EventHandler(MP_RemovePath);
                    PathLists.Add(MP);
                    sb.MoveWithPath(false);*/
                }
                //Pointer.Visibility = System.Windows.Visibility.Collapsed;
                Map.Children.Remove(MouseLine);
                MouseLine = null;
            }
            resetPointer(false);
            PO.Visibility = System.Windows.Visibility.Collapsed;
            if (MouseLine != null)
            {
                Pointer.Visibility = System.Windows.Visibility.Collapsed;
                Map.Children.Remove(MouseLine);
                MouseLine = null;
            }
            /*
            if (Pointer != null)
            {
                Pointer.Visibility = System.Windows.Visibility.Collapsed;
            }*/
            //throw new NotImplementedException();
        }

        void MP_PathMenuShow(object sender, EventArgs e)
        {
            PathMenuEvent PME = e as PathMenuEvent;
            if (PME.ACTION == PathMenuEvent.EventAction.SHOW_MENU)
            {
                Canvas.SetLeft(RemovePathButton, MousePoint.X - 10);
                Canvas.SetTop(RemovePathButton, MousePoint.Y - 10);
                RemovePathButton.Visibility = System.Windows.Visibility.Visible;
                RemovePathButton.Tag = sender;
            }
            else if (PME.ACTION == PathMenuEvent.EventAction.HIDE_MENU && !inRemovePathButton)
            {
                RemovePathButton.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (PME.ACTION == PathMenuEvent.EventAction.REMOVE_PATH)
            {

            }
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 获取食物
        /// </summary>
        /// <param name="sb">虫子</param>
        /// <returns>返回食物</returns>
        FoodItem FindFood(SpriteBase sb)
        {
            foreach (object obj in Map.Children)
            {
                if (obj is FoodItem)
                {
                    double range = sb.Range((UIElement)obj);
                    if (range <= 100)
                    {
                        return (FoodItem)obj;
                    }
                }
            }
            return null;
        }
        void MainPage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            return;
            if (FoodCount >= 10)
            {
                FoodCount -= 10;
                label1.Content = string.Format("食物:{0}", FoodCount);
                SpriteBase sb = new SpriteBase(304, 311, 3, 56, 48);
                SpriteGoHome(sb);
            }
            //throw new NotImplementedException();
        }

        void TM_Tick(object sender, EventArgs e)
        {
            label2.Content = string.Format("当前分数：{0}", WeiGame.WeiGame.totalScroll);
        }
        public void InitSprites()
        {
            for (int i = 0; i < 1; i++)
            {
                SpriteBase sb1 = new SpriteBase(304, 311, 3, 56, 48);
                sb1.ScanRange = 100;
                sb1.AttackRange = 100;
                SpriteGoHome(sb1);
            }
            SpriteBase sb = new SpriteBase(328, 334, 4, 84, 61);
            sb.Position = StartPoint;
            sb.StartPoint = StartPoint;
            sb.Health = 150;
            sb.MaxHP = 150;
            SpriteGoHome(sb);
        }
        Random RND = new Random(DateTime.Now.Millisecond);
        private MovePath getWay(int id)
        {
            List<MovePath> rPoint = new List<MovePath>() { };
            foreach (MovePath obj in PathLists)
            {
                if (obj.PathID == id)
                {
                    rPoint.Add(obj);
                }
            }
            if (rPoint.Count > 0)
            {
                return rPoint[RND.Next(0, rPoint.Count)];
            }
            return new MovePath(null, 0);
        }
        void sb_MoveOver(object sender, EventArgs e)
        {
            SpriteGoHome((SpriteBase)sender);
            /*
            int sID = ((SpriteBase)sender).SpriteID;
            MovePath MPP = getWay(sID);
            if (MPP.PathID != 0) { ((SpriteBase)sender).Paths = MPP.Paths; } else { ((SpriteBase)sender).Move(StartPoint); }
            */
        }

        private void Map_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RectangleGeometry R = new RectangleGeometry();
            R.Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
            //Map.Clip = R;
            Canvas.SetLeft(hud, 0);
            Canvas.SetTop(hud, 0);
            //Map.Clip.Bounds = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);
        }
        void OutofFood(object sender, EventArgs e)
        {
            RemoveItem((FoodItem)sender);
            sender = null;
        }
        private void GameStart()
        {
            InitSprites();
            Map.Children.Add(PO);
            Canvas.SetZIndex(PO, 9999);
            PO.Visibility = System.Windows.Visibility.Collapsed;
            PO.PathDone += new EventHandler(PO_PathDone);
            //this.MouseWheel += new MouseWheelEventHandler(MainPage_MouseWheel);
            LayoutRoot.Children.Add(hud);
            this.SizeChanged += new SizeChangedEventHandler(MainPage_SizeChanged);
            this.MouseRightButtonDown += new MouseButtonEventHandler(MainPage_MouseRightButtonDown);
            Pointer.Source = new BitmapImage(new Uri(@"Images/Pointer.png", UriKind.Relative));
            //Pointer.Visibility = System.Windows.Visibility.Collapsed;
            Map.Children.Add(Pointer);
            Canvas.SetZIndex(Pointer, 100);
            resetPointer();
            DispatcherTimer HomeTimer = new DispatcherTimer();
            HomeTimer.Interval = TimeSpan.FromMilliseconds(400);
            HomeTimer.Tick += new EventHandler(HomeTimer_Tick);
            HomeTimer.Start();
            TM.Tick += new EventHandler(TM_Tick);
            TM.Interval = new TimeSpan(0, 0, 0, 0, 10);
            TM.Start();
            DispatcherTimer DT = new DispatcherTimer();
            DT.Tick += new EventHandler((ss, ee) =>
            {
                InitGameBugs();
                DT.Stop();
            });
            DT.Interval = new TimeSpan(0, 0, 0, 10);
            DT.Start();
            bgm = new BGM("boa_vista");
            Map.Children.Add(bgm);
            bgm.Play(true);
            RemovePathButton = new Button() { Width = 40, Height = 40, Content = "X" };
            RemovePathButton.Visibility = System.Windows.Visibility.Collapsed;
            Map.Children.Add(RemovePathButton);
            RemovePathButton.MouseEnter += new MouseEventHandler(RemovePathButton_MouseEnter);
            RemovePathButton.MouseLeave += new MouseEventHandler(RemovePathButton_MouseLeave);
            RemovePathButton.Click += new RoutedEventHandler(RemovePathButton_Click);
            Canvas.SetZIndex(RemovePathButton, 9999);
            hud.ButtonClicked += new EventHandler(hud_ButtonClicked);

            Map.MouseLeftButtonDown += new MouseButtonEventHandler(Map_MouseLeftButtonDown);
            Map.MouseLeftButtonUp += new MouseButtonEventHandler(Map_MouseLeftButtonUp);
            FoodItem FI = new FoodItem(237, 50000, new Point(780, 200));
            FI.OutOfFood += OutofFood;
            Map.Children.Add(FI);
            if (!NoBugs)
            {
                for (int i = 0; i < 10; i++)
                {
                    SpriteBase sb = new SpriteBase(229, 236, 8, 41, 32, System.Windows.Visibility.Collapsed);
                    sb.StartPoint = new Point(0, 0);
                    sb.EndPoint = new Point(RND.Next(0, 148), RND.Next(0, 134));
                    FI.Children.Add(sb);
                    sb.Camp = SpriteCampation.NEUTRALITY;
                    sb.StandFrameInterval = 20;
                    sb.BaseSpeed = 7;
                    sb.MoveOver += new EventHandler(small_bugMove);
                    sb.Move(sb.EndPoint);
                }
            }
            Thread t = new Thread(DoAI);
            t.Start();
            anthillBase = new AnthillBase(157, 156, StartPoint, 300);
            Map.Children.Add(anthillBase);
            anthillBase.BaseDestory += new EventHandler(anthillBase_BaseDestory);
        }
        private Storyboard dropper = null;
        private Image Dropper = null;
        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            Dropper = new Image() { Width = 534, Height = 115, Stretch = Stretch.None };
            LayoutRoot.Children.Add(Dropper);
            dropper = new Storyboard();
            //GameStart();
            ShowTitle();
            GameStart();

            //WeiGame.WeiGame.auth = "wyx_user_id=2661166445;wyx_session_key=cac2e0b70d78ea02b0ff6bd8c8aedb1a8f770953_1340029379_2661166445;wyx_create=1340029379;wyx_expire=1340065379;wyx_signature=ee2e3f497a9878e242b95fa8ddcc92e10988b9ba";//HtmlPage.Document.DocumentUri.Query.Substring(1).Replace ("&",";");
            if(HtmlPage.Document.DocumentUri.Query.Length >1) WeiGame.WeiGame.auth = HtmlPage.Document.DocumentUri.Query.Substring(1).Replace ("&",";");
            //MessageBox.Show(WeiGame.WeiGame.auth);
            //WeiGame.WeiGame.get("http://tl.fkmz.net/api/api.php?action=unlock&scroll=15");
            //HtmlPage.Document.Cookies
            //GameStart();

        }
        private bool autoHideTitle = true;
        void ShowTitle(string title = "Images/Start.png",bool autoHide = true)
        {
            dropper.Pause();
            dropper.Stop();
            dropper = null;
            dropper = new Storyboard();
//            dropper.Children.Clear();
            autoHideTitle = autoHide;
            Dropper.Visibility = System.Windows.Visibility.Visible;
            Dropper.Source = new BitmapImage(new Uri(title, UriKind.Relative));
            Canvas.SetLeft(Dropper, Map.Width / 2 - Dropper.Width / 2);
            DoubleAnimationUsingKeyFrames DAUKF = new DoubleAnimationUsingKeyFrames();
            DAUKF.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)), Value = -120 });
            EasingDoubleKeyFrame edkf = new EasingDoubleKeyFrame() { KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000)), Value = Map.Height / 2 };
            edkf.EasingFunction = new ElasticEase() { EasingMode = EasingMode.EaseOut };
            DAUKF.KeyFrames.Add(edkf);
            Storyboard.SetTarget(DAUKF, Dropper);
            Storyboard.SetTargetProperty(DAUKF, new PropertyPath("(Canvas.Top)"));
            dropper.Completed += new EventHandler(dropper_Completed);
            dropper.Children.Add(DAUKF);
            dropper.Begin();
        }
        void dropper_Completed(object sender, EventArgs e)
        {
            if (autoHideTitle)
            {
                DispatcherTimer dt = new DispatcherTimer();
                dt.Tick += new EventHandler(dt_Tick);
                dt.Interval = TimeSpan.FromMilliseconds(1000);
                dt.Start();
            }
            //throw new NotImplementedException();
        }
        void dt_Tick(object sender, EventArgs e)
        {
            Dropper.Visibility = System.Windows.Visibility.Collapsed;
            DispatcherTimer dt = sender as DispatcherTimer;
            dt.Tick -= dt_Tick;
            dt.Stop();
            //throw new NotImplementedException();
        }

        void anthillBase_BaseDestory(object sender, EventArgs e)
        {
            if (WeiGame.WeiGame.totalScroll > 0)
            {
                WeiGame.WeiGame.get("http://tl.fkmz.net/app/anthill/api/api.php?action=scroll&scroll=" + WeiGame.WeiGame.totalScroll);
            }
            HomeSprites.Clear();
            hud.Visibility = System.Windows.Visibility.Collapsed;
            Pointer.Visibility = System.Windows.Visibility.Collapsed;
            while (PathLists.Count > 0)
            {
                RemovePaths(PathLists[0]);
            }
            ShowTitle("Images/Failed.png",false);
            //MessageBox.Show("任务失败，基地已被摧毁。");
            //throw new NotImplementedException();
        }
        private AnthillBase anthillBase = null;
        private Point ClickPoint = new Point();
        void small_bugMove(object sender, EventArgs e)
        {
            SpriteBase sb = sender as SpriteBase;
            sb.StartPoint = new Point(RND.Next(0, 148), RND.Next(0, 134));
            Point p = new Point(RND.Next(0, 148), RND.Next(0, 134));
            sb.Move(p);
        }
        void Bomb_MoveOver(object sender, EventArgs e)
        {
            SpriteBase sb = sender as SpriteBase;
            double r = Maths.GameMath.GetDistance(sb.Position, sb.StartPoint);
            if (sb.Tag==null)
            {
                sb.MoveOver -= Bomb_MoveOver;
                Map.Children.Remove(sb);
                sb.Dispose();
            }
            else
            {
                Map.Children.Remove((Arrow)sb.Tag);
                sb.Tag = null;
                for (int i = 0; i < 3; i++)
                {
                    Point p = sb.EndPoint;
                    if (RND.Next(0, 1) == 1)
                    {
                        p.X += RND.Next(1, 50);
                        p.Y += RND.Next(1, 50);
                    }
                    else
                    {
                        p.X -= RND.Next(1, 50);
                        p.Y -= RND.Next(1, 50);
                    }
                    Effect eff = new Effect("Bomb", p, 1, 101, 200, 150);
                    eff.EffectOver += new EventHandler(eff_EffectOver);
                    Map.Children.Add(eff);
                    Canvas.SetZIndex(eff, 20);
                }
                sb.EndPoint = StartPoint;
                sb.EndPoint = sb.StartPoint;
                sb.sfx.Play("bomb1");
                //炸弹伤害
                List<SpriteBase> LSB = GetNearBy(sb.Position, 100);
                if (LSB.Count > 0)
                {
                    for (int i = 0; i < LSB.Count; i++)
                    {
                        if (LSB[i].Camp != sb.Camp) LSB[i].Damage(sb, 80 + RND.Next(10, 100));
                    }
                }

            }
        }

        void eff_EffectOver(object sender, EventArgs e)
        {
            Map.Children.Remove((Effect)sender); //throw new NotImplementedException();
            ((Effect)sender).EffectOver -= eff_EffectOver;
        }

        void Map_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (anthillBase.currHealth == 0) return;
            if (PO.Visibility == System.Windows.Visibility.Visible) return;
            Point destination = e.GetPosition(Map);
            if (move == 0 && Paths.Count == 0 && FoodCount >= 10 && !PODone)
            {
                //if (FoodCount >= 50)
                //{
                FoodCount -= 10;
                SpriteBase sb = new SpriteBase(336, 337, 7, 77, 113);
                sb.Heart.Interval = new TimeSpan(0, 0, 0, 0, 100);
                sb.BaseSpeed = 5;
                sb.StartPoint = StartPoint;
                sb.sfx.Play("pilotant_loop2");
                sb.Position = StartPoint;
                sb.MoveOver += new EventHandler(Bomb_MoveOver);
                this.Map.Children.Add(sb);
                Canvas.SetZIndex(sb, 100);
                //sb.StartPoint = new Point(0, 0);
                Arrow a = new Arrow();
                Canvas.SetLeft(a, destination.X - 28);
                Canvas.SetTop(a, destination.Y - 27.5);
                Map.Children.Add(a);
                sb.Tag = a;
                sb.EndPoint = destination;
                sb.Move(sb.EndPoint);
                Map.Children.Remove(MouseLine);
                MouseLine = null;
                //}
                return;
            }
            move = 0;
            PODone = false;
            if (!leftDown)
            {
                Map.Children.Remove(MouseLine);
                MouseLine = null;
                return;
            }
            if (Pointer.Visibility == System.Windows.Visibility.Collapsed)
            {
                return;
            }
            leftDown = false;
            Canvas.SetLeft(PO, MousePoint.X - 125);
            Canvas.SetTop(PO, MousePoint.Y - 105);
            PO.Visibility = System.Windows.Visibility.Visible;

            //throw new NotImplementedException();
        }
        private bool leftDown = false;
        void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (anthillBase.currHealth == 0) return;
            ClickPoint = e.GetPosition(Map);
            if (PO.Visibility == System.Windows.Visibility.Visible) return;
            Point destination = e.GetPosition(Map);
            //if (Maths.GameMath.GetDistance(destination, StartPoint) >= 300) { return; }
            if (Maths.GameMath.GetDistance(destination, StartPoint) <= 20)
            {
                leftDown = true;
                if (MouseLine != null)
                {
                    Pointer.Visibility = System.Windows.Visibility.Collapsed;
                    Map.Children.Remove(MouseLine);
                    MouseLine = null;
                    Image Foods = new Image();
                    Foods.Source = new BitmapImage(new Uri(@"Images/foods.png", UriKind.Relative));
                    Canvas.SetLeft(Foods, destination.X - 21);
                    Canvas.SetTop(Foods, destination.Y - 19);
                    Canvas.SetZIndex(Foods, 0);
                }
            }


            /*
            Map.Children.Add(Foods);
            EndPoint.Add(destination);
            Canvas.SetLeft(Foods, destination.X - 74);
            Canvas.SetTop(Foods, destination.Y - 73);
            Canvas.SetZIndex(Foods, 0);*/
            // sb.Move(destination);
        }
        private Line MouseLine = null;
        Point MousePoint = new Point();
        MousePointer Pointer = new MousePointer();
        int move = 0;

        private List<MovePathPoint> Paths = new List<MovePathPoint>() { };
        int FindPointByPoint(Point p)
        {
            int i = 0;
            foreach (MovePathPoint item in Paths)
            {
                if (Maths.GameMath.GetDistance(item.Point, p) <= 5) return i;
                i++;
            }
            return -1;
        }
        void RemovePaths(MovePath MP)
        {
            while (MP.Paths.Count > 0)
            {
                Map.Children.Remove(MP.Paths[0].GraphicPoint);
                MP.Paths.RemoveAt(0);
            }
            MP.PathMenuShow -= MP_PathMenuShow;
            PathLists.Remove(MP);
        }
        void RemoveLines(int offset)
        {
            if (offset < 0) return;
            while (Paths.Count > offset)
            {
                Map.Children.Remove(Paths[offset].GraphicPoint);
                Paths.RemoveAt(offset);
            }
        }
        private List<MovePath> PathLists = new List<MovePath>() { };

        /// <summary>
        /// 路径点是否重复
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool isPointRepeat(Point p)
        {
            foreach (MovePathPoint item in Paths)
            {
                if (Maths.GameMath.GetDistance(p, item.Point) <= 10)
                {
                    return true;
                }
            }
            return false;
        }
        private double ZoomRange = 0;
        private void Map_MouseMove(object sender, MouseEventArgs e)
        {

            //ClickPoint = new Point();
            if (PO.Visibility == System.Windows.Visibility.Visible) return;
            MousePoint = e.GetPosition(Map);
            /*if (Maths.GameMath.GetDistance(MousePoint, StartPoint) >= 300) {
                if (Pointer.Visibility == System.Windows.Visibility.Visible)
                {
                    leftDown = false;
                    Pointer.Visibility = System.Windows.Visibility.Collapsed;
                    Map.Children.Remove(MouseLine);
                    MouseLine = null;
                }
                return;
            }*/
            dest = MousePoint;
            //Application.Current
            if (leftDown)
            {
                Pointer.Stop();
                move++;
                Pointer.Visibility = System.Windows.Visibility.Visible;
                Canvas.SetLeft(Pointer, MousePoint.X - 24);
                Canvas.SetTop(Pointer, MousePoint.Y - 23);
                Canvas.SetZIndex(Pointer, 100);
                Point lastPoint = Paths != null && Paths.Count > 0 ? Paths[Paths.Count - 1].Point : StartPoint;
                double range = Maths.GameMath.GetDistance(MousePoint, lastPoint);
                //label1.Content = string.Format("{0} / {1}", ZoomRange, range);
                if (ZoomRange == 0)
                {
                    ZoomRange = range;
                }
                else
                {
                    if (ZoomRange > range && Paths.Count > 0)
                    {
                        Map.Children.Remove(Paths[Paths.Count - 1].GraphicPoint);
                        Paths.RemoveAt(Paths.Count - 1);
                        ZoomRange = 0;
                        return;
                    }
                }
                ZoomRange = range;
                double pRate = range / 10;
                double xRate = Math.Abs(Math.Abs(dest.X) - Math.Abs(lastPoint.X));
                double yRate = Math.Abs(Math.Abs(dest.Y) - Math.Abs(lastPoint.Y));
                xRate /= pRate;
                yRate /= pRate;
                if (lastPoint.X > dest.X) xRate = -xRate;
                if (lastPoint.Y > dest.Y) yRate = -yRate;
                for (int i = 0; i < (int)pRate; i++)
                {
                    Point nPoint = new Point(lastPoint.X + xRate, lastPoint.Y + yRate);
                    if (isPointRepeat(nPoint)) return;
                    Paths.Add(new MovePathPoint(1, nPoint));
                    lastPoint = nPoint;
                    Map.Children.Add(Paths[Paths.Count - 1].GraphicPoint);

                }
                if (move % 5 == 0) PlaySFX("text_blip");
            }
        }
        void PlaySFX(string name)
        {
            SFX sfx = new SFX();
            sfx.SFXOver += new EventHandler(sfx_SFXOver);
            Map.Children.Add(sfx);
            sfx.Play(name);
        }
        void sfx_SFXOver(object sender, EventArgs e)
        {
            Map.Children.Remove((SFX)sender);
            sender = null;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 分配在家的虫虫干活
        /// </summary>
        private void SpriteHomme()
        {
            try
            {
                if (HomeSprites.Count > 0)
                {
                    int offset = 0;
                    SpriteBase SB = HomeSprites[offset];
                    MovePath MP = getWay(SB.SpriteID);
                    while (MP.PathID == 0)
                    {
                        offset++;
                        if (offset >= HomeSprites.Count)
                        {
                            return;
                        }
                        SB = HomeSprites[offset];
                        MP = getWay(SB.SpriteID);
                    }
                    HomeSprites.RemoveAt(offset);
                    SB.Paths = MP.Paths;
                    SB.MoveOver += new EventHandler(sb_MoveOver);
                    SB.Dead += new EventHandler(sb_Dead);
                    this.Map.Children.Add(SB);
                    try
                    {
                        SB.Position = StartPoint;
                    }
                    catch (Exception ex)
                    {

                    }
                    SB.StartPoint = StartPoint;
                    Sprites.Add(SB);
                    TotalSprite += 1;
                    Canvas.SetZIndex(SB, 10);
                    if (SB.SpriteID == 3) SB.GotFood += GotFood;
                    SB.MoveWithPath();
                }
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 虫虫回家家
        /// </summary>
        /// <param name="SB">虫虫</param>
        private void SpriteGoHome(SpriteBase SB)
        {
            SB.Paths = new List<MovePathPoint>() { };
            TotalSprite -= 1;
            SB.Attacks -= sb_Attacks;
            SB.StopAll();
            SB.Damaged -= sb_Damaged;
            SB.MoveOver -= sb_MoveOver;
            SB.MoveOver -= BugMoveOver;
            SB.GotFood -= GotFood;
            SB.HasFood = false;
            RemoveSprite(SB);
            Sprites.Remove(SB);
            RemoveItem((UIElement)SB);
            HomeSprites.Add(SB);
        }
        private List<SpriteBase> HomeSprites = new List<SpriteBase>() { };

        private Button RemovePathButton = null;

        private void Map_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScaleTransform st = (ScaleTransform)Map.RenderTransform ;
            if (e.Delta > 0)
            {
                if (st.ScaleX < 1)
                {
                    Map.RenderTransform = new ScaleTransform() { ScaleX = st.ScaleX + 0.1, ScaleY = st.ScaleY + 0.1 };
                }
            }
            else
            {
                if (st.ScaleX > 0.4)
                {
                    Map.RenderTransform = new ScaleTransform() { ScaleX = st.ScaleX - 0.1, ScaleY = st.ScaleY - 0.1 };
                }
            }
        }
    }
}
