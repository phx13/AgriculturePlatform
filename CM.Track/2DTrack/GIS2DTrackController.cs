using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Threading;
using Digihail.AVE.Controls.GIS.Engine;
using Digihail.AVE.Controls.GIS.Engine.Models;
using Digihail.AVE.Controls.Utils;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.AVE.Playback.Models;
using Digihail.CCP4.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS2D.Controllers;
using Digihail.DAD3.Charts.GIS2D.Models;
using Digihail.DAD3.Charts.GIS2D.Utils;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Utils;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Digihail.DAD3.Models.Utils;

//using Digihail.AVE.Infrastructure;

namespace CM.Track._2DTrack
{
    /// <summary>
    ///     2D节点轨迹图控制器
    /// </summary>
    public class GIS2DTrackController : GIS2DControllerBase
    {
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();
        private readonly string m_Village;
        private AdapterDataTable m_AdapterTable;
        private bool m_Flag;

        #region Constructor

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public GIS2DTrackController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = DataViewModel as GIS2DTrackDataViewModel;
            SetBaseValue();
            if (DVM != null) m_Village = DVM.VillageField.AsName;
            m_MessageAggregator.GetMessage<SelectedSettingMessage>().Subscribe(ReceivedUpdateMessage);
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        private void ReceivedUpdateMessage(SelectedSettingInfoData receiveData)
        {
            if (m_AdapterTable != null)
            {
                RemoveAll();
                var columnName = receiveData.ColumnName;
                var selectValue = receiveData.Value;
                var selectState = receiveData.OperationType;

                if (selectState == "取消选中")
                {
                    foreach (var row in m_AdapterTable.Rows)
                    {
                        var rowKey = row[m_GroupKeyName].ToString();
                        AddModel(rowKey, row);
                    }
                }
                else
                {
                    switch (columnName)
                    {
                        case "Village":
                            //在三维上面重新描画散点
                            foreach (var row in m_AdapterTable.Rows)
                            {
                                var key = row[m_Village].ToString();
                                if (!string.IsNullOrEmpty(key) && key.Equals(selectValue))
                                {
                                    var rowKey = row[m_GroupKeyName].ToString();
                                    AddModel(rowKey, row);
                                }
                            }
                            break;
                    }
                }
            }
        }

        #region 设置选中项

        /// <summary>
        ///     设置选中项
        /// </summary>
        /// <param name="adt"></param>
        private void SelectedItemQueryCallBack(AdapterDataTable adt)
        {
            if (m_EngineContainer == null
                || DVM == null
                || adt == null
                || adt.Rows == null
                || adt.Rows.Count <= 0) return;

            var key = adt.Rows[0][DVM.GroupKeyField.AsName].ToString();
            if (m_CurrentModelDictionary.ContainsKey(key))
            {
                var mpCurrentMap = m_EngineContainer.Convert4326toMap(m_CurrentModelDictionary[key].GetMapPoint());
                m_EngineContainer.SurfaceView.Map.PanTo(mpCurrentMap);

                m_EngineContainer.CurrentSelectedModel = m_CurrentModelDictionary[key];
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///     释放资源
        /// </summary>
        public override void Dispose()
        {
            if (m_EngineContainer != null
                && m_EngineContainer.SurfaceView != null
                && m_EngineContainer.SurfaceView.Map != null)
            {
                RemoveAll();
            }

            base.Dispose();
        }

        #endregion

        #region Fields

        /// <summary>
        ///     引擎容器
        /// </summary>
        protected EngineContainer m_EngineContainer;

        /// <summary>
        ///     模型定义
        /// </summary>
        private GISModelDefinition m_ModelDefinition;

        /// <summary>
        ///     经度列名称
        /// </summary>
        private string m_LongitudeName;

        /// <summary>
        ///     纬度列名称
        /// </summary>
        private string m_LatitudeName;

        /// <summary>
        ///     批号列名称
        /// </summary>
        private string m_GroupKeyName;

        /// <summary>
        ///     显示名称
        /// </summary>
        private string m_LabelName;

        /// <summary>
        ///     图例列名称
        /// </summary>
        private string m_LegendName;

        /// <summary>
        ///     当前图标的缩放比例
        /// </summary>
        private double m_IconScale = 1;

        /// <summary>
        ///     是否显示圆点
        /// </summary>
        private bool m_ShowPoint;

        /// <summary>
        ///     是否显示Label
        /// </summary>
        private bool m_ShowLabel;

        /// <summary>
        ///     添加的数量
        /// </summary>
        private int m_AddCount;

        /// <summary>
        ///     更新的数量
        /// </summary>
        private int m_UpdateCount;

        /// <summary>
        ///     删除的数量
        /// </summary>
        private int m_RemoveCount;

        /// <summary>
        ///     x方向偏移
        /// </summary>
        private double m_OffsetX;

        /// <summary>
        ///     y方向偏移
        /// </summary>
        private double m_OffsetY;

        #endregion

        #region Properties

        /// <summary>
        ///     要素点图层
        /// </summary>
        public LayerModel Layer { get; private set; }

        private readonly ConcurrentDictionary<string, GIS2DPlaybackModel> m_CurrentModelDictionary =
            new ConcurrentDictionary<string, GIS2DPlaybackModel>();

        /// <summary>
        ///     当前管理的全部对象
        /// </summary>
        public ConcurrentDictionary<string, GIS2DPlaybackModel> CurrentModelDictionary
        {
            get { return m_CurrentModelDictionary; }
        }

        /// <summary>
        ///     当前控制器的dvm
        /// </summary>
        public GIS2DTrackDataViewModel DVM { get; private set; }

        #endregion

        #region Override

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (m_Flag)
            {
                return;
            }
            m_Flag = true;

            m_AdapterTable = adt;

            InitMap();

            if (m_EngineContainer == null) return;

            UpdateByDataTable(adt);
        }

        /// <summary>
        ///     刷新图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
            // 清空界面现有数据
            RemoveAll();

            // 重置基础值
            SetBaseValue();
        }

