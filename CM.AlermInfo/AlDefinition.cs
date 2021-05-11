using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.AlermInfo
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class AlDefinition : IChartDefinitionsImporter
    {
        /// <summary>
        ///     获取当前模块的定义。
        /// </summary>
        /// <returns></returns>
        public List<ChartDefinition> GetChartDefinitions()
        {
            var ChartDefinitions = new List<ChartDefinition>();
            var ChartDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "AlermInfo",
                DisplayName = "告警信息",
                DataViewModelType = typeof (AlDvm),
                ChartViewType = typeof (AlView),
                ChartControllerType = typeof (AlControl)
            };

            ChartDefinitions.Add(ChartDefinition);

            return ChartDefinitions;
        }
    }
}