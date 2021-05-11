using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.DeliverData
{
    [Serializable]
    public class DdDvm : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(DepotNameField);
            columns.Add(CountField);
            return columns;
        }

        #region DepotNameField

        private DimensionColumnModel m_DepotNameField;

        /// <summary>
        ///     仓库名称
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "仓库名称",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel DepotNameField
        {
            get { return m_DepotNameField; }
            set
            {
                m_DepotNameField = value;
                RaisePropertyChanged(() => DepotNameField);
            }
        }

        #endregion

        #region TypeField

        private DimensionColumnModel m_CountField;

        /// <summary>
        ///     总量
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "总量",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel CountField
        {
            get { return m_CountField; }
            set
            {
                m_CountField = value;
                RaisePropertyChanged(() => CountField);
            }
        }

        #endregion
    }
}