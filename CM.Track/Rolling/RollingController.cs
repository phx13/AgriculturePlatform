using System.Collections.Generic;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.Track.Rolling
{
    /// <summary>
    ///     控制器
    /// </summary>
    public class RollingController : ChartControllerBase
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public RollingController(RollingDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {

        }

        #region 重写

        /// <summary>
        ///     图表初始化和时间轴播放时，接收当前图表数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                return;
            }
        }

        /// <summary>
        ///     刷新现有图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}