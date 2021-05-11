using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Digihail.AVE.Playback.Models;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Extensions;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Charts.Utils;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.DataViewModels.Basic;
using Digihail.DAD3.Models.Utils;
using Steema.TeeChart.WPF;
using Steema.TeeChart.WPF.Styles;
using Steema.TeeChart.WPF.Tools;

namespace CM_NewBar
{
    /// <summary>
    /// 高性能柱图控件
    /// </summary>
    public partial class BarViewHP : ChartViewBase
    {
        #region Fields

        /// <summary>
        /// 控制器
        /// </summary>
        private BarHPController m_Controller;

        /// <summary>
        /// DVM
        /// </summary>
        private BarDataViewModel m_DVM;

        /// <summary>
        /// 维度名称
        /// </summary>
        private string m_DimensionName;

        /// <summary>
        /// 指标名称
        /// </summary>
        private string m_MeasureName;

        /// <summary>
        /// 图例名称
        /// </summary>
        private string m_LegendName;

        /// <summary>
        /// 是否为簇状数据源
        /// 默认为false
        /// </summary>
        private bool m_IsCluster = false;

        /// <summary>
        /// 指标名称集合
        /// </summary>
        private List<string> m_MeasureNames = new List<string>();

        /// <summary>
        /// 选中图例索引
        /// </summary>
        private int m_LegendIndex = -1;

        /// <summary>
        /// 选中Series
        /// </summary>
        private Series m_PreSeries = null;

        /// <summary>
        /// 选中Series索引
        /// </summary>
        private int m_PreValueIndex = -1;

        /// <summary>
        /// 记录下传出的condition用以区别是否是自己传出的
        /// </summary>
        private AdapterConditionModel m_Condition = null;

        /// <summary>
        /// 联动记录的数据结构
        /// </summary>
        private struct SelectedItemModel
        {
            public int seriesIndex;
            public int valueIndex;
            public double measureValue;
            public Color color;
        }

        /// <summary>
        /// 记录的数据的列表
        /// </summary>
        private List<SelectedItemModel> m_SelectedItemModelList = new List<SelectedItemModel>();

        /// <summary>
        /// 记录每次刷新过程中的维度和对应的应该添加的index
        /// </summary>
        private Dictionary<string, int> m_LabelToValueIndex = new Dictionary<string, int>();

        /// <summary>
        /// 记录tchart1中的bar和lengend的对应
        /// </summary>
        private Dictionary<string, Bar> m_LegendToBar = new Dictionary<string, Bar>();

        /// <summary>
        ///legend与第几个index的对应 
        /// </summary>
        private Dictionary<string, int> m_LegendToIndex = new Dictionary<string, int>();

        private string m_OldLegendName = null;

        private List<string> m_LabelList = new List<string>();

        private List<string> m_LegendList = new List<string>();

        private Dictionary<int, Bar> m_BarToIndex = new Dictionary<int, Bar>();

        private Dictionary<string, double> m_LabelToCount = new Dictionary<string, double>();

        private Dictionary<string, Dictionary<string, double>> m_MeasureToCount = new Dictionary<string, Dictionary<string, double>>();

        /// <summary>
        /// GroupKey--地图联动使用
        /// </summary>
        private GroupKeyModel m_GroupKeyModel;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model"></param>
        public BarViewHP(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            m_Controller = (BarHPController)this.Controllers[0];
            m_Controller.IsMap = false;

            m_DVM = m_Controller.DataViewModel as BarDataViewModel;

            m_Controller.PropertyChanged += m_Controller_PropertyChanged;

            tChart.Tools.Add(new ColorLine());
            tChart.Tools.Add(new MarksTip());
            tChart.Tools.Add(new GridBand());

            tChart.ClickLegend += tChart_ClickLegend;
            tChart.ClickSeries += tChart_ClickSeries;
            tChart.BeforeDraw += tChart_BeforeDraw;
            tChart1.BeforeDraw += tChart1_BeforeDraw;
            tChart.BeforeDrawAxes += tChart_BeforeDrawAxes;
            tChart1.BeforeDrawAxes += tChart1_BeforeDrawAxes;
            tChart.GetAxisLabel += tChart_GetAxisLabel;
            tChart1.GetAxisLabel += tChart1_GetAxisLabel;

            //初始化Chart
            ChartInitialize();

            this.Loaded += BarViewHP_Loaded;
        }

        /// <summary>
        /// gis图表构造函数--地图
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="groupKeyModel"></param>
        public BarViewHP(ChartControllerBase controller, GroupKeyModel groupKeyModel)
            : base(controller, groupKeyModel)
        {
            InitializeComponent();

            m_Controller = (BarHPController)controller;
            m_Controller.IsMap = true;
            m_DVM = m_Controller.DataViewModel as BarDataViewModel;

            m_Controller.PropertyChanged += m_Controller_PropertyChanged;

            tChart.Tools.Add(new ColorLine());
            tChart.Tools.Add(new MarksTip());
            tChart.Tools.Add(new GridBand());

            tChart.ClickLegend += tChart_ClickLegend;
            tChart.ClickSeries += tChart_ClickSeries;
            tChart.BeforeDraw += tChart_BeforeDraw;
            tChart1.BeforeDraw += tChart1_BeforeDraw;
            tChart.BeforeDrawAxes += tChart_BeforeDrawAxes;
            tChart1.BeforeDrawAxes += tChart1_BeforeDrawAxes;
            tChart.GetAxisLabel += tChart_GetAxisLabel;
            tChart1.GetAxisLabel += tChart1_GetAxisLabel;

            //初始化Chart
            ChartInitialize();

            this.Loaded += BarViewHP_Loaded;

            m_GroupKeyModel = groupKeyModel;
        }

        /// <summary>
        /// 当前图表控件加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarViewHP_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
        }

