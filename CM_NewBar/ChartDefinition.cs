using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM_NewBar
{
    [Export(typeof(IChartDefinitionsImporter))]
    public class MyChartDefinition : IChartDefinitionsImporter
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
                ChartType = "CustomBarChart",
                DisplayName = "圆角柱图",
                DataViewModelType = typeof(BarDataViewModel),
                ChartViewType = typeof(BarViewHP),
                ChartControllerType = typeof(BarHPController)
            };

            ChartDefinitions.Add(ChartDefinition);

            return ChartDefinitions;
        }
    }
}