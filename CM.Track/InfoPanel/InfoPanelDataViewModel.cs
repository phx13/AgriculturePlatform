using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.Track.InfoPanel
{
    /// <summary>
    ///     测试DVM
    /// </summary>
    [Serializable]
    public class InfoPanelDataViewModel : ChartDataViewModel
    {
        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(IdField);
            columns.Add(FarmerField);
            columns.Add(UseTypeField);
            columns.Add(AreaField);
            columns.Add(TownField);
            columns.Add(ProductField);
            columns.Add(CountField);
            columns.Add(AmountField);
            columns.Add(TotalPriceField);
            columns.Add(BtPriceField);
            columns.Add(ScPriceField);
            columns.Add(BtTotalField);
            columns.Add(BtUsedField);
            columns.Add(BtSurplusField);
            columns.Add(IndentTotalField);
            columns.Add(RecycleTotalField);
            columns.Add(LonField);
            columns.Add(LatField);
            return columns;
        }

        #region 数据设置

        #region 基本信息

        private DimensionColumnModel m_IdField;

        /// <summary>
        ///     路径
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "地块编号",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel IdField
        {
            get { return m_IdField; }
            set
            {
                m_IdField = value;
                RaisePropertyChanged(() => IdField);
            }
        }

        private DimensionColumnModel m_FarmerField;

        /// <summary>
        ///     种养户
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "种养户",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel FarmerField
        {
            get { return m_FarmerField; }
            set
            {
                m_FarmerField = value;
                RaisePropertyChanged(() => FarmerField);
            }
        }

        private DimensionColumnModel m_UseTypeField;

        /// <summary>
        ///     用途
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "用途",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel UseTypeField
        {
            get { return m_UseTypeField; }
            set
            {
                m_UseTypeField = value;
                RaisePropertyChanged(() => UseTypeField);
            }
        }

        private DimensionColumnModel m_AreaField;

        /// <summary>
        ///     面积
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "面积",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel AreaField
        {
            get { return m_AreaField; }
            set
            {
                m_AreaField = value;
                RaisePropertyChanged(() => AreaField);
            }
        }

        private DimensionColumnModel m_TownField;

        /// <summary>
        ///     所属
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "所属",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel TownField
        {
            get { return m_TownField; }
            set
            {
                m_TownField = value;
                RaisePropertyChanged(() => TownField);
            }
        }

        #endregion

        #region 订单信息

        private DimensionColumnModel m_ProductField;

        /// <summary>
        ///     产品名称
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "产品名称",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel ProductField
        {
            get { return m_ProductField; }
            set
            {
                m_ProductField = value;
                RaisePropertyChanged(() => ProductField);
            }
        }

        private DimensionColumnModel m_CountField;

        /// <summary>
        ///     数量字段
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "数量",
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

        private DimensionColumnModel m_AmountField;

        /// <summary>
        ///     单价
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "单价",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel AmountField
        {
            get { return m_AmountField; }
            set
            {
                m_AmountField = value;
                RaisePropertyChanged(() => AmountField);
            }
        }

        private DimensionColumnModel m_TotalPriceField;

        /// <summary>
        ///     总价
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "总价",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel TotalPriceField
        {
            get { return m_TotalPriceField; }
            set
            {
                m_TotalPriceField = value;
                RaisePropertyChanged(() => TotalPriceField);
            }
        }

        private DimensionColumnModel m_BtPriceField;

        /// <summary>
        ///     补贴金额
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "补贴金额",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel BtPriceField
        {
            get { return m_BtPriceField; }
            set
            {
                m_BtPriceField = value;
                RaisePropertyChanged(() => BtPriceField);
            }
        }

        private DimensionColumnModel m_ScPriceField;

        /// <summary>
        ///     自付金额
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "自付金额",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel ScPriceField
        {
            get { return m_ScPriceField; }
            set
            {
                m_ScPriceField = value;
                RaisePropertyChanged(() => ScPriceField);
            }
        }

        #endregion

        #region 统计信息

        private DimensionColumnModel m_BtTotalField;

        /// <summary>
        ///     年度补贴
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "年度补贴",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel BtTotalField
        {
            get { return m_BtTotalField; }
            set
            {
                m_BtTotalField = value;
                RaisePropertyChanged(() => BtTotalField);
            }
        }

        private DimensionColumnModel m_BtUsedField;

        /// <summary>
        ///     已用补贴
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "已用补贴",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel BtUsedField
        {
            get { return m_BtUsedField; }
            set
            {
                m_BtUsedField = value;
                RaisePropertyChanged(() => BtUsedField);
            }
        }

        private DimensionColumnModel m_BtSurplusField;

        /// <summary>
        ///     剩余补贴
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "剩余补贴",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel BtSurplusField
        {
            get { return m_BtSurplusField; }
            set
            {
                m_BtSurplusField = value;
                RaisePropertyChanged(() => BtSurplusField);
            }
        }

        private DimensionColumnModel m_IndentTotalField;

        /// <summary>
        ///     农药总采购量
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "农药总采购量",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel IndentTotalField
        {
            get { return m_IndentTotalField; }
            set
            {
                m_IndentTotalField = value;
                RaisePropertyChanged(() => IndentTotalField);
            }
        }

        private DimensionColumnModel m_RecycleTotalField;

        /// <summary>
        ///     已回收废弃物
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "已回收废弃物",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel RecycleTotalField
        {
            get { return m_RecycleTotalField; }
            set
            {
                m_RecycleTotalField = value;
                RaisePropertyChanged(() => RecycleTotalField);
            }
        }

        #endregion

        #region 其他字段

        private DimensionColumnModel m_LonField;

        /// <summary>
        ///     经度字段
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "经度字段",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel LonField
        {
            get { return m_LonField; }
            set
            {
                m_LonField = value;
                RaisePropertyChanged(() => LonField);
            }
        }

        private DimensionColumnModel m_LatField;

        /// <summary>
        ///     纬度字段
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "纬度字段",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel LatField
        {
            get { return m_LatField; }
            set
            {
                m_LatField = value;
                RaisePropertyChanged(() => LatField);
            }
        }

        private double m_DurationData = 30;

        /// <summary>
        ///     持续时间
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示持续时间", Category = "样式设置", SubCategory = "基本样式")]
        public double DurationData
        {
            get { return m_DurationData; }
            set
            {
                m_DurationData = value;
                RaisePropertyChanged(() => DurationData);
            }
        }

        #endregion

        #endregion
    }
}