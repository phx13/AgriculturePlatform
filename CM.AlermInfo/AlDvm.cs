using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.AlermInfo
{
    [Serializable]
    public class AlDvm : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(AlermInfoField);
            return columns;
        }

        #region DepotNameField

        private DimensionColumnModel m_AlermInfoField;

        /// <summary>
        ///     告警信息
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "告警信息",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel AlermInfoField
        {
            get { return m_AlermInfoField; }
            set
            {
                m_AlermInfoField = value;
                RaisePropertyChanged(() => AlermInfoField);
            }
        }

        #endregion
    }
}