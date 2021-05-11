using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using CM.Track.SwitchTrack;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Transform;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Transform;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;

namespace CM.PopupPanels.OrderInfomation
{
    /// <summary>
    ///     控制器
    /// </summary>
    public class EarthController : GIS3DControllerBase
    {
        /// <summary>
        ///     构造
        /// </summary>
        public EarthController(EarthDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_Dvm = dvm;
            var m_MessageAggregator = new MessageAggregator();
            m_MessageAggregator.GetMessage<DeleteObjectMessage>().Subscribe(RevDeleteMessage);

            m_OriginalSizeDistance = m_Dvm.NarrowHeight;
            m_MinimumSizeDistance = m_Dvm.NarrowEndHeight;
        }

        private void RevDeleteMessage(bool obj)
        {
            if (obj)
            {
                if (m_Entity != null)
                {
                    m_Engine.GlobeWorld.World.RemoveEntity(m_Entity);
                }
            }
        }

        /// <summary>
        ///     控制图层显隐
        /// </summary>
        /// <param name="showLayer"></param>
        public override void SetShowLayer(bool showLayer)
        {
            base.SetShowLayer(showLayer);

            if (m_Entity != null)
            {
                m_Entity.Visible = showLayer;
            }
        }

        /// <summary>
        ///     获取数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            base.ReceiveData(adt);

            if (adt == null || adt.Rows == null || adt.Rows.Count <= 0)
            {
                return;
            }

            InitEngine();

            if (m_Engine == null)
            {
                return;
            }

            InitTimer();

            var row = adt.Rows.Last(); //永远处理每一帧最后一行数据

            Update(row);
        }

        /// <summary>
        ///     初始化定时器
        /// </summary>
        private void InitTimer()
        {
            m_Timer = new Timer(m_Dvm.DurationData*1000);
            m_Timer.AutoReset = false;
            m_Timer.Elapsed += M_Timer_Elapsed;
        }

