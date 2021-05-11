using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.VideoAnalysis
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
            var ImageSwitchDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "VideoAnalysis",
                DisplayName = "视频分析矩阵",
                DataViewModelType = typeof (VideoAnalysisDvm),
                ChartViewType = typeof (VideoAnalysisView),
                ChartControllerType = typeof (VideoAnalysisControl)
            };
            ChartDefinitions.Add(ImageSwitchDefinition);
            return ChartDefinitions;
        }
    }
}