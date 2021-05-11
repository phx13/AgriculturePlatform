using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.MoveMap
{
    internal class DefinitionImporter
    {
        [Export(typeof (IChartDefinitionsImporter))]
        public class ChartDefinitions : IChartDefinitionsImporter
        {
            public List<ChartDefinition> GetChartDefinitions()
            {
                return new List<ChartDefinition>
                {
                    new ChartDefinition
                    {
                        Id = Guid.NewGuid(),
                        Category = "崇明农业",
                        ChartType = "二维地图",
                        DisplayName = "移动地图",
                        DataViewModelType = typeof (MapMoveDataViewModel),
                        ChartViewType = typeof (MapMove),
                        ChartControllerType = typeof (MapMoveController)
                    }
                };
            }
        }
    }
}