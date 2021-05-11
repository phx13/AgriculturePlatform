using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Controls.GIS3D.OSG.Engine.Models;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.AVE.Playback.Models;
using Digihail.AVECLI.Controls.GIS3D.Core;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.LineStyles;
using Digihail.CCP4.Models.LauncherMessage;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Charts.GIS3D.Models;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Charts.Utils;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;

namespace CM.Track._3DTrack
{
    /// <summary>
    ///     3D节点轨迹图控制器
    /// </summary>
    public class GIS3DTrackController : GIS3DControllerBase
    {
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
        /// <param name="presentDateTime"></param>
        public GIS3DTrackController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player,
            DateTime presentDateTime)
            : base(dvm, dataProxy, player)
        {
            m_PresentDateTime = presentDateTime;
            DVM = DataViewModel as GIS3DTrackDataViewModel;
            if (DVM != null) m_Village = DVM.VillageField.AsName;
            SetBaseValue();
            ReceiveMessages();
        }

        #endregion

        private void ReceivedFocusMessage(SelectedFocusData obj)
        {
            var str = obj.SelectedInfoList[0];
            var strarr = str.Split('=');
            var name = strarr[2];
            foreach (var row in m_AdapterTable.Rows)
            {
                var key = row[m_GroupKeyName].ToString();
                if (!string.IsNullOrEmpty(key) && key.Equals(name))
                {
                    var model = m_CurrentModelDictionary[key];
                    model.IsVisible = true;
                    model.IsSelected = true;
                }
            }
        }

        private void ReceivedSelectedMessage(SelectedSettingInfoData receiveData)
        {
            if (m_AdapterTable != null)
            {
                foreach (var key in m_CurrentModelDictionary.Keys)
                {
                    if (!m_CurrentModelDictionary.ContainsKey(key)) // 异步处理保证，查看当前屏幕上是否有这个点
                    {
                        continue;
                    }

                    var model = m_CurrentModelDictionary[key]; // 取到屏幕上这个点的模型
                    model.IsVisible = false;
                    model.IsSelected = false;
                }

                var columnName = receiveData.ColumnName;
                var selectValue = receiveData.Value;
                var selectState = receiveData.OperationType;

                if (selectState == "取消选中")
                {
                    foreach (var key in m_CurrentModelDictionary.Keys)
                    {
                        if (!m_CurrentModelDictionary.ContainsKey(key)) // 异步处理保证，查看当前屏幕上是否有这个点
                        {
                            continue;
                        }
                        var model = m_CurrentModelDictionary[key]; // 取到屏幕上这个点的模型
                        model.IsVisible = true;
                    }
                }
                else
                {
                    if (columnName == "Village")
                    {
                        foreach (var row in m_AdapterTable.Rows)
                        {
                            var key = row[m_Village].ToString();
                            if (!string.IsNullOrEmpty(key) && key.Equals(selectValue))
                            {
                                var rowKey = row[m_GroupKeyName].ToString();
                                var model = m_CurrentModelDictionary[rowKey]; // 取到屏幕上这个点的模型
                                model.IsVisible = true;
                            }
                        }
                    }
                }
            }
        }

        private void Globe3DControler_Click(object sender, MouseEventArgs e)
        {
        }

        #region IDisposable

