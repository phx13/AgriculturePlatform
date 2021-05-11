using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Playback;
using Digihail.AVECLI.Controls.GIS3D.Core.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;

namespace CM.RegionColoring
{
    public class RcController : GIS3DControllerBase
    {
        /// <summary>
        ///     DVM
        /// </summary>
        private readonly RcDvm m_RcDvm;

        /// <summary>
        ///     区域实体
        /// </summary>
        private readonly Dictionary<string, Entity3D> m_RegionEntity;

        /// <summary>
        ///     引擎
        /// </summary>
        private EngineContainer m_Engine;

        /// <summary>
        ///     是否显示实体
        /// </summary>
        private bool m_IsShowEntity;

        /// <summary>
        ///     构造
        /// </summary>
        public RcController(RcDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_RcDvm = dvm;
            m_RegionEntity = new Dictionary<string, Entity3D>();
        }

        /// <summary>
        ///     是否显示图层
        /// </summary>
        /// <param name="showLayer"></param>
        public override void SetShowLayer(bool showLayer)
        {
            base.SetShowLayer(showLayer);

            m_IsShowEntity = showLayer;

            foreach (var item in m_RegionEntity)
            {
                item.Value.Visible = m_IsShowEntity;
            }
        }

        /// <summary>
        ///     接收数据
        /// </summary>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count <= 0)
            {
                return;
            }

            base.ReceiveData(adt);

            InitEngine();

            AnalysisData(adt);
        }

        /// <summary>
        ///     初始化引擎
        /// </summary>
        private void InitEngine()
        {
            if (m_Engine == null)
            {
                if (EngineContainer != null)
                {
                    m_Engine = EngineContainer as EngineContainer; //获取三维引擎
                }
            }
        }

        /// <summary>
        ///     解析数据
        /// </summary>
        /// <param name="adt"></param>
        private void AnalysisData(AdapterDataTable adt)
        {
            if (m_Engine == null)
            {
                return;
            }

            var styleModel = m_RcDvm.LegendStyle;

            foreach (var row in adt.Rows)
            {
                var name = row[m_RcDvm.RegionName.AsName].ToString();
                var pointsStr = row[m_RcDvm.PolygonField.AsName].ToString();
                var type = row[m_RcDvm.LegendField.AsName].ToString();

                var entityName = type + "-" + name;

                if (m_RegionEntity.Keys.Contains(entityName))
                {
                    break;
                }

                var entity = m_Engine.GlobeWorld.World.AddEntity(entityName);
                entity.Visible = m_IsShowEntity;

                float r = 1;
                float g = 0;
                float b = 0;

                var model = styleModel.SolutionColorList.FirstOrDefault(i => i.LegendValue == type);
                if (model != null)
                {
                    var color = ColorTranslator.FromHtml(model.ColorString);
                    r = color.R/255f;
                    g = color.G/255f;
                    b = color.B/255f;
                }

                var polygon = new OsgPolygonComponent();
                polygon.FillColor = new Vector4(r, g, b, 0.6f);

                var polyline = new OsgPolylineComponent();
                polyline.LineColor = new Vector4(r, g, b, 1);
                polyline.LineWidth = 2;
                polyline.WidthUnit = OsgPolylineComponent.WidthUnitEnum.Pixels;
                polyline.StipplePattern = 0xffff;

                var pointStrArray = pointsStr.Split(new[] {"],", "[", "]"}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pointStr in pointStrArray)
                {
                    var xy = pointStr.Split(',');
                    var x = double.Parse(xy[0]);
                    var y = double.Parse(xy[1]);

                    polygon.AddPoint(new Vector3d(x, y, 0));
                    polyline.AddPoint(new Vector3d(x, y, 0));
                }
                var firstPoint = pointStrArray[0].Split(',');
                var firstX = double.Parse(firstPoint[0]);
                var firstY = double.Parse(firstPoint[1]);
                polyline.AddPoint(new Vector3d(firstX, firstY, 0));

                entity.AddComponent(polygon);
                entity.AddComponent(polyline);

                m_RegionEntity[entityName] = entity;
            }
        }
    }
}