        /// <summary>
        /// 当前图表控件对应的控制器的属性改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_Controller_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DT")
            {
                if (m_Controller.DT == null || m_Controller.DT.Rows == null || m_Controller.DT.Rows.Count == 0) return;

                UpdateByDataTable(m_Controller.DT);
            }
        }

        #region Override

        /// <summary>
        /// 更新样式
        /// </summary>
        public override void RefreshStyle()
        {
            UpdateAllStyle();
        }

        /// <summary>
        /// 更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            UpdateAllStyle();
        }

        /// <summary>
        /// 外部直接传入图表数据来更新图表
        /// </summary>
        /// <param name="adtList"></param>
        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
            return;
        }

        /// <summary>
        /// 图表被联动时更新图表
        /// </summary>
        /// <param name="selectedModel"></param>
        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
            if (m_DVM == null) return;

            if (!m_DVM.IsLinkage) return;

            if (selectedModel.LinkageGroupName.ToLower() != m_DVM.LinkageGroupName.ToLower()) return;

            if (m_Condition == selectedModel.Condition)
            {
                return;
            }

            if (m_DVM == null || tChart.Series.Count <= 0) return;

            this.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                //将上一次联动的series清除
                ClearSeletedSeries();

                MakeTransparent();

                List<AdapterConditionModel> conditionList = new List<AdapterConditionModel>();
                conditionList.Add(selectedModel.Condition);
                conditionList.Add(m_Controller.Condition);
                AdapterConditionModel conditions = AdapterConditionModelHelper.GetConditions(conditionList);

                if (this.Player == null)
                {
                    m_DataProxy.QueryAsync("", m_DVM.ID.ToString(), conditions).ContinueWith(t =>
                    {
                        this.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
                        {
                            SelectedItemQueryCallBack(t.Result);
                        }));
                    });
                }
                else
                {
                    if (this.Player.State == Enums.PlayState.Stopped)
                    {
                        m_DataProxy.QueryAsync("", m_DVM.ID.ToString(), conditions).ContinueWith(t =>
                        {
                            this.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
                            {
                                SelectedItemQueryCallBack(t.Result);
                            }));
                        });
                    }
                    else
                    {
                        AdapterConditionModel condition = DataUtils.GetStepCondition(this.Player.CurrentAbsoluteTime,
                            this.Player.StartTime,
                            this.Player.PlayStepSize,
                            this.Player.PlayStepGranularity,
                            this.Player.StartTime,
                            this.Player.StopTime,
                            m_DVM,
                            conditions);

                        m_DataProxy.QueryAsync("", m_DVM.ID.ToString(), condition).ContinueWith(t =>
                        {
                            this.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
                            {
                                SelectedItemQueryCallBack(t.Result);
                            }));
                        });
                    }
                }
            }));
        }

        /// <summary>
        /// 图表取消联动时更新图表
        /// </summary>
        /// <param name="clearModel"></param>
        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
            if (m_DVM == null) return;

            if (!m_DVM.IsLinkage) return;

            if (clearModel.LinkageGroupName.ToLower() != m_DVM.LinkageGroupName.ToLower()) return;

            this.Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                UodoTransparent();
                ClearSeletedSeries();
            }));
        }

        /// <summary>
        /// 导出图表
        /// </summary>
        /// <param name="type">导出类型</param>
        public override void ExportChart(ExportType type)
        {
            return;
        }

        /// <summary>
        /// 更新dvm
        /// </summary>
        /// <param name="dvms"></param>
        public override void UpdateDataViewModels(List<ChartDataViewModel> dvms)
        {
            m_DVM = this.DataViewModels[0] as BarDataViewModel;
            RefreshStyle();
        }

        #endregion

        #region TeeChart图表事件

        /// <summary>
        /// 点击图例事件
        /// </summary>
        private void tChart_ClickLegend(object sender, MouseEventArgs e)
        {
            Point point = e.GetPosition(tChart);

            int CurrentIndex = -1;

            CurrentIndex = LegendHelperExtension.FindLegendIndex(point, tChart);

            if (CurrentIndex == -1 || CurrentIndex == m_LegendIndex)
            {
                UodoTransparent();

                m_LegendIndex = -1;

                //清除联动信息
                OnClearSelectedItemEvent(m_DVM);
            }
            else
            {
                MakeTransparent();

                Series series = tChart.Series[CurrentIndex];
                SolidColorBrush brush = GetLegendBrush(series.Legend.Text) as SolidColorBrush;
                series.Color = brush.Color;

                for (int i = 0; i < series.Count; i++)
                {
                    series[i].Color = brush.Color;
                }

                m_LegendIndex = CurrentIndex;

                AdapterConditionModel condition = null;

                if (!m_Controller.IsMap)
                {
                    //创建联动信息
                    condition = AdapterConditionModelHelper.GetConditions(
                                new List<DataColumnModel>() { m_DVM.LegendField },
                                new List<string>() { tChart.Series[CurrentIndex].Legend.Text });
                }
                else
                {
                    //创建地图联动信息
                    List<AdapterConditionModel> conditionList = new List<AdapterConditionModel>();
                    // 图例自身联动条件
                    AdapterConditionModel legendConditon =
                        AdapterConditionModelHelper.GetCondition(m_DVM.LegendField, tChart.Series[CurrentIndex].Legend.Text);
                    conditionList.Add(legendConditon);
                    // 分组条件
                    AdapterConditionModel groupCondition = m_GroupKeyModel.GroupCondition;
                    conditionList.Add(groupCondition);
                    // 拼接成为一个条件
                    condition = AdapterConditionModelHelper.GetConditions(conditionList);
                }

                m_Condition = condition;

                OnSetSelectedItemEvent(m_DVM, condition);
            }
        }

        /// <summary>
        /// 点击图表事件
        /// </summary>
        private void tChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            if (m_PreSeries == s && m_PreValueIndex == valueIndex)
            {
                m_PreSeries = null;
                m_PreValueIndex = -1;

                UodoTransparent();

                //清除联动信息
                OnClearSelectedItemEvent(m_DVM);
            }
            else
            {
                ClearSeletedSeries();
                m_PreSeries = s;
                m_PreValueIndex = valueIndex;
                MakeTransparent();

                SolidColorBrush brush = GetLegendBrush(s.Legend.Text) as SolidColorBrush;
                s.Color = brush.Color;
                s[valueIndex].Color = brush.Color;

                // 找到点击的对象，拼成一个条件，发送给其他图表
                //创建联动信息 
                if (!m_Controller.IsMap)
                {
                    List<string> values = new List<string>();
                    if (m_IsCluster)
                    {
                        values = new List<string>() { s[valueIndex].Label, "" };
                    }
                    else
                    {
                        values = new List<string>() { s[valueIndex].Label, s.Legend.Text };
                    }

                    AdapterConditionModel condition = AdapterConditionModelHelper.GetConditions(
                                                      new List<DataColumnModel>() { m_DVM.DimensionField, m_DVM.LegendField },
                                                      values);
                    m_Condition = condition;

                    OnSetSelectedItemEvent(m_DVM, condition);
                }
                else
                {
                    AdapterConditionModel condition = null;
                    if (m_IsCluster)
                    {
                        // 创建地图联动信息
                        List<AdapterConditionModel> conditionList = new List<AdapterConditionModel>();
                        // 图例自身联动条件
                        AdapterConditionModel legendConditon =
                            AdapterConditionModelHelper.GetCondition(m_DVM.LegendField, "");
                        conditionList.Add(legendConditon);
                        // 维度自身联动条件
                        AdapterConditionModel dimensionConditon =
                            AdapterConditionModelHelper.GetCondition(m_DVM.DimensionField, s[valueIndex].Label);
                        conditionList.Add(dimensionConditon);
                        // 分组条件
                        AdapterConditionModel groupCondition = m_GroupKeyModel.GroupCondition;
                        conditionList.Add(groupCondition);
                        // 拼接成为一个条件
                        condition = AdapterConditionModelHelper.GetConditions(conditionList);
                    }
                    else
                    {
                        // 创建地图联动信息
                        List<AdapterConditionModel> conditionList = new List<AdapterConditionModel>();
                        // 图例自身联动条件
                        AdapterConditionModel legendConditon =
                            AdapterConditionModelHelper.GetCondition(m_DVM.LegendField, s.Legend.Text);
                        conditionList.Add(legendConditon);
                        // 维度自身联动条件
                        AdapterConditionModel dimensionConditon =
                            AdapterConditionModelHelper.GetCondition(m_DVM.DimensionField, s[valueIndex].Label);
                        conditionList.Add(dimensionConditon);
                        // 分组条件
                        AdapterConditionModel groupCondition = m_GroupKeyModel.GroupCondition;
                        conditionList.Add(groupCondition);
                        // 拼接成为一个条件
                        condition = AdapterConditionModelHelper.GetConditions(conditionList);
                    }

                    m_Condition = condition;

                    OnSetSelectedItemEvent(m_DVM, condition);
                }
            }
        }

        /// <summary>
        /// 修改Tip小数位数
        /// </summary>
        private void MarksTipToolGetText(MarksTip sender, MarksTipGetTextEventArgs e)
        {
            string decimalDigits = m_DVM.LabelDecimalDigits.ToString();
            double value = Convert.ToDouble(e.Text);
            e.Text = value.ToString("F" + decimalDigits);
        }

        /// <summary>
        /// 修改单柱宽度
        /// </summary>
        void tChart_BeforeDraw(object sender, Steema.TeeChart.WPF.Drawing.Graphics3D g)
        {
            if (m_LabelList.Count == 1)
            {
                foreach (Series series in tChart.Series)
                {
                    (series as Bar).CustomBarWidth = m_DVM.DefaultWidthRatio * tChart.Chart.Width;
                }
            }
        }

        /// <summary>
        /// 修改单柱宽度
        /// </summary>
        void tChart1_BeforeDraw(object sender, Steema.TeeChart.WPF.Drawing.Graphics3D g)
        {
            if (m_LabelList.Count == 1)
            {
                foreach (Series series in tChart1.Series)
                {
                    (series as Bar).CustomBarWidth = m_DVM.DefaultWidthRatio * tChart1.Chart.Width;
                }
            }
        }

        /// <summary>
        /// 修改数量轴分割数
        /// </summary>
        void tChart_BeforeDrawAxes(object sender, Steema.TeeChart.WPF.Drawing.Graphics3D g)
        {
            tChart.Axes.Left.Increment = m_DVM.MeasureAxisInterval;
        }

        /// <summary>
        /// 修改数量轴分割数
        /// </summary>
        void tChart1_BeforeDrawAxes(object sender, Steema.TeeChart.WPF.Drawing.Graphics3D g)
        {
            tChart1.Axes.Left.Increment = m_DVM.MeasureAxisInterval;
        }

        /// <summary>
        /// 修改类别轴标签长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tChart_GetAxisLabel(object sender, GetAxisLabelEventArgs e)
        {
            if ((sender as Axis) == tChart.Axes.Bottom)
            {
                if (m_DVM.DimensionAxisLabelLength != -1)
                {
                    if (e.LabelText.Length > m_DVM.DimensionAxisLabelLength)
                    {
                        e.LabelText = e.LabelText.Substring(0, m_DVM.DimensionAxisLabelLength);
                    }
                }
            }
        }

        /// <summary>
        /// 修改类别轴标签长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tChart1_GetAxisLabel(object sender, GetAxisLabelEventArgs e)
        {
            if ((sender as Axis) == tChart1.Axes.Bottom)
            {
                if (m_DVM.DimensionAxisLabelLength != -1)
                {
                    if (e.LabelText.Length > m_DVM.DimensionAxisLabelLength)
                    {
                        e.LabelText = e.LabelText.Substring(0, m_DVM.DimensionAxisLabelLength);
                    }
                }
            }
        }

        #endregion

        #region 图表样式处理

        /// <summary>
        /// 更新所有样式
        /// </summary>
        private void UpdateAllStyle()
        {
            //初始化Chart样式
            ChartInitialize();
            RefreshLegendColor();

            tChart.Invalidate();
            tChart1.Invalidate();
        }

        /// <summary>
        /// 初始化Chart样式
        /// </summary>
        private void ChartInitialize()
        {
            InitChart();

            InitChartPanel();

            UpdateLegend();

            UpdateAxes();

            UpdateLabel();

            UpdateMultiStyle();

            UpdateReferenceLine();

            UpdateMarksTip();

            UpdateGridBand();
        }

        /// <summary>
        /// 初始化chart的各种初始属性和状态
        /// </summary>
        private void InitChart()
        {
            //默认设置 - 禁止ZOOM
            tChart.Zoom.Direction = ZoomDirections.None;
            tChart.Panning.Allow = ScrollModes.None;
            tChart1.Zoom.Direction = ZoomDirections.None;
            tChart1.Panning.Allow = ScrollModes.None;
        }

        /// <summary>
        /// 初始化图表Panel
        /// </summary>
        private void InitChartPanel()
        {
            //默认设置 - 隐藏标题
            tChart.Header.Visible = false;
            //默认设置 - 背景透明
            tChart.Panel.Transparency = 100;
            tChart.Panel.Bevel.Inner = Steema.TeeChart.WPF.Drawing.BevelStyles.None;
            tChart.Panel.Bevel.Outer = Steema.TeeChart.WPF.Drawing.BevelStyles.None;
            tChart.Panel.MarginTop = 10;
            tChart.Panel.MarginBottom = 10;
            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginUnits = PanelMarginUnits.Pixels;

            tChart1.Header.Visible = false;
            tChart1.Panel.Transparency = 100;
            tChart1.Panel.Bevel.Inner = Steema.TeeChart.WPF.Drawing.BevelStyles.None;
            tChart1.Panel.Bevel.Outer = Steema.TeeChart.WPF.Drawing.BevelStyles.None;
            tChart1.Panel.MarginTop = 10;
            tChart1.Panel.MarginBottom = 10;
            tChart1.Panel.MarginLeft = 10;
            tChart1.Panel.MarginRight = 10;
            tChart1.Panel.MarginUnits = PanelMarginUnits.Pixels;
        }

        /// <summary>
        /// 更新图例样式
        /// </summary>
        private void UpdateLegend()
        {
            //默认设置 - 图例样式设定
            tChart.Legend.LegendStyle = LegendStyles.Series;
            tChart1.Legend.LegendStyle = LegendStyles.Series;

            //样式设置 - 图例 - 是否显示图例
            tChart.Legend.Visible = m_DVM.ShowLegendControl;
            tChart1.Legend.Visible = m_DVM.ShowLegendControl;

            //样式设置 - 图例 - 图例位置
            switch (m_DVM.LegendPosition)
            {
                case Dock.Left:
                    tChart.Legend.Alignment = LegendAlignments.Left;
                    tChart1.Legend.Alignment = LegendAlignments.Left;
                    break;

                case Dock.Top:
                    tChart.Legend.Alignment = LegendAlignments.Top;
                    tChart1.Legend.Alignment = LegendAlignments.Top;
                    break;

                case Dock.Right:
                    tChart.Legend.Alignment = LegendAlignments.Right;
                    tChart1.Legend.Alignment = LegendAlignments.Right;
                    break;

                case Dock.Bottom:
                    tChart.Legend.Alignment = LegendAlignments.Bottom;
                    tChart1.Legend.Alignment = LegendAlignments.Bottom;
                    break;

                default:
                    break;
            }
            //样式设置 - 图例 - 文字大小
            tChart.Legend.Font.Size = m_DVM.LegendTextSize;
            tChart1.Legend.Font.Size = m_DVM.LegendTextSize;
            //样式设置 - 图例 - 文字颜色
            tChart.Legend.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.LegendTextColor);
            tChart1.Legend.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.LegendTextColor);
            //样式设置 - 图例 - 文字字体
            tChart.Legend.Font.Name = m_DVM.AxisLabelFontFamily;
            tChart1.Legend.Font.Name = m_DVM.AxisLabelFontFamily;

            //样式设置 - 图例 - 是否显示图例标题
            tChart.Legend.Title.Visible = m_DVM.ShowLegendTitle;
            tChart1.Legend.Title.Visible = m_DVM.ShowLegendTitle;

            //样式设置 - 图例 - 标签文字
            tChart.Legend.Title.Text = m_DVM.LegendHeadline;
            tChart1.Legend.Title.Text = m_DVM.LegendHeadline;

            //样式设置 - 图例 - 标签文字大小
            tChart.Legend.Title.Font.Size = m_DVM.LegendTextSize;
            tChart1.Legend.Title.Font.Size = m_DVM.LegendTextSize;

            //样式设置 - 图例 - 标签文字颜色
            tChart.Legend.Title.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.LegendTextColor);
            tChart1.Legend.Title.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.LegendTextColor);

            //样式设置 - 图例 - 标签文字字体
            tChart.Legend.Title.Font.Name = m_DVM.AxisLabelFontFamily;
            tChart1.Legend.Title.Font.Name = m_DVM.AxisLabelFontFamily;

            //默认设置 - 图例标题透明
            tChart.Legend.Title.Transparent = true;
            tChart.Legend.Title.Transparency = 0;

            tChart1.Legend.Title.Transparent = true;
            tChart1.Legend.Title.Transparency = 0;

            //默认设置 - 图例标题黑体取消
            tChart.Legend.Title.Font.Bold = false;
            tChart1.Legend.Title.Font.Bold = false;

        }

        /// <summary>
        /// 更新坐标轴
        /// </summary>
        private void UpdateAxes()
        {
            #region tChart

            // 更新X轴
            UpdateAxisX(tChart);

            // 更新Y轴
            UpdateAxisY(tChart);

            //默认设置 - 隐藏其他坐标轴
            tChart.Axes.Right.Visible = false;
            tChart.Axes.Top.Visible = false;

            // x轴坐标网格相关设置
            UpdateAxisXGrid(tChart);

            // y轴坐标网格相关设置
            UpdateAxisYGrid(tChart);

            #endregion

            #region tChart1

            // 更新X轴
            UpdateAxisX(tChart1);

            // 更新Y轴
            UpdateAxisY(tChart1);

            //默认设置 - 隐藏其他坐标轴
            tChart1.Axes.Right.Visible = false;
            tChart1.Axes.Top.Visible = false;

            tChart1.Axes.Bottom.Grid.Visible = false;
            tChart1.Axes.Left.Grid.Visible = false;

            #endregion
        }

        /// <summary>
        /// 更新X轴
        /// </summary>
        /// <param name="chart"></param>
        private void UpdateAxisX(TChart chart)
        {
            //默认设置 - 显示类别轴
            chart.Axes.Bottom.Visible = true;
            //样式设置 - 类别轴 - 是否显示维度标题
            chart.Axes.Bottom.Title.Visible = m_DVM.ShowDimensionTitle;
            //样式设置 - 类别轴 - 标题文字
            chart.Axes.Bottom.Title.Text = m_DVM.DimensionHeadline;
            //样式设置 - 基本文字 - 文字字体
            chart.Axes.Bottom.Title.Font.Name = m_DVM.AxisLabelFontFamily;
            //样式设置 - 基本文字 - 文字大小
            chart.Axes.Bottom.Title.Font.Size = m_DVM.AxisLabelFontSize;
            //样式设置 - 基本文字 - 文字颜色
            chart.Axes.Bottom.Title.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.AxisLabelForeground);
            //样式设置 - 基本文字 - 文字字体
            chart.Axes.Bottom.Labels.Font.Name = m_DVM.AxisLabelFontFamily;
            //样式设置 - 基本文字 - 文字大小
            chart.Axes.Bottom.Labels.Font.Size = m_DVM.AxisLabelFontSize;
            //样式设置 - 基本文字 - 文字颜色
            chart.Axes.Bottom.Labels.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.AxisLabelForeground);
            //样式设置 - 类别轴 - 显示轴标签
            chart.Axes.Bottom.Labels.Visible = m_DVM.ShowDimensionAxisLabel;
            //样式设置 - 类别轴 - 标签旋转角度
            chart.Axes.Bottom.Labels.Angle = m_DVM.DimensionAxisLabelAngle;
            //样式设置 - 类别轴 - 显示刻度
            chart.Axes.Bottom.Ticks.Visible = m_DVM.ShowDimensionAxisTick;
            chart.Axes.Bottom.MinorTicks.Visible = false;
            //样式设置 - 类别轴 - 刻度线颜色
            chart.Axes.Bottom.Ticks.Color = (Color)ColorConverter.ConvertFromString(m_DVM.DimensionAxisTickLineColor);
            //样式设置 - 类别轴 - 轴线颜色
            chart.Axes.Bottom.AxisPen.Color = (Color)ColorConverter.ConvertFromString(m_DVM.DimensionAxisLineColor);
            chart.Axes.Bottom.AxisPen.Width = 1;
        }

        /// <summary>
        /// 更新Y轴
        /// </summary>
        /// <param name="chart"></param>
        private void UpdateAxisY(TChart chart)
        {
            //默认设置 - 显示数量轴
            chart.Axes.Left.Visible = true;
            //样式设置 - 数量轴 - 是否显示指标标题
            chart.Axes.Left.Title.Visible = m_DVM.ShowMeasureTitle;
            //样式设置 - 数量轴 - 标题文字
            chart.Axes.Left.Title.Text = m_DVM.MeasureHeadline;
            //样式设置 - 基本文字 - 文字字体
            chart.Axes.Left.Title.Font.Name = m_DVM.AxisLabelFontFamily;
            //样式设置 - 基本文字 - 文字大小
            chart.Axes.Left.Title.Font.Size = m_DVM.AxisLabelFontSize;
            //样式设置 - 基本文字 - 文字颜色
            chart.Axes.Left.Title.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.AxisLabelForeground);
            //样式设置 - 基本文字 - 文字字体
            chart.Axes.Left.Labels.Font.Name = m_DVM.AxisLabelFontFamily;
            //样式设置 - 基本文字 - 文字大小
            chart.Axes.Left.Labels.Font.Size = m_DVM.AxisLabelFontSize;
            //样式设置 - 基本文字 - 文字颜色
            chart.Axes.Left.Labels.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.AxisLabelForeground);
            //样式设置 - 数量轴 - 使用最大值最小值设置
            if (m_DVM.UseMaxMinSetting)
            {
                chart.Axes.Left.Automatic = false;
                chart.Axes.Left.AutomaticMinimum = false;
                chart.Axes.Left.AutomaticMaximum = false;
                chart.Axes.Left.Minimum = m_DVM.MeasureMinimum;
                chart.Axes.Left.Maximum = m_DVM.MeasureMaximum;
            }
            else
            {
                chart.Axes.Left.Automatic = true;
                chart.Axes.Left.AutomaticMinimum = true;
                chart.Axes.Left.AutomaticMaximum = true;
            }
            //默认设置 - 显示数量轴
            chart.Axes.Left.AxisPen.Visible = true;
            //样式设置 - 数量轴 - 显示轴标签
            chart.Axes.Left.Labels.Visible = m_DVM.ShowMeasureAxisLabel;
            //样式设置 - 数量轴 - 显示刻度
            chart.Axes.Left.Ticks.Visible = m_DVM.ShowMeasuereAxisTick;
            chart.Axes.Left.MinorTicks.Visible = false;
            //样式设置 - 数量轴 - 刻度线颜色
            chart.Axes.Left.Ticks.Color = (Color)ColorConverter.ConvertFromString(m_DVM.MeasureAxisTickLineColor);
            //样式设置 - 数量轴 - 轴线颜色
            chart.Axes.Left.AxisPen.Color = (Color)ColorConverter.ConvertFromString(m_DVM.MeasureAxisLineColor);
            chart.Axes.Left.AxisPen.Width = 1;
        }

        /// <summary>
        /// 更新x轴坐标网格
        /// </summary>
        /// <param name="chart"></param>
        private void UpdateAxisXGrid(TChart chart)
        {
            if (m_DVM.DimensionGridType == GridLineType.Line)
            {
                chart.Axes.Bottom.Grid.Visible = m_DVM.ShowDimensionGrid;
            }
            else
            {
                chart.Axes.Bottom.Grid.Visible = false;
            }
            chart.Axes.Bottom.Grid.Color = (Color)ColorConverter.ConvertFromString(m_DVM.DimensionGridColor1);
            chart.Axes.Bottom.Grid.Width = m_DVM.DimensionGridLineWidth;
            switch (m_DVM.DimensionGridLineStyle)
            {
                case LineStyle.Solid:
                    chart.Axes.Bottom.Grid.Style = DashStyles.Solid;
                    break;

                case LineStyle.Dash:
                    chart.Axes.Bottom.Grid.Style = DashStyles.Dash;
                    break;

                case LineStyle.DashDot:
                    chart.Axes.Bottom.Grid.Style = DashStyles.DashDot;
                    break;

                case LineStyle.Dot:
                    chart.Axes.Bottom.Grid.Style = DashStyles.Dot;
                    break;

                default:
                    break;
            }
            chart.Axes.Bottom.Grid.DrawEvery = 1;
        }

        /// <summary>
        /// 更新y轴坐标网格
        /// </summary>
        /// <param name="chart"></param>
        private void UpdateAxisYGrid(TChart chart)
        {
            if (m_DVM.MeasureGridType == GridLineType.Line)
            {
                chart.Axes.Left.Grid.Visible = m_DVM.ShowMeasureGrid;
            }
            else
            {
                chart.Axes.Left.Grid.Visible = false;
            }
            chart.Axes.Left.Grid.Color = (Color)ColorConverter.ConvertFromString(m_DVM.MeasureGridColor1);
            chart.Axes.Left.Grid.Width = m_DVM.MeasureGridLineWidth;
            switch (m_DVM.MeasureGridLineStyle)
            {
                case LineStyle.Solid:
                    chart.Axes.Left.Grid.Style = DashStyles.Solid;
                    break;

                case LineStyle.Dash:
                    chart.Axes.Left.Grid.Style = DashStyles.Dash;
                    break;

                case LineStyle.DashDot:
                    chart.Axes.Left.Grid.Style = DashStyles.DashDot;
                    break;

                case LineStyle.Dot:
                    chart.Axes.Left.Grid.Style = DashStyles.Dot;
                    break;

                default:
                    break;
            }
            chart.Axes.Left.Grid.DrawEvery = 1;
        }

        /// <summary>
        /// 更新数据堆积模式
        /// </summary>
        private void UpdateMultiStyle()
        {
            //数据组合模式/数据展现形式
            MultiBars multiBars = new MultiBars();
            switch (m_DVM.CombineMode)
            {
                case ChartCombineMode.Stack:
                    multiBars = MultiBars.Stacked;
                    break;

                case ChartCombineMode.Cluster:
                    multiBars = MultiBars.Side;
                    break;

                case ChartCombineMode.Stack100:
                    multiBars = MultiBars.Stacked100;
                    break;

                default:
                    break;
            }
            //样式设置 - 标签文字
            foreach (Series series in tChart.Series)
            {
                //数据组合模式/数据展现形式
                (series as Bar).MultiBar = multiBars;
                (series as Bar).Pen.Visible = false;
                (series as Bar).Legend.Pen.Visible = false;

                (series as Bar).BarWidthPercent = (int)(m_DVM.DefaultWidthRatio * 100);
            }

            foreach (Series series in tChart1.Series)
            {
                //数据组合模式/数据展现形式
                (series as Bar).MultiBar = multiBars;
                (series as Bar).Pen.Visible = false;
                (series as Bar).Legend.Pen.Visible = false;

                (series as Bar).BarWidthPercent = (int)(m_DVM.DefaultWidthRatio * 100);
            }

            tChart.Legend.Pen.Visible = false;
            tChart1.Legend.Pen.Visible = false;
        }

        /// <summary>
        /// 更新标记和数据组合模式/数据展现形式
        /// </summary>
        private void UpdateLabel()
        {
            //样式设置 - 标签文字
            foreach (Series series in tChart.Series)
            {
                //样式设置 - 标签文字 - 显示标签
                series.Marks.Visible = m_DVM.ShowLabel;
                //样式设置 - 标签文字 - 文字大小
                series.Marks.Font.Size = m_DVM.LabelTextSize;
                //样式设置 - 标签文字 - 文字颜色
                series.Marks.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.LabelTextColor);
                //样式设置 - 标签文字 - 文字字体
                series.Marks.Font.Name = m_DVM.AxisLabelFontFamily;

                //默认设置 - 标签文字
                series.Marks.Style = MarksStyles.Value;
                series.Marks.OnTop = true;
                series.Marks.Transparent = true;
                series.Marks.Transparency = 0;
                series.Marks.Arrow.Visible = false;
                series.Marks.ArrowLength = 0;
                series.Marks.AutoPosition = m_DVM.LabelOverlap;

                if (series.Tag != null)
                {
                    if (series.Tag.ToString() == "统合堆积柱标签")
                    {
                        if ((m_DVM.ShowSum) &&
                            (m_DVM.CombineMode == ChartCombineMode.Stack ||
                            m_DVM.CombineMode == ChartCombineMode.Stack100))
                        {
                            series.Active = true;
                        }
                        else
                        {
                            series.Active = false;
                        }
                    }
                }

                series.ValueFormat = "";
                series.GetSeriesMark -= SeriesGetSeriesMark;
                series.GetSeriesMark += SeriesGetSeriesMark;
            }

            foreach (Series series in tChart1.Series)
            {
                //样式设置 - 标签文字 - 显示标签
                series.Marks.Visible = m_DVM.ShowLabel;
                //样式设置 - 标签文字 - 文字大小
                series.Marks.Font.Size = m_DVM.LabelTextSize;
                //样式设置 - 标签文字 - 文字颜色
                series.Marks.Font.Color = (Color)ColorConverter.ConvertFromString(m_DVM.LabelTextColor);
                //样式设置 - 标签文字 - 文字字体
                series.Marks.Font.Name = m_DVM.AxisLabelFontFamily;

                //默认设置 - 标签文字
                series.Marks.Style = MarksStyles.Value;
                series.Marks.OnTop = true;
                series.Marks.Transparent = true;
                series.Marks.Transparency = 0;
                series.Marks.Arrow.Visible = false;
                series.Marks.ArrowLength = 0;
                series.Marks.AutoPosition = m_DVM.LabelOverlap;

                if (series.Tag != null)
                {
                    if (series.Tag.ToString() == "统合堆积柱标签")
                    {
                        if ((m_DVM.ShowSum) &&
                            (m_DVM.CombineMode == ChartCombineMode.Stack ||
                            m_DVM.CombineMode == ChartCombineMode.Stack100))
                        {
                            series.Active = true;
                        }
                        else
                        {
                            series.Active = false;
                        }
                    }
                }

                series.ValueFormat = "";
                series.GetSeriesMark -= SeriesGetSeriesMark;
                series.GetSeriesMark += SeriesGetSeriesMark;
            }
        }

        /// <summary>
        /// 修改堆积柱标签
        /// </summary>
        private void SeriesGetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
            string decimalDigits = m_DVM.LabelDecimalDigits.ToString();
            if ((m_DVM.ShowSum) &&
                (m_DVM.CombineMode == ChartCombineMode.Stack ||
                m_DVM.CombineMode == ChartCombineMode.Stack100))
            {
                if (series.Tag != null && series.Tag.ToString() == "统合堆积柱标签")
                {
                    double value = 0;
                    for (int i = 0; i < tChart.Series.Count; i++)
                    {
                        Series seriesTemp = tChart.Series[i];
                        for (int j = 0; j < seriesTemp.Count; j++)
                        {
                            if (seriesTemp[j].Label == series[e.ValueIndex].Label)
                            {
                                value += seriesTemp.YValues[j];
                            }
                        }
                    }
                    e.MarkText = value.ToString("F" + decimalDigits);
                }
                else
                {
                    e.MarkText = "";
                }
            }
            else
            {
                if (series.Tag != null && series.Tag.ToString() == "统合堆积柱标签")
                {
                    e.MarkText = "";
                }
                else
                {
                    if (e.MarkText != "")
                    {
                        double value = Convert.ToDouble(e.MarkText);
                        e.MarkText = value.ToString("F" + decimalDigits);
                    }
                }
            }

            if (e.MarkText != "" && Convert.ToDouble(e.MarkText) == 0)
            {
                e.MarkText = "";
            }
        }

        /// <summary>
        /// 统合堆积柱标签   
        /// </summary>
        private void UpdateStackMarks()
        {
            Bar bar = new Bar() { Tag = "统合堆积柱标签" };
            tChart.Series.Add(bar);

            foreach (var LabelToValue in m_LabelToValueIndex)
            {
                bar.Add(LabelToValue.Value, 0, LabelToValue.Key);

            }

            bar.Legend.Visible = false;
        }

        /// <summary>
        /// 更新参考线样式
        /// </summary>
        private void UpdateReferenceLine()
        {
            ColorLine colorLineTool = (tChart.Tools[0] as ColorLine);
            //样式设置 - 参考线 - 显示参考线
            colorLineTool.Active = m_DVM.ShowReferenceLine;
            //默认设置 - 参考线坐标轴
            colorLineTool.Axis = tChart.Axes.Left;
            //样式设置 - 参考线 - 数值
            colorLineTool.Value = m_DVM.ReferenceLineNumber;
            //样式设置 - 参考线 - 颜色
            colorLineTool.Pen.Color = (Color)ColorConverter.ConvertFromString(m_DVM.ReferenceLineColor);
            //样式设置 - 参考线 - 线宽
            colorLineTool.Pen.Width = m_DVM.ReferenceLineWidth;
            //样式设置 - 参考线 - 样式
            switch (m_DVM.ReferenceLineStyle)
            {
                case LineStyle.Solid:
                    colorLineTool.Pen.Style = DashStyles.Solid;
                    break;

                case LineStyle.Dash:
                    colorLineTool.Pen.Style = DashStyles.Dash;
                    break;

                case LineStyle.DashDot:
                    colorLineTool.Pen.Style = DashStyles.DashDot;
                    break;

                case LineStyle.Dot:
                    colorLineTool.Pen.Style = DashStyles.Dot;
                    break;

                default:
                    break;
            }

            colorLineTool.AllowDrag = false;
        }

        /// <summary>
        /// 更新标记提示
        /// </summary>
        private void UpdateMarksTip()
        {
            //默认设置 - 标记提示
            MarksTip marksTipTool = (tChart.Tools[1] as MarksTip);
            marksTipTool.MouseAction = MarksTipMouseAction.Move;
            marksTipTool.Style = MarksStyles.Value;

            marksTipTool.GetText -= MarksTipToolGetText;
            marksTipTool.GetText += MarksTipToolGetText;

            marksTipTool.Active = m_DVM.ShowTooltip;
        }

        /// <summary>
        /// 更新GridBand
        /// </summary>
        private void UpdateGridBand()
        {
            GridBand MeasureGridBandTool = (tChart.Tools[2] as GridBand);
            if (m_DVM.MeasureGridType == GridLineType.Area)
            {
                MeasureGridBandTool.Active = m_DVM.ShowMeasureGrid;
            }
            else
            {
                MeasureGridBandTool.Active = false;
            }
            MeasureGridBandTool.Axis = tChart.Axes.Left;
            MeasureGridBandTool.Band1.Color = (Color)ColorConverter.ConvertFromString(m_DVM.MeasureGridColor1);
            MeasureGridBandTool.Band2.Color = (Color)ColorConverter.ConvertFromString(m_DVM.MeasureGridColor2);

            //GridBand DimensionGridBandTool = (tChart.Tools[3] as GridBand);
            //if (m_DVM.DimensionGridType == GridLineType.Area)
            //{
            //    DimensionGridBandTool.Active = m_DVM.ShowDimensionGrid;
            //}
            //else
            //{
            //    DimensionGridBandTool.Active = false;
            //}
            //DimensionGridBandTool.Axis = tChart.Axes.Bottom;
            //DimensionGridBandTool.Band1.Color = (Color)ColorConverter.ConvertFromString(m_DVM.DimensionGridColor1);
            //DimensionGridBandTool.Band2.Color = (Color)ColorConverter.ConvertFromString(m_DVM.DimensionGridColor2);
        }

        /// <summary>
        /// 刷新图例颜色
        /// </summary>
        private void RefreshLegendColor()
        {
            foreach (Series series in tChart.Series)
            {
                SolidColorBrush brush = GetLegendBrush(series.Legend.Text) as SolidColorBrush;
                series.Color = brush.Color;
                for (int i = 0; i < series.Count; i++)
                {
                    series[i].Color = brush.Color;
                }
            }
        }

        #endregion

        #region Private Methods 数据处理

        /// <summary>
        /// 根据ADT刷新图表
        /// </summary>
        /// <param name="adt"></param>
        private void UpdateByDataTable(AdapterDataTable adt)
        {
            //初始赋值
            JudgeChartType();

            if (m_IsCluster)
            {
                SetClusterBaseFieldValue();
            }
            else
            {
                SetStackBaseFieldValue();
            }

            if (string.IsNullOrEmpty(m_DimensionName))
            {
                return;
            }

            Reset();

            SetOrderToDimension(adt);

            if (m_IsCluster)
            {
                ClusterUpdate(adt);
            }
            else
            {
                for (int i = 0; i < m_LegendList.Count; i++)
                {
                    Bar bar = new Bar() { Tag = m_LegendList[i] };
                    tChart.Series.Add(bar);
                    foreach (var LabelToValue in m_LabelToValueIndex)
                    {
                        bar.Add(LabelToValue.Value, 0, LabelToValue.Key);
                    }

                    SolidColorBrush brush = GetLegendBrush(m_LegendList[i]) as SolidColorBrush;
                    bar.Legend.Text = m_LegendList[i];
                    bar.Color = brush.Color;
                    //渐变
                    //bar.Gradient.StartColor = brush.Color; ;
                    //bar.Gradient.EndColor =  Color.FromArgb(0,0,0,0);
                    bar.BarWidthPercent = (int)(m_DVM.DefaultWidthRatio * 100);
                    //圆角
                    //bar.BarStyle = BarStyles.RoundRectangle;
                    bar.Gradient.StartColor = solidColorBrush.Color;
                    bar.Gradient.EndColor = Color.FromArgb(0, 0, 0, 0);
                    bar.BarStyle = BarStyles.RoundRectangle;
                    bar.Repaint();
                }

                StackUpdate(adt);
            }

            SetOrderToLegend();

            UpdateStackMarks();

            CloneTchartToSecond();

            //更新标记和数据组合模式/数据展现形式
            UpdateLabel();

            UpdateMultiStyle();

            //样式设置 - 数量轴 - 使用最大值最小值设置
            if (m_DVM.UseMaxMinSetting)
            {
                tChart1.Axes.Left.Automatic = false;
                tChart1.Axes.Left.AutomaticMinimum = false;
                tChart1.Axes.Left.AutomaticMaximum = false;
                tChart1.Axes.Left.Minimum = m_DVM.MeasureMinimum;
                tChart1.Axes.Left.Maximum = m_DVM.MeasureMaximum;
            }
            else
            {
                tChart1.Axes.Left.Automatic = true;
                tChart1.Axes.Left.AutomaticMinimum = true;
                tChart1.Axes.Left.AutomaticMaximum = true;
            }
        }

        /// <summary>
        /// 判断是簇状还是堆积数据源
        /// </summary>
        private void JudgeChartType()
        {
            if (m_DVM.MeasureFields != null && m_DVM.MeasureFields.Count > 1
                && m_DVM.DimensionField != null)
            {
                m_IsCluster = true;
            }
            else
            {
                m_IsCluster = false;
            }
        }

        /// <summary>
        /// 堆积图 - 基础值赋值
        /// </summary>
        private void SetStackBaseFieldValue()
        {
            if (m_DVM.MeasureFields != null && m_DVM.MeasureFields.Count > 0 && m_DVM.DimensionField != null)
            {
                m_DimensionName = m_DVM.DimensionField.AsName;
                m_MeasureName = m_DVM.MeasureFields[0].AsName;
                m_LegendName = "";
                if (m_DVM.LegendField != null)
                {
                    m_LegendName = m_DVM.LegendField.AsName;
                }
            }
        }

        /// <summary>
        /// 簇状图 - 基础值赋值
        /// </summary>
        private void SetClusterBaseFieldValue()
        {
            if (m_DVM.MeasureFields != null && m_DVM.MeasureFields.Count > 0 && m_DVM.DimensionField != null)
            {
                m_DimensionName = m_DVM.DimensionField.AsName;
                foreach (MeasureColumnModel measureField in m_DVM.MeasureFields)
                {
                    m_MeasureNames.Add(measureField.AsName);
                }
            }
        }

        /// <summary>
        /// 重置所有的List
        /// </summary>
        private void Reset()
        {
            tChart.Series.Clear();

            m_BarToIndex.Clear();

            m_LabelToValueIndex.Clear();

            if (m_OldLegendName != m_LegendName)
            {
                m_LegendList.Clear();
                m_OldLegendName = m_LegendName;
            }


            m_LabelList.Clear();

            m_LabelToCount.Clear();

            m_MeasureToCount.Clear();

            ClearSeletedSeries();
        }

        /// <summary>
        /// 设置图例的排序
        /// </summary>
        private void SetOrderToLegend()
        {
            if (m_IsCluster)
            {
                return;
            }
            for (int i = 0; i < m_LegendList.Count; i++)
            {
                foreach (var series in tChart.Series)
                {
                    if (m_LegendList[i] == (series as Bar).Legend.Text)
                    {
                        m_BarToIndex[i] = (series as Bar);
                    }
                }
            }

            foreach (var variable in m_BarToIndex)
            {
                tChart.Series.MoveTo(variable.Value, variable.Key);
            }
        }

        /// <summary>
        /// 设置维度的排序
        /// </summary>
        private void SetOrderToDimension(AdapterDataTable adt)
        {
            foreach (AdapterDataRow adapterDataRow in adt.Rows)
            {
                string label = adapterDataRow[m_DimensionName].ToString();

                foreach (var measure in m_DVM.MeasureFields)
                {
                    double count;
                    if (m_LabelToCount.TryGetValue(label, out count))
                    {
                        count = count + Convert.ToDouble(adapterDataRow[measure.AsName]);
                        m_LabelToCount[label] = count;
                    }
                    else
                    {
                        m_LabelToCount[label] = Convert.ToDouble(adapterDataRow[measure.AsName]);
                    }

                    if (!m_MeasureToCount.ContainsKey(measure.AsName))
                    {
                        Dictionary<string, double> m_LabelToCount1 = new Dictionary<string, double>();
                        m_LabelToCount1.Add(label, Convert.ToDouble(adapterDataRow[measure.AsName]));
                        m_MeasureToCount.Add(measure.AsName, m_LabelToCount1);
                    }
                    else
                    {
                        Dictionary<string, double> m_LabelToCount1 = m_MeasureToCount[measure.AsName];

                        double count1;
                        if (m_LabelToCount1.TryGetValue(label, out count1))
                        {
                            count1 = count1 + Convert.ToDouble(adapterDataRow[measure.AsName]);
                            m_LabelToCount1[label] = count1;
                        }
                        else
                        {
                            m_LabelToCount1[label] = Convert.ToDouble(adapterDataRow[measure.AsName]);
                        }
                    }
                }

                if (m_LabelList.Contains(label))
                {

                }
                else
                {
                    m_LabelList.Add(label);
                }

                if ((!string.IsNullOrEmpty(m_LegendName)) && !m_IsCluster)
                {
                    string legendName = adapterDataRow[m_LegendName].ToString();
                    if (m_LegendList.Contains(legendName))
                    {

                    }
                    else
                    {
                        m_LegendList.Add(legendName);
                    }
                }

            }

            if (m_DVM.DimensionField.OrderType == OrderTypes.Ascending)
            {
                //m_LabelList.Sort();

                for (int i = 0; i < m_LabelList.Count; i++)
                {
                    m_LabelToValueIndex[m_LabelList[i]] = i;
                }
            }
            else if (m_DVM.DimensionField.OrderType == OrderTypes.Descending)
            {
                //m_LabelList.Sort();
                //m_LabelList.Reverse();

                for (int i = 0; i < m_LabelList.Count; i++)
                {
                    m_LabelToValueIndex[m_LabelList[i]] = i;
                }
            }
            else if (m_DVM.DimensionField.OrderType == OrderTypes.None)
            {
                SetOrderToMeasure();
            }

            if (!string.IsNullOrEmpty(m_LegendName) && !m_IsCluster)
            {
                if (m_DVM.LegendField.OrderType == OrderTypes.Ascending)
                {
                    m_LegendList.Sort();
                }
                else if (m_DVM.LegendField.OrderType == OrderTypes.Descending)
                {
                    m_LegendList.Sort();
                    m_LegendList.Reverse();
                }
                else if (m_DVM.LegendField.OrderType == OrderTypes.None)
                {

                }
            }
        }

        /// <summary>
        /// 设置指标的排序（当没有维度的排序的时候才会触发）
        /// </summary>
        private void SetOrderToMeasure()
        {
            if (m_DVM.MeasuresUseSameSort == true)
            {
                if (!string.IsNullOrEmpty(m_DVM.MeasureFields[0].AsName))
                {
                    if (m_DVM.MeasureFields[0].OrderType == OrderTypes.Ascending)
                    {
                        var orderList = m_LabelToCount.OrderBy(item => item.Value).ToList();
                        int j = 0;
                        foreach (var labelToCount in orderList)
                        {
                            m_LabelToValueIndex[labelToCount.Key] = j;
                            j++;
                        }
                    }
                    else if (m_DVM.MeasureFields[0].OrderType == OrderTypes.Descending)
                    {
                        var orderList = m_LabelToCount.OrderByDescending(item => item.Value).ToList();
                        int j = 0;
                        foreach (var labelToCount in orderList)
                        {
                            m_LabelToValueIndex[labelToCount.Key] = j;
                            j++;
                        }
                    }
                    else if (m_DVM.MeasureFields[0].OrderType == OrderTypes.None)
                    {
                        for (int i = 0; i < m_LabelList.Count; i++)
                        {
                            m_LabelToValueIndex[m_LabelList[i]] = i;
                        }
                    }
                }
            }
            else
            {
                foreach (var measure in m_DVM.MeasureFields)
                {
                    Dictionary<string, double> m_LabelToCount1 = new Dictionary<string, double>();
                    m_LabelToCount1 = m_MeasureToCount[measure.AsName];

                    if (measure.OrderType == OrderTypes.Ascending)
                    {
                        var orderList = m_LabelToCount1.OrderBy(item => item.Value).ToList();
                        int j = 0;
                        foreach (var labelToCount in orderList)
                        {
                            m_LabelToValueIndex[labelToCount.Key] = j;
                            j++;
                        }

                        break;
                    }
                    else if (measure.OrderType == OrderTypes.Descending)
                    {
                        var orderList = m_LabelToCount1.OrderByDescending(item => item.Value).ToList();
                        int j = 0;
                        foreach (var labelToCount in orderList)
                        {
                            m_LabelToValueIndex[labelToCount.Key] = j;
                            j++;
                        }

                        break;
                    }
                    else if (measure.OrderType == OrderTypes.None)
                    {
                        for (int i = 0; i < m_LabelList.Count; i++)
                        {
                            m_LabelToValueIndex[m_LabelList[i]] = i;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 堆积图更新
        /// </summary>
        /// <param name="adt"></param>
        private void StackUpdate(AdapterDataTable adt)
        {
            foreach (AdapterDataRow adapterDataRow in adt.Rows)
            {
                if (!string.IsNullOrEmpty(m_LegendName))
                {
                    AddModel(adapterDataRow, m_MeasureName, adapterDataRow[m_LegendName].ToString());
                }
                else
                {
                    AddModel(adapterDataRow, m_MeasureName, m_DVM.MeasureFields[0].ColumnName);
                }
            }
        }

        /// <summary>
        /// 簇状图更新
        /// </summary>
        /// <param name="adt"></param>
        private void ClusterUpdate(AdapterDataTable adt)
        {
            for (int i = 0; i < m_DVM.MeasureFields.Count; i++)
            {
                string measureName = m_DVM.MeasureFields[i].AsName;
                string measureColumnName = m_DVM.MeasureFields[i].ColumnName;

                Bar existBar = (from object series in tChart.Series select series as Bar)
               .FirstOrDefault(bar => bar.Tag.ToString().ToLower() == measureColumnName.ToLower());
                if (existBar != null)
                {

                }
                else
                {
                    existBar = new Bar() { Tag = measureColumnName.ToLower() };
                    tChart.Series.Add(existBar);

                    foreach (var LabelToValue in m_LabelToValueIndex)
                    {
                        existBar.Add(LabelToValue.Value, 0, LabelToValue.Key);
                    }
                    existBar.Repaint();
                }
                SolidColorBrush brush = GetLegendBrush(measureColumnName) as SolidColorBrush;
                existBar.Legend.Text = measureColumnName;
                existBar.Color = brush.Color;
                existBar.BarWidthPercent = (int)(m_DVM.DefaultWidthRatio * 100);

                foreach (AdapterDataRow adapterDataRow in adt.Rows) // 遍历所有需要初始化的对象
                {
                    Object dimension = adapterDataRow[m_DimensionName];
                    Object measure = adapterDataRow[measureName];
                    string dimensionValue = dimension.ToString();
                    double measureValue = Convert.ToDouble(measure);
                    UpdateBarData(existBar, dimensionValue, measureValue);
                }
            }
        }

        /// <summary>
        /// 对一行数据进行处理
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="measureName"></param>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private bool AddModel(AdapterDataRow dataRow, string measureName, string legendValue)
        {
            bool result = false;
            try
            {
                double measureValue = Convert.ToDouble(dataRow[measureName]);
                string dimensionValue = dataRow[m_DimensionName].ToString();

                Bar existBar = (from object series in tChart.Series select series as Bar)
                    .FirstOrDefault(bar => bar.Tag.ToString().ToLower() == legendValue.ToLower());
                if (existBar != null)
                {
                    SolidColorBrush brush = GetLegendBrush(legendValue) as SolidColorBrush;
                    existBar.Legend.Text = legendValue;
                    existBar.Color = brush.Color;
                }
                else
                {
                    Bar bar = new Bar() { Tag = legendValue };
                    tChart.Series.Add(bar);
                    existBar = bar;

                    foreach (var LabelToValue in m_LabelToValueIndex)
                    {
                        existBar.Add(LabelToValue.Value, 0, LabelToValue.Key);
                    }
                    existBar.Repaint();
                    SolidColorBrush brush = GetLegendBrush(legendValue) as SolidColorBrush;
                    existBar.Legend.Text = legendValue;
                    existBar.Color = brush.Color;
                    existBar.BarWidthPercent = (int)(m_DVM.DefaultWidthRatio * 100);
                }

                UpdateBarData(existBar, dimensionValue, measureValue);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="existBar"></param>
        /// <param name="dimensionValue"></param>
        /// <param name="measureValue"></param>
        private void UpdateBarData(Bar existBar, string dimensionValue, double measureValue)
        {
            int valueIndex;
            if (m_LabelToValueIndex.TryGetValue(dimensionValue, out valueIndex))
            {
                existBar.YValues[valueIndex] = measureValue;
                existBar.Repaint();
            }
        }

        #endregion

        #region 联动相关

        /// <summary>
        /// 创建筛选条件
        /// </summary>
        /// <param name="column"></param>
        /// <param name="judgmentValue"></param>
        /// <returns></returns>
        private AdapterConditionModel GetCondition(DataColumnModel column, string judgmentValue)
        {
            AdapterConditionModel condition = new AdapterConditionModel();

            condition.Column = column;
            condition.JudgmentType = ConditionJudgmentTypes.Equal;
            condition.JudgmentObject = judgmentValue;

            return condition;
        }


        /// <summary>
        /// 堆积数据源联动更新
        /// </summary>
        /// <param name="adt"></param>
        private void StackSelectedItem(AdapterDataTable adt)
        {
            string dimensionName = m_DVM.DimensionField.AsName;
            string measureName = m_DVM.MeasureFields[0].AsName;
            string legendValue = m_DVM.MeasureFields[0].ColumnName;

            UpdateSelectedItem(dimensionName, measureName, legendValue, adt);

            UpdateSelectedAddedSeries();
        }


        /// <summary>
        /// 簇状数据源联动更新
        /// </summary>
        /// <param name="adt"></param>
        private void ClusterSelectedItem(AdapterDataTable adt)
        {
            for (int i = 0; i < m_DVM.MeasureFields.Count; i++)
            {
                string dimensionName = m_DVM.DimensionField.AsName;
                string measureName = m_DVM.MeasureFields[i].AsName;
                string measureColumnName = m_DVM.MeasureFields[i].ColumnName;
                //string legendValue = measureName;
                UpdateSelectedItem(dimensionName, measureName, measureColumnName, adt);
            }
            UpdateSelectedAddedSeries();
        }

        /// <summary>
        /// 将联动的Tchart1中的series清理
        /// </summary>
        private void ClearSeletedSeries()
        {

            tChart1.Series.Clear();
            m_LegendToBar.Clear();
            m_LegendToIndex.Clear();

            tChart1.Axes.Visible = false;
            tChart1.Legend.Visible = false;

        }

        /// <summary>
        /// 设置tchart的透明度
        /// </summary>
        private void MakeTransparent()
        {
            foreach (Series series in tChart.Series)
            {
                //series.ColorEach = true;
                Color color = new Color();
                color = series.Color;
                color.ScA = (float)m_DVM.SubOpacity;
                series.Color = color;
                for (int i = 0; i < series.Count; i++)
                {
                    series[i].Color = color;
                }
            }
        }

        /// <summary>
        /// 重置tchart的透明度
        /// </summary>
        private void UodoTransparent()
        {
            foreach (Series series in tChart.Series)
            {
                //series.ColorEach = true;
                SolidColorBrush brush = GetLegendBrush(series.Legend.Text) as SolidColorBrush;
                series.Color = brush.Color;
                for (int i = 0; i < series.Count; i++)
                {
                    series[i].Color = brush.Color;
                }
            }
        }

        /// <summary>
        ///检查需要更新的条图并将其记录至m_SelectedItemModelList 
        /// </summary>
        /// <param name="dimensionName"></param>
        /// <param name="measureName"></param>
        /// <param name="legendValue"></param>
        /// <param name="adt"></param>
        private void UpdateSelectedItem(string dimensionName, string measureName, string legendValue, AdapterDataTable adt)
        {
            foreach (AdapterDataRow adapterDataRow in adt.Rows)
            {
                if (!string.IsNullOrEmpty(m_LegendName))
                {
                    legendValue = adapterDataRow[m_LegendName].ToString();
                }
                Object dimension = adapterDataRow[dimensionName];
                Object measure = adapterDataRow[measureName];
                string dimensionValue = dimension.ToString();
                double measureValue = Convert.ToDouble(measure);


                Bar barSeries;
                if (m_LegendToBar.TryGetValue(legendValue, out barSeries))
                {
                    int j;
                    if (m_LabelToValueIndex.TryGetValue(dimensionValue, out j))
                    {
                        double rememberValue = barSeries.YValues[j] - measureValue;
                        if (m_DVM.CombineMode == ChartCombineMode.Cluster)
                        {
                            SolidColorBrush brush = GetLegendBrush(barSeries.Legend.Text) as SolidColorBrush;
                            barSeries[j].Color = brush.Color;
                            if (rememberValue > 0)
                            {
                                barSeries.YValues[j] = measureValue;
                            }
                        }
                        else
                        {
                            SolidColorBrush brush = GetLegendBrush(barSeries.Legend.Text) as SolidColorBrush;
                            barSeries[j].Color = brush.Color;

                            if (rememberValue > 0)
                            {
                                bool isNotHave = true;
                                foreach (var itemModel in m_SelectedItemModelList)
                                {
                                    if (itemModel.seriesIndex == m_LegendToIndex[legendValue] && itemModel.valueIndex == m_LabelToValueIndex[dimensionValue])
                                    {
                                        isNotHave = false;
                                    }
                                }
                                if (isNotHave)
                                {
                                    SelectedItemModel selectedItemModel = new SelectedItemModel();
                                    selectedItemModel.seriesIndex = m_LegendToIndex[legendValue];
                                    selectedItemModel.color = barSeries.Color;
                                    selectedItemModel.valueIndex = m_LabelToValueIndex[dimensionValue];
                                    selectedItemModel.measureValue = rememberValue;
                                    m_SelectedItemModelList.Add(selectedItemModel);
                                    barSeries.YValues[j] = measureValue;
                                }

                            }

                        }
                    }
                    barSeries.Repaint();

                }
            }
        }

        /// <summary>
        /// 柱图联动更新m_SelectedItemModelList中记录的柱体
        /// </summary>
        private void UpdateSelectedAddedSeries()
        {
            int count = tChart1.Series.Count - 1;
            for (int i = count; i >= 0; i--)
            {
                Bar bar = new Bar();
                foreach (var selectedModel in m_SelectedItemModelList)
                {
                    if (selectedModel.seriesIndex == i)
                    {
                        if (bar.Chart == null)
                        {
                            tChart1.Series.Add(bar);
                            bar.Legend.Visible = false;
                            bar.BarWidthPercent = (int)(m_DVM.DefaultWidthRatio * 100);
                            Color color = selectedModel.color;
                            color.ScA = (float)m_DVM.SubOpacity;
                            bar.Color = color;

                            foreach (var LabelToValue in m_LabelToValueIndex)
                            {
                                bar.Add(LabelToValue.Value, 0, LabelToValue.Key);

                            }

                            switch (m_DVM.CombineMode)
                            {
                                case ChartCombineMode.Stack:
                                    bar.MultiBar = MultiBars.Stacked;
                                    break;

                                case ChartCombineMode.Cluster:
                                    bar.MultiBar = MultiBars.Side;
                                    break;

                                case ChartCombineMode.Stack100:
                                    bar.MultiBar = MultiBars.Stacked100;
                                    break;

                                default:
                                    break;
                            }

                            tChart1.Series.MoveTo(bar, i + 1);
                        }

                        bar.YValues[selectedModel.valueIndex] = selectedModel.measureValue;
                    }
                }
                bar.Repaint();
            }
            m_SelectedItemModelList.Clear();
        }

        /// <summary>
        /// 通过tchart生成tchart1
        /// </summary>
        private void CloneTchartToSecond()
        {
            tChart1.Axes.Visible = true;
            tChart1.Legend.Visible = m_DVM.ShowLegendControl;

            for (int i = 0; i < tChart.Series.Count; i++)
            {
                Bar selectedBar = new Bar();
                tChart1.Series.Add(selectedBar);
                selectedBar.DataSource = tChart.Series[i];
                selectedBar.Tag = tChart.Series[i].Tag;

                switch (m_DVM.CombineMode)
                {
                    case ChartCombineMode.Stack:
                        selectedBar.MultiBar = MultiBars.Stacked;
                        break;

                    case ChartCombineMode.Cluster:
                        selectedBar.MultiBar = MultiBars.Side;
                        break;

                    case ChartCombineMode.Stack100:
                        selectedBar.MultiBar = MultiBars.Stacked100;
                        break;

                    default:
                        break;
                }

                selectedBar.Legend.Text = (tChart.Series[i] as Bar).Legend.Text;

                int width;
                if ((tChart.Series[i] as Bar).BarWidthPercent != null)
                {
                    width = (tChart.Series[i] as Bar).BarWidthPercent;
                }
                else
                {
                    width = (int)(m_DVM.DefaultWidthRatio * 100);
                }

                selectedBar.BarWidthPercent = width;

                Color color = (tChart.Series[i] as Bar).Color;
                color.ScA = 0.0f;
                selectedBar.Color = color;
                if (tChart.Series[i].Tag != null && tChart.Series[i].Tag.ToString() == "统合堆积柱标签")
                {
                    selectedBar.Legend.Visible = false;
                }
                selectedBar.Repaint();
            }
        }

        /// <summary>
        /// 联动异步查询数据 回调函数
        /// </summary>
        /// <param name="data"></param>
        private void SelectedItemQueryCallBack(AdapterDataTable data)
        {
            if (data == null || data.Rows.Count <= 0)
            {
                return;
            }
            else
            {
                tChart1.Axes.Visible = true;
                tChart1.Legend.Visible = m_DVM.ShowLegendControl;
                for (int i = 0; i < tChart.Series.Count; i++)
                {
                    Bar selectedBar = new Bar();
                    tChart1.Series.Add(selectedBar);
                    selectedBar.DataSource = tChart.Series[i];
                    selectedBar.Tag = tChart.Series[i].Tag;

                    switch (m_DVM.CombineMode)
                    {
                        case ChartCombineMode.Stack:
                            selectedBar.MultiBar = MultiBars.Stacked;
                            break;

                        case ChartCombineMode.Cluster:
                            selectedBar.MultiBar = MultiBars.Side;
                            break;

                        case ChartCombineMode.Stack100:
                            selectedBar.MultiBar = MultiBars.Stacked100;
                            break;

                        default:
                            break;
                    }


                    int width;
                    if ((tChart.Series[i] as Bar).BarWidthPercent != null)
                    {
                        width = (tChart.Series[i] as Bar).BarWidthPercent;
                    }
                    else
                    {
                        width = (int)(m_DVM.DefaultWidthRatio * 100);
                    }
                    selectedBar.BarWidthPercent = width;

                    if (tChart.Series[i].Tag != null && tChart.Series[i].Tag.ToString() == "统合堆积柱标签")
                    {
                        selectedBar.Legend.Visible = false;
                    }
                    else
                    {
                        m_LegendToBar[(tChart.Series[i] as Bar).Legend.Text] = selectedBar;
                        m_LegendToIndex[(tChart.Series[i] as Bar).Legend.Text] = i;
                        selectedBar.Legend.Text = (tChart.Series[i] as Bar).Legend.Text;
                        Color color = (GetLegendBrush((tChart.Series[i] as Bar).Legend.Text) as SolidColorBrush).Color;
                        color.ScA = 0.0f;
                        selectedBar.Color = color;
                    }
                }

                if (m_IsCluster)
                {
                    ClusterSelectedItem(data);
                }
                else
                {
                    StackSelectedItem(data);
                }

            }

            tChart1.Axes.Left.Automatic = false;
            tChart1.Axes.Left.AutomaticMinimum = false;
            tChart1.Axes.Left.AutomaticMaximum = false;
            tChart1.Axes.Left.Maximum = tChart.Axes.Left.Maximum;
            tChart1.Axes.Left.Minimum = tChart.Axes.Left.Minimum;

            UpdateLabel();
            UpdateMultiStyle();


        }


        #endregion

        #region 图例颜色相关

        /// <summary>
        /// 根据图例值获取颜色值
        /// 1:1 时，legendValue为指标名称
        /// 1:1:1 时，legendValue为图例值
        /// 1:N 时，legendValue为指标名称
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private Brush GetLegendBrush(string legendValue)
        {
            ChartStyleModel chartStyle = m_DVM.LegendStyle;

            return GetBrushHelper.GetSolidColorBrush(chartStyle, legendValue);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            tChart.ClickLegend -= tChart_ClickLegend;
            tChart.ClickSeries -= tChart_ClickSeries;

            base.Dispose();
        }

        #endregion
    }
}
