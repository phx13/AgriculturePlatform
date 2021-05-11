using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CM.AnnularProgress.Pie
{
    /// <summary>
    ///     饼图DVM
    /// </summary>
    [Serializable]
    public class PieDataViewModel : ChartDataViewModel, ILegendColor
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public PieDataViewModel()
        {
            m_LegendColors = new List<string>
            {
                "#FFFFFF00",
                "#FFBDFD00",
                "#FF72D803",
                "#FF00A880",
                "#FF009ACF",
                "#FF9673FF",
                "#FFDC73FF",
                "#FFFF7A4D",
                "#FFFF9326",
                "#FFFFC926"
            };
        }

        #region Override

        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(LegendField);
            columns.Add(MeasureField);
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #endregion

        #region 数据设置 - 数据设置

        private MeasureColumnModel m_MeasureField = new MeasureColumnModel();

        /// <summary>
        ///     数量字段（指标）
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public MeasureColumnModel MeasureField
        {
            get { return m_MeasureField; }
            set
            {
                m_MeasureField = value;
                RaisePropertyChanged(() => MeasureField);
            }
        }

        private LegendColumnModel m_LegendField;

        /// <summary>
        ///     图例字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("图例字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public LegendColumnModel LegendField
        {
            get { return m_LegendField; }
            set
            {
                m_LegendField = value;
                if (value != null)
                {
                    m_LegendHeadline = value.ColumnName;
                }
                RaisePropertyChanged(() => LegendField);
            }
        }

        #endregion

        #region 样式设置 - 颜色样式

        private ChartStyleModel m_LegendStyle = new ChartStyleModel();

        /// <summary>
        ///     样式设置 - 颜色设置 - 枚举颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("枚举颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式,
            PropertyType = EditorType.ColorCollection)]
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

        #region 样式设置 - 图例

        private bool m_ShowLegendControl = true;

        /// <summary>
        ///     是否显示图例
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示图例控件", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例)]
        public bool ShowLegendControl
        {
            get { return m_ShowLegendControl; }
            set
            {
                m_ShowLegendControl = value;
                RaisePropertyChanged(() => ShowLegendControl);
            }
        }

        private bool m_ShowLegendTitle;

        /// <summary>
        ///     样式设置 - 图例 - 是否显示图例标题
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示图例标题", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例)]
        public bool ShowLegendTitle
        {
            get { return m_ShowLegendTitle; }
            set
            {
                m_ShowLegendTitle = value;
                RaisePropertyChanged(() => ShowLegendTitle);
            }
        }

        private Dock m_LegendPosition = Dock.Right;

        /// <summary>
        ///     样式设置 - 图例 - 图例位置
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("图例位置", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例)]
        public Dock LegendPosition
        {
            get { return m_LegendPosition; }
            set
            {
                m_LegendPosition = value;
                RaisePropertyChanged(() => LegendPosition);
            }
        }

        private string m_LegendHeadline = "图例标题";

        /// <summary>
        ///     样式设置 - 图例 - 标签文字
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签文字", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例)]
        public string LegendHeadline
        {
            get { return m_LegendHeadline; }
            set
            {
                m_LegendHeadline = value;
                RaisePropertyChanged(() => LegendHeadline);
            }
        }

        private int m_LegendTextSize = 9;

        /// <summary>
        ///     样式设置 - 图例 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例, MinValue = 2,
            MaxValue = 50, DefaultValue = 12)]
        public int LegendTextSize
        {
            get { return m_LegendTextSize; }
            set
            {
                m_LegendTextSize = value;
                RaisePropertyChanged(() => LegendTextSize);
            }
        }

        private string m_LegendTextColor = "#FFBFBFBF";

        /// <summary>
        ///     样式设置 - 图例 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例,
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

        private List<string> m_LegendColors;

        /// <summary>
        ///     图例颜色集合
        /// </summary>
        public List<string> LegendColors
        {
            get { return m_LegendColors; }
            set
            {
                m_LegendColors = value;
                RaisePropertyChanged(() => LegendColors);
            }
        }

        #endregion

        #region 样式设置 - 标签文字

        private bool m_ShowLabel;

        /// <summary>
        ///     样式设置 - 标签文字 - 显示标签
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示标签", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool ShowLabel
        {
            get { return m_ShowLabel; }
            set
            {
                m_ShowLabel = value;
                RaisePropertyChanged(() => ShowLabel);
            }
        }

        private string m_LabelTextColor = "#FFA5A5A5";

        /// <summary>
        ///     样式设置 - 标签文字 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字,
            PropertyType = EditorType.Color)]
        public string LabelTextColor
        {
            get { return m_LabelTextColor; }
            set
            {
                m_LabelTextColor = value;
                RaisePropertyChanged(() => LabelTextColor);
            }
        }

        private int m_LabelTextSize = 9;

        /// <summary>
        ///     样式设置 - 标签文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, MinValue = 2,
            MaxValue = 50, DefaultValue = 12)]
        public int LabelTextSize
        {
            get { return m_LabelTextSize; }
            set
            {
                m_LabelTextSize = value;
                RaisePropertyChanged(() => LabelTextSize);
            }
        }

        private int m_LabelDecimalDigits;

        /// <summary>
        ///     样式设置 - 标签文字 - 小数位数
        /// </summary>
        [Synchronous]
        [PropertyDescription("小数位数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, MinValue = 0,
            MaxValue = 10, DefaultValue = 0)]
        public virtual int LabelDecimalDigits
        {
            get { return m_LabelDecimalDigits; }
            set
            {
                m_LabelDecimalDigits = value;
                RaisePropertyChanged(() => LabelDecimalDigits);
            }
        }

        private LabelContentTypeEnum m_LabelContentTypeEnum = LabelContentTypeEnum.Value;

        /// <summary>
        ///     样式设置 - 标签文字 - 标签内容类型
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("标签内容类型", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public LabelContentTypeEnum LabelContentTypeEnum
        {
            get { return m_LabelContentTypeEnum; }
            set
            {
                m_LabelContentTypeEnum = value;
                RaisePropertyChanged(() => LabelContentTypeEnum);
            }
        }

        private bool m_ShowLabelBorder = true;

        /// <summary>
        ///     样式设置 - 标签文字 - 显示标签外边框
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示标签外边框", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool ShowLabelBorder
        {
            get { return m_ShowLabelBorder; }
            set
            {
                m_ShowLabelBorder = value;
                RaisePropertyChanged(() => ShowLabelBorder);
            }
        }

        private bool m_LabelOverlap;

        /// <summary>
        ///     样式设置 - 标签文字 - 处理标签重叠
        /// </summary>
        [Synchronous]
        [PropertyDescription("处理标签重叠", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool LabelOverlap
        {
            get { return m_LabelOverlap; }
            set
            {
                m_LabelOverlap = value;
                RaisePropertyChanged(() => LabelOverlap);
            }
        }

        private LabelContentTypeEnum m_LabelTypeEnum;

        /// <summary>
        ///     样式设置 - 标签文字 - 提示标签内容类型
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("提示标签内容类型", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public LabelContentTypeEnum LabelTypeEnum
        {
            get { return m_LabelTypeEnum; }
            set
            {
                m_LabelTypeEnum = value;
                RaisePropertyChanged(() => LabelTypeEnum);
            }
        }

        #endregion

        #region 样式设置 - 其他

        private bool m_IsShowRealValue = true;

        /// <summary>
        ///     是否显示真实值
        ///     true:显示真实值（默认）
        ///     false:显示UI上进行过限制的值
        /// </summary>
        [Synchronous]
        //[PropertyDescription("是否显示真实值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他)]
        public bool IsShowRealValue
        {
            get { return m_IsShowRealValue; }
            set
            {
                m_IsShowRealValue = value;
                RaisePropertyChanged(() => IsShowRealValue);
            }
        }

        private double m_PieMinLimitAngle = 1;

        /// <summary>
        ///     饼图最小限制值，默认值为1
        /// </summary>
        [Synchronous]
        //[PropertyDescription("饼图最小限制值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0, MaxValue = double.MaxValue, DefaultValue = 1)]
        public double PieMinLimitAngle
        {
            get
            {
                //if (m_PieMinLimitAngle > 20 || m_PieMinLimitAngle <= 0)
                //{
                //    return 1;
                //}
                return m_PieMinLimitAngle;
            }
            set
            {
                m_PieMinLimitAngle = value;
                RaisePropertyChanged(() => PieMinLimitAngle);
            }
        }

        private double m_PieMinDisplayAngle = 5;

        /// <summary>
        ///     饼图最小值对应的用于显示的角度，默认值为5
        ///     数据在 0~20 之间
        /// </summary>
        [Synchronous]
        //[PropertyDescription("最小值角度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0, MaxValue = double.MaxValue, DefaultValue = 5)]
        public double PieMinDisplayAngle
        {
            get
            {
                //if (this.IsShowRealValue)
                //{
                //    return 5;
                //}
                //if (m_PieMinDisplayAngle <= 0)
                //{
                //    return 5; // 如果是显示假值,PieMinDisplayAngle不能为0，默认设置为5
                //}
                //if (m_PieMinDisplayAngle > 20)
                //{
                //    return 5;
                //}
                //if (this.PieMinLimitAngle > m_PieMinDisplayAngle)
                //{
                //    return this.PieMinLimitAngle; // 需要保证设置的用于显示的值大于界限值
                //}
                return m_PieMinDisplayAngle;
            }
            set
            {
                m_PieMinDisplayAngle = value;
                RaisePropertyChanged(() => PieMinDisplayAngle);
            }
        }

        private double m_PieHoleSize;

        /// <summary>
        ///     饼图洞的大小(取值范围0~0.99之间)，默认为0
        ///     0：表示饼图
        /// </summary>
        [Synchronous]
        [PropertyDescription("洞大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0,
            MaxValue = 0.99, DefaultValue = 0)]
        public double PieHoleSize
        {
            get
            {
                if (m_PieHoleSize > 0.99)
                {
                    return 0.99;
                }
                return m_PieHoleSize;
            }
            set
            {
                m_PieHoleSize = value;
                RaisePropertyChanged(() => PieHoleSize);
            }
        }

        private double m_PieStrokeThickness;

        /// <summary>
        ///     饼块儿外边框宽度
        ///     默认为0，范围0~10
        /// </summary>
        [Synchronous]
        [PropertyDescription("外边框宽度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0,
            MaxValue = 10, DefaultValue = 0)]
        public virtual double PieStrokeThickness
        {
            get { return m_PieStrokeThickness; }
            set
            {
                m_PieStrokeThickness = value;
                RaisePropertyChanged(() => PieStrokeThickness);
            }
        }

        private string m_PieStroke = "#FFFFFFFF";

        /// <summary>
        ///     饼块儿外边框颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("外边框颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他,
            PropertyType = EditorType.Color)]
        public virtual string PieStroke
        {
            get { return m_PieStroke; }
            set
            {
                m_PieStroke = value;
                RaisePropertyChanged(() => PieStroke);
            }
        }

        private double m_SubOpacity = 0.5;

        /// <summary>
        ///     子图透明度
        /// </summary>
        [Synchronous]
        [PropertyDescription("子图透明度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0.01,
            MaxValue = 0.99, DefaultValue = 0.5)]
        public virtual double SubOpacity
        {
            get { return m_SubOpacity; }
            set
            {
                m_SubOpacity = value;
                RaisePropertyChanged(() => SubOpacity);
            }
        }

        private bool m_ShowTooltip;

        /// <summary>
        ///     显示提示信息
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示提示信息", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他)]
        public bool ShowTooltip
        {
            get { return m_ShowTooltip; }
            set
            {
                m_ShowTooltip = value;
                RaisePropertyChanged(() => ShowTooltip);
            }
        }

        #endregion

        #region ILegendColor

        /// <summary>
        ///     获取枚举颜色相关内容
        /// </summary>
        /// <returns></returns>
        public ChartLegendHelperModel GetLegendColumns(string propertyName)
        {
            var model = new ChartLegendHelperModel();

            model.UseLegend = true;
            model.LegendColumns.Add(LegendField);

            // 去掉空列
            model.LegendColumns.RemoveAll(item => item == null);

            return model;
        }

        /// <summary>
        ///     根据属性字段名称 匹配对应的LegendStyle
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ChartStyleModel GetLegendStyleProperties(string propertyName)
        {
            return LegendStyle;
        }

        #endregion

        #region 圆环中间字体样式

        private double m_BasicHorizontalMargin = 10.0;

        /// <summary>
        ///     基本样式-左右偏移
        /// </summary>
        [Synchronous]
        [PropertyDescription("左右偏移", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double BasicHorizontalMargin
        {
            get { return m_BasicHorizontalMargin; }
            set
            {
                m_BasicHorizontalMargin = value;
                RaisePropertyChanged(() => BasicHorizontalMargin);
            }
        }

        private double m_BasicVerticalMargin = 10.0;

        /// <summary>
        ///     上下偏移
        /// </summary>
        [Synchronous]
        [PropertyDescription("上下偏移", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double BasicVerticalMargin
        {
            get { return m_BasicVerticalMargin; }
            set
            {
                m_BasicVerticalMargin = value;
                RaisePropertyChanged(() => BasicVerticalMargin);
            }
        }

        private VerticalAlignmentEnum m_BasicVerticalAlignment = VerticalAlignmentEnum.Top;

        /// <summary>
        ///     上下对齐
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("上下对齐", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual VerticalAlignmentEnum BasicVerticalAlignment
        {
            get { return m_BasicVerticalAlignment; }
            set
            {
                m_BasicVerticalAlignment = value;
                RaisePropertyChanged(() => BasicVerticalAlignment);
            }
        }

        private HorizontalAlignmentEnum m_BasicHorizontalAlignment = HorizontalAlignmentEnum.Left;

        /// <summary>
        ///     左右对齐
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("左右对齐", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual HorizontalAlignmentEnum BasicHorizontalAlignment
        {
            get { return m_BasicHorizontalAlignment; }
            set
            {
                m_BasicHorizontalAlignment = value;
                RaisePropertyChanged(() => BasicHorizontalAlignment);
            }
        }

        private int m_AxisLabelFontSize = 12;

        /// <summary>
        ///     样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("中间文字大小", Category = DescriptionEnum.样式设置, SubCategory = "基本样式", MinValue = 2,
            MaxValue = 50, DefaultValue = 12)]
        public virtual int AxisLabelFontSize
        {
            get { return m_AxisLabelFontSize; }
            set
            {
                m_AxisLabelFontSize = value;
                RaisePropertyChanged(() => AxisLabelFontSize);
            }
        }

        private string m_AxisLabelFontFamily = "微软雅黑";

        /// <summary>
        ///     样式设置 - 基本文字 - 文字字体
        /// </summary>
        [Synchronous]
        [PropertyDescription("中间文字字体", Category = DescriptionEnum.样式设置, SubCategory = "基本样式",
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

        #endregion
    }
}

#region 未实现的属性

//private string m_ColorStyle;
///// <summary>
///// 样式设置 - 颜色样式 - 样式
///// </summary>
//[Synchronous]
//[PropertyDescription("样式", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式)]
//public string ColorStyle
//{
//    get { return m_ColorStyle; }
//    set
//    {
//        m_ColorStyle = value;
//        RaisePropertyChanged(() => ColorStyle);
//    }
//}

#endregion