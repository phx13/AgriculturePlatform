using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels.GIS2D;

namespace CM.Track._2DTrack
{
    /// <summary>
    ///     2D节点轨迹图DVM
    /// </summary>
    [Serializable]
    public class GIS2DTrackDataViewModel : GIS2DPlaybackDataViewModel
    {
        #region Override

        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>
            {
                LonField,
                LatField,
                GroupKeyField,
                LegendField,
                LabelField,
                DataTimeColumn,
                VillageField
            };
            columns.RemoveAll(item => item == null);
            return columns;
        }

        #endregion

        #region 数据设置 - 数据设置

        private DimensionColumnModel m_VillageField;

        /// <summary>
        ///     数据设置 - 数据设置 - 省份字段
        /// </summary>
        [Synchronous]
        [PropertyDescription("乡镇字段", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, RefreshChartData = true)]
        public DimensionColumnModel VillageField
        {
            get { return m_VillageField; }
            set
            {
                m_VillageField = value;
                RaisePropertyChanged(() => VillageField);
            }
        }

        #endregion
    }
}