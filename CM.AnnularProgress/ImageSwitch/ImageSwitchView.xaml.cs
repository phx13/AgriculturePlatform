using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.AnnularProgress.ImageSwitch
{
    public partial class ImageSwitchView : ChartViewBase
    {
        private readonly DoubleAnimation m_Animation;
        private readonly ImageSwitchControl m_Controller;
        private readonly ImageSwitchDvm m_DVM;
        private Timer m_Timer;

        public ImageSwitchView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = Controllers[0] as ImageSwitchControl;
            m_DVM = (ImageSwitchDvm) model.DataViewModels[0];

            DataContext = m_Controller;
            m_Controller.DataChanged += DataChanged_Event;

            Loaded += (s, e) => { OnDadChartLoaded(); };

            //m_Animation = new DoubleAnimation();
            //m_Animation.From = 1;
            //m_Animation.To = 0;
            //m_Animation.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            //m_Animation.AutoReverse = true;
            //m_Animation.RepeatBehavior = new RepeatBehavior(2);

            //m_Timer = new Timer(TimerCallback, null, 0, 5000);
        }

        private void DataChanged_Event(AdapterDataTable adt)
        {
            foreach (var row in adt.Rows)
            {
                var month = row[m_DVM.MonthField.AsName].ToString();
                month = month.Substring(5, 3);
                var path = "./Images/" + month + ".png";
                Dispatcher.Invoke(
                    () => { ShowImage.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute)); });
            }
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