        /// <summary>
        ///     定时器回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_Entity.Visible = false;
        }

        /// <summary>
        ///     初始化引擎
        /// </summary>
        private void InitEngine()
        {
            if (m_Engine != null)
            {
                return;
            }

            if (EngineContainer == null)
            {
                return;
            }

            var engine = (EngineContainer) EngineContainer;
            if (engine.GlobeWorld == null || engine.GlobeWorld.World == null)
            {
                return;
            }

            m_Engine = engine;

            //CreateEntity();
            //CreateBillboard();

            m_Engine.SurfaceView.PropertyChanged += SurfaceView_PropertyChanged;
        }

        /// <summary>
        ///     当前高度属性改变事件回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SurfaceView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentHeight")
            {
                var realCurrent = (float) m_Engine.SurfaceView.CurrentHeight;
                if (realCurrent >= m_MinimumSizeDistance)
                {
                    m_Scaling = (float) m_Dvm.MinScaling;
                }
                else if (realCurrent >= m_OriginalSizeDistance)
                {
                    var section = m_MinimumSizeDistance - m_OriginalSizeDistance;
                    var current = realCurrent - m_OriginalSizeDistance;
                    var scaling = (section - current)/section;

                    if (scaling < m_Dvm.MinScaling)
                    {
                        scaling = (float) m_Dvm.MinScaling;
                    }

                    if (scaling > 1)
                    {
                        scaling = 1;
                    }

                    m_Scaling = scaling;
                }
                else
                {
                    m_Scaling = 1;
                }

                if (m_Billboard != null)
                {
                    m_Billboard.Width = m_Width*m_Scaling;
                    m_Billboard.Height = m_Height*m_Scaling;
                }
            }
        }

        /// <summary>
        ///     创建实体
        /// </summary>
        /// <param name="engine"></param>
        private void CreateEntity(double lon, double lat)
        {
            var world = m_Engine.GlobeWorld.World;

            if (m_Entity != null)
            {
                world.RemoveEntity(m_Entity);
            }

            m_Entity = world.AddEntity(Guid.NewGuid().ToString("D") + "_Entity");
            m_Entity.AddComponent(new SRTTransformComponent());
            m_Entity.Visible = false;

            m_Coordinate = new GeographicCoordinateTransform();
            m_Coordinate.Longitude = lon;
            m_Coordinate.Latitude = lat;
            m_Coordinate.Height = 0;
            m_Coordinate.AlwaysFaceGeoCenter = true;
            m_Entity.AddComponent(m_Coordinate);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                //m_Engine.PanTo(lon, lat, m_Engine.SurfaceView.CurrentHeight);
                m_Engine.OnSetCameraPositionCommand(new Vector3d(lon, lat, m_Engine.SurfaceView.CurrentHeight));
            }));
        }

        /// <summary>
        ///     创建背景
        /// </summary>
        /// <param name="engine"></param>
        private void CreateBillboard(string path)
        {
            var world = m_Engine.GlobeWorld.World;

            m_BillboardEntity = world.AddEntity(Guid.NewGuid().ToString("D") + "_Billboard");
            m_BillboardEntity.Parent = m_Entity;
            m_BillboardEntity.AddComponent(new SRTTransformComponent());

            m_Billboard = new BillboardComponent();
            var style = new DefaultBillboardMaterialStyle(world.ContentManager);
            style.Texture = world.ContentManager.LoadTexture(path);
            style.NearFactor = new Vector2(1, 1);
            style.FarFactor = new Vector2(1, 1);
            style.ClipRange = 5000;
            style.IsPerspective = false;
            m_Billboard.MaterialStyle = style;
            m_Billboard.AutoHideByDistance = true;
            m_Billboard.MaxVisibleDistance = m_Dvm.MaxHeight;
            m_Billboard.MinVisibleDistance = 0;
            m_Billboard.Width = m_Width*m_Scaling;
            m_Billboard.Height = m_Height*m_Scaling;
            m_Billboard.Color = new Vector4(1f, 1f, 1f, 1f);
            m_Billboard.Pickable = false;
            m_BillboardEntity.AddComponent(m_Billboard);
        }

        /// <summary>
        ///     更新文本内容
        /// </summary>
        /// <param name="adt"></param>
        private void Update(AdapterDataRow row)
        {
            if (!m_Dvm.ShowLayer)
            {
                return;
            }

            var lon = (double) row[m_Dvm.LonField.AsName];
            var lat = (double) row[m_Dvm.LatField.AsName];

            m_Window = new Window1(row, m_Dvm);
            m_Window.Show();

            CreateEntity(lon, lat);

            var path = m_Window.CreatePic();

            CreateBillboard(path);

            m_Window.Close();
            m_Window = null;


            m_Timer.Stop();
            m_Entity.Visible = true;
            m_Timer.Start();
        }

        /// <summary>
        ///     处理实体
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (m_Entity != null)
            {
                m_Engine.GlobeWorld.World.RemoveEntity(m_Entity);
            }

            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer.Dispose();
            }
        }

        #region Basics Field

        /// <summary>
        ///     引擎
        /// </summary>
        private EngineContainer m_Engine;

        /// <summary>
        ///     DVM
        /// </summary>
        private readonly EarthDataViewModel m_Dvm;

        #endregion

        #region View Field

        /// <summary>
        ///     标牌组件
        /// </summary>
        private BillboardComponent m_Billboard;

        /// <summary>
        ///     实体
        /// </summary>
        private Entity3D m_Entity;

        /// <summary>
        /// </summary>
        private Entity3D m_BillboardEntity;

        /// <summary>
        ///     坐标组件
        /// </summary>
        private GeographicCoordinateTransform m_Coordinate;

        #endregion

        #region Control Field

        /// <summary>
        ///     标牌缩放比例
        /// </summary>
        private float m_Scaling = 1;

        /// <summary>
        ///     显示原始尺寸距离
        /// </summary>
        private readonly float m_OriginalSizeDistance = 1000;

        /// <summary>
        ///     显示最小尺寸距离
        /// </summary>
        private readonly float m_MinimumSizeDistance = 50000;

        /// <summary>
        ///     标牌宽度
        /// </summary>
        private readonly double m_Width = 3840;

        /// <summary>
        ///     标牌高度
        /// </summary>
        private readonly double m_Height = 1728;

        /// <summary>
        ///     显隐控制定时器
        /// </summary>
        private Timer m_Timer;

        /// <summary>
        ///     窗口
        /// </summary>
        private Window1 m_Window;

        #endregion
    }
}