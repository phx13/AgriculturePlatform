using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.Track.Rolling
{
    /// <summary>
    ///     测试DVM
    /// </summary>
    [Serializable]
    public class RollingDataViewModel : ChartDataViewModel
    {
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