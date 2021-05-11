using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.DeliverData
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class DdDefinition : IChartDefinitionsImporter
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
                ChartType = "DeliverData",
                DisplayName = "发货数据",
                DataViewModelType = typeof (DdDvm),
                ChartViewType = typeof (DdView),
                ChartControllerType = typeof (DdControl)
            };

            ChartDefinitions.Add(ChartDefinition);

            return ChartDefinitions;
        }
    }
}