using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.GrowActivity
{
    public partial class ActivityView : ChartViewBase
    {
        private readonly DoubleAnimation m_Animation;
        private readonly GrowControl m_Controller;
        private readonly GrowDvm m_DVM;

        private Storyboard m_StoryBoard;
        private Timer m_Timer;

        public ActivityView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = Controllers[0] as GrowControl;
            m_DVM = DataViewModels[0] as GrowDvm;
            DataContext = m_Controller;
            m_Controller.DataChanged += DataChanged_Event;

            Loaded += (s, e) => { OnDadChartLoaded(); };

            //m_Animation = new DoubleAnimation();
            //m_Animation.From = 1;
            //m_Animation.To = 0;
            //m_Animation.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            //m_Animation.AutoReverse = true;
            //m_Animation.RepeatBehavior = new RepeatBehavior(2);

            m_Timer = new Timer(TimerCallback, null, 0, 5000);

            InitialAnimation();
        }

        private void InitialAnimation()
        {
            m_StoryBoard = new Storyboard();
            //背景图片移动
            var animation = new DoubleAnimation();
            animation.RepeatBehavior = new RepeatBehavior(1);
            animation.From = m_DVM.ImageFrom;
            animation.To = m_DVM.ImageTo;
            animation.Duration = TimeSpan.FromSeconds(m_DVM.ImageTime);
            Storyboard.SetTarget(animation, rootGrid);

            if (m_DVM.ImageMargin == MarginAlignmentEnum.Bottom)
            {
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.BottomProperty));
            }
            else if (m_DVM.ImageMargin == MarginAlignmentEnum.Left)
            {
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            }
            else if (m_DVM.ImageMargin == MarginAlignmentEnum.Top)
            {
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.TopProperty));
            }
            else if (m_DVM.ImageMargin == MarginAlignmentEnum.Right)
            {
                Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.RightProperty));
            }

            //文字图片移动
            var m_Animation1 = new DoubleAnimation();
            m_Animation1.From = 1;
            m_Animation1.To = 0;
            m_Animation1.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            m_Animation1.AutoReverse = true;
            m_Animation1.RepeatBehavior = new RepeatBehavior(2);
            Storyboard.SetTarget(m_Animation1, firstText);
            Storyboard.SetTargetProperty(m_Animation1, new PropertyPath(OpacityProperty));

            var m_Animation2 = new DoubleAnimation();
            m_Animation2.From = 1;
            m_Animation2.To = 0;
            m_Animation2.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            m_Animation2.AutoReverse = true;
            m_Animation2.RepeatBehavior = new RepeatBehavior(2);
            Storyboard.SetTarget(m_Animation2, secondText);
            Storyboard.SetTargetProperty(m_Animation2, new PropertyPath(OpacityProperty));

            var m_Animation3 = new DoubleAnimation();
            m_Animation3.From = 1;
            m_Animation3.To = 0;
            m_Animation3.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            m_Animation3.AutoReverse = true;
            m_Animation3.RepeatBehavior = new RepeatBehavior(2);
            Storyboard.SetTarget(m_Animation3, thirdText);
            Storyboard.SetTargetProperty(m_Animation3, new PropertyPath(OpacityProperty));

            var m_Animation4 = new DoubleAnimation();
            m_Animation4.From = 1;
            m_Animation4.To = 0;
            m_Animation4.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            m_Animation4.AutoReverse = true;
            m_Animation4.RepeatBehavior = new RepeatBehavior(2);
            Storyboard.SetTarget(m_Animation4, fouthText);
            Storyboard.SetTargetProperty(m_Animation4, new PropertyPath(OpacityProperty));

            m_StoryBoard.Children.Add(m_Animation1);
            m_StoryBoard.Children.Add(m_Animation2);
            m_StoryBoard.Children.Add(m_Animation3);
            m_StoryBoard.Children.Add(m_Animation4);
            m_StoryBoard.Children.Add(animation);
        }

        private void DataChanged_Event()
        {
            m_StoryBoard.Begin();
            //Dispatcher.Invoke(() => { BeginAnimation(OpacityProperty, m_Animation); });
        }

        private void TimerCallback(object boj)
        {
            //Dispatcher.Invoke(() => { BeginAnimation(OpacityProperty, m_Animation); });
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }
    }
}