using System;
using System.Drawing;
using System.Linq;
using System.Timers;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Playback;
using Digihail.AVECLI.Controls.GIS3D.Core;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Transform;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Transform;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;

namespace CM.PopupPanels.RealtimeAlerm
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

            CreateEntity();
            CreateBillboard();
            CreateTextComponent();
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
        ///     创建实体
        /// </summary>
        /// <param name="engine"></param>
        private void CreateEntity()
        {
            //注册字体
            m_Engine.GlobeWorld.RegisterTextSystem("RealtimeAlermTextStyle", @"Resources\Fonts\MSYaHei_GBK.fnt",
                GlobeWorld.SceneGroupAll, new Vector2(1, 1), new Vector2(0.2f, 0.2f), 50000000, false, true, -200);

            var world = m_Engine.GlobeWorld.World;

            m_Entity = world.AddEntity(Guid.NewGuid().ToString("D") + "_TextEntity");
            m_Entity.AddComponent(new SRTTransformComponent());
            m_Entity.Visible = false;

            m_Coordinate = new GeographicCoordinateTransform();
            m_Coordinate.AlwaysFaceGeoCenter = true;

            m_Entity.AddComponent(m_Coordinate);
        }

        /// <summary>
        ///     创建背景
        /// </summary>
        /// <param name="engine"></param>
        private void CreateBillboard()
        {
            var world = m_Engine.GlobeWorld.World;

            var component = new BillboardComponent();
            var style = new DefaultBillboardMaterialStyle(world.ContentManager);
            style.Texture = world.ContentManager.LoadTexture(m_Dvm.PicturePath);
            style.NearFactor = new Vector2(1, 1);
            style.FarFactor = new Vector2(1, 1);
            style.ClipRange = 5000;
            style.IsPerspective = false;
            component.MaterialStyle = style;
            component.Offset = new Vector2d(0.2, 0.2);
            component.AutoHideByDistance = true;
            component.MaxVisibleDistance = m_Dvm.MaxHeight;
            component.MinVisibleDistance = 0;
            component.Width = 1004;
            component.Height = 213;
            component.Color = new Vector4(0.9f, 0.9f, 0.9f, 0.9f);
            component.Pickable = false;

            m_Entity.AddComponent(component);
        }

        /// <summary>
        ///     创建文本组件
        /// </summary>
        private void CreateTextComponent()
        {
            m_TextComponent = new BatchedTextComponent();
            m_TextComponent.TextSystemKey = "RealtimeAlermTextStyle";
            m_TextComponent.AutoHideByDistance = true;
            m_TextComponent.MaxVisibleDistance = m_Dvm.MaxHeight;
            m_TextComponent.MinVisibleDistance = 0;
            m_TextComponent.CharacterSize = 40;
            m_TextComponent.LocalOffset = new Vector2(50, -40);
            m_TextComponent.Color = Vector4.One;
            m_TextComponent.Pickable = false;
            m_TextComponent.Text = "";

            var color = ColorTranslator.FromHtml(m_Dvm.TextColor);
            var r = color.R/255f;
            var g = color.G/255f;
            var b = color.B/255f;

            m_TextComponent.TextColor = new Vector4(r, g, b, 0.9f);
            m_TextComponent.WriteMode = BatchedTextComponent.WriteModes.MultiLine;
            m_TextComponent.HorizontalAligenment = BatchedTextComponent.Alignment.Near;
            m_Entity.AddComponent(m_TextComponent);
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

            m_Coordinate.Longitude = lon;
            m_Coordinate.Latitude = lat;
            m_Coordinate.Height = m_Dvm.HeightValue;

            var content = row[m_Dvm.NameField.AsName].ToString();
            m_TextComponent.Text = m_Dvm.AlarmType + content;

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
        ///     实体
        /// </summary>
        private Entity3D m_Entity;

        /// <summary>
        ///     文本组件
        /// </summary>
        private BatchedTextComponent m_TextComponent;

        /// <summary>
        ///     坐标组件
        /// </summary>
        private GeographicCoordinateTransform m_Coordinate;

        /// <summary>
        ///     显隐控制定时器
        /// </summary>
        private Timer m_Timer;

        #endregion
    }
}