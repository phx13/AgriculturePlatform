using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.MediaPlay
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class MpDefinition : IChartDefinitionsImporter
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
                ChartType = "MediaPlay",
                DisplayName = "媒体播放",
                DataViewModelType = typeof (MpDvm),
                ChartViewType = typeof (MpView),
                ChartControllerType = typeof (MpControl)
            };

            ChartDefinitions.Add(ChartDefinition);

            return ChartDefinitions;
        }
    }
}