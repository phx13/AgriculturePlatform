using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.DeliverData
{
    public partial class DdView : ChartViewBase
    {
        private readonly DoubleAnimation m_Animation;
        private Timer m_Timer;

        public DdView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            DataContext = Controllers[0];

            Loaded += (s, e) => { OnDadChartLoaded(); };

            m_Animation = new DoubleAnimation();
            m_Animation.From = 1;
            m_Animation.To = 0;
            m_Animation.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            m_Animation.AutoReverse = true;

            m_Timer = new Timer(TimerCallback, null, 0, 5000);
        }

        private void TimerCallback(object boj)
        {
            Dispatcher.Invoke(() => { BeginAnimation(OpacityProperty, m_Animation); });
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