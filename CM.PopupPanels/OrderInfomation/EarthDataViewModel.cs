using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.PopupPanels.OrderInfomation
{
    /// <summary>
    ///     dvm
    /// </summary>
    [Serializable]
    public class EarthDataViewModel : ChartDataViewModel, IGIS3DDataViewModel
    {
        public string LayerGroupName { get; set; }

        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(PlantingField);
            columns.Add(PurposeField);
            columns.Add(AreaField);
            columns.Add(OwnerField);
            columns.Add(ProductNameField);
            columns.Add(CountField);
            columns.Add(UnitPriceField);
            columns.Add(TotalPriceField);
            columns.Add(SubsidyAmountField);
            columns.Add(PaidAmountField);
            columns.Add(PictureCatalog);
            columns.Add(LonField);
            columns.Add(LatField);
            return columns;
        }

        #region 数据设置

        #region 基本信息

        private DimensionColumnModel m_PlantingField;

        /// <summary>
        ///     种植户
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "种植户",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel PlantingField
        {
            get { return m_PlantingField; }
            set
            {
                m_PlantingField = value;
                RaisePropertyChanged(() => PlantingField);
            }
        }

        private DimensionColumnModel m_PurposeField;

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
        public DimensionColumnModel PurposeField
        {
            get { return m_PurposeField; }
            set
            {
                m_PurposeField = value;
                RaisePropertyChanged(() => PurposeField);
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

        private DimensionColumnModel m_OwnerField;

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
        public DimensionColumnModel OwnerField
        {
            get { return m_OwnerField; }
            set
            {
                m_OwnerField = value;
                RaisePropertyChanged(() => OwnerField);
            }
        }

        #endregion

        #region 订单信息

        private DimensionColumnModel m_ProductNameField;

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
        public DimensionColumnModel ProductNameField
        {
            get { return m_ProductNameField; }
            set
            {
                m_ProductNameField = value;
                RaisePropertyChanged(() => ProductNameField);
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

        private DimensionColumnModel m_UnitPriceField;

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
        public DimensionColumnModel UnitPriceField
        {
            get { return m_UnitPriceField; }
            set
            {
                m_UnitPriceField = value;
                RaisePropertyChanged(() => UnitPriceField);
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

        private DimensionColumnModel m_SubsidyAmountField;

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
        public DimensionColumnModel SubsidyAmountField
        {
            get { return m_SubsidyAmountField; }
            set
            {
                m_SubsidyAmountField = value;
                RaisePropertyChanged(() => SubsidyAmountField);
            }
        }

        private DimensionColumnModel m_PaidAmountField;

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
        public DimensionColumnModel PaidAmountField
        {
            get { return m_PaidAmountField; }
            set
            {
                m_PaidAmountField = value;
                RaisePropertyChanged(() => PaidAmountField);
            }
        }

        #endregion

        #region 其他字段

        private DimensionColumnModel m_PictureCatalog;

        /// <summary>
        ///     图片地址
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "图片地址",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel PictureCatalog
        {
            get { return m_PictureCatalog; }
            set
            {
                m_PictureCatalog = value;
                RaisePropertyChanged(() => PictureCatalog);
            }
        }

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

        #endregion

        #endregion

        #region 样式设置

        private string m_TextColor = "#00D9A3";

        /// <summary>
        ///     样式设置 - 基本文字 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = "基本样式",
            PropertyType = EditorType.Color)]
        public string TextColor
        {
            get { return m_TextColor; }
            set
            {
                m_TextColor = value;
                RaisePropertyChanged(() => TextColor);
            }
        }

        private double m_MaxHeight = 500000;

        /// <summary>
        ///     最大可见距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最大可见距离", Category = "样式设置", SubCategory = "基本样式")]
        public double MaxHeight
        {
            get { return m_MaxHeight; }
            set
            {
                m_MaxHeight = value;
                RaisePropertyChanged(() => MaxHeight);
            }
        }

        private float m_NarrowHeight = 1000;

        /// <summary>
        ///     开始缩小距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("开始缩小距离", Category = "样式设置", SubCategory = "基本样式")]
        public float NarrowHeight
        {
            get { return m_NarrowHeight; }
            set
            {
                m_NarrowHeight = value;
                RaisePropertyChanged(() => NarrowHeight);
            }
        }

        private float m_NarrowEndHeight = 50000;

        /// <summary>
        ///     缩小结束距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("缩小结束距离", Category = "样式设置", SubCategory = "基本样式")]
        public float NarrowEndHeight
        {
            get { return m_NarrowEndHeight; }
            set
            {
                m_NarrowEndHeight = value;
                RaisePropertyChanged(() => NarrowEndHeight);
            }
        }

        private string m_PicturePath = @".\CM\基地信息.png";

        /// <summary>
        ///     标牌目录
        /// </summary>
        [Synchronous]
        [PropertyDescription("背景图片目录", Category = "样式设置", SubCategory = "基本样式")]
        public string PicturePath
        {
            get { return m_PicturePath; }
            set
            {
                m_PicturePath = value;
                RaisePropertyChanged(() => PicturePath);
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

        private double m_MinScaling = 0.1;

        /// <summary>
        ///     最小比例
        /// </summary>
        [Synchronous]
        [PropertyDescription("最小比例", Category = "样式设置", SubCategory = "基本样式", MinValue = 0, MaxValue = 1.0,
            DefaultValue = 0.1)]
        public double MinScaling
        {
            get { return m_MinScaling; }
            set
            {
                m_MinScaling = value;
                RaisePropertyChanged(() => MinScaling);
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

        #endregion
    }
}