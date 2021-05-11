using System.Collections.Generic;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.AnnularProgress.ShowText
{
    public partial class ShowTextView : ChartViewBase
    {
        private ShowTextControl m_Controller;

        public ShowTextView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            DataContext = Controllers[0];

            Loaded += (s, e) => { OnDadChartLoaded(); };
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