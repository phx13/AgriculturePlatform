using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.Track.Recognition
{
    /// <summary>
    ///     测试DVM
    /// </summary>
    [Serializable]
    public class RecognitionDataViewModel : ChartDataViewModel
    {
        private DimensionColumnModel m_Image;

        /// <summary>
        ///     准确率
        /// </summary>
        [Synchronous]
        [PropertyDescription("图片路径", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, RefreshChartData = true)]
        public DimensionColumnModel Image
        {
            get { return m_Image; }
            set
            {
                m_Image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        /// <summary>
        ///     获取所有用于查询分组的列
        /// </summary>
        /// <returns></returns>
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(Image);
            return columns;
        }
    }
}