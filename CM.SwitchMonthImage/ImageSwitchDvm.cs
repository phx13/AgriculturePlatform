using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.SwitchMonthImage
{
    [Serializable]
    public class ImageSwitchDvm : ChartDataViewModel
    {
        private string m_AxisLabelFontFamily = "微软雅黑";


        private int m_AxisLabelFontSize = 12;
        private string m_LegendTextColor = "#00FFFF";

        /// <summary>
        ///     样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = "基本样式", MinValue = 2, MaxValue = 50,
            DefaultValue = 12)]
        public virtual int AxisLabelFontSize
        {
            get { return m_AxisLabelFontSize; }
            set
            {
                m_AxisLabelFontSize = value;
                RaisePropertyChanged(() => AxisLabelFontSize);
            }
        }

        /// <summary>
        ///     样式设置 - 基本文字 - 文字字体
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字字体", Category = DescriptionEnum.样式设置, SubCategory = "基本样式",
            PropertyType = EditorType.FontFamily)]
        public virtual string AxisLabelFontFamily
        {
            get { return m_AxisLabelFontFamily; }
            set
            {
                m_AxisLabelFontFamily = value;
                RaisePropertyChanged(() => AxisLabelFontFamily);
            }
        }

        /// <summary>
        ///     样式设置 - 基本文字 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = "基本样式",
            PropertyType = EditorType.Color)]
        public string LegendTextColor
        {
            get { return m_LegendTextColor; }
            set
            {
                m_LegendTextColor = value;
                RaisePropertyChanged(() => LegendTextColor);
            }
        }

        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(MonthField);
            return columns;
        }

        #region DepotNameField

        private MeasureColumnModel m_MonthField;

        /// <summary>
        ///     行为
        /// </summary>
        [Synchronous]
        [PropertyDescription("月份",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public MeasureColumnModel MonthField
        {
            get { return m_MonthField; }
            set
            {
                m_MonthField = value;
                RaisePropertyChanged(() => MonthField);
            }
        }

        #endregion
    }
}