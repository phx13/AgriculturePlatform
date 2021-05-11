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
using Digihail.DAD3.Models.Utils;
using Steema.TeeChart.WPF;
using Steema.TeeChart.WPF.Drawing;
using Steema.TeeChart.WPF.Styles;
using Steema.TeeChart.WPF.Tools;

namespace CM.AnnularProgress.Pie
{
    /// <summary>
    ///     高性能饼图控件
    /// </summary>
    public partial class PieViewHP : ChartViewBase
    {
        private readonly DispatcherTimer m_SubTimer = new DispatcherTimer();


        private readonly DispatcherTimer m_Timer = new DispatcherTimer();
        private int m_Index;
        private int m_SubIndex;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public PieViewHP(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            m_Controller = (PieHPController) Controllers[0];
            m_DVM = m_Controller.DataViewModel as PieDataViewModel;

            DataContext = m_DVM;
            m_Controller.PropertyChanged += m_Controller_PropertyChanged;


            tChart.Tools.Add(new MarksTip());

            tChart.ClickLegend += tChart_ClickLegend;
            tChart.ClickSeries += tChart_ClickSeries;
            tChart.GetLegendText += tChart_GetLegendText;

            //初始化Chart样式
            ChartInitialize();

            Loaded += PieViewHP_Loaded;

            m_Timer.Interval = TimeSpan.FromSeconds(10);
            m_Timer.Tick += DispatcherTimer_Event;

            m_SubTimer.Interval = TimeSpan.FromMilliseconds(1);
            m_SubTimer.Tick += DispatcherSubTimer_Event;
        }


        /// <summary>
        ///     gis图表构造函数--地图
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="groupKeyModel"></param>
        public PieViewHP(ChartControllerBase controller, GroupKeyModel groupKeyModel)
            : base(controller, groupKeyModel)
        {
            InitializeComponent();

            m_Controller = (PieHPController) Controllers[0];
            m_DVM = m_Controller.DataViewModel as PieDataViewModel;

            m_Controller.IsMap = true;

            m_Controller.PropertyChanged += m_Controller_PropertyChanged;

            tChart.Tools.Add(new MarksTip());

            tChart.ClickLegend += tChart_ClickLegend;
            tChart.ClickSeries += tChart_ClickSeries;
            tChart.GetLegendText += tChart_GetLegendText;

            //初始化Chart样式
            ChartInitialize();

            Loaded += PieViewHP_Loaded;

            //初始化Chart
            ChartInitialize();

            Loaded += PieViewHP_Loaded;

            m_GroupKeyModel = groupKeyModel;
        }

        private void DispatcherSubTimer_Event(object sender, EventArgs e)
        {
            m_PreSeries = tChart.Series[0];

            var brush = GetLegendBrush(m_PreSeries[m_Index].Label) as SolidColorBrush;
            var A = brush.Color.A;
            var R = brush.Color.R;
            var G = brush.Color.G;
            var B = brush.Color.B;
            if (A - m_SubIndex > 125)
            {
                A = Convert.ToByte(A - m_SubIndex);
            }
            else if (m_SubIndex > 255)
            {
                m_SubIndex = 0;
            }
            else
            {
                A = Convert.ToByte(m_SubIndex);
            }
            m_SubIndex += 5;

            m_PreSeries[m_Index].Color = Color.FromArgb(A, R, G, B);
        }


