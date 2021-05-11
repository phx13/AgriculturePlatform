using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CM_NewBar
{
    /// <summary>
    /// 柱图DVM
    /// </summary>
    [Serializable]
    public class BarDataViewModel : ChartDataViewModel, ILegendColor
    {
        #region 数据设置 - 数据设置

        /// <summary>
        /// 
        /// </summary>
        protected MeasureFieldCollection m_MeasureFields = new MeasureFieldCollection();

        /// <summary>
        /// 数量字段（指标，多选）
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.MeasureCollection, IsNecessary = true, RefreshChartData = true)]
        public virtual MeasureFieldCollection MeasureFields
        {
            get { return m_MeasureFields; }
            set
            {
                m_MeasureFields = value;
                if (value != null && value.Count >= 1)
                {
                    m_MeasureHeadline = value[0].ColumnName;
                }
                RaisePropertyChanged(() => MeasureFields);
            }
        }

        private DimensionColumnModel m_DimensionField = null;
        /// <summary>
        /// 数据设置 - 数据设置 - 类别字段（维度）
        /// </summary>
        [Synchronous]
        [PropertyDescription("类别字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public virtual DimensionColumnModel DimensionField
        {
            get { return m_DimensionField; }
            set
            {
                m_DimensionField = value;
                if (value != null)
                {
                    m_DimensionHeadline = value.ColumnName;
                }
                RaisePropertyChanged(() => DimensionField);
            }
        }

        private LegendColumnModel m_LegendField = null;
        /// <summary>
        /// 图例字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("图例字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, RefreshChartData = true)]
        public virtual LegendColumnModel LegendField
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

        #region 样式设置 - 基本文字

        private string m_AxisLabelForeground = "#FF7F7F7E";
        /// <summary>
        /// 样式设置 - 基本文字 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.基本文字, PropertyType = EditorType.Color)]
        public virtual string AxisLabelForeground
        {
            get { return m_AxisLabelForeground; }
            set
            {
                m_AxisLabelForeground = value;
                RaisePropertyChanged(() => AxisLabelForeground);
            }
        }

        private int m_AxisLabelFontSize = 12;
        /// <summary>
        /// 样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.基本文字, MinValue = 2, MaxValue = 50, DefaultValue = 12)]
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
        /// 样式设置 - 基本文字 - 文字字体
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字字体", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.基本文字, PropertyType = EditorType.FontFamily)]
        public virtual string AxisLabelFontFamily
        {
            get { return m_AxisLabelFontFamily; }
            set
            {
                m_AxisLabelFontFamily = value;
                RaisePropertyChanged(() => AxisLabelFontFamily);
            }
        }

        private int m_DecimalDigits = 0;
        /// <summary>
        /// 样式设置 - 基本文字 - 小数位数
        /// </summary>
        [Synchronous]
        [PropertyDescription("小数位数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.基本文字, MinValue = 0, MaxValue = 10, DefaultValue = 0)]
        public virtual int DecimalDigits
        {
            get { return m_DecimalDigits; }
            set
            {
                m_DecimalDigits = value;
                RaisePropertyChanged(() => DecimalDigits);
            }
        }

        #endregion

        #region 样式设置 - 颜色样式

        private ChartStyleModel m_LegendStyle = new ChartStyleModel();
        /// <summary>
        /// 样式设置 - 颜色设置 - 枚举颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("枚举颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式, PropertyType = EditorType.ColorCollection)]
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

        #region 样式设置 - 类别轴

        private bool m_ShowDimensionTitle = false;
        /// <summary>
        /// 样式设置 - 类别轴 - 是否显示维度标题
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示类别轴标题", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public virtual bool ShowDimensionTitle
        {
            get { return m_ShowDimensionTitle; }
            set
            {
                m_ShowDimensionTitle = value;
                RaisePropertyChanged(() => ShowDimensionTitle);
            }
        }

        private string m_DimensionHeadline = "类别标题";
        /// <summary>
        /// 样式设置 - 类别轴 - 标题文字
        /// </summary>
        [Synchronous]
        [PropertyDescription("标题文字", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public virtual string DimensionHeadline
        {
            get { return m_DimensionHeadline; }
            set
            {
                m_DimensionHeadline = value;
                RaisePropertyChanged(() => DimensionHeadline);
            }
        }

        private bool m_IsCompletionTime;
        /// <summary>
        /// 样式设置 - 类别轴 - 补全时间
        /// todo: 20170411 lidan 该属性功能未实现，暂时在面板中不可见，实现后可以放开
        /// </summary>
        [Synchronous]
        //[PropertyDescription("补全时间", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public virtual bool IsCompletionTime
        {
            get { return m_IsCompletionTime; }
            set
            {
                m_IsCompletionTime = value;
                RaisePropertyChanged(() => IsCompletionTime);
            }
        }

        private DateGranularitys m_TimeGranularity;
        /// <summary>
        /// 样式设置 - 类别轴 - 时间粒度
        /// todo: 20170411 lidan 该属性功能未实现，暂时在面板中不可见，实现后可以放开
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof(StringEnumConverter))]
        //[PropertyDescription("时间粒度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public virtual DateGranularitys TimeGranularity
        {
            get { return m_TimeGranularity; }
            set
            {
                m_TimeGranularity = value;
                RaisePropertyChanged(() => TimeGranularity);
            }
        }

        private int m_DimensionShowCount = 0;
        /// <summary>
        /// 样式设置 - 类别轴 — 显示的维度的个数
        /// 默认为0：显示全部数据
        /// 大于0： 显示前DimensionShowCount条数据
        /// 小于0： 显示后DimensionShowCount条数据
        /// </summary>
        [Synchronous]
        //[PropertyDescription("显示的维度的个数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public int DimensionShowCount
        {
            get { return m_DimensionShowCount; }
            set
            {
                m_DimensionShowCount = value;
                this.RaisePropertyChanged(() => this.DimensionShowCount);
            }
        }

        private bool m_ShowDimensionAxisTick = true;
        /// <summary>
        /// 样式设置 - 类别轴 - 显示刻度
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示刻度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public virtual bool ShowDimensionAxisTick
        {
            get { return m_ShowDimensionAxisTick; }
            set
            {
                m_ShowDimensionAxisTick = value;
                RaisePropertyChanged(() => this.ShowDimensionAxisTick);
            }
        }

        private string m_DimensionAxisTickLineColor = "#FF7F7F7E";
        /// <summary>
        /// 样式设置 - 类别轴 - 刻度线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("刻度线颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴, PropertyType = EditorType.Color)]
        public virtual string DimensionAxisTickLineColor
        {
            get { return m_DimensionAxisTickLineColor; }
            set
            {
                m_DimensionAxisTickLineColor = value;
                RaisePropertyChanged(() => this.DimensionAxisTickLineColor);
            }
        }

        private bool m_ShowDimnesionAxisLabel = true;
        /// <summary>
        /// 样式设置 - 类别轴 - 显示轴标签
        /// </summary>
        /// <returns></returns>
        [Synchronous]
        [PropertyDescription("显示轴标签", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴)]
        public virtual bool ShowDimensionAxisLabel
        {
            get { return m_ShowDimnesionAxisLabel; }
            set
            {
                m_ShowDimnesionAxisLabel = value;
                RaisePropertyChanged(() => this.ShowDimensionAxisLabel);
            }
        }

        private string m_DimensionAxisLineColor = "#FF7F7F7E";
        /// <summary>
        /// 样式设置 - 类别轴 - 轴线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("轴线颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴, PropertyType = EditorType.Color)]
        public virtual string DimensionAxisLineColor
        {
            get { return m_DimensionAxisLineColor; }
            set
            {
                m_DimensionAxisLineColor = value;
                RaisePropertyChanged(() => this.DimensionAxisLineColor);
            }
        }

        private int m_DimensionAxisLabelLength = -1;
        /// <summary>
        /// 样式设置 - 类别轴 — 类别轴标签长度
        /// </summary>
        [Synchronous]
        [PropertyDescription("类别轴标签长度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴, MinValue = -1, MaxValue = 10, DefaultValue = -1)]
        public int DimensionAxisLabelLength
        {
            get { return m_DimensionAxisLabelLength; }
            set
            {
                m_DimensionAxisLabelLength = value;
                RaisePropertyChanged(() => DimensionAxisLabelLength);
            }
        }

        private int m_DimensionAxisLabelAngle = 0;
        /// <summary>
        /// 样式设置 - 类别轴 — 标签旋转角度
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签旋转角度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.类别轴, MinValue = 0, MaxValue = 360, DefaultValue = 0)]
        public int DimensionAxisLabelAngle
        {
            get { return m_DimensionAxisLabelAngle; }
            set
            {
                m_DimensionAxisLabelAngle = value;
                RaisePropertyChanged(() => DimensionAxisLabelAngle);
            }
        }

        #endregion

        #region 样式设置 - 数量轴

        private bool m_ShowMeasureTitle = false;
        /// <summary>
        /// 样式设置 - 数量轴 - 是否显示指标标题
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示数量轴标题", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴)]
        public virtual bool ShowMeasureTitle
        {
            get { return m_ShowMeasureTitle; }
            set
            {
                m_ShowMeasureTitle = value;
                RaisePropertyChanged(() => ShowMeasureTitle);
            }
        }

        private string m_MeasureHeadline = "数量标题";
        /// <summary>
        /// 样式设置 - 数量轴 - 标题文字
        /// </summary>
        [Synchronous]
        [PropertyDescription("标题文字", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴)]
        public virtual string MeasureHeadline
        {
            get { return m_MeasureHeadline; }
            set
            {
                m_MeasureHeadline = value;
                RaisePropertyChanged(() => MeasureHeadline);
            }
        }

        private double m_MeasureMaximum = 10000;
        /// <summary>
        /// 样式设置 - 数量轴 - 最大值
        /// </summary>
        [Synchronous]
        [PropertyDescription("最大值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴, MinValue = double.MinValue, MaxValue = double.MaxValue, DefaultValue = 100)]
        public virtual double MeasureMaximum
        {
            get { return m_MeasureMaximum; }
            set
            {
                if (value < m_MeasureMinimum)
                {
                    value = m_MeasureMinimum;
                }

                m_MeasureMaximum = value;
                RaisePropertyChanged(() => MeasureMaximum);
            }
        }

        private double m_MeasureMinimum = 0;
        /// <summary>
        /// 样式设置 - 数量轴 - 最小值
        /// </summary>
        [Synchronous]
        [PropertyDescription("最小值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴, MinValue = double.MinValue, MaxValue = double.MaxValue, DefaultValue = 0)]
        public virtual double MeasureMinimum
        {
            get { return m_MeasureMinimum; }
            set
            {
                if (value > m_MeasureMaximum)
                {
                    value = m_MeasureMaximum;
                }

                m_MeasureMinimum = value;
                RaisePropertyChanged(() => MeasureMinimum);
            }
        }

        private bool m_UseMaxMinSetting = false;
        /// <summary>
        /// 样式设置 - 数量轴 - 使用最大值最小值设置
        /// </summary>
        [Synchronous]
        [PropertyDescription("使用最大值最小值设置", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴)]
        public virtual bool UseMaxMinSetting
        {
            get { return m_UseMaxMinSetting; }
            set
            {
                m_UseMaxMinSetting = value;
                RaisePropertyChanged(() => UseMaxMinSetting);
            }
        }

        private bool m_ShowMeasureAxisTick = true;
        /// <summary>
        /// 样式设置 - 数量轴 - 显示刻度
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示刻度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴)]
        public virtual bool ShowMeasuereAxisTick
        {
            get { return m_ShowMeasureAxisTick; }
            set
            {
                m_ShowMeasureAxisTick = value;
                RaisePropertyChanged(() => this.ShowMeasuereAxisTick);
            }
        }

        private string m_MeasureAxisTickLineColor = "#FF7F7F7E";
        /// <summary>
        /// 样式设置 - 数量轴 - 刻度线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("刻度线颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴, PropertyType = EditorType.Color)]
        public virtual string MeasureAxisTickLineColor
        {
            get { return m_MeasureAxisTickLineColor; }
            set
            {
                m_MeasureAxisTickLineColor = value;
                RaisePropertyChanged(() => this.MeasureAxisTickLineColor);
            }
        }

        private bool m_ShowMeasureAxisLabel = true;
        /// <summary>
        /// 样式设置 - 数量轴 - 显示轴标签
        /// </summary>
        /// <returns></returns>
        [Synchronous]
        [PropertyDescription("显示轴标签", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴)]
        public virtual bool ShowMeasureAxisLabel
        {
            get { return m_ShowMeasureAxisLabel; }
            set
            {
                m_ShowMeasureAxisLabel = value;
                RaisePropertyChanged(() => this.ShowMeasureAxisLabel);
            }
        }

        private string m_MeasureAxisLineColor = "#FF7F7F7E";
        /// <summary>
        /// 样式设置 - 数量轴 - 轴线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("轴线颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴, PropertyType = EditorType.Color)]
        public virtual string MeasureAxisLineColor
        {
            get { return m_MeasureAxisLineColor; }
            set
            {
                m_MeasureAxisLineColor = value;
                RaisePropertyChanged(() => this.MeasureAxisLineColor);
            }
        }

        private double m_MeasureAxisInterval = 0;
        /// <summary>
        /// 样式设置 - 数量轴 - 数值间隔
        /// </summary>
        [Synchronous]
        [PropertyDescription("数值间隔", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.数量轴, DefaultValue = 0)]
        public virtual double MeasureAxisInterval
        {
            get { return m_MeasureAxisInterval; }
            set
            {
                m_MeasureAxisInterval = value;
                this.RaisePropertyChanged(() => this.MeasureAxisInterval);
            }
        }

        #endregion

        #region 样式设置 - 图例

        private bool m_ShowLegendControl = true;
        /// <summary>
        /// 是否显示图例
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

        private bool m_ShowLegendTitle = false;
        /// <summary>
        /// 样式设置 - 图例 - 是否显示图例标题
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
        /// 样式设置 - 图例 - 图例位置
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof(StringEnumConverter))]
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

        private String m_LegendHeadline = "图例标题";
        /// <summary>
        /// 样式设置 - 图例 - 标签文字
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签文字", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例)]
        public String LegendHeadline
        {
            get { return m_LegendHeadline; }
            set
            {
                m_LegendHeadline = value;
                RaisePropertyChanged(() => LegendHeadline);
            }
        }

        private int m_LegendTextSize = 12;
        /// <summary>
        /// 样式设置 - 图例 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例, MinValue = 2, MaxValue = 50, DefaultValue = 12)]
        public int LegendTextSize
        {
            get { return m_LegendTextSize; }
            set
            {
                m_LegendTextSize = value;
                RaisePropertyChanged(() => LegendTextSize);
            }
        }

        private string m_LegendTextColor = "#FF7F7F7E";
        /// <summary>
        /// 样式设置 - 图例 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.图例, PropertyType = EditorType.Color)]
        public string LegendTextColor
        {
            get { return m_LegendTextColor; }
            set
            {
                m_LegendTextColor = value;
                RaisePropertyChanged(() => LegendTextColor);
            }
        }

        #endregion

        #region 样式设置 - 标签文字

        private bool m_ShowLabel;
        /// <summary>
        /// 样式设置 - 标签文字 - 显示标签
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

        private string m_LabelTextColor = "#FFFFFFFF";
        /// <summary>
        /// 样式设置 - 标签文字 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, PropertyType = EditorType.Color)]
        public string LabelTextColor
        {
            get { return m_LabelTextColor; }
            set
            {
                m_LabelTextColor = value;
                RaisePropertyChanged(() => LabelTextColor);
            }
        }

        private int m_LabelTextSize = 12;
        /// <summary>
        /// 样式设置 - 标签文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, MinValue = 2, MaxValue = 50, DefaultValue = 12)]
        public int LabelTextSize
        {
            get { return m_LabelTextSize; }
            set
            {
                m_LabelTextSize = value;
                RaisePropertyChanged(() => LabelTextSize);
            }
        }

        private int m_LabelDecimalDigits = 0;
        /// <summary>
        /// 样式设置 - 标签文字 - 小数位数
        /// </summary>
        [Synchronous]
        [PropertyDescription("小数位数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, MinValue = 0, MaxValue = 10, DefaultValue = 0)]
        public virtual int LabelDecimalDigits
        {
            get { return m_LabelDecimalDigits; }
            set
            {
                m_LabelDecimalDigits = value;
                RaisePropertyChanged(() => LabelDecimalDigits);
            }
        }

        private bool m_ShowSum = false;
        /// <summary>
        /// 样式设置 - 其他 - 标签显示总和
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签显示总和", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool ShowSum
        {
            get { return m_ShowSum; }
            set
            {
                m_ShowSum = value;
                this.RaisePropertyChanged(() => this.ShowSum);
            }
        }

        private bool m_LabelOverlap = true;
        /// <summary>
        /// 样式设置 - 其他 - 处理标签重叠
        /// </summary>
        [Synchronous]
        [PropertyDescription("处理标签重叠", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool LabelOverlap
        {
            get { return m_LabelOverlap; }
            set
            {
                m_LabelOverlap = value;
                this.RaisePropertyChanged(() => this.LabelOverlap);
            }
        }

        #endregion

        #region 样式设置 - 参考线

        private bool m_ShowReferenceLine = false;
        /// <summary>
        /// 样式设置 - 参考线 - 显示参考线
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示参考线", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.参考线)]
        public virtual bool ShowReferenceLine
        {
            get { return m_ShowReferenceLine; }
            set
            {
                m_ShowReferenceLine = value;
                RaisePropertyChanged(() => ShowReferenceLine);
            }
        }

        private double m_ReferenceLineNumber;
        /// <summary>
        /// 样式设置 - 参考线 - 数值
        /// </summary>
        [Synchronous]
        [PropertyDescription("数值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.参考线, MinValue = 0, MaxValue = double.MaxValue, DefaultValue = 0)]
        public virtual double ReferenceLineNumber
        {
            get { return m_ReferenceLineNumber; }
            set
            {
                m_ReferenceLineNumber = value;
                RaisePropertyChanged(() => ReferenceLineNumber);
            }
        }

        private string m_ReferenceLineColor = "#FFFF0000";
        /// <summary>
        /// 样式设置 - 参考线 - 颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.参考线, PropertyType = EditorType.Color)]
        public virtual string ReferenceLineColor
        {
            get { return m_ReferenceLineColor; }
            set
            {
                m_ReferenceLineColor = value;
                RaisePropertyChanged(() => ReferenceLineColor);
            }
        }

        private double m_ReferenceLineWidth = 2;
        /// <summary>
        /// 样式设置 - 参考线 - 线宽
        /// </summary>
        [Synchronous]
        [PropertyDescription("线宽", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.参考线, MinValue = 0, MaxValue = 10, DefaultValue = 2)]
        public virtual double ReferenceLineWidth
        {
            get { return m_ReferenceLineWidth; }
            set
            {
                m_ReferenceLineWidth = value;
                RaisePropertyChanged(() => ReferenceLineWidth);
            }
        }

        private LineStyle m_ReferenceLineStyle;
        /// <summary>
        /// 样式设置 - 参考线 - 样式
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof(StringEnumConverter))]
        [PropertyDescription("样式", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.参考线)]
        public virtual LineStyle ReferenceLineStyle
        {
            get { return m_ReferenceLineStyle; }
            set
            {
                m_ReferenceLineStyle = value;
                RaisePropertyChanged(() => ReferenceLineStyle);
            }
        }

        #endregion

        #region 样式设置 - 坐标网格

        private bool m_ShowMeasureGrid = true;
        /// <summary>
        /// 样式设置 - 坐标网格 - 显示数量轴网格
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示数量轴网格", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格)]
        public virtual bool ShowMeasureGrid
        {
            get { return m_ShowMeasureGrid; }
            set
            {
                m_ShowMeasureGrid = value;
                this.RaisePropertyChanged(() => this.ShowMeasureGrid);
            }
        }

        private GridLineType m_MeasureGridType = GridLineType.Line;
        /// <summary>
        /// 样式设置 - 坐标网格 - 数量轴网格类型
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量轴网格类型", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格)]
        public virtual GridLineType MeasureGridType
        {
            get { return m_MeasureGridType; }
            set
            {
                m_MeasureGridType = value;
                this.RaisePropertyChanged(() => this.MeasureGridType);
            }
        }

        private string m_MeasureGridColor1 = "#1AFFFFFF";
        /// <summary>
        /// 样式设置 - 坐标网格 - 数量轴网格颜色1
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量轴网格颜色1", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格, PropertyType = EditorType.Color)]
        public virtual string MeasureGridColor1
        {
            get { return m_MeasureGridColor1; }
            set
            {
                m_MeasureGridColor1 = value;
                this.RaisePropertyChanged(() => this.MeasureGridColor1);
            }
        }

        private string m_MeasureGridColor2 = "#4CFFFFFF";
        /// <summary>
        /// 样式设置 - 坐标网格 - 数量轴网格颜色2
        /// </summary>
        [Synchronous]
        [PropertyDescription("数量轴网格颜色2", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格, PropertyType = EditorType.Color)]
        public virtual string MeasureGridColor2
        {
            get { return m_MeasureGridColor2; }
            set
            {
                m_MeasureGridColor2 = value;
                this.RaisePropertyChanged(() => this.MeasureGridColor2);
            }
        }

        private double m_MeasureGridLineWidth = 1;
        /// <summary>
        /// 样式设置 - 坐标网格 - 数量轴网格线线宽
        /// </summary>
        /// <returns></returns>
        [Synchronous]
        [PropertyDescription("数量轴网格线线宽", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格, MinValue = 1, MaxValue = 5, DefaultValue = 1)]
        public virtual double MeasureGridLineWidth
        {
            get { return m_MeasureGridLineWidth; }
            set
            {
                m_MeasureGridLineWidth = value;
                this.RaisePropertyChanged(() => this.MeasureGridLineWidth);
            }
        }

        private LineStyle m_MeasureGridLineStyle = LineStyle.Solid;
        /// <summary>
        /// 样式设置 - 坐标网格 - 数量轴网格线样式
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof(StringEnumConverter))]
        [PropertyDescription("数量轴网格线样式", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格)]
        public virtual LineStyle MeasureGridLineStyle
        {
            get { return m_MeasureGridLineStyle; }
            set
            {
                m_MeasureGridLineStyle = value;
                RaisePropertyChanged(() => MeasureGridLineStyle);
            }
        }

        private bool m_ShowDimensionGrid = true;
        /// <summary>
        /// 样式设置 - 坐标网格 - 显示类别轴网格线
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示类别轴网格线", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格)]
        public virtual bool ShowDimensionGrid
        {
            get { return m_ShowDimensionGrid; }
            set
            {
                m_ShowDimensionGrid = value;
                this.RaisePropertyChanged(() => this.ShowDimensionGrid);
            }
        }

        private GridLineType m_DimensionGridType = GridLineType.Line;
        /// <summary>
        /// 样式设置 - 坐标网格 - 类别轴网格类型
        /// </summary>
        [Synchronous]
        [PropertyDescription("类别轴网格类型", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格)]
        public virtual GridLineType DimensionGridType
        {
            get { return m_DimensionGridType; }
            set
            {
                m_DimensionGridType = value;
                this.RaisePropertyChanged(() => this.DimensionGridType);
            }
        }

        private string m_DimensionGridColor1 = "#33FFFFFF";
        /// <summary>
        /// 样式设置 - 坐标网格 - 类别轴网格颜色1
        /// </summary>
        [Synchronous]
        [PropertyDescription("类别轴网格颜色1", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格, PropertyType = EditorType.Color)]
        public virtual string DimensionGridColor1
        {
            get { return m_DimensionGridColor1; }
            set
            {
                m_DimensionGridColor1 = value;
                this.RaisePropertyChanged(() => this.DimensionGridColor1);
            }
        }

        private string m_DimensionGridColor2 = "#4CFFFFFF";
        /// <summary>
        /// 样式设置 - 坐标网格 - 类别轴网格颜色2
        /// </summary>
        [Synchronous]
        [PropertyDescription("类别轴网格颜色2", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格, PropertyType = EditorType.Color)]
        public virtual string DimensionGridColor2
        {
            get { return m_DimensionGridColor2; }
            set
            {
                m_DimensionGridColor2 = value;
                this.RaisePropertyChanged(() => this.DimensionGridColor2);
            }
        }

        private double m_DimensionGridLineWidth = 1;
        /// <summary>
        /// 样式设置 - 坐标网格 - 类别轴网格线线宽
        /// </summary>
        /// <returns></returns>
        [Synchronous]
        [PropertyDescription("类别轴网格线线宽", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格, MinValue = 1, MaxValue = 5, DefaultValue = 1)]
        public virtual double DimensionGridLineWidth
        {
            get { return m_DimensionGridLineWidth; }
            set
            {
                m_DimensionGridLineWidth = value;
                this.RaisePropertyChanged(() => this.DimensionGridLineWidth);
            }
        }

        private LineStyle m_DimensionGridLineStyle = LineStyle.Solid;
        /// <summary>
        /// 样式设置 - 坐标网格 - 类别轴网格线样式
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof(StringEnumConverter))]
        [PropertyDescription("类别轴网格线样式", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.坐标网格)]
        public virtual LineStyle DimensionGridLineStyle
        {
            get { return m_DimensionGridLineStyle; }
            set
            {
                m_DimensionGridLineStyle = value;
                RaisePropertyChanged(() => DimensionGridLineStyle);
            }
        }

        #endregion

        #region 样式设置 - 其他

        private ChartCombineMode m_CombineModel = ChartCombineMode.Stack;
        /// <summary>
        /// 数据组合模式/数据展现形式
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof(StringEnumConverter))]
        [PropertyDescription("展现形式", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他)]
        public virtual ChartCombineMode CombineMode
        {
            get
            {
                return m_CombineModel;
            }
            set
            {
                if (value == ChartCombineMode.Stack100 && !string.IsNullOrEmpty(this.MeasureHeadline) && this.MeasureHeadline == "数量标题")
                {
                    this.MeasureHeadline = "%";
                }
                m_CombineModel = value;
                this.RaisePropertyChanged(() => this.CombineMode);
            }
        }

        private double m_DefaultWidthRatio = 0.4;
        /// <summary>
        /// 样式设置 - 其他 - 默认宽度比例
        /// </summary>
        [Synchronous]
        [PropertyDescription("默认宽度比例", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0, MaxValue = 1, DefaultValue = 0.4)]
        public virtual double DefaultWidthRatio
        {
            get { return m_DefaultWidthRatio; }
            set
            {
                m_DefaultWidthRatio = value;
                this.RaisePropertyChanged(() => this.DefaultWidthRatio);
            }
        }

        private double m_SubOpacity = 0.5;
        /// <summary>
        /// 联动组名称
        /// </summary>
        [Synchronous]
        [PropertyDescription("子图透明度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他, MinValue = 0.01, MaxValue = 0.99, DefaultValue = 0.5)]
        public virtual double SubOpacity
        {
            get { return m_SubOpacity; }
            set
            {
                m_SubOpacity = value;
                this.RaisePropertyChanged(() => this.SubOpacity);
            }
        }

        private bool m_ShowTooltip = false;
        /// <summary>
        /// 显示提示信息
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示提示信息", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.其他)]
        public bool ShowTooltip
        {
            get { return m_ShowTooltip; }
            set
            {
                m_ShowTooltip = value;
                this.RaisePropertyChanged(() => this.ShowTooltip);
            }
        }

        #endregion

        #region Override

        /// <summary>
        /// 获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            List<DataColumnModel> columns = new List<DataColumnModel>();
            // HACK: lidan 指标个数>=2时，不管图例列的配置，只根据指标配置获取数据
            if (this.MeasureFields.Count >= 2)
            {
                columns.AddRange(this.MeasureFields);
            }
            else
            {
                columns.Add(this.LegendField);
                columns.AddRange(this.MeasureFields);
            }
            columns.Add(this.DimensionField);
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #endregion

        #region ILegendColor

        /// <summary>
        /// 获取枚举颜色相关内容
        /// </summary>
        /// <returns></returns>
        public ChartLegendHelperModel GetLegendColumns(string propertyName)
        {
            ChartLegendHelperModel model = new ChartLegendHelperModel();

            if (this.MeasureFields != null)
            {
                if (this.MeasureFields.Count > 1) // 指标列数量大于1，1:N 形式
                {
                    model.UseLegend = false;
                    model.MeasureColumns.AddRange(this.MeasureFields);
                }
                else if (this.MeasureFields.Count == 1)  // 指标列数量等于1
                {
                    if (this.LegendField == null)
                    {
                        model.UseLegend = false;
                        model.MeasureColumns.AddRange(this.MeasureFields); // 1：1 形式
                    }
                    else
                    {
                        model.UseLegend = true;
                        model.LegendColumns.Add(this.LegendField); // 1:1:1 形式
                    }
                }
                else
                {
                    model.UseLegend = true;
                    model.LegendColumns.Add(this.LegendField); // 没选择指标，只选择了图例时
                }
            }
            else
            {
                model.UseLegend = true;
                model.LegendColumns.Add(this.LegendField); // 没选择指标，只选择了图例时
            }

            // 去掉空列
            model.LegendColumns.RemoveAll(item => item == null);
            model.MeasureColumns.RemoveAll(item => item == null);

            return model;
        }

        /// <summary>
        /// 根据属性字段名称 匹配对应的LegendStyle
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ChartStyleModel GetLegendStyleProperties(string propertyName)
        {
            return this.LegendStyle;
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public BarDataViewModel()
        {
            m_MeasureGridType = GridLineType.Area;
            m_MeasureGridColor1 = "#1AFFFFFF";
            m_MeasureGridColor2 = "#4CFFFFFF";
            m_DimensionGridType = GridLineType.Line;
            m_DimensionGridColor1 = "#33FFFFFF";
            m_DimensionGridColor2 = "#4CFFFFFF";
        }
    }
}

#region 未实现的属性

//#region 数据设置 - 比较图设置

//private string m_CompareGraphKind;
///// <summary>
///// 数据设置 - 比较图设置 - 比较图类型
///// </summary>
//[Synchronous]
//[PropertyDescription("比较图类型", Category = DescriptionEnum.数据设置, SubCategory = "比较图设置")]
//public virtual string CompareGraphKind
//{
//    get { return m_CompareGraphKind; }
//    set
//    {
//        m_CompareGraphKind = value;
//        RaisePropertyChanged(() => CompareGraphKind);
//    }
//}

//private MeasureColumnModel m_CompareGraphMeasureField;
///// <summary>
///// 数据设置 - 比较图设置 - 数量字段
///// </summary>
//[Synchronous]
//[PropertyDescription("数量字段", Category = DescriptionEnum.数据设置, SubCategory = "比较图设置", PropertyType = EditorType.Field)]
//public virtual MeasureColumnModel CompareGraphMeasureField
//{
//    get { return m_CompareGraphMeasureField; }
//    set
//    {
//        m_CompareGraphMeasureField = value;
//        RaisePropertyChanged(() => CompareGraphMeasureField);
//    }
//}

//#endregion


//private string m_CompareColor;
///// <summary>
///// 样式设置 - 颜色样式 - 比较颜色
///// </summary>
//[Synchronous]
//[PropertyDescription("比较颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.颜色样式, PropertyType = EditorType.Color)]
//public virtual string CompareColor
//{
//    get { return m_CompareColor; }
//    set
//    {
//        m_CompareColor = value;
//        RaisePropertyChanged(() => CompareColor);
//    }
//}


//#region 样式设置 - 比较轴

//private bool m_ShowCompareAxis = false;
///// <summary>
///// 样式设置 - 比较轴 - 是否显示
///// </summary>
//[Synchronous]
//[PropertyDescription("是否显示", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.比较轴)]
//public virtual bool ShowCompareAxis
//{
//    get { return m_ShowCompareAxis; }
//    set
//    {
//        m_ShowCompareAxis = value;
//        this.RaisePropertyChanged(() => this.ShowCompareAxis);
//    }
//}

//private string m_CompareAxisHeadline;
///// <summary>
///// 样式设置 - 比较轴 - 标题文字
///// </summary>
//[Synchronous]
//[PropertyDescription("标题文字", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.比较轴)]
//public virtual string CompareAxisHeadline
//{
//    get { return m_CompareAxisHeadline; }
//    set
//    {
//        m_CompareAxisHeadline = value;
//        this.RaisePropertyChanged(() => this.CompareAxisHeadline);
//    }
//}

//private bool m_UseCompareAxisMaxMinSetting;
///// <summary>
///// 样式设置 - 比较轴 - 使用最大值最小值设置
///// </summary>
//[Synchronous]
//[PropertyDescription("使用最大值最小值设置", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.比较轴)]
//public virtual bool UseCompareAxisMaxMinSetting
//{
//    get { return m_UseCompareAxisMaxMinSetting; }
//    set { m_UseCompareAxisMaxMinSetting = value; }
//}

//private double m_CompareAxisMinValue = 0;
///// <summary>
///// 样式设置 - 比较轴 - 最小值
///// </summary>
//[Synchronous]
//[PropertyDescription("最小值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.比较轴)]
//public virtual double CompareAxisMinValue
//{
//    get { return m_CompareAxisMinValue; }
//    set
//    {
//        if (value > m_CompareAxisMaxValue)
//        {
//            value = m_CompareAxisMaxValue - 1;
//        }
//        m_CompareAxisMinValue = value;
//        this.RaisePropertyChanged(() => this.CompareAxisMinValue);
//    }
//}

//private double m_CompareAxisMaxValue = 10000;
///// <summary>
///// 样式设置 - 比较轴 - 最大值
///// </summary>
//[Synchronous]
//[PropertyDescription("最大值", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.比较轴)]
//public virtual double CompareAxisMaxValue
//{
//    get { return m_CompareAxisMaxValue; }
//    set
//    {
//        if (value < m_CompareAxisMinValue)
//        {
//            value = m_CompareAxisMinValue + 1;
//        }
//        m_CompareAxisMaxValue = value;
//        this.RaisePropertyChanged(() => this.CompareAxisMaxValue);
//    }
//}

//#endregion

#endregion

