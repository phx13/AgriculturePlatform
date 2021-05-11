using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.MediaPlay
{
    [Serializable]
    public class MpDvm : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            return columns;
        }

        #region 样式设置

        private string m_ActMediaCatalog = "";

        /// <summary>
        ///     媒体目录
        /// </summary>
        [Synchronous]
        [PropertyDescription("实时媒体目录", Category = "样式设置", SubCategory = "媒体目录")]
        public string ActMediaCatalog
        {
            get { return m_ActMediaCatalog; }
            set
            {
                m_ActMediaCatalog = value;
                RaisePropertyChanged(() => ActMediaCatalog);
            }
        }

        private string m_HisMediaCatalog = "";

        /// <summary>
        ///     媒体目录
        /// </summary>
        [Synchronous]
        [PropertyDescription("历史媒体目录", Category = "样式设置", SubCategory = "媒体目录")]
        public string HisMediaCatalog
        {
            get { return m_HisMediaCatalog; }
            set
            {
                m_HisMediaCatalog = value;
                RaisePropertyChanged(() => HisMediaCatalog);
            }
        }

        private bool m_IsAct = true;

        /// <summary>
        ///     媒体目录
        /// </summary>
        [Synchronous]
        [PropertyDescription("是否为实时视频", Category = "样式设置", SubCategory = "媒体目录")]
        public bool IsAct
        {
            get { return m_IsAct; }
            set
            {
                m_IsAct = value;
                RaisePropertyChanged(() => IsAct);
            }
        }

        #endregion
    }
}