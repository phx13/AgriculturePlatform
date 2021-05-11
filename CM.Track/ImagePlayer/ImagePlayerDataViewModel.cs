using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.Track.ImagePlayer
{
    /// <summary>
    ///     测试DVM
    /// </summary>
    [Serializable]
    public class ImagePlayerDataViewModel : ChartDataViewModel
    {
        private string m_Catalog = "";

        public ImagePlayerDataViewModel()
        {
            DataSourceModel = new DataSourceModel();
        }

        /// <summary>
        ///     图片目录
        /// </summary>
        [Synchronous]
        [PropertyDescription("图片目录", Category = "样式设置", SubCategory = "图片目录")]
        public string Catalog
        {
            get { return m_Catalog; }
            set
            {
                m_Catalog = value;
                RaisePropertyChanged(() => Catalog);
            }
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