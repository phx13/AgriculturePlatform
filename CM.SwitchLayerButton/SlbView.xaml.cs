using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.CCP4.Helper;
using Digihail.CCP4.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.SwitchLayerButton
{
    public partial class SlbView : ChartViewBase
    {
        /// <summary>
        ///     当前CCP页面ID
        /// </summary>
        private readonly Guid m_CurrentPageGuid;

        /// <summary>
        ///     消息聚合器
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator;

        public SlbView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            DataContext = Controllers[0];

            Loaded += (s, e) => { OnDadChartLoaded(); };

            //获取当前CCP页面的GUID
            m_CurrentPageGuid = CCPHelper.Instance.GetCurrentPageModel().PageGuid;

            m_MessageAggregator = new MessageAggregator();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mode = (DataModel) ((Button) sender).DataContext;

            var ctl = (SlbControl) DataContext;

            var data = new ObjectShowOrHideData();
            data.From = m_CurrentPageGuid;
            data.PageInstanceId = Guid.NewGuid();
            data.ObjectShowOrHideInvokeBy = ObjectShowOrHideInvokeBys.SwitchObjectGroup;

            if (mode.ButtonName == "地块划分")
            {
                data.VisibleObjects = new Dictionary<Guid, string>();

                var newDic = new Dictionary<Guid, string>();
                foreach (var item in ctl.Datas)
                {
                    if (item.ButtonName != "地块划分")
                    {
                        newDic = newDic.Concat(item.LayerInfo).ToDictionary(kv => kv.Key, kv => kv.Value);
                    }
                }

                data.HiddenObjects = newDic;
            }
            else
            {
                var newDic = new Dictionary<Guid, string>();
                foreach (var item in ctl.Datas)
                {
                    if (item.ButtonName == mode.ButtonName)
                    {
                        data.VisibleObjects = item.LayerInfo;
                    }
                    else
                    {
                        newDic = newDic.Concat(item.LayerInfo).ToDictionary(kv => kv.Key, kv => kv.Value);
                    }
                }

                data.HiddenObjects = newDic;
            }

            m_MessageAggregator.GetMessage<ObjectShowOrHideMessage>().Publish(data);
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