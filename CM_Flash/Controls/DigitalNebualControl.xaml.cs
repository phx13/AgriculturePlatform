using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CM_Flash.Controls
{
    /// <summary>
    ///     DigitalNebualControl.xaml 的交互逻辑
    /// </summary>
    public partial class DigitalNebualControl : UserControl
    {
        #region 事件

        /// <summary>
        ///     帧渲染事件
        /// </summary>
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            StarRoamAnimation();
            //RenderLine();
        }

        #endregion

        #region RoutedEvent

        public static readonly RoutedEvent DigitalNebualAnimationCompletedEvent =
            EventManager.RegisterRoutedEvent("DigitalNebualAnimationCompleted",
                RoutingStrategy.Bubble,
                typeof (EventHandler<AnimationCompletedEventArgs>),
                typeof (DigitalNebualControl));

        /// <summary>
        ///     路由事件DigitalNebualAnimationCompletedEvent的CLR事件包装器。
        /// </summary>
        public event RoutedEventHandler DigitalNebualAnimationCompleted
        {
            add { AddHandler(DigitalNebualAnimationCompletedEvent, value); }
            remove { RemoveHandler(DigitalNebualAnimationCompletedEvent, value); }
        }

        /// <summary>
        ///     触发DigitalNebualAnimationCompletedEvent事件。
        /// </summary>
        /// <returns></returns>
        protected AnimationCompletedEventArgs RaiseDigitalNebualAnimationCompleted()
        {
            // 触发路由事件。
            var eventArgs = new AnimationCompletedEventArgs(DigitalNebualAnimationCompletedEvent, this);
            RaiseEvent(eventArgs);
            return eventArgs;
        }

        #endregion

        #region Fileds

        /// <summary>
        ///     星星PathData
        /// </summary>
        /// <summary>
        ///     计数器
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        ///     最大个数
        /// </summary>
        private readonly int _maxStarCount = 100;

        /// <summary>
        ///     最小尺寸
        /// </summary>
        private static readonly int _starSizeMin = 15;

        /// <summary>
        ///     最大尺寸
        /// </summary>
        private static readonly int _starSizeMax = 30;

        /// <summary>
        ///     运动的最小速度(没有使用,为了简单)
        /// </summary>
        private int _starVMin = 10;

        /// <summary>
        ///     运动的最大速度
        /// </summary>
        private readonly int _starVMax = 20;

        /// <summary>
        ///     转动的最小速度
        /// </summary>
        private readonly int _starRVMin = 90;

        /// <summary>
        ///     转动的最大速度
        /// </summary>
        private readonly int _starRVMax = 360;

        /// <summary>
        ///     倍率 (乘以长度)
        /// </summary>
        private readonly int _lineRate = 5;

        /// <summary>
        ///     随机数
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        ///     数组
        /// </summary>
        private List<StarInfo> _stars;

        #endregion

        #region Constructor

        public DigitalNebualControl()
        {
            InitializeComponent();
            //InitUI();
            //注册帧动画
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            Loaded += DigitalNebualControl_Loaded;
        }

        private void DigitalNebualControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
            //SetParaFromUI();
            InitStar();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_stars.Count >= _maxStarCount)
            {
                RaiseDigitalNebualAnimationCompleted();
                timer.Stop();
                timer = null;
            }
            //生成星星
            AddStar(20);
        }

        #endregion

        #region 方法

        /// <summary>
        ///     初始化UI
        /// </summary>
        private void InitUI()
        {
        }

        /// <summary>
        ///     设置参数
        /// </summary>
        private void SetParaFromUI()
        {
        }

        /// <summary>
        ///     初始化星星
        /// </summary>
        private void InitStar()
        {
            //清空星星容器
            _stars = new List<StarInfo>();
            cvs_starContainer.Children.Clear();
            grid_lineContainer.Children.Clear();
            //生成星星
            AddStar(0);
        }


        private void AddStar(int number)
        {
            for (var i = 0; i < number; i++)
            {
                double size = _random.Next(_starSizeMin, _starSizeMax + 1); //星星尺寸
                var starInfo = new StarInfo
                {
                    X = _random.Next(0, (int) cvs_starContainer.ActualWidth),
                    XV = (double) _random.Next(-_starVMax, _starVMax)/60,
                    XT = _random.Next(6, 301), //帧
                    Y = _random.Next(0, (int) cvs_starContainer.ActualHeight),
                    YV = (double) _random.Next(-_starVMax, _starVMax)/60,
                    YT = _random.Next(6, 301) //帧
                };
                //Path star = new Path()
                //{
                //    Data = _pathDataStar,
                //    Width = size,
                //    Height = size,
                //    Stretch = Stretch.Fill,
                //    Fill = GetRandomColorBursh(),
                //    RenderTransformOrigin = new Point(0.5, 0.5),
                //    RenderTransform = new RotateTransform() { Angle = 0 }
                //};

                var star = new Grid();

                var star1 = new Ellipse
                {
                    Width = size,
                    Height = size,
                    Fill = GetRandomColorBursh(),
                    Stretch = Stretch.Fill,
                    Opacity = 0.8,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform {Angle = 0},
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                star1.Effect = new BlurEffect
                {
                    Radius = 10
                };

                var star2 = new Ellipse
                {
                    Width = size*2/5,
                    Height = size*2/5,
                    Fill = new SolidColorBrush(Colors.White),
                    Stretch = Stretch.Fill,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                    RenderTransform = new RotateTransform {Angle = 0},
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                star.Children.Add(star1);
                star.Children.Add(star2);

                Canvas.SetLeft(star, starInfo.X);
                Canvas.SetTop(star, starInfo.Y);
                starInfo.StarRef = star;
                //设置星星旋转动画
                SetStarRotateAnimation(star);
                _stars.Add(starInfo);
                cvs_starContainer.Children.Add(star);
            }
        }

        /// <summary>
        ///     获取随机颜色画刷(偏亮)
        /// </summary>
        /// <returns>SolidColorBrush</returns>
        private SolidColorBrush GetRandomColorBursh()
        {
            var r = (byte) _random.Next(1, 256);
            var g = (byte) _random.Next(1, 256);
            var b = (byte) _random.Next(1, 256);
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        private Color GetRandomColor()
        {
            var r = (byte) _random.Next(128, 256);
            var g = (byte) _random.Next(128, 256);
            var b = (byte) _random.Next(128, 256);
            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        ///     设置星星旋转动画
        /// </summary>
        /// <param name="star"></param>
        private void SetStarRotateAnimation(FrameworkElement star)
        {
            double v = _random.Next(_starRVMin, _starRVMax + 1); //速度
            double a = _random.Next(0, 360*5); //角度
            var t = a/v; //时间
            var dur = new Duration(new TimeSpan(0, 0, 0, 0, (int) (t*1000)));

            var sb = new Storyboard
            {
                Duration = dur
            };
            //动画完成事件 再次设置此动画
            sb.Completed += (S, E) => { SetStarRotateAnimation(star); };

            var da = new DoubleAnimation
            {
                To = a,
                Duration = dur
            };
            Storyboard.SetTarget(da, star);
            Storyboard.SetTargetProperty(da, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            sb.Children.Add(da);
            sb.Begin(this);
        }

        /// <summary>
        ///     星星漫游动画
        /// </summary>
        private void StarRoamAnimation()
        {
            if (_stars == null)
                return;
            foreach (var starInfo in _stars)
            {
                //X轴运动
                if (starInfo.XT > 0)
                {
                    //运动时间大于0,继续运动
                    if (starInfo.X >= cvs_starContainer.ActualWidth || starInfo.X <= 0)
                    {
                        //碰到边缘,速度取反向
                        starInfo.XV = -starInfo.XV;
                    }
                    //位移加,时间减
                    starInfo.X += starInfo.XV;
                    starInfo.XT--;
                    Canvas.SetLeft(starInfo.StarRef, starInfo.X);
                }
                else
                {
                    //运动速度小于0,重新设置速度和时间
                    starInfo.XV = (double) _random.Next(-_starVMax, _starVMax)/60;
                    starInfo.XT = _random.Next(6, 301);
                }
                //Y轴运动
                if (starInfo.YT > 0)
                {
                    //运动时间大于0,继续运动
                    if (starInfo.Y >= cvs_starContainer.ActualHeight || starInfo.Y <= 0)
                    {
                        //碰到边缘,速度取反向
                        starInfo.YV = -starInfo.YV;
                    }
                    //位移加,时间减
                    starInfo.Y += starInfo.YV;
                    starInfo.YT--;
                    Canvas.SetTop(starInfo.StarRef, starInfo.Y);
                }
                else
                {
                    //运动速度小于0,重新设置速度和时间
                    starInfo.YV = (double) _random.Next(-_starVMax, _starVMax)/60;
                    starInfo.YT = _random.Next(6, 301);
                }
            }
        }

        /// <summary>
        ///     呈现星星之间的连线
        /// </summary>
        private void RenderLine()
        {
            #region 方式一

            /*
             * 在每一帧都先清空全部连线
             * 然后再生成全部连线
             */
            //RefreshLine();

            #endregion

            #region 方式二

            /*
             * 遍历星星 如果两个星星的距离小于阀值 且不存在连线 就在两个星星间添加一个连线
             * 遍历连线 如果连线两端的星星的距离小于阀值 就改变连线坐标 否则移除连线
             */
            AddStarLine();
            MoveOrRemoveStarLine();

            #endregion
        }

        /// <summary>
        ///     刷新连线
        /// </summary>
        private void RefreshLine()
        {
            //清空连线
            grid_lineContainer.Children.Clear();
            //没有星星 直接返回
            if (_stars == null)
                return;
            //生成星星间的连线
            for (var i = 0; i < _stars.Count - 1; i++)
            {
                for (var j = i + 1; j < _stars.Count; j++)
                {
                    var star1 = _stars[i];
                    var x1 = star1.X + star1.StarRef.ActualWidth/2;
                    var y1 = star1.Y + star1.StarRef.ActualHeight/2;
                    var star2 = _stars[j];
                    var x2 = star2.X + star2.StarRef.ActualWidth/2;
                    var y2 = star2.Y + star2.StarRef.ActualHeight/2;
                    var s = Math.Sqrt((y2 - y1)*(y2 - y1) + (x2 - x1)*(x2 - x1)); //两个星星间的距离
                    var threshold = star1.StarRef.ActualWidth*_lineRate + star2.StarRef.ActualWidth*_lineRate;
                    if (s <= threshold)
                    {
                        var line = new Line
                        {
                            X1 = x1,
                            Y1 = y1,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = new SolidColorBrush(Colors.AliceBlue),
                            Tag = new[] {star1, star2}
                        };
                        grid_lineContainer.Children.Add(line);
                    }
                }
            }
        }

        /// <summary>
        ///     添加星星之间的连线
        /// </summary>
        private void AddStarLine()
        {
            //没有星星 直接返回
            if (_stars == null)
                return;
            //生成星星间的连线
            for (var i = 0; i < _stars.Count - 1; i++)
            {
                for (var j = i + 1; j < _stars.Count; j++)
                {
                    var star1 = _stars[i];
                    var x1 = star1.X + star1.StarRef.ActualWidth/2;
                    var y1 = star1.Y + star1.StarRef.ActualHeight/2;
                    var star2 = _stars[j];
                    var x2 = star2.X + star2.StarRef.ActualWidth/2;
                    var y2 = star2.Y + star2.StarRef.ActualHeight/2;
                    var s = Math.Sqrt((y2 - y1)*(y2 - y1) + (x2 - x1)*(x2 - x1)); //两个星星间的距离
                    var threshold = star1.StarRef.ActualWidth*_lineRate + star2.StarRef.ActualWidth*_lineRate;
                    if (s <= threshold)
                    {
                        var isLineExist = false; //这两个星之间是否已存在连线
                        foreach (Line existLine in grid_lineContainer.Children)
                        {
                            var stars = existLine.Tag as StarInfo[];
                            if (star1.StarRef == stars[0].StarRef && star2.StarRef == stars[1].StarRef)
                            {
                                isLineExist = true;
                                break;
                            }
                        }
                        //如果不存在 就new一个
                        if (!isLineExist)
                        {
                            var line = new Line
                            {
                                X1 = x1,
                                Y1 = y1,
                                X2 = x2,
                                Y2 = y2,
                                Stroke = new SolidColorBrush(Colors.AliceBlue),
                                Tag = new[] {star1, star2}
                            };
                            grid_lineContainer.Children.Add(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     移动或者移除星星之间的连线
        /// </summary>
        private void MoveOrRemoveStarLine()
        {
            //没有星星 直接返回
            for (var i = 0; i < grid_lineContainer.Children.Count; i++)
            {
                var line = grid_lineContainer.Children[i] as Line;
                var stars = line.Tag as StarInfo[];
                var star1 = stars[0];
                var x1 = star1.X + star1.StarRef.ActualWidth/2;
                var y1 = star1.Y + star1.StarRef.ActualHeight/2;
                var star2 = stars[1];
                var x2 = star2.X + star2.StarRef.ActualWidth/2;
                var y2 = star2.Y + star2.StarRef.ActualHeight/2;
                var s = Math.Sqrt((y2 - y1)*(y2 - y1) + (x2 - x1)*(x2 - x1)); //两个星星间的距离
                var threshold = star1.StarRef.ActualWidth*_lineRate + star2.StarRef.ActualWidth*_lineRate;
                if (s <= threshold)
                {
                    line.X1 = x1;
                    line.Y1 = y1;
                    line.X2 = x2;
                    line.Y2 = y2;
                }
                else
                {
                    grid_lineContainer.Children.Remove(line);
                    i--;
                }
            }
        }

        /// <summary>
        ///     获取星星连线颜色画刷
        /// </summary>
        /// <param name="star0">起始星星</param>
        /// <param name="star1">终点星星</param>
        /// <returns>LinearGradientBrush</returns>
        private LinearGradientBrush GetStarLineBrush(Ellipse star0, Ellipse star1)
        {
            return new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
                {
                    new GradientStop {Offset = 0, Color = (star0.Fill as SolidColorBrush).Color},
                    new GradientStop {Offset = 1, Color = (star1.Fill as SolidColorBrush).Color}
                }
            };
        }

        #endregion
    }
}