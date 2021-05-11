using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.PopupPanels.RealtimeAlerm
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
            columns.Add(NameField);
            columns.Add(LonField);
            columns.Add(LatField);
            return columns;
        }

        #region 数据设置

        private DimensionColumnModel m_NameField;

        /// <summary>
        ///     显示名称
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "名称字段",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel NameField
        {
            get { return m_NameField; }
            set
            {
                m_NameField = value;
                RaisePropertyChanged(() => NameField);
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

        #region 样式设置

        private string m_TextColor = "#FF7F00";

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

        private string m_AlarmType = "生长周期：";

        /// <summary>
        ///     报警类型
        /// </summary>
        [Synchronous]
        [PropertyDescription("报警类型", Category = "样式设置", SubCategory = "基本样式")]
        public string AlarmType
        {
            get { return m_AlarmType; }
            set
            {
                m_AlarmType = value;
                RaisePropertyChanged(() => AlarmType);
            }
        }

        private double m_HeightValue = 300.0;

        /// <summary>
        ///     标牌高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("标牌高度", Category = "样式设置", SubCategory = "基本样式")]
        public double HeightValue
        {
            get { return m_HeightValue; }
            set
            {
                m_HeightValue = value;
                RaisePropertyChanged(() => HeightValue);
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

        private string m_PicturePath = @".\CM\实时报警.fw.png";

        /// <summary>
        ///     标牌目录
        /// </summary>
        [Synchronous]
        [PropertyDescription("图片目录", Category = "样式设置", SubCategory = "基本样式")]
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
        [PropertyDescription("持续时间", Category = "样式设置", SubCategory = "基本样式")]
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