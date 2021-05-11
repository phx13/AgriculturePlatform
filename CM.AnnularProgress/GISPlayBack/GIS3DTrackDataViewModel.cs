using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.LineStyles;
using Digihail.DAD3.Charts.GIS3D.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.DataViewModels.GIS3D;
using Digihail.DAD3.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.Reflection;

namespace CM.AnnularProgress.GISPlayBack
{

    /// <summary>
	/// 3D节点轨迹图DVM
	/// </summary>
	// Token: 0x0200051A RID: 1306
	[Serializable]
	public class GIS3DTrackDataViewModel : ChartDataViewModel, IGIS3DDataViewModel, IShowLayer, IIcon, IGlobeStyle, ILegendColor, ILonLat, I3DSpatialQuery
	{
        #region 数据设置 - 数据设置

        private DimensionColumnModel m_LonField;
        /// <summary>
        /// 数据设置 - 数据设置 - 经度
        /// </summary>
        [Synchronous]
        [PropertyDescription("经度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public DimensionColumnModel LonField
        {
            get { return m_LonField; }
            set
            {
                m_LonField = value;
                RaisePropertyChanged(() => this.LonField);
            }
        }

        private DimensionColumnModel m_LatField;
        /// <summary>
        /// 数据设置 - 数据设置 - 纬度
        /// </summary>
        [Synchronous]
        [PropertyDescription("纬度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public DimensionColumnModel LatField
        {
            get { return m_LatField; }
            set
            {
                m_LatField = value;
                RaisePropertyChanged(() => this.LatField);
            }
        }

        private DimensionColumnModel m_AltField;
        /// <summary>
        /// 数据设置 - 数据设置 - 高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("高度字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = false, RefreshChartData = true)]
        public DimensionColumnModel AltField
        {
            get { return m_AltField; }
            set
            {
                m_AltField = value;
                RaisePropertyChanged(() => this.AltField);
            }
        }

        private DimensionColumnModel m_GroupKeyField;
        /// <summary>
        /// 数据设置 - 数据设置 - 批号
        /// </summary>
        [Synchronous]
        [PropertyDescription("批号字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public DimensionColumnModel GroupKeyField
        {
            get { return m_GroupKeyField; }
            set
            {
                m_GroupKeyField = value;
                RaisePropertyChanged(() => this.GroupKeyField);
            }
        }

        private LegendColumnModel m_LegendField;
        /// <summary>
        /// 数据设置 - 数据设置 - 图例
        /// </summary>
        [Synchronous]
        [PropertyDescription("图例字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, RefreshChartData = true)]
        public LegendColumnModel LegendField
        {
            get { return m_LegendField; }
            set
            {
                m_LegendField = value;
                RaisePropertyChanged(() => this.LegendField);
            }
        }

        private DimensionColumnModel m_LabelField;
        /// <summary>
        /// 数据设置 - 数据设置 - 标签字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置, PropertyType = EditorType.Field, RefreshChartData = true)]
        public DimensionColumnModel LabelField
        {
            get { return m_LabelField; }
            set
            {
                m_LabelField = value;
                RaisePropertyChanged(() => this.LabelField);
            }
        }

        #endregion

        #region 数据设置 - 其他

        private bool m_ShowLayer = true;
        /// <summary>
        /// 数据设置 - 其他 - 显示图层
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.显示图层, Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        public bool ShowLayer
        {
            get { return m_ShowLayer; }
            set
            {
                m_ShowLayer = value;
                RaisePropertyChanged(() => this.ShowLayer);
            }
        }

        private string m_LayerGroupName = "";
        /// <summary>
        /// 数据设置 - 其他 - 图层组名称
        /// </summary>
        [Synchronous]
        [PropertyDescription("图层组名称", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        public string LayerGroupName
        {
            get { return m_LayerGroupName; }
            set
            {
                m_LayerGroupName = value;
                RaisePropertyChanged(() => this.LayerGroupName);
            }
        }

        private bool m_EnableHeightMap;
        /// <summary>
        /// 数据设置 - 其他 - 启用贴地
        /// </summary>
        [Synchronous]
        [PropertyDescription("启用贴地", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        public bool EnableHeightMap
        {
            get { return m_EnableHeightMap; }
            set
            {
                m_EnableHeightMap = value;
                RaisePropertyChanged(() => this.EnableHeightMap);
            }
        }

        private bool m_EnableSpatialQuery = false;
        /// <summary>
        /// 数据设置 - 其他 - 开启空间查询
        /// </summary>
        [Synchronous]
        [PropertyDescription("开启空间查询", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他)]
        public bool EnableSpatialQuery
        {
            get { return m_EnableSpatialQuery; }
            set
            {
                m_EnableSpatialQuery = value;
                RaisePropertyChanged(() => this.EnableSpatialQuery);
            }
        }

        private double m_SpatialQueryRatio = 1.0;
        /// <summary>
        /// 空间范围查询比例
        /// </summary>
        [Synchronous]
        [PropertyDescription("空间范围查询比例", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.其他, MinValue = 0.01, MaxValue = 1000, DefaultValue = 1.0)]
        public double SpatialQueryRatio
        {
            get { return m_SpatialQueryRatio; }
            set
            {
                m_SpatialQueryRatio = value;
                this.RaisePropertyChanged(() => this.SpatialQueryRatio);
            }
        }

        #endregion

        #region 样式设置 - 标识

        private ChartIconModel m_Icon = new ChartIconModel();
        /// <summary>
        /// 样式设置 - 标识 - 类别图标
        /// </summary>
        [Synchronous]
        [PropertyDescription("类别图标", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识, PropertyType = EditorType.Icon)]
        public ChartIconModel Icon
        {
            get { return m_Icon; }
            set
            {
                m_Icon = value;
                RaisePropertyChanged(() => this.Icon);
            }
        }

        private ChartStyleModel m_LegendStyle = new ChartStyleModel();
        /// <summary>
        /// 样式设置 - 标识 - 枚举颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("散点颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识, PropertyType = EditorType.ColorCollection)]
        public ChartStyleModel LegendStyle
        {
            get { return m_LegendStyle; }
            set
            {
                m_LegendStyle = value;
                RaisePropertyChanged(() => LegendStyle);
            }
        }

        private double m_PointSize = 10;
        /// <summary>
        /// 样式设置 - 标识 - 点大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("点大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识)]
        public double PointSize
        {
            get { return m_PointSize; }
            set
            {
                m_PointSize = value;
                RaisePropertyChanged(() => PointSize);
            }
        }

        private double m_IconTransparency = 1;
        /// <summary>
        /// 样式设置 - 标识 - 标牌透明度
        /// </summary>
        [Synchronous]
        [PropertyDescription("标牌透明度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识, MinValue = 0, MaxValue = 1, DefaultValue = 1)]
        public double IconTransparency
        {
            get { return m_IconTransparency; }
            set
            {
                m_IconTransparency = value;
                RaisePropertyChanged(() => IconTransparency);
            }
        }

        private double m_PostProcessThreshold = 1;
        /// <summary>
        /// 样式设置 - 标识 - 后期特效过曝光度
        /// </summary>
        [Synchronous]
        [PropertyDescription("后期特效过曝光度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识, MinValue = 1, MaxValue = 10, DefaultValue = 1)]
        public double PostProcessThreshold
        {
            get { return m_PostProcessThreshold; }
            set
            {
                m_PostProcessThreshold = value;
                RaisePropertyChanged(() => PostProcessThreshold);
            }
        }

        private bool m_IsFlickingWhenAdd;
        /// <summary>
        /// 样式设置 - 标识 - 添加时是否闪烁
        /// </summary>
        [Synchronous]
        [PropertyDescription("添加时是否闪烁", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识)]
        public bool IsFlickingWhenAdd
        {
            get { return m_IsFlickingWhenAdd; }
            set
            {
                m_IsFlickingWhenAdd = value;
                RaisePropertyChanged(() => IsFlickingWhenAdd);
            }
        }

        private int m_FlickingCount = 2;
        /// <summary>
        /// 样式设置 - 标识 - 闪烁次数
        /// </summary>
        [Synchronous]
        [PropertyDescription("闪烁次数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识, MinValue = 1, MaxValue = 100000000, DefaultValue = 2)]
        public int FlickingCount
        {
            get { return m_FlickingCount; }
            set
            {
                m_FlickingCount = value;
                RaisePropertyChanged(() => FlickingCount);
            }
        }

        private int m_FlickingInterval = 25;
        /// <summary>
        /// 样式设置 - 标识 - 闪烁间隔(帧数)
        /// </summary>
        [Synchronous]
        [PropertyDescription("闪烁间隔(帧数)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识, MinValue = 1, MaxValue = 1000, DefaultValue = 25)]
        public int FlickingInterval
        {
            get { return m_FlickingInterval; }
            set
            {
                m_FlickingInterval = value;
                RaisePropertyChanged(() => FlickingInterval);
            }
        }

        private bool m_IsRotateByPath = false;
        /// <summary>
        /// 样式设置 - 标识 - 是否根据路径旋转
        /// </summary>
        [Synchronous]
        [PropertyDescription("是否根据路径旋转", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标识)]
        public bool IsRotateByPath
        {
            get { return m_IsRotateByPath; }
            set
            {
                m_IsRotateByPath = value;
                RaisePropertyChanged(() => IsRotateByPath);
            }
        }

        #endregion

        #region 样式设置 - 放缩显示

        private double m_PointMapMaxHeight = 100000000;
        /// <summary>
        /// 样式设置 - 放缩显示 - 点 地图最大高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("点地图最大高度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示)]
        public double PointMapMaxHeight
        {
            get
            {
                return m_PointMapMaxHeight;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                //if (value < m_PointMapHeight)
                //{
                //    value = m_PointMapHeight;
                //}
                m_PointMapMaxHeight = value;
                RaisePropertyChanged(() => PointMapMaxHeight);
            }
        }

        //private double m_ClipRange = 100000;
        ///// <summary>
        ///// 模型标牌ClipRange
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("尺寸变化距离范围", Category = DescriptionEnum.样式设置, SubCategory = "放缩显示")]
        //public double ClipRange
        //{
        //    get
        //    {
        //        return m_ClipRange;
        //    }
        //    set
        //    {
        //        if (value < 0)
        //        {
        //            value = 0;
        //        }
        //        m_ClipRange = value;
        //        RaisePropertyChanged(() => ClipRange);
        //    }
        //}

        private double m_FarFactor = 0.5;
        /// <summary>
        /// 模型标牌FarFactor
        /// </summary>
        [Synchronous]
        [PropertyDescription("1级图标比例", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示, MinValue = 0, MaxValue = 10, DefaultValue = 0.5)]
        public double FarFactor
        {
            get
            {
                return m_FarFactor;
            }
            set
            {
                m_FarFactor = value;
                RaisePropertyChanged(() => FarFactor);
            }
        }

        private double m_NearFactor = 1;
        /// <summary>
        /// 模型标牌NearFactor
        /// </summary>
        [Synchronous]
        [PropertyDescription("2级图标比例", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示, MinValue = 0, MaxValue = 10, DefaultValue = 1)]
        public double NearFactor
        {
            get
            {
                return m_NearFactor;
            }
            set
            {
                m_NearFactor = value;
                RaisePropertyChanged(() => NearFactor);
            }
        }

        private double m_MinVisibleDistance = 0;
        /// <summary>
        /// 样式设置 - 放缩显示 - 2级图标高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("2级图标高度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示)]
        public double MinVisibleDistance
        {
            get { return m_MinVisibleDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                //if (value > m_MaxVisibleDistance)
                //{
                //    value = m_MaxVisibleDistance;
                //}
                m_MinVisibleDistance = value;
                RaisePropertyChanged(() => this.MinVisibleDistance);
            }
        }

        private double m_PointMapHeight = 1000000;
        /// <summary>
        /// 样式设置 - 放缩显示 - 点 地图高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("点地图高度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示)]
        public double PointMapHeight
        {
            get
            {
                return m_PointMapHeight;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                //if (value < m_MaxVisibleDistance)
                //{
                //    value = m_MaxVisibleDistance;
                //}
                if (value > m_PointMapMaxHeight)
                {
                    value = m_PointMapMaxHeight;
                }
                m_PointMapHeight = value;
                RaisePropertyChanged(() => PointMapHeight);
            }
        }

        private double m_MaxVisibleDistance = 100000;
        /// <summary>
        /// 样式设置 - 放缩显示 - 1级图标高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("1级图标高度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.放缩显示)]
        public double MaxVisibleDistance
        {
            get { return m_MaxVisibleDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value < m_MinVisibleDistance)
                {
                    value = m_MinVisibleDistance;
                }
                //if (value > m_PointMapHeight)
                //{
                //    value = m_PointMapHeight;
                //}
                m_MaxVisibleDistance = value;
                RaisePropertyChanged(() => this.MaxVisibleDistance);
            }
        }

        #endregion

        #region 样式设置 - 位置偏移

        private double m_EastWestOffset = 0;
        /// <summary>
        /// 样式设置 - 位置偏移 - 东西偏移(m)
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.东西偏移米, Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.位置偏移)]
        public double EastWestOffset
        {
            get
            {
                return m_EastWestOffset;
            }
            set
            {
                m_EastWestOffset = value;
                RaisePropertyChanged(() => EastWestOffset);
            }
        }

        private double m_NorthSouthOffset = 0;
        /// <summary>
        /// 样式设置 - 位置偏移 - 南北偏移(m)
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.南北偏移米, Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.位置偏移)]
        public double NorthSouthOffset
        {
            get
            {
                return m_NorthSouthOffset;
            }
            set
            {
                m_NorthSouthOffset = value;
                RaisePropertyChanged(() => NorthSouthOffset);
            }
        }

        private double m_UpDownOffset = 0;
        /// <summary>
        /// 样式设置 - 位置偏移 - 上下偏移(m)
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.上下偏移米, Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.位置偏移)]
        public double UpDownOffset
        {
            get
            {
                return m_UpDownOffset;
            }
            set
            {
                m_UpDownOffset = value;
                RaisePropertyChanged(() => UpDownOffset);
            }
        }

        #endregion

        #region 样式设置 - 历史轨迹

        private bool m_EnableTrackingPoint = true;
        /// <summary>
        /// 样式设置 - 尾迹 - 显示轨迹
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示轨迹", Category = DescriptionEnum.样式设置, SubCategory = "历史轨迹")]
        public bool EnableTrackingPoint
        {
            get { return m_EnableTrackingPoint; }
            set
            {
                m_EnableTrackingPoint = value;
                RaisePropertyChanged(() => EnableTrackingPoint);
            }
        }

        private double m_AltitudeOffset = 0;
        /// <summary>
        /// 样式设置 - 尾迹 - 显示轨迹
        /// </summary>
        [Synchronous]
        [PropertyDescription("高度偏移(m)", Category = DescriptionEnum.样式设置, SubCategory = "位置偏移")]
        public double AltitudeOffset
        {
            get { return m_AltitudeOffset; }
            set
            {
                m_AltitudeOffset = value;
                RaisePropertyChanged(() => AltitudeOffset);
            }
        }


        private bool m_IsOverlapAltitude = true;
        /// <summary>
        /// 样式设置 - 尾迹 - 位置偏移
        /// </summary>
        [Synchronous]
        [PropertyDescription("是否叠加高度", Category = DescriptionEnum.样式设置, SubCategory = "位置偏移")]
        public bool IsOverlapAltitude
        {
            get { return m_IsOverlapAltitude; }
            set
            {
                m_IsOverlapAltitude = value;
                RaisePropertyChanged(() => IsOverlapAltitude);
            }
        }

        private bool m_IsOverlapShine = true;
        /// <summary>
        /// 样式设置 - 尾迹 - 位置偏移
        /// </summary>
        [Synchronous]
        [PropertyDescription("是否叠加高度", Category = DescriptionEnum.样式设置, SubCategory = "位置偏移")]
        public bool IsOverlapShine
        {
            get { return m_IsOverlapShine; }
            set
            {
                m_IsOverlapShine = value;
                RaisePropertyChanged(() => IsOverlapShine);
            }
        }

        private int m_TrailDuration = 5;
        /// <summary>
        /// 样式设置 - 尾迹 - 保留多长时间的尾迹 
        /// 单位步长 默认为5
        /// </summary>
        [Synchronous]
        [PropertyDescription("轨迹时长(几个步长)", Category = DescriptionEnum.样式设置, SubCategory = "历史轨迹")]
        public int TrailDuration
        {
            get
            {
                return m_TrailDuration;
            }
            set
            {
                m_TrailDuration = value;
                RaisePropertyChanged(() => TrailDuration);
            }
        }

        private TrailType m_TrailType = TrailType.Path;
        /// <summary>
        /// 尾迹形式
        /// </summary>
        [Synchronous]
        public TrailType TrailType
        {
            get { return m_TrailType; }
            set
            {
                m_TrailType = value;
                RaisePropertyChanged(() => TrailType);
            }
        }

        private ChartStyleModel m_TrailColor = new ChartStyleModel();
        /// <summary>
        /// 样式设置 - 历史轨迹 - 尾迹颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("尾迹颜色", Category = DescriptionEnum.样式设置, SubCategory = "历史轨迹", PropertyType = EditorType.ColorCollection)]
        public ChartStyleModel TrailColor
        {
            get { return m_TrailColor; }
            set
            {
                m_TrailColor = value;
                RaisePropertyChanged(() => TrailColor);
            }
        }

        private double m_TrailWidth = 160;
        /// <summary>
        /// 样式设置 - 历史轨迹 - 条带宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription("尾迹宽度", Category = DescriptionEnum.样式设置, SubCategory = "历史轨迹")]
        public double TrailWidth
        {
            get { return m_TrailWidth; }
            set
            {
                m_TrailWidth = value;
                RaisePropertyChanged(() => TrailWidth);
            }
        }

        private bool m_EnableAutoRemoveTrack = true;
        /// <summary>
        /// 样式设置 - 历史轨迹 - 是否启用自动消批
        /// </summary>
        [Synchronous]
        [PropertyDescription("自动消批", Category = DescriptionEnum.样式设置, SubCategory = "历史轨迹")]
        public bool EnableAutoRemoveTrack
        {
            get { return m_EnableAutoRemoveTrack; }
            set
            {
                m_EnableAutoRemoveTrack = value;
                RaisePropertyChanged(() => EnableAutoRemoveTrack);
            }
        }

        private int m_AutoRemoveTrackDeferred = 300;
        /// <summary>
        /// 样式设置 - 历史轨迹 - 自动消批延时时间，单位：秒
        /// </summary>
        [Synchronous]
        [PropertyDescription("自动消批时间(s)", Category = DescriptionEnum.样式设置, SubCategory = "历史轨迹")]
        public int AutoRemoveTrackDeferred
        {
            get { return m_AutoRemoveTrackDeferred; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                m_AutoRemoveTrackDeferred = value;
                RaisePropertyChanged(() => AutoRemoveTrackDeferred);
            }
        }

        #endregion

        #region 样式设置 - 标签文字

        private bool m_ShowLabel = false;
        /// <summary>
        /// 数据设置 - 标签文字 - 显示标签
        /// </summary>
        [Synchronous]
        [PropertyDescription("显示标签", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool ShowLabel
        {
            get { return m_ShowLabel; }
            set
            {
                m_ShowLabel = value;
                RaisePropertyChanged(() => this.ShowLabel);
            }
        }

        private string m_LabelForeground = "#FFFFFFFF";
        /// <summary>
        /// 样式设置 - 标签文字 - 文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, PropertyType = EditorType.Color)]
        public string LabelForeground
        {
            get { return m_LabelForeground; }
            set
            {
                m_LabelForeground = value;
                RaisePropertyChanged(() => LabelForeground);
            }
        }

        private string m_LabelBackground = "Blue";
        /// <summary>
        /// 样式设置 - 标签文字 - 文字背景颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字背景颜色", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, PropertyType = EditorType.Color)]
        public string LabelBackground
        {
            get { return m_LabelBackground; }
            set
            {
                m_LabelBackground = value;
                RaisePropertyChanged(() => LabelBackground);
            }
        }

        private int m_LabelFontSize = 12;
        /// <summary>
        /// 样式设置 - 标签文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字大小", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public int LabelFontSize
        {
            get { return m_LabelFontSize; }
            set
            {
                m_LabelFontSize = value;
                RaisePropertyChanged(() => LabelFontSize);
            }
        }

        private string m_LabelFontFamily = "微软雅黑";
        /// <summary>
        /// 样式设置 - 标签文字 - 文字字体
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字字体", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, PropertyType = EditorType.FontFamily)]
        public string LabelFontFamily
        {
            get { return m_LabelFontFamily; }
            set
            {
                m_LabelFontFamily = value;
                RaisePropertyChanged(() => LabelFontFamily);
            }
        }

        private int m_DecimalDigits = -1;
        /// <summary>
        /// 样式设置 - 标签文字 — 小数位数
        /// </summary>
        [Synchronous]
        [PropertyDescription("小数位数", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字, MinValue = -1, MaxValue = 10, DefaultValue = -1)]
        public int DecimalDigits
        {
            get { return m_DecimalDigits; }
            set
            {
                m_DecimalDigits = value;
                RaisePropertyChanged(() => DecimalDigits);
            }
        }

        private double m_LabelHorOffset;
        /// <summary>
        /// 样式设置 - 标签文字 - 水平偏移
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签水平偏移", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public double LabelHorOffset
        {
            get { return m_LabelHorOffset; }
            set
            {
                m_LabelHorOffset = value;
                RaisePropertyChanged(() => LabelHorOffset);
            }
        }

        private double m_LabelVerOffset;
        /// <summary>
        /// 样式设置 - 标签文字 - 垂直偏移
        /// </summary>
        [Synchronous]
        [PropertyDescription("标签垂直偏移", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public double LabelVerOffset
        {
            get { return m_LabelVerOffset; }
            set
            {
                m_LabelVerOffset = value;
                RaisePropertyChanged(() => LabelVerOffset);
            }
        }

        private bool m_LabelAutoHiddenByDistance = true;
        /// <summary>
        /// 标签是否根据距离自动隐藏，默认为true
        /// </summary>
        [Synchronous]
        [PropertyDescription("根据距离自动隐藏", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.标签文字)]
        public bool LabelAutoHiddenByDistance
        {
            get { return m_LabelAutoHiddenByDistance; }
            set
            {
                m_LabelAutoHiddenByDistance = value;
                RaisePropertyChanged(() => LabelAutoHiddenByDistance);
            }
        }

        #endregion

        #region 样式设置 - 地图样式

        private string m_GlobeConfigPath = "";
        /// <summary>
        /// 样式设置 - 地图样式 - 样式配置
        /// </summary>
        [Synchronous]
        [PropertyDescription(DescriptionEnum.样式配置, Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, PropertyType = EditorType.GCE)]
        public string GlobeConfigPath
        {
            get { return m_GlobeConfigPath; }
            set
            {
                m_GlobeConfigPath = value;
                RaisePropertyChanged(() => this.GlobeConfigPath);
            }
        }

        private double m_InitLon = 116.23;
        /// <summary>
        /// 样式设置 - 地图样式 - 起始视点经度
        /// </summary>
        [Synchronous]
        [PropertyDescription("起始视点经度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = -180, MaxValue = 180, DefaultValue = 116.23)]
        public double InitLon
        {
            get { return m_InitLon; }
            set
            {
                m_InitLon = value;
                RaisePropertyChanged(() => this.InitLon);
            }
        }

        private double m_InitLat = 39.54;
        /// <summary>
        /// 样式设置 - 地图样式 - 起始视点纬度
        /// </summary>
        [Synchronous]
        [PropertyDescription("起始视点纬度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = -85, MaxValue = 85, DefaultValue = 39.54)]
        public double InitLat
        {
            get { return m_InitLat; }
            set
            {
                m_InitLat = value;
                RaisePropertyChanged(() => this.InitLat);
            }
        }

        private double m_InitAlt = 30000000;
        /// <summary>
        /// 样式设置 - 地图样式 - 起始视点距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("起始视点距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = 10, MaxValue = 90000000, DefaultValue = 30000000)]
        public double InitAlt
        {
            get { return m_InitAlt; }
            set
            {
                m_InitAlt = value;
                RaisePropertyChanged(() => this.InitAlt);
            }
        }

        private double m_SurroundOffset = 0;
        /// <summary>
        /// 样式设置 - 地图样式 - 起始水平旋转角度
        /// </summary>
        [Synchronous]
        [PropertyDescription("摄像机初始水平角度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = 0, MaxValue = 360, DefaultValue = 0)]
        public double SurroundOffset
        {
            get { return m_SurroundOffset; }
            set
            {
                m_SurroundOffset = value;
                this.RaisePropertyChanged(() => this.SurroundOffset);
            }
        }

        private double m_PitchOffset = 0;
        /// <summary>
        /// 样式设置 - 地图样式 - 摄像机初始垂直角度
        /// </summary>
        [Synchronous]
        [PropertyDescription("摄像机初始垂直角度", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = -90, MaxValue = 0, DefaultValue = 0)]
        public double PitchOffset
        {
            get { return m_PitchOffset; }
            set
            {
                m_PitchOffset = value;
                this.RaisePropertyChanged(() => this.PitchOffset);
            }
        }

        private double m_NearDistance = 0;
        /// <summary>
        /// 样式设置 - 地图样式 - 最小视点距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最小视点距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式)]
        public double NearDistance
        {
            get { return m_NearDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value > m_FarDistance)
                {
                    value = m_FarDistance;
                }
                m_NearDistance = value;
                RaisePropertyChanged(() => this.NearDistance);
            }
        }

        private double m_FarDistance = 600000000;
        /// <summary>
        /// 样式设置 - 地图样式 - 最大视点距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("最大视点距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式)]
        public double FarDistance
        {
            get { return m_FarDistance; }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                if (value < m_NearDistance)
                {
                    value = m_NearDistance;
                }
                m_FarDistance = value;
                RaisePropertyChanged(() => this.FarDistance);
            }
        }


        private double m_StartLeanHeight = 100;
        /// <summary>
        /// 样式设置 - 地图样式 - 起始倾斜高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("自动倾斜开始距离(m)", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = 1, MaxValue = 90000000, DefaultValue = 100)]
        public double StartLeanHeight
        {
            get { return m_StartLeanHeight; }
            set
            {
                m_StartLeanHeight = value;
                this.RaisePropertyChanged(() => this.StartLeanHeight);
            }
        }

        private double m_MinLeanRadius = 0;
        /// <summary>
        /// 样式设置 - 地图样式 - 自动倾斜最小垂角
        /// </summary>
        [Synchronous]
        [PropertyDescription("自动倾斜最小垂角", Category = DescriptionEnum.样式设置, SubCategory = DescriptionEnum.地图样式, MinValue = 0, MaxValue = 90, DefaultValue = 0)]
        public double MinLeanRadius
        {
            get { return m_MinLeanRadius; }
            set
            {
                m_MinLeanRadius = value;
                this.RaisePropertyChanged(() => this.MinLeanRadius);
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
            columns.Add(LonField);
            columns.Add(LatField);
            columns.Add(AltField);
            columns.Add(GroupKeyField);
            columns.Add(LegendField);
            columns.Add(LabelField);
            columns.Add(DataTimeColumn);
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #endregion

        #region ILegendColor

        /// <summary>
        /// 根据属性字段名称 匹配对应的LegendStyle
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ChartStyleModel GetLegendStyleProperties(string propertyName)
        {
            ChartStyleModel styleModel = new ChartStyleModel();

            switch (propertyName)
            {
                case "LegendStyle":
                    styleModel = this.LegendStyle;
                    break;
                case "TrailColor":
                    styleModel = this.TrailColor;
                    break;
                default:
                    break;
            }

            return styleModel;
        }

        /// <summary>
        /// 获取枚举颜色相关内容
        /// </summary>
        /// <returns></returns>
        public ChartLegendHelperModel GetLegendColumns(string propertyName)
        {
            ChartLegendHelperModel model = new ChartLegendHelperModel();

            if (this.LegendField != null)
            {
                model.UseLegend = true;
                model.LegendColumns.Add(this.LegendField);
            }
            else
            {
                model.UseLegend = false;
                model.MeasureColumns.Add(this.GroupKeyField);
            }

            return model;
        }

        #endregion

        /// <summary>
        /// 获取图标列
        /// </summary>
        /// <returns></returns>
        public DataColumnModel GetIconColumn()
        {
            if (this.LegendField != null)
            {
                return this.LegendField;
            }
            return null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public GIS3DTrackDataViewModel()
        {
            this.m_CanSort = false;
            this.IsGetFirstDataImmediate = false;
        }
    
	}
}
