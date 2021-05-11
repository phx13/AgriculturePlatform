using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.CmDataGrid
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class DgImporter : IChartDefinitionsImporter
    {
        /// <summary>
        ///     获取当前模块的定义。
        /// </summary>
        /// <returns></returns>
        public List<ChartDefinition> GetChartDefinitions()
        {
            var ChartDefinitions = new List<ChartDefinition>();
            var ImageSwitchDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "DataGridChartRoll",
                DisplayName = "滚动表格",
                DataViewModelType = typeof (DgDvm),
                ChartViewType = typeof (DgView),
                ChartControllerType = typeof (DgControl)
            };
            ChartDefinitions.Add(ImageSwitchDefinition);

            return ChartDefinitions;
        }
    }
}