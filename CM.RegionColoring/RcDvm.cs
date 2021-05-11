using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.RegionColoring
{
    [Serializable]
    public class RcDvm : ChartDataViewModel, ILegendColor, IGIS3DDataViewModel
    {
        /// <summary>
        ///     返回数据列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(RegionName);
            columns.Add(PolygonField);
            columns.Add(LegendField);
            return columns;
        }

        #region 数据设置

        private DimensionColumnModel m_RegionName;

        /// <summary>
        ///     区域名称
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "区域名称",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel RegionName
        {
            get { return m_RegionName; }
            set
            {
                m_RegionName = value;
                RaisePropertyChanged(() => RegionName);
            }
        }

        private DimensionColumnModel m_PolygonField;

        /// <summary>
        ///     点集字段
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "点集字段",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel PolygonField
        {
            get { return m_PolygonField; }
            set
            {
                m_PolygonField = value;
                RaisePropertyChanged(() => PolygonField);
            }
        }

        private LegendColumnModel m_LegendField;

        /// <summary>
        ///     图例字段
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "图例字段",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public LegendColumnModel LegendField
        {
            get { return m_LegendField; }
            set
            {
                m_LegendField = value;
                RaisePropertyChanged(() => LegendField);
            }
        }

        #endregion

        #region 样式设置

        private ChartStyleModel m_LegendStyle = new ChartStyleModel();

        /// <summary>
        ///     枚举颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("枚举颜色", Category = "样式设置", SubCategory = "样式设置", PropertyType = EditorType.ColorCollection)
        ]
        public ChartStyleModel LegendStyle
        {
            get { return m_LegendStyle; }
            set
            {
                m_LegendStyle = value;
                RaisePropertyChanged(() => LegendStyle);
            }
        }

        #endregion

        #region ShowLayer

        private bool m_ShowLayer = true;

        public bool ShowLayer
        {
            get { return m_ShowLayer; }
            set
            {
                m_ShowLayer = value;
                RaisePropertyChanged(() => ShowLayer);
            }
        }

        public string LayerGroupName { get; set; }

        #endregion

        #region 图例字段

        public ChartLegendHelperModel GetLegendColumns(string propertyName)
        {
            var model = new ChartLegendHelperModel();
            model.UseLegend = true;
            model.LegendColumns.Add(LegendField);

            return model;
        }

        public ChartStyleModel GetLegendStyleProperties(string propertyName)
        {
            return LegendStyle;
        }

        #endregion
    }
}