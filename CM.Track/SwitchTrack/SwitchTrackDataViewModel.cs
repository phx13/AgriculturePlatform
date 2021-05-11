using System;
using System.Collections.Generic;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.Track.SwitchTrack
{
    /// <summary>
    ///     测试DVM
    /// </summary>
    [Serializable]
    public class SwitchTrackDataViewModel : ChartDataViewModel
    {
        public SwitchTrackDataViewModel()
        {
            DataSourceModel = new DataSourceModel();
        }

        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            return columns;
        }
    }
}