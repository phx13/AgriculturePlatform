using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.Track.TopList
{
    public partial class TopListView : ChartViewBase
    {
        #region Fields

        /// <summary>
        ///     是否已加载
        /// </summary>
        private bool m_IsLoaded;

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly TopListController m_Control;

        #endregion

        #region Constructor

        /// <summary>
        ///     构造
        /// </summary>
        public TopListView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            Loaded += CampViewControl_Loaded;
            m_Control = (TopListController) Controllers[0];
            m_Control.DatetableAction = Receive;
            DataContext = m_Control;
        }

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="table"></param>
        private void Receive(AdapterDataTable table)
        {
            if (table == null || table.Rows == null || table.Rows.Count == 0)
            {
                return;
            }
            try
            {
                m_Control.ModelList.Clear();
                foreach (var row in table.Rows)
                {
                    var count = row[m_Control.TopListDvm.CountField.AsName].ToString();
                    var name = row[m_Control.TopListDvm.NameField.AsName].ToString();

                    var model = new EnumColorModel();
                    model.EnumName = name;
                    model.EnumCount = int.Parse(count);
                    //model.EnumColor = GetBrushHelper.GetSolidColorBrush(m_Control.TopListDvm.LegendStyle, name);
                    var colorModel =
                        m_Control.TopListDvm.LegendStyle.SolutionColorList.FirstOrDefault(i => i.LegendValue == name);
                    model.EnumColor =
                        new SolidColorBrush((Color) ColorConverter.ConvertFromString(colorModel.ColorString));
                    m_Control.ModelList.Add(model);
                }
            }
            catch (Exception exception)
            {
            }
        }

        /// <summary>
        ///     加载窗体
        /// </summary>
        private void CampViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
                return;
            }
            OnDadChartLoaded();
            m_IsLoaded = true;

            m_Control.MyHeight = ActualHeight;
            m_Control.MyWidth = ActualWidth;
        }

        #endregion

        #region override

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

        public override void Dispose()
        {
        }

        #endregion
    }
}