        private void DispatcherTimer_Event(object sender, EventArgs e)
        {
            m_PreSeries = tChart.Series[0];

            var brush = GetLegendBrush(m_PreSeries[m_Index].Label) as SolidColorBrush;

            m_PreSeries[m_Index].Color = brush.Color;

            m_SubTimer.Stop();

            m_SubTimer.Start();
            m_SubIndex = 0;
            m_Index++;

            if (m_Index > m_PreSeries.Colors.Count - 1)
            {
                m_Index = 0;
            }

            var existPie = (from object series in tChart.Series select series as Donut)
                .FirstOrDefault(Pie => Pie.Tag.ToString().ToLower() == m_PreSeries[m_Index].Label.ToLower());
            var label = m_PreSeries[m_Index].Label;
            var selectedRow =
                m_Controller.DT.Rows.FirstOrDefault(item => item[m_DVM.LegendField.AsName].ToString().Equals(label));

            var value = selectedRow[m_DVM.MeasureField.AsName].ToString();
            var legend = selectedRow[m_DVM.LegendField.AsName].ToString();

            ShowLegend.Text = legend;
            ShowValue.Text = value;
            var newBrush = GetLegendBrush(m_PreSeries[m_Index].Label) as SolidColorBrush;
            ShowValue.Foreground = newBrush;
            ShowLegend.Foreground = newBrush;
        }

        /// <summary>
        ///     当前图表控件加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PieViewHP_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
        }

        /// <summary>
        ///     当前图表控件对应的控制器的属性改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_Controller_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DT")
            {
                if (m_Controller.DT == null || m_Controller.DT.Rows.Count == 0 || m_Controller.DT.ColumnNames.Count == 0)
                {
                    if (m_DVM.IsNullReceiveData)
                    {
                        Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    Visibility = Visibility.Visible;
                    UpdateByDataTable(m_Controller.DT);

                    #region 外环扇形开始闪烁

                    m_Timer.Start();
                    var sreSeries = tChart.Series[0];

                    var brush = GetLegendBrush(sreSeries[m_Index].Label) as SolidColorBrush;

                    sreSeries[m_Index].Color = brush.Color;

                    m_SubTimer.Stop();

                    m_SubTimer.Start();
                    m_SubIndex = 0;
                    m_Index++;

                    if (m_Index > sreSeries.Colors.Count - 1)
                    {
                        m_Index = 0;
                    }

                    var existPie = (from object series in tChart.Series select series as Donut)
                        .FirstOrDefault(Pie => Pie.Tag.ToString().ToLower() == sreSeries[m_Index].Label.ToLower());
                    var label = sreSeries[m_Index].Label;
                    var selectedRow =
                        m_Controller.DT.Rows.FirstOrDefault(
                            item => item[m_DVM.LegendField.AsName].ToString().Equals(label));

                    var value = selectedRow[m_DVM.MeasureField.AsName].ToString();
                    var legend = selectedRow[m_DVM.LegendField.AsName].ToString();

                    ShowLegend.Text = legend;
                    ShowValue.Text = value;
                    var newBrush = GetLegendBrush(sreSeries[m_Index].Label) as SolidColorBrush;
                    ShowValue.Foreground = newBrush;
                    ShowLegend.Foreground = newBrush;

                    #endregion
                }
            }
        }

        #region 联动相关

        /// <summary>
        ///     创建筛选条件
        /// </summary>
        /// <param name="column"></param>
        /// <param name="judgmentValue"></param>
        /// <returns></returns>
        private AdapterConditionModel GetCondition(DataColumnModel column, string judgmentValue)
        {
            var condition = new AdapterConditionModel();

            condition.Column = column;
            condition.JudgmentType = ConditionJudgmentTypes.Equal;
            condition.JudgmentObject = judgmentValue;

            return condition;
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///     释放资源
        /// </summary>
        public override void Dispose()
        {
            tChart.ClickLegend -= tChart_ClickLegend;
            tChart.ClickSeries -= tChart_ClickSeries;

            base.Dispose();
        }

        #endregion

        #region Fields

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly PieHPController m_Controller;

        /// <summary>
        ///     DVM
        /// </summary>
        private PieDataViewModel m_DVM;

        /// <summary>
        ///     指标名称
        /// </summary>
        /// <summary>
        ///     图例名称
        /// </summary>
        private string m_LegendName;

        /// <summary>
        ///     需要remove的Pie
        /// </summary>
        /// <summary>
        ///     指标名称集合
        /// </summary>
        private readonly List<string> m_MeasureNames = new List<string>();

        /// <summary>
        ///     选中图例索引
        /// </summary>
        private int m_LegendIndex = -1;

        /// <summary>
        ///     选中Series
        /// </summary>
        private Series m_PreSeries;

        /// <summary>
        ///     选中Series索引
        /// </summary>
        private int m_PreValueIndex = -1;

        /// <summary>
        ///     默认颜色
        /// </summary>
        /// <summary>
        ///     饼图半径百分比
        /// </summary>
        private readonly Dictionary<Donut, double> m_SeriesToRadiusPercent = new Dictionary<Donut, double>();

        private AdapterConditionModel m_Condition;

        /// <summary>
        ///     GroupKey--地图联动使用
        /// </summary>
        private readonly GroupKeyModel m_GroupKeyModel;

        #endregion

        #region Override

        /// <summary>
        ///     更新样式
        /// </summary>
        public override void RefreshStyle()
        {
            UpdateAllStyle();
        }

        /// <summary>
        ///     更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            UpdateAllStyle();
        }

