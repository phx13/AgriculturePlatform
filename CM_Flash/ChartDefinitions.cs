using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM_Flash
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
                    ChartType = "纯动态效果",
                    DisplayName = "扫描效果",
                    DataViewModelType = typeof (FlashViewModel),
                    ChartViewType = typeof (FlashView),
                    ChartControllerType = typeof (FlashController)
                }
            };
        }
    }
}