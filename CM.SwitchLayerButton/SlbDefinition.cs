using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.SwitchLayerButton
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class SlbDefinition : IChartDefinitionsImporter
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
                ChartType = "SwitchLayer",
                DisplayName = "切换图层",
                DataViewModelType = typeof (SlbDvm),
                ChartViewType = typeof (SlbView),
                ChartControllerType = typeof (SlbControl)
            };

            ChartDefinitions.Add(ChartDefinition);

            return ChartDefinitions;
        }
    }
}