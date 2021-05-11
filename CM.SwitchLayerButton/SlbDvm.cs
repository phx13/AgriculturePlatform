using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.SwitchLayerButton
{
    [Serializable]
    public class SlbDvm : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(ButtonName);
            columns.Add(LayerName);
            columns.Add(LayerGuid);
            return columns;
        }

        #region ButtonName

        private DimensionColumnModel m_ButtonName;

        /// <summary>
        ///     按钮名称
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "按钮名称",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel ButtonName
        {
            get { return m_ButtonName; }
            set
            {
                m_ButtonName = value;
                RaisePropertyChanged(() => ButtonName);
            }
        }

        #endregion

        #region LayerName

        private DimensionColumnModel m_LayerName;

        /// <summary>
        ///     图层名称
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "图层名称",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel LayerName
        {
            get { return m_LayerName; }
            set
            {
                m_LayerName = value;
                RaisePropertyChanged(() => LayerName);
            }
        }

        #endregion

        #region LayerGuid

        private DimensionColumnModel m_LayerGuid;

        /// <summary>
        ///     图层Guid
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "图层Guid",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel LayerGuid
        {
            get { return m_LayerGuid; }
            set
            {
                m_LayerGuid = value;
                RaisePropertyChanged(() => LayerGuid);
            }
        }

        #endregion
    }
}