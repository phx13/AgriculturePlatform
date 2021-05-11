using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Digihail.CCP4.Utilities;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.GOE.Viewer.Library;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;

namespace CM.MoveMap
{
    /// <summary>
    ///     MapMove.xaml 的交互逻辑
    /// </summary>
    public partial class MapMove : ChartViewBase
    {
        #region Fields

        private Map m_arcMap;
        private MapPoint m_location;

        /// <summary>
        ///     是否加载过图表
        /// </summary>
        private bool m_IsLoaded;

        private MapMoveController m_Controller;
        private readonly MapMoveDataViewModel m_DVM;

        #endregion

        #region Init

        public MapMove(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = Controllers[0] as MapMoveController;
            m_DVM = (MapMoveDataViewModel) model.DataViewModels[0];
            Loaded += MapMove_Loaded;
        }

        private void MapMove_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
                return;
            }
            OnDadChartLoaded();
            m_IsLoaded = true;
            Init();
        }

        /// <summary>
        ///     初始化二维地图
        /// </summary>
        private void Init()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    var grid = this.GetParentByName<Grid>("gridRoot");

                    if (grid != null)
                    {
                        var mapControl = grid.GetChildByName<MapControlCore>("mapControl");
                        var map = grid.GetChildByName<Grid>("grdRoot");
                        var arcMap = grid.GetChildByName<Map>("arcMap");

                        arcMap.SizeChanged += arcMap_SizeChanged;
                        mapControl.Height = m_DVM.ControlHeight;
                        mapControl.Width = m_DVM.ControlWidth;
                        mapControl.HorizontalAlignment = ConverterHorizontalAlignment(m_DVM.BasicHorizontalAlignment);
                        mapControl.VerticalAlignment = ConverterHorizontalAlignment(m_DVM.BasicVerticalAlignment);
                        map.Margin = new Thickness(m_DVM.MarginLeft, m_DVM.MarginUp, m_DVM.MarginRight, m_DVM.MarginDown);
                    }
                }
                catch (Exception ex)
                {
                }
            }));
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="enumString"></param>
        /// <returns></returns>
        private HorizontalAlignment ConverterHorizontalAlignment(HorizontalAlignmentEnum enumString)
        {
            switch (enumString)
            {
                case HorizontalAlignmentEnum.Center:
                    return HorizontalAlignment.Center;
                case HorizontalAlignmentEnum.Left:
                    return HorizontalAlignment.Left;
                case HorizontalAlignmentEnum.Right:
                    return HorizontalAlignment.Right;
            }
            return HorizontalAlignment.Stretch;
        }

        /// <summary>
        /// </summary>
        /// <param name="enumString"></param>
        /// <returns></returns>
        private VerticalAlignment ConverterHorizontalAlignment(VerticalAlignmentEnum enumString)
        {
            switch (enumString)
            {
                case VerticalAlignmentEnum.Center:
                    return VerticalAlignment.Center;
                case VerticalAlignmentEnum.Top:
                    return VerticalAlignment.Top;
                case VerticalAlignmentEnum.Bottom:
                    return VerticalAlignment.Bottom;
            }
            return VerticalAlignment.Stretch;
        }

        private void arcMap_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var map = sender as Map;
            var engine = map.GetParentByType<MapControlCore>().EngineContainer;
            engine.ZoomToLevel(m_DVM.CenterLevel);
            var location = engine.Convert4326toMap(new MapPoint(122.254779, 31.65055));
            if (map != null) map.PanTo(location);
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

        #endregion
    }
}