using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CM.PopupPanels.RealtimeAlerm;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controls;
using Digihail.DAD3.Models.Charts;

namespace CM.PopupPanels
{
    /// <summary>
    ///     定义导入器
    /// </summary>
    [Export(typeof (IChartDefinitionsImporter))]
    public class EarthDefinitionImporter : IChartDefinitionsImporter
    {
        public List<ChartDefinition> GetChartDefinitions()
        {
            return new List<ChartDefinition>
            {
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "gis3d.RealtimeAlerm",
                    DisplayName = "实时报警",
                    DataViewModelType = typeof (EarthDataViewModel),
                    ChartViewType = typeof (GIS3DComplexView),
                    ChartControllerType = typeof (EarthController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "gis3d.OrderInfomation",
                    DisplayName = "订单信息",
                    DataViewModelType = typeof (OrderInfomation.EarthDataViewModel),
                    ChartViewType = typeof (GIS3DComplexView),
                    ChartControllerType = typeof (OrderInfomation.EarthController)
                }
            };
        }
    }
}