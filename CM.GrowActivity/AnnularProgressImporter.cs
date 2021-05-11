using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.GrowActivity
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class AnnularProgressImporter : IChartDefinitionsImporter
    {
        /// <summary>
        ///     获取当前模块的定义。
        /// </summary>
        /// <returns></returns>
        public List<ChartDefinition> GetChartDefinitions()
        {
            var ChartDefinitions = new List<ChartDefinition>();


            var GrowDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "NewGrowChart",
                DisplayName = "生长周期动画",
                DataViewModelType = typeof (GrowDvm),
                ChartViewType = typeof (GrowView),
                ChartControllerType = typeof (GrowControl)
            };

            var ActivityDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "NewActivityChart",
                DisplayName = "农事行为动画",
                DataViewModelType = typeof (GrowDvm),
                ChartViewType = typeof (ActivityView),
                ChartControllerType = typeof (GrowControl)
            };

            ChartDefinitions.Add(GrowDefinition);
            ChartDefinitions.Add(ActivityDefinition);
            return ChartDefinitions;
        }
    }
}