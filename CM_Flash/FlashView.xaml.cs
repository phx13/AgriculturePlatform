using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using CM_Flash.Controls;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM_Flash
{
    /// <summary>
    ///     FlashView.xaml 的交互逻辑
    /// </summary>
    public partial class FlashView : ChartViewBase
    {
        private double m_Height = 0;
        private FlashViewModel m_vm;
        private double m_Width = 0;

        private Storyboard treeStoryboard;

        public FlashView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            Loaded += FlashView_Loaded;
        }

        private void FlashView_Loaded(object sender, RoutedEventArgs e)
        {
            //DigitalNebualControl balls = new DigitalNebualControl();
            //myGrid.Children.Add(balls);
            var balls = new StarrySkyBased();
            myGrid.Children.Add(balls);
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

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void RefreshStyle()
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }
    }
}