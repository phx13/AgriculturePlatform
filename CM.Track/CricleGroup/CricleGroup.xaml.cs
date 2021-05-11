using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.CCP4.Utilities;
using System.Windows.Controls;
using Digihail.DAD.Controls.ViewLayout;
using System.Windows.Media;

namespace CM.Track.CricleGroup
{
    /// <summary>
    ///     CricleGroupControl.xaml 的交互逻辑
    /// </summary>
    public partial class CricleGroup : ChartViewBase
    {
        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public CricleGroup(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = (CricleGroupController)Controllers[0];
            DataContext = m_Controller;
            Loaded += CricleGroupControl_Loaded;
        }

        /// <summary>
        ///     控件的加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CricleGroupControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
            }
            OnDadChartLoaded();
            InitAnimation();
            m_IsLoaded = true;
        }

        private void InitAnimation()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    if (this.GetParentByName<Grid>("PART_LayoutGrid") != null)
                    {
                        var layoutPanel = this.GetParentByName<Grid>("PART_LayoutGrid");

                        var panels = layoutPanel.GetChildsByType<DadViewPanel>();
                        foreach (var panel in panels)
                        {
                            if (panel.PaneTitle == "旋转")
                            {
                                var img = panel.GetChildByName<System.Windows.Controls.Image>("image1");
                                RotateTransform rtf = new RotateTransform();
                                img.RenderTransform = rtf;
                                img.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);

                                m_DoubleAnimation.From = 0;
                                m_DoubleAnimation.To = 360d;
                                m_DoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                                m_DoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(8));

                                rtf.BeginAnimation(RotateTransform.AngleProperty, m_DoubleAnimation);
                            }
                            else
                            {

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }));
        }

        #endregion

        #region 属性

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly CricleGroupController m_Controller;

        private readonly DoubleAnimation m_DoubleAnimation = new DoubleAnimation();

        /// <summary>
        ///     是否加载过图表
        /// </summary>
        private bool m_IsLoaded;

        #endregion

        #region 重写

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        #endregion
    }
}