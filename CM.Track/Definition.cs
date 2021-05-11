using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using CM.Track.ImagePlayer;
using CM.Track.InfoPanel;
using CM.Track.Recognition;
using CM.Track.SwitchTrack;
using CM.Track.TopList;
using CM.Track._2DTrack;
using CM.Track._3DTrack;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS2D.Controls;
using Digihail.DAD3.Charts.GIS3D.Controls;
using Digihail.DAD3.Models.Charts;
using CM.Track.GIS;
using CM.Track.Rolling;

namespace CM.Track
{
    [Export(typeof (IChartDefinitionsImporter))]
    public class Definition : IChartDefinitionsImporter
    {
        /// <summary>
        ///     获取当前模块的定义。
        /// </summary>
        /// <returns></returns>
        public List<ChartDefinition> GetChartDefinitions()
        {
            var chartDefinitions = new List<ChartDefinition>
            {
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "2DTrack",
                    DisplayName = "可筛选2D散点",
                    DataViewModelType = typeof (GIS2DTrackDataViewModel),
                    ChartViewType = typeof (ComplexView),
                    ChartControllerType = typeof (GIS2DTrackController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "3DTrack",
                    DisplayName = "可筛选3D散点",
                    DataViewModelType = typeof (GIS3DTrackDataViewModel),
                    ChartViewType = typeof (GIS3DComplexView),
                    ChartControllerType = typeof (GIS3DTrackController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "SwitchTrack",
                    DisplayName = "乡镇筛选",
                    DataViewModelType = typeof (SwitchTrackDataViewModel),
                    ChartViewType = typeof (SwitchTrack.SwitchTrack),
                    ChartControllerType = typeof (SwitchTrackController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "Recognition",
                    DisplayName = "识别",
                    DataViewModelType = typeof (RecognitionDataViewModel),
                    ChartViewType = typeof (Recognition.Recognition),
                    ChartControllerType = typeof (RecognitionController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "TopList",
                    DisplayName = "Top图例",
                    DataViewModelType = typeof (TopListDataViewModel),
                    ChartViewType = typeof (TopListView),
                    ChartControllerType = typeof (TopListController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "ImagePlayer",
                    DisplayName = "图片轮播",
                    DataViewModelType = typeof (ImagePlayerDataViewModel),
                    ChartViewType = typeof (ImagePlayer.ImagePlayer),
                    ChartControllerType = typeof (ImagePlayerController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "InfoPanel",
                    DisplayName = "订单详细信息",
                    DataViewModelType = typeof (InfoPanelDataViewModel),
                    ChartViewType = typeof (InfoPanel.InfoPanel),
                    ChartControllerType = typeof (InfoPanelController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "GIS",
                    DisplayName = "GIS地图",
                    DataViewModelType = typeof (GISDataViewModel),
                    ChartViewType = typeof (GIS.GIS),
                    ChartControllerType = typeof (GISController)
                },
                new ChartDefinition
                {
                    Id = Guid.NewGuid(),
                    Category = "崇明农业",
                    ChartType = "Rolling",
                    DisplayName = "圆圈旋转",
                    DataViewModelType = typeof (RollingDataViewModel),
                    ChartViewType = typeof (Rolling.Rolling),
                    ChartControllerType = typeof (RollingController)
                }
            };
            return chartDefinitions;
        }
    }
}