        /// <summary>
        ///     更改dvm,将必须配置属性清空，清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
            // 清空界面现有数据
            RemoveAll();

            // 重置基础值
            SetBaseValue();
        }

        /// <summary>
        ///     属性更新设置
        /// </summary>
        public override void RefreshStyle()
        {
            SetMapStyleConfig(DVM);
            SetMaxMinLevel(DVM);
            SetMapStyle(DVM);

            if (Layer != null)
            {
                SetShowLayer(DVM.ShowLayer);
                SetEnableTracking(DVM.TrailCountType);
                SetRefreshStyle();
            }

            m_OffsetX = DVM.HorOffset;
            m_OffsetY = DVM.VerOffset;
        }

        /// <summary>
        ///     更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            if (propertyDescription.SubCategory == DescriptionEnum.地图样式)
            {
                if (propertyDescription.DisplayName == DescriptionEnum.样式配置)
                {
                    SetMapStyleConfig(DVM);
                    SetMaxMinLevel(DVM);
                    SetMapStyle(DVM);
                }
                else if (propertyDescription.DisplayName == DescriptionEnum.最低级别
                         || propertyDescription.DisplayName == DescriptionEnum.最高级别)
                {
                    SetMaxMinLevel(DVM);
                }
                else
                {
                    SetMapStyle(DVM);
                }
            }
            else
            {
                if (Layer != null)
                {
                    SetShowLayer(DVM.ShowLayer);
                    SetEnableTracking(DVM.TrailCountType);
                    SetRefreshStyle();
                }

                m_OffsetX = DVM.HorOffset;
                m_OffsetY = DVM.VerOffset;
            }
        }

        /// <summary>
        ///     图表被联动时进行选中
        /// </summary>
        /// <param name="selectedModel"></param>
        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
            if (DVM == null || !DVM.IsLinkage) return;

            if (selectedModel.LinkageGroupName.ToLower() != DVM.LinkageGroupName.ToLower()) return;

            Dispatcher.Invoke(DispatcherPriority.Send, new Action(delegate
            {
                var conditionList = new List<AdapterConditionModel>();
                conditionList.Add(selectedModel.Condition);
                conditionList.Add(Condition);
                var conditions = AdapterConditionModelHelper.GetConditions(conditionList);

                if (Player == null)
                {
                    m_DataProxy.QueryAsync("", DVM.ID.ToString(), conditions)
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
                        m_DataProxy.QueryAsync("", DVM.ID.ToString(), conditions)
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
                            DVM,
                            conditions);

                        m_DataProxy.QueryAsync("", DVM.ID.ToString(), condition)
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
        ///     图表取消联动时取消选中
        /// </summary>
        /// <param name="clearModel"></param>
        public override void ClearSelectedItemModel(ClearSelectedItemModel clearModel)
        {
            if (DVM == null
                || m_EngineContainer == null
                || m_EngineContainer.CurrentSelectedModel == null
                || !DVM.IsLinkage
                || clearModel.LinkageGroupName.ToLower() != DVM.LinkageGroupName.ToLower()) return;

            m_EngineContainer.CurrentSelectedModel.IsSelected = false;
            m_EngineContainer.CurrentSelectedModel = null;
        }

        /// <summary>
        ///     时间轴停止时
        /// </summary>
        public override void OnAVEPlayerStoped()
        {
            // 如果初始化加载了全部数据
            if (DVM.IsGetFirstDataImmediate)
            {
                // 如果没有时间字段，说明不能播放，不需要重新获取数据
                if (DVM.DataTimeColumn == null)
                {
                }
                else
                {
                    // 如果有时间字段，说明能播放
                    RemoveAll();
                    LoadInitDatas();
                }
            }
            else
            {
                // 初始化未加载全部数据，清空数据，不再次加载数据
                RemoveAll();
            }
        }

        /// <summary>
        ///     更新dvm
        /// </summary>
        /// <param name="dvm"></param>
        public override void UpdateDataViewModel(ChartDataViewModel dvm)
        {
            DVM = dvm as GIS2DTrackDataViewModel;
            SetBaseValue();
            // 更新所有样式
            RefreshStyle();
        }

        /// <summary>
        ///     时间轴播放到最后
        /// </summary>
        public override void OnTimerEndStoped()
        {
            if (DVM == null
                || Layer == null
                || Layer.ObjectModels == null) return;

            if (DVM.TrailCountType == TrailCountType.None
                || DVM.TrailCount <= 0) return;

            foreach (var model in m_CurrentModelDictionary.Values)
            {
                model.TrackingPoint.Trim(DVM.TrailCount);
            }
        }

        /// <summary>
        ///     显示搜索结果
        /// </summary>
        /// <param name="adt"></param>
        public override void SetSearchResult(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                // 清空搜索
                foreach (var model in m_CurrentModelDictionary.Values)
                {
                    model.IsShining = false;
                }
            }
            else
            {
                foreach (var row in adt.Rows)
                {
                    var key = GetAdapterDataRowKey(row);
                    if (m_CurrentModelDictionary.ContainsKey(key))
                    {
                        // 显示搜索结果
                        m_CurrentModelDictionary[key].IsShining = true;
                    }
                }
            }
        }

        #endregion

        #region Add Update Remove

        /// <summary>
        ///     界面进行更新
        /// </summary>
        /// <param name="adt"></param>
        private void UpdateByDataTable(AdapterDataTable adt)
        {
            if (IsPlayerJump)
            {
                RemoveAll();

                IsPlayerJump = false;
            }

            var toRemoveModelKeyCollection = m_CurrentModelDictionary.Keys.ToList(); // 当前所有显示的对象Id

            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                RemoveModelByKeys(toRemoveModelKeyCollection);
                return;
            }

            m_AddCount = 0;
            m_UpdateCount = 0;
            m_RemoveCount = 0;

            //AdapterDataTable adtNew = AdapterDataTableLinqHelper.GetDataTable(adt,this.TempQueryCondition);

            #region 整理数据

            // 把数据按照key进行分组
            var datas = new Dictionary<string, AdapterDataRow>();

            foreach (var row in adt.Rows)
            {
                var key = GetAdapterDataRowKey(row);
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                if (datas.ContainsKey(key))
                {
                    if (DVM.DataTimeColumn != null)
                    {
                        var dt1 = Convert.ToDateTime(datas[key][DVM.DataTimeColumn.AsName]);
                        var dt2 = Convert.ToDateTime(row[DVM.DataTimeColumn.AsName]);
                        var de = (long) (dt2 - dt1).TotalSeconds;
                        if (de > 0)
                        {
                            datas[key] = row;
                        }
                    }
                    else
                    {
                        datas[key] = row;
                    }
                }
                else
                {
                    datas.Add(key, row);
                }
            }

            #endregion

            foreach (var pair in datas) // 遍历所有需要初始化的对象
            {
                if (m_CurrentModelDictionary.ContainsKey(pair.Key))
                {
                    var model = m_CurrentModelDictionary[pair.Key];

                    var x = Convert.ToDouble(pair.Value[m_LongitudeName]) + DVM.HorOffset;
                    var y = Convert.ToDouble(pair.Value[m_LatitudeName]) + DVM.VerOffset;

                    // 在当前屏幕范围内，进行更新
                    //if (InExtent(m_NewExtent, model.X, model.Y))
                    {
                        SetPropertyValue(pair.Value, model, x, y);

                        toRemoveModelKeyCollection.Remove(pair.Key);

                        m_UpdateCount++;

                        if (DVM.TrailCountType == TrailCountType.Number)
                        {
                            if (DVM.TrailCount != 0)
                            {
                                model.SaveCurrentPonit();
                                if (model.TrackingPoint.Count > DVM.TrailCount)
                                {
                                    model.TrackingPoint.RemoveAt(0);
                                }
                                model.TrackingPoint.Trim(DVM.TrailCount);
                            }
                        }
                        if (DVM.TrailCountType == TrailCountType.All)
                        {
                            model.SaveCurrentPonit();
                        }
                    }
                }
                else
                {
                    AddModel(pair.Key, pair.Value);
                }
            }

            RemoveModelByKeys(toRemoveModelKeyCollection);

            ChartLogManager.PrintDebugMesage("GIS2DPlayBackController", "UpdateByDataTable",
                string.Format("dvmName:{0} , 更新结束 , 当前页面的节点个数 {4}, addCount: {1} updateCount: {2} removeCount: {3}",
                    DataViewModel.Name, m_AddCount, m_UpdateCount, m_RemoveCount, Layer.ObjectModels.Count));
        }

        /// <summary>
        ///     添加散点
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="row"></param>
        private void AddModel(string dataKey, AdapterDataRow row)
        {
            var x = Convert.ToDouble(row[m_LongitudeName]) + DVM.HorOffset;
            var y = Convert.ToDouble(row[m_LatitudeName]) + DVM.VerOffset;

            // 在当前屏幕范围内，进行添加
            //if (InExtent(m_NewExtent, x, y))
            {
                var model = m_EngineContainer.CreateObjectModel(m_ModelDefinition) as GIS2DPlaybackModel;

                model.DataViewModel = DVM;
                model.IconScale = m_IconScale;
                model.ShowPoint = m_ShowPoint;
                model.ShowLabel = m_ShowLabel;

                SetPropertyValue(row, model, x, y);

                Layer.AddObject(model);

                m_AddCount++;

                m_CurrentModelDictionary.AddOrUpdate(dataKey, model, (key, value) => { return value; });
            }
        }

        /// <summary>
        ///     设置轨迹点的属性
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetPropertyValue(AdapterDataRow row, GIS2DPlaybackModel model, double x, double y)
        {
            model.Row = row;

            model.X = x;
            model.Y = y;

            if (DataViewModel.DataTimeColumn != null)
            {
                model.OccurDateTime = Player.CurrentAbsoluteTime;
            }

            model.CanEditMove = false;
            if (!string.IsNullOrEmpty(m_LabelName))
            {
                model.DisplayName = row[m_LabelName].ToString();
            }
            else
            {
                model.DisplayName = "";
            }
            SetLegendIcon(row, model);

            var colorString = GetBrushHelper.GetColorString(DVM.LegendStyle, model.LegendName);
            model.TrackingPointColor = colorString.ToColor(); //Color.FromArgb(255,255,255,0); //
        }

        /// <summary>
        ///     删除散点
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="model"></param>
        private void RemoveModel(string dataKey, GIS2DPlaybackModel model)
        {
            m_CurrentModelDictionary.TryRemove(dataKey, out model);

            Layer.RemoveObject(model);
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="toRemoveModelKeyCollection"></param>
        private void RemoveModelByKeys(List<string> toRemoveModelKeyCollection)
        {
            // 自动消批逻辑
            foreach (var key in toRemoveModelKeyCollection) // -------------------------处理之前屏幕上存在，但是此次更新数据中没有这个点的操作
            {
                if (!m_CurrentModelDictionary.ContainsKey(key)) // 异步处理保证，查看当前屏幕上是否有这个点
                {
                    continue;
                }

                var model = m_CurrentModelDictionary[key]; // 取到屏幕上这个点的模型
                if (model == null)
                {
                    continue;
                }

                if (DataViewModel.DataTimeColumn != null) // 如果播放过程中退出了，则直接退出
                {
                    // 注释掉这部分存在疑问的逻辑。
                    //if (model.OccurDateTime >= this.Player.CurrentAbsoluteTime) // 删除屏幕上大于当前时刻的点
                    //{
                    //    RemoveModel(key, model);
                    //    m_RemoveCount++;
                    //}
                    //else

                    {
                        // 如果启用了消批 且最后一个点的位置发现时间超过自动消批的时间，则删除
                        if (DVM.EnableAutoRemoveTrack
                            &&
                            (Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds >
                            DVM.AutoRemoveTrackDeferred)
                        {
                            RemoveModel(key, model);
                            m_RemoveCount++;
                        }
                    }
                }
                else
                {
                    RemoveModel(key, model);
                    m_RemoveCount++;
                }
            }
        }

        /// <summary>
        ///     清除图层上所有对象
        /// </summary>
        private void RemoveAll()
        {
            if (DVM == null
                || Layer == null
                || Layer.ObjectModels == null) return;

            // 清空界面现有数据 
            RemoveAll(Layer);

            m_CurrentModelDictionary.Clear();
        }

        /// <summary>
        ///     获取数据唯一键值
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private string GetAdapterDataRowKey(AdapterDataRow dataRow)
        {
            if (dataRow == null) return "";

            var groupKeyValue = dataRow[m_GroupKeyName];
            if (groupKeyValue == null) return "";

            return groupKeyValue.ToString();
        }

        /// <summary>
        ///     设置图例图标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private void SetLegendIcon(AdapterDataRow row, GIS2DPlaybackModel model)
        {
            var legendName = "";
            var iconName = "";
            if (!string.IsNullOrEmpty(m_LegendName)) // 有图例时
            {
                if (row[m_LegendName] != null)
                {
                    legendName = row[m_LegendName].ToString().Trim();
                    iconName = legendName;
                }
            }
            else // 无图例时
            {
                legendName = DVM.GroupKeyField.ToString();
                iconName = "DefaultIcon";
            }

            var iconPath = GetIconUri(iconName);

            model.IconName = iconName;
            model.LegendName = legendName;
            model.LegendIcon = iconPath;
        }

        /// <summary>
        ///     获取图标路径
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private string GetIconPath(string legendValue)
        {
            if (!string.IsNullOrEmpty(legendValue) && DVM.Icon != null && DVM.Icon.IconList != null)
            {
                var itemModel =
                    DVM.Icon.IconList.FirstOrDefault(item => item.IconName.ToLower() == legendValue.ToLower());
                if (itemModel != null)
                {
                    var path = AppDomain.CurrentDomain.BaseDirectory + itemModel.IconPath.Substring(2);
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "DefaultIcon.png");
        }

        /// <summary>
        ///     获取图标路径
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private Uri GetIconUri(string legendValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(legendValue) && DVM.Icon != null && DVM.Icon.IconList != null)
                {
                    var itemModel =
                        DVM.Icon.IconList.FirstOrDefault(
                            item => string.Equals(item.IconName, legendValue, StringComparison.CurrentCultureIgnoreCase));
                    if (itemModel != null)
                    {
                        var fileUri = m_DataProxy.GetResourceFileUri(DataViewModel.ID, itemModel.IconPath.Substring(2));
                        var newUri = new Uri(Path.Combine(fileUri.AbsoluteUri, itemModel.IconPath.Substring(2)),
                            UriKind.Absolute);
                        return newUri;
                    }
                }
            }
            catch (Exception ex)
            {
                ChartLogManager.PrintDebugMesage("GIS2DPlaybackController",
                    "GetIconUri",
                    string.Format("获取 {0} 图标路径出现问题", legendValue));

                ChartLogManager.WriteDadChartsError(ex);

                return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "DefaultIcon.png"));
            }
            return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "DefaultIcon.png"));
        }

        /// <summary>
        ///     设置对象的可见性
        /// </summary>
        /// <param name="showLayer">当前图层是否可见</param>
        /// <param name="checkedLegend">图例项是否可见</param>
        /// <param name="models">当前图层的所有对象</param>
        public void SetObjectVisibility(bool showLayer, bool checkedLegend, IEnumerable<GIS2DPlaybackModel> models)
        {
            var isVisible = false;
            if (showLayer && checkedLegend)
            {
                isVisible = true;
            }
            foreach (var pointModel in models)
            {
                pointModel.IsVisible = isVisible;
                pointModel.LegendItemChecked = checkedLegend;
            }
        }

        #endregion

        #region Init

        /// <summary>
        ///     设置基础值
        /// </summary>
        private void SetBaseValue()
        {
            if (DVM == null) return;

            if (DVM.LonField == null || DVM.LatField == null || DVM.GroupKeyField == null) return;

            m_LongitudeName = DVM.LonField.AsName;
            m_LatitudeName = DVM.LatField.AsName;
            m_GroupKeyName = DVM.GroupKeyField.AsName;

            if (DVM.LegendField != null)
            {
                m_LegendName = DVM.LegendField.AsName;
            }
            else
            {
                m_LegendName = string.Empty;
            }

            if (DVM.LabelField != null)
            {
                m_LabelName = DVM.LabelField.AsName;
            }
            else
            {
                m_LabelName = string.Empty;
            }
        }

        /// <summary>
        ///     初始化地图控件相关内容
        /// </summary>
        private void InitMap()
        {
            if (EngineContainer == null) return;

            if (m_EngineContainer == null)
            {
                m_EngineContainer = (EngineContainer) EngineContainer;

                SetMapStyleConfig(DVM);
                SetMaxMinLevel(DVM);
                SetMapStyle(DVM);
            }

            if (m_EngineContainer.ModelDefinitionList != null && m_ModelDefinition == null)
            {
                m_ModelDefinition =
                    m_EngineContainer.ModelDefinitionList.FirstOrDefault(item => item.DisplayName == "节点轨迹模型");
            }
            if (Layer == null)
            {
                int currentLevel = m_EngineContainer.GetCurrentLevel();
                m_IconScale = ScaleHelper.CaculateScale(DVM, currentLevel);

                SetShowPointShowLabelByMapLevel(currentLevel);

                Layer = new LayerModel();
                Layer.Name = DataViewModel.Name;
                Layer.IsVisible = DVM.ShowLayer;
                m_EngineContainer.SurfaceModel.Layers.Add(Layer);
            }
        }

        #endregion

        #region Map Events 更新对象状态 和 空间范围筛选

        /// <summary>
        ///     xMin
        /// </summary>
        private double m_XMin;

        /// <summary>
        ///     yMin
        /// </summary>
        private double m_YMin;

        /// <summary>
        ///     xMax
        /// </summary>
        private double m_XMax;

        /// <summary>
        ///     yMax
        /// </summary>
        private double m_YMax;

        /// <summary>
        ///     地图范围改变
        /// </summary>
        /// <param name="level"></param>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMax"></param>
        public override void MapExtentChanged(int level, double xMin, double yMin, double xMax, double yMax)
        {
            m_XMin = xMin;
            m_YMin = yMin;
            m_XMax = xMax;
            m_YMax = yMax;

            if (DVM != null && DVM.EnableSpatialQuery)
            {
                SetAdditionCondition(xMin, yMin, xMax, yMax);

                ReloadByAdditionCondition();
            }

            SetScaleByMapLevel(level);
            SetModelShowPointShowLabelByMapLevel(level);
        }

        /// <summary>
        ///     地图范围改变事件
        /// </summary>
        /// <param name="level"></param>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMax"></param>
        public override void MapExtentChanging(int level, double xMin, double yMin, double xMax, double yMax)
        {
            m_XMin = xMin;
            m_YMin = yMin;
            m_XMax = xMax;
            m_YMax = yMax;

            SetScaleByMapLevel(level);

            SetModelShowPointShowLabelByMapLevel(level);
        }

        /// <summary>
        ///     创建附加筛选条件
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMax"></param>
        /// <returns></returns>
        public override AdapterConditionModel CreateAdditionConditions(double xMin, double yMin, double xMax,
            double yMax)
        {
            return CreatePointAdditionConditions(DVM.LonField, DVM.LatField, xMin, yMin, xMax, yMax);
        }

        #endregion

        #region Refresh Style

        /// <summary>
        ///     图层显隐控制
        /// </summary>
        /// <param name="showLayer"></param>
        public override void SetShowLayer(bool showLayer)
        {
            if (Layer == null) return;
            Layer.IsVisible = showLayer;

            //foreach (GIS2DPlaybackModel pointModel in m_Layer.ObjectModels)
            //{
            //    pointModel.IsVisible = showLayer;
            //}
        }

        /// <summary>
        ///     model中属性更新
        /// </summary>
        private void SetRefreshStyle()
        {
            if (Layer == null) return;

            foreach (var objectModel in Layer.ObjectModels)
            {
                var model = (GIS2DPlaybackModel) objectModel;
                UpdateLegendIcon(model);

                model.DataViewModel = DVM;

                model.X = model.X - m_OffsetX + DVM.HorOffset; // 坐标偏移
                model.Y = model.Y - m_OffsetY + DVM.VerOffset;
            }
        }

        /// <summary>
        ///     更新图标
        /// </summary>
        /// <param name="model"></param>
        private void UpdateLegendIcon(GIS2DPlaybackModel model)
        {
            var iconPath = GetIconUri(model.IconName);
            model.LegendIcon = iconPath;
        }

        /// <summary>
        ///     设置是否启用自动历史尾迹
        /// </summary>
        /// <param name="trailCountType"></param>
        private void SetEnableTracking(TrailCountType trailCountType)
        {
            if (Layer == null) return;
            foreach (var objectModel in Layer.ObjectModels)
            {
                var pointModel = (GIS2DPlaybackModel) objectModel;
                if (trailCountType == TrailCountType.None)
                {
                    pointModel.TrackingPoint.ClearUsingRemove(); // 自动尾迹功能关闭，清除所有尾迹点
                }
            }
        }

        /// <summary>
        ///     根据当前地图级别，设置图标的缩放值
        /// </summary>
        /// <param name="currentLevel"></param>
        private void SetScaleByMapLevel(int currentLevel)
        {
            if (!DVM.UseScale) return;

            var scale = ScaleHelper.CaculateScale(DVM, currentLevel);

            m_IconScale = scale;

            foreach (var model in m_CurrentModelDictionary.Values)
            {
                model.IconScale = scale;
            }
        }

        /// <summary>
        ///     根据当前地图级别，设置 是否显示圆点 和 设置是否显示Label
        /// </summary>
        /// <param name="currentLevel"></param>
        private void SetShowPointShowLabelByMapLevel(int currentLevel)
        {
            if (currentLevel < DVM.ShowPointLevel)
            {
                m_ShowPoint = true; // 地图级别小于配置的点大小  remove image ; add point
            }
            else
            {
                m_ShowPoint = false;
            }

            if (currentLevel < DVM.ShowLabelLevel)
            {
                m_ShowLabel = true; // 地图级别小于标签显示的大小   remove label
            }
            else
            {
                m_ShowLabel = false; // add 
            }
        }

        /// <summary>
        ///     根据当前地图级别，设置模型 是否显示圆点 和 设置是否显示Label
        /// </summary>
        /// <param name="currentLevel"></param>
        private void SetModelShowPointShowLabelByMapLevel(int currentLevel)
        {
            SetShowPointShowLabelByMapLevel(currentLevel);

            foreach (var model in m_CurrentModelDictionary.Values)
            {
                model.ShowPoint = m_ShowPoint;
                model.ShowLabel = m_ShowLabel;
            }
        }

        #endregion
    }
}