        /// <summary>
        ///     外部直接传入图表数据来更新图表
        /// </summary>
        /// <param name="adtList"></param>
        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        /// <summary>
        ///     图表被联动时更新图表
        /// </summary>
        /// <param name="selectedModel"></param>
        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
            if (m_DVM == null) return;

            if (!m_DVM.IsLinkage) return;

            if (selectedModel.LinkageGroupName.ToLower() != m_DVM.LinkageGroupName.ToLower()) return;

            if (m_Condition == selectedModel.Condition) return;

            Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                ClearSelectedSeries();

                //m_DataProxy.QuerySync("", m_DVM.ID.ToString(), selectedModel.Condition, SelectedItemQueryCallBack);

                var conditionList = new List<AdapterConditionModel>();
                conditionList.Add(selectedModel.Condition);
                conditionList.Add(m_Controller.Condition);
                var conditions = AdapterConditionModelHelper.GetConditions(conditionList);

                if (Player == null)
                {
                    m_DataProxy.QueryAsync("", m_DVM.ID.ToString(), conditions)
                        .ContinueWith(
                            t =>
                            {
                                Dispatcher.Invoke(DispatcherPriority.Send,
                                    new Action(delegate { SelectedItemQueryCallBack(t.Result); }));
                            });
                }
                else
                {
                    if (Player.State == Enums.PlayState.Stopped)
                    {
                        m_DataProxy.QueryAsync("", m_DVM.ID.ToString(), conditions)
                            .ContinueWith(
                                t =>
                                {
                                    Dispatcher.Invoke(DispatcherPriority.Send,
                                        new Action(delegate { SelectedItemQueryCallBack(t.Result); }));
                                });
                    }
                    else
                    {
                        var condition = DataUtils.GetStepCondition(Player.CurrentAbsoluteTime,
                            Player.StartTime,
                            Player.PlayStepSize,
                            Player.PlayStepGranularity,
                            Player.StartTime,
                            Player.StopTime,
                            m_DVM,
                            conditions);
                        m_DataProxy.QueryAsync("", m_DVM.ID.ToString(), condition)
                            .ContinueWith(
                                t =>
                                {
                                    Dispatcher.Invoke(DispatcherPriority.Send,
                                        new Action(delegate { SelectedItemQueryCallBack(t.Result); }));
                                });
                    }
                }
            }));
        }

        /// <summary>
        ///     数据查询异步回调函数
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dvmID"></param>
        /// <param name="condition"></param>
        /// <param name="adt"></param>
        private void SelectedItemQueryCallBack(AdapterDataTable adt)
        {
            MakeTransparent();
            if (adt == null)
            {
                // todo: 高国平 半透明 opaity=0.3  被联动的时候的更新
            }
            else
            {
                // todo: 高国平 进行比较
                if (m_MeasureNames.Count == 0 || string.IsNullOrEmpty(m_LegendName))
                {
                    return;
                }

                foreach (var adapterDataRow in adt.Rows)
                {
                    var legendValue = adapterDataRow[m_LegendName].ToString();
                    var measureValue = Convert.ToDouble(adapterDataRow[m_MeasureNames[0]]);

                    var pie = tChart.Series[0].Clone() as Donut;
                    pie.MultiPie = MultiPies.Disabled;
                    pie.Circled = true;
                    pie.Pen.Color = (Color) ColorConverter.ConvertFromString(m_DVM.PieStroke);
                    pie.Pen.Width = m_DVM.PieStrokeThickness;
                    pie.DonutPercent = (int) (m_DVM.PieHoleSize*100);

                    pie.BeforeDrawValues += BeforeDrawValues;

                    for (var i = 0; i < pie.Count; i++)
                    {
                        if (pie[i].Label == legendValue)
                        {
                            var brush = GetLegendBrush(pie[i].Label) as SolidColorBrush;
                            pie[i].Color = brush.Color;

                            m_SeriesToRadiusPercent.Add(pie,
                                measureValue/pie.PieValues[i]*(1 - m_DVM.PieHoleSize) + m_DVM.PieHoleSize);
                        }
                        else
                        {
                            var color = new Color();
                            color = pie[i].Color;
                            color.ScA = 0.0F;
                            pie[i].Color = color;
                        }
                    }

                    m_SeriesToRadiusPercent.OrderBy(t => t.Value);
                    for (var i = 0; i < m_SeriesToRadiusPercent.Count; i++)
                    {
                        tChart.Series.Add(m_SeriesToRadiusPercent.ElementAt(i).Key);
                    }
                }
            }
        }

        /// <summary>
        ///     修改半径
        /// </summary>
        private void BeforeDrawValues(object sender, Graphics3D g)
        {
            var donut = sender as Donut;
            var radiusPercent = m_SeriesToRadiusPercent[donut];
            if (radiusPercent > 1)
            {
                radiusPercent = 1;
            }
            donut.CustomXRadius = radiusPercent*(tChart.Series[0] as Donut).XRadius;
            donut.DonutPercent = (int) (m_DVM.PieHoleSize/radiusPercent*100);

            radiusPercent = m_DVM.PieHoleSize*100/donut.DonutPercent;
            donut.CustomXRadius = radiusPercent*(tChart.Series[0] as Donut).XRadius;
        }

        /// <summary>
        ///     图表取消联动时更新图表
        /// </summary>
        /// <param name="clearModel"></param>
        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
            if (m_DVM == null) return;

            if (!m_DVM.IsLinkage) return;

            if (clearModel.LinkageGroupName.ToLower() != m_DVM.LinkageGroupName.ToLower()) return;

            Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                // todo: 高国平  清理掉联动 
                ClearSelectedSeries();
                UodoTransparent();
            }));
        }

        /// <summary>
        ///     导出图表
        /// </summary>
        /// <param name="type">导出类型</param>
        public override void ExportChart(ExportType type)
        {
        }

        /// <summary>
        ///     更新dvm
        /// </summary>
        /// <param name="dvms"></param>
        public override void UpdateDataViewModels(List<ChartDataViewModel> dvms)
        {
            m_DVM = DataViewModels[0] as PieDataViewModel;
            RefreshStyle();
        }

        #endregion

        #region TeeChart图表事件

        /// <summary>
        ///     点击图例事件
        /// </summary>
        private void tChart_ClickLegend(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(tChart);

            var CurrentIndex = -1;

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

                foreach (Series series in tChart.Series)
                {
                    var brush = GetLegendBrush(series[CurrentIndex].Label) as SolidColorBrush;
                    series[CurrentIndex].Color = brush.Color;
                }

                m_LegendIndex = CurrentIndex;

                AdapterConditionModel condition = null;

                if (!m_Controller.IsMap)
                {
                    // 创建联动信息
                    condition = AdapterConditionModelHelper.GetConditions(
                        new List<DataColumnModel> {m_DVM.LegendField},
                        new List<string> {tChart.Series[0].Labels[CurrentIndex]});
                }
                else
                {
                    // 创建地图联动信息
                    var conditionList = new List<AdapterConditionModel>();
                    // 图例自身联动条件
                    var legendConditon =
                        AdapterConditionModelHelper.GetCondition(m_DVM.LegendField,
                            tChart.Series[0].Labels[CurrentIndex]);
                    conditionList.Add(legendConditon);
                    // 分组条件
                    var groupCondition = m_GroupKeyModel.GroupCondition;
                    conditionList.Add(groupCondition);
                    // 拼接成为一个条件
                    condition = AdapterConditionModelHelper.GetConditions(conditionList);
                }

                m_Condition = condition;

                OnSetSelectedItemEvent(m_DVM, condition);
            }
        }

        /// <summary>
        ///     点击图表事件
        /// </summary>
        private void tChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            if (m_PreSeries == s && m_PreValueIndex == valueIndex)
            {
                m_PreSeries = null;
                m_PreValueIndex = -1;

                //清除联动信息
                OnClearSelectedItemEvent(m_DVM);
            }
            else
            {
                m_PreSeries = s;
                m_PreValueIndex = valueIndex;
                ClearSelectedSeries();
                MakeTransparent();

                var brush = GetLegendBrush(s[valueIndex].Label) as SolidColorBrush;
                s[valueIndex].Color = brush.Color;

                //创建联动信息 
                AdapterConditionModel condition = null;

                if (!m_Controller.IsMap)
                {
                    // 创建联动信息
                    condition = AdapterConditionModelHelper.GetConditions(
                        new List<DataColumnModel> {m_DVM.LegendField},
                        new List<string> {s.Labels[valueIndex]});
                }
                else
                {
                    //创建地图联动信息
                    var conditionList = new List<AdapterConditionModel>();
                    // 图例自身联动条件
                    var legendConditon =
                        AdapterConditionModelHelper.GetCondition(m_DVM.LegendField, s.Labels[valueIndex]);
                    conditionList.Add(legendConditon);
                    // 分组条件
                    var groupCondition = m_GroupKeyModel.GroupCondition;
                    conditionList.Add(groupCondition);
                    // 拼接成为一个条件
                    condition = AdapterConditionModelHelper.GetConditions(conditionList);
                }

                m_Condition = condition;

                OnSetSelectedItemEvent(m_DVM, condition);
            }
        }

        /// <summary>
        ///     修改图例名称
        /// </summary>
        private void tChart_GetLegendText(object sender, GetLegendTextEventArgs e)
        {
            var index = e.Text.IndexOf("");
            e.Text = e.Text.Substring(index);
        }

        #endregion

        #region 图表样式处理

        /// <summary>
        ///     更新所有样式
        /// </summary>
        private void UpdateAllStyle()
        {
            //初始化Chart样式
            ChartInitialize();
            RefreshLegendColor();

            tChart.Invalidate();
        }

        /// <summary>
        ///     初始化Chart样式
        /// </summary>
        private void ChartInitialize()
        {
            InitChartPanel();

            UpdateLegend();

            UpdateAxes();

            UpdateLabel();

            UpdateMarksTip();

            UpdateOtherStyle();
        }

        /// <summary>
        ///     初始化图表Panel
        /// </summary>
        private void InitChartPanel()
        {
            //默认设置 - 隐藏标题
            tChart.Header.Visible = false;
            //默认设置 - 背景透明
            tChart.Panel.Transparency = 100;
            tChart.Panel.Bevel.Inner = BevelStyles.None;
            tChart.Panel.Bevel.Outer = BevelStyles.None;
            tChart.Panel.MarginTop = 10;
            tChart.Panel.MarginBottom = 10;
            tChart.Panel.MarginLeft = 10;
            tChart.Panel.MarginRight = 10;
            tChart.Panel.MarginUnits = PanelMarginUnits.Pixels;
            //默认设置 - 禁止ZOOM
            tChart.Zoom.Direction = ZoomDirections.None;
            tChart.Panning.Allow = ScrollModes.None;
        }

        /// <summary>
        ///     更新图例样式
        /// </summary>
        private void UpdateLegend()
        {
            //默认设置 - 图例样式设定
            tChart.Legend.LegendStyle = LegendStyles.Palette;

            //样式设置 - 图例 - 是否显示图例
            tChart.Legend.Visible = m_DVM.ShowLegendControl;
            //样式设置 - 图例 - 图例位置
            switch (m_DVM.LegendPosition)
            {
                case Dock.Left:
                    tChart.Legend.Alignment = LegendAlignments.Left;
                    break;

                case Dock.Top:
                    tChart.Legend.Alignment = LegendAlignments.Top;
                    break;

                case Dock.Right:
                    tChart.Legend.Alignment = LegendAlignments.Right;
                    break;

                case Dock.Bottom:
                    tChart.Legend.Alignment = LegendAlignments.Bottom;
                    break;

                default:
                    break;
            }
            //样式设置 - 图例 - 文字大小
            tChart.Legend.Font.Size = m_DVM.LegendTextSize;
            //样式设置 - 图例 - 文字颜色
            tChart.Legend.Font.Color = (Color) ColorConverter.ConvertFromString(m_DVM.LegendTextColor);
            //样式设置 - 图例 - 文字字体
            tChart.Legend.Font.Name = "微软雅黑";

            //样式设置 - 图例 - 是否显示图例标题
            tChart.Legend.Title.Visible = m_DVM.ShowLegendTitle;
            //样式设置 - 图例 - 标签文字
            tChart.Legend.Title.Text = m_DVM.LegendHeadline;
            //样式设置 - 图例 - 标签文字大小
            tChart.Legend.Title.Font.Size = m_DVM.LegendTextSize;
            //样式设置 - 图例 - 标签文字颜色
            tChart.Legend.Title.Font.Color = (Color) ColorConverter.ConvertFromString(m_DVM.LegendTextColor);
            //样式设置 - 图例 - 标签文字字体
            tChart.Legend.Title.Font.Name = "微软雅黑";

            //默认设置 - 图例标题透明
            tChart.Legend.Title.Transparent = true;
            tChart.Legend.Title.Transparency = 0;
            //默认设置 - 图例标题黑体取消
            tChart.Legend.Title.Font.Bold = false;

            tChart.Legend.Symbol.DefaultPen = false;
        }

        /// <summary>
        ///     更新坐标轴
        /// </summary>
        private void UpdateAxes()
        {
            //默认设置 - 隐藏类别轴
            tChart.Axes.Bottom.Visible = false;

            //默认设置 - 隐藏数量轴
            tChart.Axes.Left.Visible = false;

            //默认设置 - 隐藏其他坐标轴
            tChart.Axes.Right.Visible = false;
            tChart.Axes.Top.Visible = false;
        }

        /// <summary>
        ///     更新标记和数据组合模式/数据展现形式
        /// </summary>
        private void UpdateLabel()
        {
            //样式设置 - 标签文字
            foreach (Series series in tChart.Series)
            {
                //数据组合模式/数据展现形式
                (series as Donut).MultiPie = MultiPies.Disabled;
                //样式设置 - 标签文字 - 显示标签
                series.Marks.Visible = m_DVM.ShowLabel;
                //样式设置 - 标签文字 - 文字大小
                series.Marks.Font.Size = m_DVM.LabelTextSize;
                //样式设置 - 标签文字 - 文字颜色
                series.Marks.Font.Color = (Color) ColorConverter.ConvertFromString(m_DVM.LabelTextColor);
                //样式设置 - 标签文字 - 文字字体
                series.Marks.Font.Name = "微软雅黑";

                //默认设置 - 标签文字
                series.Marks.Style = GetTextStyle(m_DVM.LabelContentTypeEnum);
                series.Marks.OnTop = true;
                series.Marks.Transparent = !m_DVM.ShowLabelBorder;
                series.Marks.Color = new Color();
                series.Marks.Pen.Color = Colors.White;
                series.Marks.Arrow.Visible = false;

                //默认设置 - 是否画圆
                (series as Donut).Circled = true;

                series.Marks.AutoPosition = m_DVM.LabelOverlap;

                series.ValueFormat = "";
                series.GetSeriesMark -= SeriesGetSeriesMark;
                series.GetSeriesMark += SeriesGetSeriesMark;
            }
        }

        /// <summary>
        ///     获取文字类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MarksStyles GetTextStyle(LabelContentTypeEnum type)
        {
            if (type == LabelContentTypeEnum.Value)
            {
                return MarksStyles.Value;
            }
            if (type == LabelContentTypeEnum.LabelValue)
            {
                return MarksStyles.LabelValue;
            }
            if (type == LabelContentTypeEnum.Percent)
            {
                return MarksStyles.Percent;
            }
            if (type == LabelContentTypeEnum.LabelPercent)
            {
                return MarksStyles.LabelPercent;
            }
            return MarksStyles.Value;
        }

        /// <summary>
        ///     修改堆积柱标签
        /// </summary>
        private void SeriesGetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
            if (m_DVM.LabelContentTypeEnum == LabelContentTypeEnum.Value)
            {
                var decimalDigits = m_DVM.LabelDecimalDigits.ToString();
                var value = Convert.ToDouble(e.MarkText);
                e.MarkText = value.ToString("F" + decimalDigits);

                if (e.MarkText != "" && Convert.ToDouble(e.MarkText) == 0)
                {
                    e.MarkText = "";
                }
            }
        }

        /// <summary>
        ///     更新标记提示
        /// </summary>
        private void UpdateMarksTip()
        {
            //默认设置 - 标记提示
            var tip = tChart.Tools[0] as MarksTip;
            tip.MouseAction = MarksTipMouseAction.Move;
            tip.Style = GetTextStyle(m_DVM.LabelTypeEnum);
        }

        /// <summary>
        ///     更新其他样式
        /// </summary>
        private void UpdateOtherStyle()
        {
            foreach (Series series in tChart.Series)
            {
                //饼块儿外边框颜色
                (series as Donut).Pen.Color = (Color) ColorConverter.ConvertFromString(m_DVM.PieStroke);
                (series as Donut).AutoPenColor = false;
                //饼块儿外边框宽度
                (series as Donut).Pen.Width = m_DVM.PieStrokeThickness;
                //饼图洞的大小(取值范围0~1之间)，默认为0 0：表示饼图
                (series as Donut).DonutPercent = (int) (m_DVM.PieHoleSize*100);
            }
        }

        /// <summary>
        ///     刷新图例颜色
        /// </summary>
        private void RefreshLegendColor()
        {
            foreach (Series series in tChart.Series)
            {
                for (var i = 0; i < series.Count; i++)
                {
                    var brush = GetLegendBrush(series[i].Label) as SolidColorBrush;
                    series[i].Color = brush.Color;
                }
            }
        }

        #endregion

        #region Private Methods 数据处理

        /// <summary>
        ///     堆积图 - 基础值赋值
        /// </summary>
        private void SetStackBaseFieldValue()
        {
            m_MeasureNames.Clear();
            m_LegendName = null;

            if (m_DVM.MeasureField != null)
            {
                m_MeasureNames.Add(m_DVM.MeasureField.AsName);

                if (m_DVM.LegendField != null)
                {
                    m_LegendName = m_DVM.LegendField.AsName;
                }
            }
        }

        /// <summary>
        ///     堆积图更新
        /// </summary>
        private void StackUpdate()
        {
            foreach (var adapterDataRow in m_Controller.DT.Rows)
            {
                foreach (var MeasureName in m_MeasureNames)
                {
                    if (!string.IsNullOrEmpty(m_LegendName))
                    {
                        AddModel(adapterDataRow, MeasureName, adapterDataRow[m_LegendName].ToString());
                    }
                }
                //else
                //{
                //    AddModel(adapterDataRow, m_MeasureName, m_MeasureName);
                //}
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="measureName"></param>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private bool AddModel(AdapterDataRow dataRow, string measureName, string legendValue)
        {
            var result = false;
            try
            {
                var measureValue = Convert.ToDouble(dataRow[measureName]);
                //string dimensionValue = dataRow[m_DimensionName].ToString();

                var existPie = (from object series in tChart.Series select series as Donut)
                    .FirstOrDefault(Pie => Pie.Tag.ToString().ToLower() == measureName.ToLower());
                if (existPie != null)
                {
                    var brush = GetLegendBrush(legendValue) as SolidColorBrush;
                    existPie.Add(measureValue, legendValue, brush.Color);
                    existPie.Legend.Text = legendValue;
                    //existPie.Color = brush.Color;
                }
                else
                {
                    var Pie = new Donut {Tag = measureName};
                    tChart.Series.Add(Pie);
                    existPie = Pie;

                    var brush = GetLegendBrush(legendValue) as SolidColorBrush;
                    Pie.Add(measureValue, legendValue, brush.Color);
                    existPie.Legend.Text = legendValue;
                    //existPie.Color = brush.Color;
                }
                //existPie.MultiPie = MultiPies.Stacked;
                existPie.Marks.Style = GetTextStyle(m_DVM.LabelContentTypeEnum);
                existPie.Marks.OnTop = true;
                existPie.Repaint();
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }


        /// <summary>
        ///     根据ADT刷新图表
        /// </summary>
        /// <param name="adt"></param>
        private void UpdateByDataTable(AdapterDataTable adt)
        {
            SetStackBaseFieldValue();

            if (m_MeasureNames.Count == 0 || string.IsNullOrEmpty(m_LegendName))
            {
                return;
            }

            tChart.Series.Clear();
            StackUpdate();

            //更新标记和数据组合模式/数据展现形式
            UpdateLabel();

            //更新其他样式
            UpdateOtherStyle();
        }

        #endregion

        #region 图例颜色相关

        /// <summary>
        ///     根据图例值获取颜色值
        ///     1:1 时，legendValue为指标名称
        ///     1:1:1 时，legendValue为图例值
        ///     1:N 时，legendValue为指标名称
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private Brush GetLegendBrush(string legendValue)
        {
            var chartStyle = m_DVM.LegendStyle;

            return GetBrushHelper.GetSolidColorBrush(chartStyle, legendValue);
            //return GetBrushHelper.GetSolidColorBrush(chartStyle, legendValue, m_DVM.LegendColors);
        }

        /// <summary>
        ///     设置tchart的透明度
        /// </summary>
        private void MakeTransparent()
        {
            foreach (Series series in tChart.Series)
            {
                for (var i = 0; i < series.Count; i++)
                {
                    var color = new Color();
                    color = series[i].Color;
                    color.ScA = (float) m_DVM.SubOpacity;
                    series[i].Color = color;
                }
            }
        }

        /// <summary>
        ///     重置tchart的透明度
        /// </summary>
        private void UodoTransparent()
        {
            foreach (Series series in tChart.Series)
            {
                for (var i = 0; i < series.Count; i++)
                {
                    var brush = GetLegendBrush(series[i].Label) as SolidColorBrush;
                    series[i].Color = brush.Color;
                }
            }
        }

        /// <summary>
        ///     将联动的series清理
        /// </summary>
        private void ClearSelectedSeries()
        {
            while (tChart.Series.Count > 1)
            {
                //重置半径
                (tChart.Series[1] as Donut).AutoCircleResize = true;
                (tChart.Series[1] as Donut).CustomXRadius = 0;
                tChart.Series.RemoveAt(1);
            }

            m_SeriesToRadiusPercent.Clear();
        }

        #endregion
    }
}