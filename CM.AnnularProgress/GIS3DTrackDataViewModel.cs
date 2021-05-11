using System;
using System.Collections.Generic;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels.GIS3D;

namespace CM.AnnularProgress.GISPlayBack
{
    /// <summary>
    ///     3D节点轨迹图DVM
    /// </summary>
    // Token: 0x0200051A RID: 1306
    [Serializable]
    public class GIS3DTrackDataViewModel : GIS3DPlaybackDataViewModel
        // ChartDataViewModel, IGIS3DDataViewModel, IShowLayer, IIcon, IGlobeStyle, ILegendColor, ILonLat, I3DSpatialQuery
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public GIS3DTrackDataViewModel()
        {
            m_CanSort = false;
            IsGetFirstDataImmediate = false;
        }

        #region Override

        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(LonField);
            columns.Add(LatField);
            columns.Add(AltField);
            columns.Add(GroupKeyField);
            columns.Add(LegendField);
            columns.Add(LabelField);
            columns.Add(DataTimeColumn);
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #endregion
    }
}