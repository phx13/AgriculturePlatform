using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CM.AnnularProgress.GrowActivity;
using CM.AnnularProgress.ImageSwitch;
using CM.AnnularProgress.Pie;
using CM.AnnularProgress.ShowText;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.Charts;

namespace CM.AnnularProgress
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

            var ChartDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "AnimationPie",
                DisplayName = "动态饼图",
                DataViewModelType = typeof (PieDataViewModel),
                ChartViewType = typeof (PieViewHP),
                ChartControllerType = typeof (PieHPController)
            };

            var GrowDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "GrowChart",
                DisplayName = "生长周期",
                DataViewModelType = typeof (GrowDvm),
                ChartViewType = typeof (GrowView),
                ChartControllerType = typeof (GrowControl)
            };

            var ActivityDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "ActivityChart",
                DisplayName = "农事行为",
                DataViewModelType = typeof (GrowDvm),
                ChartViewType = typeof (ActivityView),
                ChartControllerType = typeof (GrowControl)
            };

            //var Track3DChart = new ChartDefinition
            //{
            //    Id = Guid.NewGuid(),
            //    Category = "崇明农业",
            //    ChartType = "3DTrackChart",
            //    DisplayName = "三维节点标牌",
            //    DataViewModelType = typeof (GIS3DTrackDataViewModel),
            //    ChartViewType = typeof (GIS3DComplexView),
            //    ChartControllerType = typeof (GIS3DTrackController)
            //};

            var ShowTextDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "ShowTextChart",
                DisplayName = "可换行文本",
                DataViewModelType = typeof (ShowTextDvm),
                ChartViewType = typeof (ShowTextView),
                ChartControllerType = typeof (ShowTextControl)
            };

            var ImageSwitchDefinition = new ChartDefinition
            {
                Id = Guid.NewGuid(),
                Category = "崇明农业",
                ChartType = "ImageSwitchChart",
                DisplayName = "月份图片序列切换",
                DataViewModelType = typeof (ImageSwitchDvm),
                ChartViewType = typeof (ImageSwitchView),
                ChartControllerType = typeof (ImageSwitchControl)
            };


            ChartDefinitions.Add(ImageSwitchDefinition);
            //ChartDefinitions.Add(Track3DChart);
            ChartDefinitions.Add(ChartDefinition);
            ChartDefinitions.Add(GrowDefinition);
            ChartDefinitions.Add(ActivityDefinition);
            ChartDefinitions.Add(ShowTextDefinition);
            return ChartDefinitions;
        }
    }
}