        /// <summary>
        ///     销毁
        /// </summary>
        public override void Dispose()
        {
            if (m_EngineContainer != null && m_EngineContainer.SurfaceView != null
                && m_EngineContainer.SurfaceView.Globe3DControler != null)
            {
                if (Layer != null)
                {
                    m_EngineContainer.SurfaceModel.Layers.Remove(Layer);
                }
                if (m_PointStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_PointStyleEntity3D);
                    m_PointStyleEntity3D = null;
                }
                if (m_TrailStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_TrailStyleEntity3D);
                    m_TrailStyleEntity3D = null;
                }
                if (m_TextBackgroundStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_TextBackgroundStyleEntity3D);
                    m_TextBackgroundStyleEntity3D = null;
                }
                if (m_IconStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_IconStyleEntity3D);
                    m_IconStyleEntity3D = null;
                }
                if (m_SelectedIconStyleEntity3D != null)
                {
                    m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(m_SelectedIconStyleEntity3D);
                    m_SelectedIconStyleEntity3D = null;
                }
            }

            if (m_CurrentModelDictionary != null)
            {
                m_CurrentModelDictionary.Clear();
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
        private GIS3DModelDefinition m_ModelDefinition;

        /// <summary>
        ///     经度列名称
        /// </summary>
        private string m_LongitudeName;

        /// <summary>
        ///     纬度列名称
        /// </summary>
        private string m_LatitudeName;

        /// <summary>
        ///     高度列名称
        /// </summary>
        private string m_AltName = string.Empty;

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

        private double m_PresentPlayStep;

        private bool m_IsInit;

        #endregion

        #region Propeties

        /// <summary>
        ///     当前控制器的dvm
        /// </summary>
        public GIS3DTrackDataViewModel DVM { get; private set; }

        /// <summary>
        ///     要素点图层
        /// </summary>
        public LayerModel Layer { get; private set; }

        private readonly ConcurrentDictionary<string, GIS3DPlaybackModel> m_CurrentModelDictionary =
            new ConcurrentDictionary<string, GIS3DPlaybackModel>();

        /// <summary>
        ///     当前管理的全部对象
        /// </summary>
        public ConcurrentDictionary<string, GIS3DPlaybackModel> CurrentModelDictionary
        {
            get { return m_CurrentModelDictionary; }
        }

        /// <summary>
        ///     图标材质
        /// </summary>
        private readonly Dictionary<string, DefaultStyleClass> m_IconNameToIconStyle =
            new Dictionary<string, DefaultStyleClass>();

        /// <summary>
        ///     图标材质（选中）
        /// </summary>
        private readonly Dictionary<string, DefaultStyleClass> m_IconNameToSelectedIconStyle =
            new Dictionary<string, DefaultStyleClass>();

        /// <summary>
        ///     图标材质（shinning）
        /// </summary>
        private readonly Dictionary<string, DefaultStyleClass> m_IconNameToShinningIconStyle =
            new Dictionary<string, DefaultStyleClass>();

        /// <summary>
        ///     点材质
        /// </summary>
        private DefaultBillboardMaterialStyle m_PointStyle;

        /// <summary>
        ///     点材质存储实体
        /// </summary>
        private Entity3D m_PointStyleEntity3D;

        /// <summary>
        ///     尾迹材质
        /// </summary>
        private TrailMaterialStyle m_TrailStyle;

        /// <summary>
        ///     尾迹材质存储实体
        /// </summary>
        private Entity3D m_TrailStyleEntity3D;

        /// <summary>
        ///     文字背景材质
        /// </summary>
        private DefaultBillboardMaterialStyle m_TextBackgroundStyle;

        /// <summary>
        ///     文字背景材质存储实体
        /// </summary>
        private Entity3D m_TextBackgroundStyleEntity3D;

        /// <summary>
        ///     材质的实体
        /// </summary>
        private Entity3D m_IconStyleEntity3D;

        /// <summary>
        ///     材质的实体（选中）
        /// </summary>
        private Entity3D m_SelectedIconStyleEntity3D;

        /// <summary>
        ///     材质的实体（shinning)
        /// </summary>
        private readonly Entity3D m_ShinningIconStyleEntity3D = null;

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
            InitGlobe();
            InitOtherStyle();
            UpdateByDataTable(adt);
            m_EngineContainer.SurfaceView.Globe3DControler.MouseClick += Globe3DControler_Click;
        }

        /// <summary>
        ///     刷新图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
            // 清空界面现有数据
            var toRemoveModelKeyCollection = m_CurrentModelDictionary.Keys.ToList(); // 当前所有显示的对象Id
            RemoveModelByKeys(toRemoveModelKeyCollection);
            m_CurrentModelDictionary.Clear();
            SetBaseValue();
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
            RemoveAll(false);
        }

        /// <summary>
        ///     属性变更设置
        /// </summary>
        public override void RefreshStyle()
        {
            SetGlobeStyleConfig(DVM);
            SetGlobeStyle(DVM);

            UpdateTextStyle();
            UpdateIconStyle();

            foreach (var model in m_CurrentModelDictionary.Values)
            {
                model.PostProcessThreshold = (float) DVM.PostProcessThreshold;
                model.UpdateHeightMap();
                model.UpdateGeographicCoordinateTransform();

                model.ImageWidth = m_IconNameToIconStyle[model.IconName].ImageWidth;
                model.ImageHeight = m_IconNameToIconStyle[model.IconName].ImageHeight;

                model.SelectedImageWidth = m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
                model.SelectedImageHeight = m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;

                model.IsVisible = DVM.ShowLayer;
                model.ShowLabel = DVM.ShowLabel;

                model.UpdateBillboard();
                model.UpdateTrailComponent();
            }
        }

        /// <summary>
        ///     更新样式
        /// </summary>
        /// <param name="propertyDescription"></param>
        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
            if (propertyDescription.Category == DescriptionEnum.数据设置
                && propertyDescription.SubCategory == DescriptionEnum.其他
                && propertyDescription.DisplayName == "启用贴地")
            {
                foreach (var model in m_CurrentModelDictionary.Values)
                {
                    model.UpdateHeightMap();
                }
            }
            if (propertyDescription.SubCategory == DescriptionEnum.地图样式)
            {
                SetGlobeStyleConfig(DVM);
                SetGlobeStyle(DVM);
            }
            else if (propertyDescription.SubCategory == "位置偏移")
            {
                foreach (var model in m_CurrentModelDictionary.Values)
                    model.UpdateGeographicCoordinateTransform();
            }
            else
            {
                if (propertyDescription.SubCategory == "标识")
                {
                    if (propertyDescription.DisplayName == "类别图标")
                    {
                        UpdateIconStyle();

                        foreach (var model in m_CurrentModelDictionary.Values)
                        {
                            model.ImageWidth = m_IconNameToIconStyle[model.IconName].ImageWidth;
                            model.ImageHeight = m_IconNameToIconStyle[model.IconName].ImageHeight;

                            model.SelectedImageWidth = m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
                            model.SelectedImageHeight = m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;

                            model.UpdateBillboard();
                        }
                    }
                    else if (propertyDescription.DisplayName == "后期特效过曝光度")
                    {
                        foreach (var model in m_CurrentModelDictionary.Values)
                        {
                            model.PostProcessThreshold = (float) DVM.PostProcessThreshold;
                        }
                    }
                }

                if (propertyDescription.SubCategory == "放缩显示")
                {
                    UpdateIconStyle();
                    UpdateTextStyle();
                }
                var isVisible = false;
                foreach (var model in m_CurrentModelDictionary.Values)
                {
                    if (DVM.ShowLayer && model.LegendItemChecked)
                    {
                        isVisible = true;
                    }
                    model.IsVisible = isVisible;
                    model.ShowLabel = DVM.ShowLabel;

                    model.UpdateBillboard();
                    model.UpdateTrailComponent();
                }
            }
        }

        /// <summary>
        ///     时间轴停止时
        /// </summary>
        public override void OnAVEPlayerStoped()
        {
            RemoveAll(true);
        }

        /// <summary>
        ///     设置搜索结果
        /// </summary>
        /// <param name="adt"></param>
        public override void SetSearchResult(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                // 清空搜索
                foreach (var model in m_CurrentModelDictionary.Values)
                {
                    model.IsShinning = false;
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
                        m_CurrentModelDictionary[key].IsShinning = true;
                    }
                }
            }
        }

        /// <summary>
        ///     更新dvm
        /// </summary>
        /// <param name="dvm"></param>
        public override void UpdateDataViewModel(ChartDataViewModel dvm)
        {
            DVM = dvm as GIS3DTrackDataViewModel;
            SetBaseValue();
            RefreshStyle();
        }

        /// <summary>
        ///     图表被联动时进行选中
        /// </summary>
        /// <param name="adt"></param>
        public override void SetSelectedItem(AdapterDataTable adt)
        {
            if (m_EngineContainer == null
                || DVM == null
                || adt == null
                || adt.Rows == null
                || adt.Rows.Count <= 0) return;

            var key = adt.Rows[0][DVM.GroupKeyField.AsName].ToString();
            if (m_CurrentModelDictionary.ContainsKey(key))
            {
                m_CurrentModelDictionary[key].IsSelected = true;
                m_EngineContainer.CurrentSelectedModel = m_CurrentModelDictionary[key];
                m_EngineContainer.OnPanToCommand(m_EngineContainer.CurrentSelectedModel);
            }
        }

        /// <summary>
        ///     图表取消联动时取消选中
        /// </summary>
        public override void ClearSelectedItem()
        {
            if (DVM == null
                || m_EngineContainer == null
                || m_EngineContainer.CurrentSelectedModel == null
                || !DVM.IsLinkage) return;

            m_EngineContainer.CurrentSelectedModel.IsSelected = false;
            m_EngineContainer.CurrentSelectedModel = null;
        }

        #endregion

        #region Add Update Remove

        private DateTime m_PresentDateTime;

        /// <summary>
        ///     界面进行更新
        /// </summary>
        /// <param name="adt"></param>
        private void UpdateByDataTable(AdapterDataTable adt)
        {
            var toRemoveModelKeyCollection = m_CurrentModelDictionary.Keys.ToList(); // 当前所有显示的对象Id

            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                RemoveModelByKeys(toRemoveModelKeyCollection);
                return;
            }
            if (IsPlayerJump)
            {
                RemoveModelByKeys(toRemoveModelKeyCollection);
                IsPlayerJump = false;
            }

            #region 整理数据

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

            m_PresentPlayStep = GetPlayStep();

            foreach (var pair in datas)
            {
                if (m_CurrentModelDictionary.ContainsKey(pair.Key))
                {
                    toRemoveModelKeyCollection.Remove(pair.Key);
                    SetPropertyValue(pair.Value, m_CurrentModelDictionary[pair.Key], true);
                }
                else
                {
                    AddModel(pair.Key, pair.Value);
                }
            }

            RemoveModelByKeys(toRemoveModelKeyCollection); // 删除

            if (IsPlayerJump)
            {
                IsPlayerJump = false;
            }
            m_ClearObject = false;
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
        ///     添加散点
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="row"></param>
        private void AddModel(string dataKey, AdapterDataRow row)
        {
            var model = m_EngineContainer.CreateObjectModel(m_ModelDefinition) as GIS3DPlaybackModel;
            model.DataViewModel = DVM;

            if (!string.IsNullOrEmpty(m_LabelName))
            {
                model.DisplayName = row[m_LabelName].ToString();
            }

            SetPropertyValue(row, model, false);
            try
            {
                Layer.AddObject(model);
            }
            catch (Exception ex)
            {
            }
            m_CurrentModelDictionary.AddOrUpdate(dataKey, model, (key, value) => { return value; });
        }

        /// <summary>
        ///     设置图例图标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private void SetLegendIcon(AdapterDataRow row, GIS3DPlaybackModel model)
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
                iconName = "Default3DIcon";
            }

            model.LegendName = legendName;
            model.LegendIcon = GetIconUri(iconName);
            model.IconName = iconName;
        }

        /// <summary>
        ///     获取图标路径
        /// </summary>
        /// <param name="legendValue"></param>
        /// <returns></returns>
        private string GetIconPath(string legendValue)
        {
            return GetIconUri(legendValue).LocalPath;
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
                        DVM.Icon.IconList.FirstOrDefault(item => item.IconName.ToLower() == legendValue.ToLower());
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
                ChartLogManager.PrintDebugMesage("GIS3DPlaybackController",
                    "GetIconUri",
                    string.Format("获取 {0} 图标路径出现问题", legendValue));

                ChartLogManager.WriteDadChartsError(ex);

                return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DIcon.png"));
            }
            return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DIcon.png"));
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

                if (DataViewModel.DataTimeColumn != null)
                {
                    {
                        if (DVM.EnableAutoRemoveTrack &&
                            (Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds >
                            DVM.AutoRemoveTrackDeferred)
                        {
                            RemoveModel(key, model);
                        }
                    }
                }
                else
                {
                    RemoveModel(key, model);
                }
            }
            if (IsPlayerJump)
            {
                IsPlayerJump = false;
            }
        }

        /// <summary>
        ///     删除散点
        /// </summary>
        /// <param name="dataKey"></param>
        /// <param name="model"></param>
        private void RemoveModel(string dataKey, GIS3DPlaybackModel model)
        {
            m_CurrentModelDictionary.TryRemove(dataKey, out model);
            if (m_EngineContainer.SurfaceView.Globe3DControler.World.CameraMode == GlobeWorld.eCameraMode.Follow
                && m_EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity == model.Entity3D)
            {
                m_EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity = null;
                m_EngineContainer.GlobeWorld.CameraMode = GlobeWorld.eCameraMode.Free;
            }
            Layer.RemoveObject(model);
        }

        /// <summary>
        ///     设置轨迹点的属性
        /// </summary>
        /// <param name="row"></param>
        /// <param name="model"></param>
        private void SetPropertyValue(AdapterDataRow row, GIS3DPlaybackModel model, bool isUpdate)
        {
            model.Row = row;
            model.X = Convert.ToDouble(row[m_LongitudeName]);
            model.Y = Convert.ToDouble(row[m_LatitudeName]);
            if (string.IsNullOrEmpty(m_AltName))
            {
                model.Z = 0; //GIS3DConfigurationValue.DefaultHeight;
            }
            else
            {
                model.Z = Convert.ToDouble(row[m_AltName]);
            }

            SetLegendIcon(row, model);

            InitIconStyle(model.IconName);

            if (!m_IsInit)
            {
                InitTextStyle();
                m_IsInit = true;
            }

            SetMaterialStyle(model);

            model.TrailColor = GetBrushHelper.GetColorString(DVM.TrailColor, model.LegendName); // 尾迹颜色
            model.TextKey = DataViewModel.Name;
            if (Player != null && Player.State != Enums.PlayState.Stopped)
            {
                if ((model.OccurDateTime - DateTime.MinValue).TotalSeconds > 0)
                {
                    model.Duration = (Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds;
                }
                else
                {
                    model.Duration = Player.Interval/1000.0; // 步长
                }
            }
            model.PostProcessThreshold = (float) DVM.PostProcessThreshold;
            if (!m_ClearObject)
            {
                model.UpdateTail();
            }
            else
            {
                model.ClearTail();
            }
            if (DVM.DataTimeColumn != null)
            {
                ChartLogManager.PrintDebugMesage("GIS3DPlaybackController",
                    "m_DVM.DataTimeColumn不为空 ",
                    string.Format("散点时间列的AsName为 {0} ， 值为{1} , 转换成时间为{2}", DVM.DataTimeColumn.AsName,
                        row[DVM.DataTimeColumn.AsName], Convert.ToDateTime(row[DVM.DataTimeColumn.AsName])));
                model.OccurDateTime = Convert.ToDateTime(row[DVM.DataTimeColumn.AsName]);
            }
            else
            {
                ChartLogManager.PrintDebugMesage("GIS3DPlaybackController",
                    "m_DVM.DataTimeColumn为空 ",
                    "m_DVM.DataTimeColumn为空");
                if (Player != null)
                {
                    ChartLogManager.PrintDebugMesage("GIS3DPlaybackController",
                        "m_DVM.DataTimeColumn为空 ",
                        "model.OccurDateTime使用了时间轴当前时间");
                    model.OccurDateTime = Player.CurrentAbsoluteTime;
                }
            }

            if (Player != null)
            {
                model.SpeedRatio = Player.SpeedRatio;
                model.PlayStep = GetPlayStep();
            }

            model.IsVisible = DVM.ShowLayer;
            model.ShowLabel = DVM.ShowLabel;
            var isChecked = GetLegendItemModelIsChecked(model.LegendName);
            SetObjectVisibility(DVM.ShowLayer, isChecked, new List<GIS3DPlaybackModel> {model});
        }

        /// <summary>
        ///     设置对象的可见性
        /// </summary>
        /// <param name="showLayer">当前图层是否可见</param>
        /// <param name="checkedLegend">图例项是否可见</param>
        /// <param name="models">当前图层的所有对象</param>
        private void SetObjectVisibility(bool showLayer, bool checkedLegend, IEnumerable<GIS3DPlaybackModel> models)
        {
            var isVisible = showLayer && checkedLegend;
            foreach (var pointModel in models)
            {
                pointModel.IsVisible = isVisible;
                pointModel.LegendItemChecked = checkedLegend;
            }
        }

        /// <summary>
        ///     override 清除图层
        /// </summary>
        /// <param name="isPlayerStoped">true:点击时间轴停止</param>
        private void RemoveAll(bool isPlayerStoped)
        {
            if (isPlayerStoped)
            {
                base.OnAVEPlayerStoped();

                if (DVM.IsGetFirstDataImmediate) // 清除轨迹图的尾迹
                {
                    if (DVM.EnableTrackingPoint)
                    {
                        foreach (var key in m_CurrentModelDictionary.Keys)
                        {
                            var model = m_CurrentModelDictionary[key]; // 取到屏幕上这个点的模型
                            model.ClearTail();
                        }
                    }
                    return;
                }

                foreach (var key in m_CurrentModelDictionary.Keys)
                {
                    if (!m_CurrentModelDictionary.ContainsKey(key))
                    {
                        continue;
                    }
                    var model = m_CurrentModelDictionary[key]; // 取到屏幕上这个点的模型
                    RemoveModel(key, model);
                }

                m_CurrentModelDictionary.Clear();
                //m_CurrentModelDictionary = new ConcurrentDictionary<string, GIS3DPlaybackModel>();
            }
            else
            {
                // 清空界面现有数据
                var toRemoveModelKeyCollection = m_CurrentModelDictionary.Keys.ToList(); // 当前所有显示的对象Id
                RemoveModelByKeys(toRemoveModelKeyCollection);
            }
        }

        #endregion

        #region 图例相关

        /// <summary>
        ///     创建图例
        /// </summary>
        private void CreateLegend()
        {
            if (ChartLegendModel == null)
            {
                var chartLegendModel = new ChartLegendModel();
                chartLegendModel.LegendTitle = DataViewModel.Name;

                var groupModels = Layer.ObjectModels.Cast<GIS3DPlaybackModel>().GroupBy(point => point.LegendName);
                foreach (var pointModels in groupModels)
                {
                    var legendItemModel = CreateLegendItemModel(pointModels.Key, pointModels.ToList()[0].LegendIcon);
                    legendItemModel.PropertyChanged += legendItemModel_PropertyChanged;
                    chartLegendModel.LegendItemModels.Add(legendItemModel);
                }
                ChartLegendModel = chartLegendModel;
            }
            else
            {
                var groupModels =
                    Layer.ObjectModels.Cast<GIS3DPlaybackModel>().GroupBy(layerModel => layerModel.LegendName);
                foreach (var pointModels in groupModels)
                {
                    if (ChartLegendModel.LegendItemModels.FirstOrDefault(item => item.LegendValue == pointModels.Key) ==
                        null)
                    {
                        var legendItemModel = CreateLegendItemModel(pointModels.Key, pointModels.ToList()[0].LegendIcon);
                        legendItemModel.PropertyChanged += legendItemModel_PropertyChanged;
                        ChartLegendModel.LegendItemModels.Add(legendItemModel);
                    }
                }
            }
        }

        /// <summary>
        ///     创建图例控件项
        /// </summary>
        /// <param name="legendValue"></param>
        /// <param name="iconPath"></param>
        /// <returns></returns>
        private ChartLegendItemModel CreateLegendItemModel(string legendValue, Uri iconPath)
        {
            var legendItemModel = new ChartLegendItemModel();
            legendItemModel.LegendValue = legendValue;
            legendItemModel.LegendIconPath = iconPath;
            legendItemModel.UseIcon = true;
            legendItemModel.IsMultiple = true;

            return legendItemModel;
        }

        /// <summary>
        ///     属性改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void legendItemModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                var legendItemModel = sender as ChartLegendItemModel;
                var models = Layer.ObjectModels.Cast<GIS3DPlaybackModel>()
                    .Where(item => item.LegendName == legendItemModel.LegendValue);
                SetObjectVisibility(DVM.ShowLayer, legendItemModel.IsChecked, models);
            }
        }

        #endregion

        #region Init

        /// <summary>
        ///     设置基础值
        /// </summary>
        private void SetBaseValue()
        {
            if (DVM == null || DVM.LonField == null || DVM.LatField == null || DVM.GroupKeyField == null)
            {
                if (Layer != null)
                {
                    Layer.RemoveAll();
                }
                return;
            }

            m_LongitudeName = DVM.LonField.AsName;

            m_LatitudeName = DVM.LatField.AsName;

            m_GroupKeyName = DVM.GroupKeyField.AsName;

            if (DVM.AltField != null)
            {
                m_AltName = DVM.AltField.AsName;
            }
            else
            {
                m_AltName = string.Empty;
            }

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
        private void InitGlobe()
        {
            if (EngineContainer == null) return;

            if (m_EngineContainer == null)
            {
                m_EngineContainer = (EngineContainer) EngineContainer;
            }
            if (m_EngineContainer.ModelDefinitionList != null)
            {
                m_ModelDefinition =
                    m_EngineContainer.ModelDefinitionList.FirstOrDefault(item => item.DisplayName == "节点轨迹模型");
            }
            if (Layer == null)
            {
                Layer = new LayerModel();
                Layer.Name = DataViewModel.Name;
                m_EngineContainer.SurfaceModel.Layers.Add(Layer);

                SetGlobeStyleConfig(DVM);
                SetGlobeStyle(DVM);
            }
        }

        /// <summary>
        ///     初始化Other材质
        /// </summary>
        private void InitOtherStyle()
        {
            if (m_EngineContainer == null
                || m_EngineContainer.SurfaceView == null
                || m_EngineContainer.SurfaceView.Globe3DControler == null) return;

            if (null == m_TrailStyleEntity3D)
            {
                m_TrailStyle = new TrailMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);

                m_TrailStyleEntity3D =
                    m_EngineContainer.GlobeWorld.World.AddEntity("m_TrailStyleEntity3D" + DataViewModel.Name);
                m_TrailStyleEntity3D.Visible = false;

                var trailComponent = new TrailComponent();
                trailComponent.Style = m_TrailStyle;

                m_TrailStyleEntity3D.AddComponent(trailComponent);
            }

            if (null == m_TextBackgroundStyleEntity3D)
            {
                m_TextBackgroundStyle =
                    new DefaultBillboardMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);
                m_TextBackgroundStyle.InitializeWithMaterialFile(m_EngineContainer.GlobeWorld.World.ContentManager,
                    @".\Resources\System\9GridsBillboardMaterial.xml");
                m_TextBackgroundStyle.ClipRange = (float) DVM.MaxVisibleDistance;
                m_TextBackgroundStyle.NearFactor = new Vector2((float) DVM.NearFactor);
                m_TextBackgroundStyle.FarFactor = new Vector2((float) DVM.FarFactor);
                m_TextBackgroundStyle.IsPerspective = false;
                m_TextBackgroundStyle.TexturePath = @".\Resources\System\WhiteTextBG.dds";

                m_TextBackgroundStyleEntity3D =
                    m_EngineContainer.GlobeWorld.World.AddEntity("m_TextBackgroundStyleEntity3D" + DataViewModel.Name);
                m_TextBackgroundStyleEntity3D.Visible = false;

                var billboardComponent = new BillboardComponent();
                billboardComponent.MaterialStyle = m_TextBackgroundStyle;

                m_TextBackgroundStyleEntity3D.AddComponent(billboardComponent);
            }

            if (null == m_PointStyleEntity3D)
            {
                m_PointStyle = new DefaultBillboardMaterialStyle(m_EngineContainer.GlobeWorld.World.ContentManager);
                m_PointStyle.ClipRange = 5000;
                m_PointStyle.NearFactor = new Vector2(1, 1);
                m_PointStyle.FarFactor = new Vector2(0.2f, 0.2f);
                m_PointStyle.IsPerspective = false;
                m_PointStyle.Texture =
                    m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(@".\Resources\Textures\Point.png");

                m_PointStyleEntity3D =
                    m_EngineContainer.GlobeWorld.World.AddEntity("m_PointStyleEntity3D" + DataViewModel.Name);
                m_PointStyleEntity3D.Visible = false;

                var billboardComponent = new BillboardComponent();
                billboardComponent.MaterialStyle = m_PointStyle;

                m_PointStyleEntity3D.AddComponent(billboardComponent);
            }
        }

        /// <summary>
        ///     初始化Icon材质
        /// </summary>
        private void InitIconStyle(string iconName)
        {
            if (m_IconNameToIconStyle.ContainsKey(iconName) == false)
            {
                var iconPath = GetIconPath(iconName);
                var meterialPath = @".\Resources\System\DefaultBillboardMaterial.xml";
                var defaultStyleClass = InitIconDefaultStyleClass(iconPath, meterialPath);
                m_IconNameToIconStyle.Add(iconName, defaultStyleClass);

                CreateIconEntity(m_IconStyleEntity3D,
                    "m_IconStyleEntity3D_" + iconName + DataViewModel.Name,
                    defaultStyleClass);
            }
            if (m_IconNameToSelectedIconStyle.ContainsKey(iconName) == false)
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DBg.png");
                var meterialPath = @".\Resources\System\TrailHeadMaterialWhite.xml";
                var defaultStyleClass = InitIconDefaultStyleClass(iconPath, meterialPath);
                m_IconNameToSelectedIconStyle.Add(iconName, defaultStyleClass);

                CreateIconEntity(m_SelectedIconStyleEntity3D,
                    "m_SelectedIconStyleEntity3D_" + iconName + DataViewModel.Name,
                    defaultStyleClass);
            }
            if (!m_IconNameToShinningIconStyle.ContainsKey(iconName))
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DShinningBg.png");
                var meterialPath = @".\Resources\System\TrailHeadMaterialWhite.xml";
                var defaultStyleClass = InitIconDefaultStyleClass(iconPath, meterialPath);
                m_IconNameToShinningIconStyle.Add(iconName, defaultStyleClass);

                CreateIconEntity(m_ShinningIconStyleEntity3D,
                    "m_ShinningIconStyleEntity3D_" + iconName + DataViewModel.Name,
                    defaultStyleClass);
            }
        }

        /// <summary>
        ///     初始化IconStyle
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="meterialPath"></param>
        /// <returns></returns>
        private DefaultStyleClass InitIconDefaultStyleClass(string iconPath, string meterialPath)
        {
            var defaultStyleClass = new DefaultStyleClass();
            defaultStyleClass.BillboardStyle = new DefaultBillboardMaterialStyle();
            defaultStyleClass.BillboardStyle.InitializeWithMaterialFile(
                m_EngineContainer.GlobeWorld.World.ContentManager, meterialPath);

            UpdateDefaultStyleClass(iconPath, defaultStyleClass);

            return defaultStyleClass;
        }

        /// <summary>
        ///     更新IconStyle
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="defaultStyleClass"></param>
        private void UpdateDefaultStyleClass(string iconPath, DefaultStyleClass defaultStyleClass)
        {
            defaultStyleClass.BillboardStyle.Texture =
                m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(iconPath);
            defaultStyleClass.BillboardStyle.ClipRange = (float) DVM.MaxVisibleDistance;
            defaultStyleClass.BillboardStyle.NearFactor = new Vector2((float) DVM.NearFactor);
            defaultStyleClass.BillboardStyle.FarFactor = new Vector2((float) DVM.FarFactor);
            defaultStyleClass.BillboardStyle.IsPerspective = false;

            InitIconWidthHeight(iconPath, defaultStyleClass);
        }

        /// <summary>
        ///     初始化IconEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityName"></param>
        /// <param name="defaultStyleClass"></param>
        private void CreateIconEntity(Entity3D entity, string entityName, DefaultStyleClass defaultStyleClass)
        {
            entity = m_EngineContainer.GlobeWorld.World.AddEntity(entityName);
            entity.Visible = false;

            var billboardComponent = new BillboardComponent();
            billboardComponent.MaterialStyle = defaultStyleClass.BillboardStyle;

            entity.AddComponent(billboardComponent);
        }

        /// <summary>
        ///     更新Icon材质
        /// </summary>
        private void UpdateIconStyle()
        {
            foreach (var item in m_IconNameToIconStyle)
            {
                var iconPath = GetIconPath(item.Key);
                UpdateDefaultStyleClass(iconPath, item.Value);
            }
            foreach (var item in m_IconNameToSelectedIconStyle)
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DBg.png");
                UpdateDefaultStyleClass(iconPath, item.Value);
            }
            foreach (var item in m_IconNameToShinningIconStyle)
            {
                var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DShinningBg.png");
                UpdateDefaultStyleClass(iconPath, item.Value);
            }
        }

        private void InitTextStyle()
        {
            if (m_EngineContainer != null)
            {
                m_EngineContainer.GlobeWorld.RegisterTextSystem(DataViewModel.Name,
                    @".\\Resources\\Fonts\\MSYaHei_GBK.fnt", GlobeWorld.SceneGroupNearMiddle,
                    new Vector2((float) DVM.NearFactor), new Vector2((float) DVM.FarFactor),
                    (float) DVM.MaxVisibleDistance, false, true, -200);
            }
        }

        private void UpdateTextStyle()
        {
            if (m_EngineContainer != null)
            {
                m_EngineContainer.GlobeWorld.ModifyTextSystem(DataViewModel.Name, new Vector2((float) DVM.NearFactor),
                    new Vector2((float) DVM.FarFactor), (float) DVM.MaxVisibleDistance);
            }
        }

        /// <summary>
        ///     初始化图标大小
        /// </summary>
        private void InitIconWidthHeight(string iconPath, DefaultStyleClass defaultStyleClass)
        {
            if (iconPath.StartsWith("."))
            {
                iconPath = AppDomain.CurrentDomain.BaseDirectory + iconPath.Substring(2, iconPath.Length - 2);
            }
            if (!File.Exists(iconPath))
            {
            }
            var image = Image.FromFile(iconPath);
            defaultStyleClass.ImageWidth = image.Width;
            defaultStyleClass.ImageHeight = image.Height;
        }

        /// <summary>
        ///     设置材质
        /// </summary>
        private void SetMaterialStyle(GIS3DPlaybackModel model)
        {
            model.TrailStyle = m_TrailStyle;
            model.TextBackgroundStyle = m_TextBackgroundStyle;
            model.PointStyle = m_PointStyle;

            model.IconStyle = m_IconNameToIconStyle[model.IconName].BillboardStyle;
            model.ImageWidth = m_IconNameToIconStyle[model.IconName].ImageWidth;
            model.ImageHeight = m_IconNameToIconStyle[model.IconName].ImageHeight;

            model.SelectedIconStyle = m_IconNameToSelectedIconStyle[model.IconName].BillboardStyle;
            model.SelectedImageWidth = m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
            model.SelectedImageHeight = m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;

            model.ShinningIconStyle = m_IconNameToShinningIconStyle[model.IconName].BillboardStyle;
            model.ShinningImageWidth = m_IconNameToShinningIconStyle[model.IconName].ImageWidth;
            model.ShinningImageHeight = m_IconNameToShinningIconStyle[model.IconName].ImageHeight;

            model.UpdateBillboard();
        }

        #endregion

        #region Map Events 更新对象状态 和 空间范围筛选

        /// <summary>
        ///     地球范围改变事件
        /// </summary>
        /// <param name="minLon"></param>
        /// <param name="maxLon"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLat"></param>
        public override void MapExtentChanged(double minLon, double maxLon, double minLat, double maxLat)
        {
            if (DVM != null && DVM.EnableSpatialQuery)
            {
                SetAdditionCondition(minLon, maxLon, minLat, maxLat);

                ReloadByAdditionCondition();
            }
        }

        /// <summary>
        ///     创建附加筛选条件
        /// </summary>
        /// <param name="minLon"></param>
        /// <param name="maxLon"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLat"></param>
        /// <returns></returns>
        public override AdapterConditionModel CreateAdditionConditions(double minLon, double maxLon, double minLat,
            double maxLat)
        {
            var conditions = new AdapterConditionModel();

            var lonGreater = AdapterConditionModelHelper.GetCondition(DVM.LonField, minLon,
                ConditionJudgmentTypes.GreaterThaOrEqual);
            var lonLess = AdapterConditionModelHelper.GetCondition(DVM.LonField, maxLon,
                ConditionJudgmentTypes.LessThaOrEqual);
            var latGreater = AdapterConditionModelHelper.GetCondition(DVM.LatField, minLat,
                ConditionJudgmentTypes.GreaterThaOrEqual);
            var latLess = AdapterConditionModelHelper.GetCondition(DVM.LatField, maxLat,
                ConditionJudgmentTypes.LessThaOrEqual);

            conditions.CompoundConditions.Add(lonGreater);
            conditions.CompoundConditions.Add(lonLess);
            conditions.CompoundConditions.Add(latGreater);
            conditions.CompoundConditions.Add(latLess);

            return conditions;
        }

        #endregion

        #region Refresh Style

        /// <summary>
        ///     设置后期特效曝光亮度门限
        /// </summary>
        /// <param name="value"></param>
        private void SetProcessThreshold(double value)
        {
            m_EngineContainer.PostProcessThreshold = value;
        }

        /// <summary>
        ///     图层显隐控制
        /// </summary>
        /// <param name="showLayer"></param>
        public override void SetShowLayer(bool showLayer)
        {
            //if (m_Layer == null) return;
            //m_Layer.IsVisible = showLayer;

            //foreach (GIS3DPlaybackModel pointModel in m_Layer.ObjectModels)
            //{
            //    pointModel.IsVisible = showLayer;
            //}
        }

        #endregion

        #region Message

        /// <summary>
        ///     进程间通信的消息聚合器对象
        /// </summary>
        private readonly IMessageAggregator m_MessageAggregator = new MessageAggregator();

        /// <summary>
        ///     是否进行了清空对象操作
        /// </summary>
        private bool m_ClearObject;

        /// <summary>
        ///     接收消息
        /// </summary>
        private void ReceiveMessages()
        {
            m_MessageAggregator.GetMessage<ClearObjectMessage>().Subscribe(ReceivedClearObjectMessage);
            m_MessageAggregator.GetMessage<SelectedSettingMessage>().Subscribe(ReceivedSelectedMessage);
            m_MessageAggregator.GetMessage<SelectedFocusMessage>().Subscribe(ReceivedFocusMessage);
        }

        /// <summary>
        ///     接收清空对象消息
        /// </summary>
        /// <param name="clearModel"></param>
        private void ReceivedClearObjectMessage(ClearObjectModel clearModel)
        {
            if (DVM == null
                || string.IsNullOrEmpty(DVM.Name)) return;

            //if (!clearModel.DVMIdList.Contains(m_DVM.ID.ToString())) return;

            m_ClearObject = true;

            if (DVM.EnableTrackingPoint)
            {
                if (clearModel.ClearTrail)
                {
                    foreach (var key in m_CurrentModelDictionary.Keys)
                    {
                        var model = m_CurrentModelDictionary[key];
                        model.ClearTail();
                    }
                }
                if (clearModel.ClearModel)
                {
                    // 清空界面现有数据
                    var toRemoveModelKeyCollection = m_CurrentModelDictionary.Keys.ToList(); // 当前所有显示的对象Id
                    RemoveModelByKeys(toRemoveModelKeyCollection);
                }
            }
        }

        #endregion
    }

    /// <summary>
    ///     默认材质结构（包含贴图大小）
    /// </summary>
    public class DefaultStyleClass
    {
        /// <summary>
        ///     标牌样式
        /// </summary>
        public DefaultBillboardMaterialStyle BillboardStyle;

        /// <summary>
        ///     图片高
        /// </summary>
        public double ImageHeight;

        /// <summary>
        ///     图片宽
        /// </summary>
        public double ImageWidth;

        /// <summary>
        ///     应用标牌的实例
        /// </summary>
        public Entity3D Entity3D { get; set; }
    }
}