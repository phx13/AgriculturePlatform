using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controls;
using Digihail.DAD3.Models.Charts;

namespace CM.RegionColoring
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class Definition : IChartDefinitionsImporter
    {
        public List<ChartDefinition> GetChartDefinitions()
        {
            return new List<ChartDefinition>
            {
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "gis3d.RegionColoring",
                    DisplayName = "区域着色图",
                    DataViewModelType = typeof (RcDvm),
                    ChartViewType = typeof (GIS3DComplexView),
                    ChartControllerType = typeof (RcController)
                }
            };
        }
    }
}