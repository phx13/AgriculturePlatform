using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.AnnularProgress.GrowActivity
{
    [Serializable]
    public class GrowDvm : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(TypeField);
            columns.Add(JiDiField);
            columns.Add(TimeField);
            columns.Add(StateField);
            return columns;
        }

        #region DepotNameField

        private DimensionColumnModel m_TypeField;

        /// <summary>
        ///     行为
        /// </summary>
        [Synchronous]
        [PropertyDescription("行为",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel TypeField
        {
            get { return m_TypeField; }
            set
            {
                m_TypeField = value;
                RaisePropertyChanged(() => TypeField);
            }
        }

        private DimensionColumnModel m_JiDiField;

        /// <summary>
        ///     基地
        /// </summary>
        [Synchronous]
        [PropertyDescription("基地",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel JiDiField
        {
            get { return m_JiDiField; }
            set
            {
                m_JiDiField = value;
                RaisePropertyChanged(() => JiDiField);
            }
        }

        private DimensionColumnModel m_TimeField;

        /// <summary>
        ///     时间
        /// </summary>
        [Synchronous]
        [PropertyDescription("时间",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel TimeField
        {
            get { return m_TimeField; }
            set
            {
                m_TimeField = value;
                RaisePropertyChanged(() => TimeField);
            }
        }

        private DimensionColumnModel m_StateField;

        /// <summary>
        ///     状态
        /// </summary>
        [Synchronous]
        [PropertyDescription("状态",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel StateField
        {
            get { return m_StateField; }
            set
            {
                m_StateField = value;
                RaisePropertyChanged(() => StateField);
            }
        }

        #endregion
    }
}