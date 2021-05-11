using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM_Flash
{
    [Serializable]
    public class FlashViewModel : ChartDataViewModel
    {
        private double m_ControlHeight;
        private double m_ControlWidth;

        /// <summary>
        ///     组件宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "组件宽度",
            Category = "样式设置",
            SubCategory = "样式设置",
            PropertyType = EditorType.None
            )]
        public double ControlWidth
        {
            get { return m_ControlWidth; }
            set
            {
                m_ControlWidth = value;
                RaisePropertyChanged(() => ControlWidth);
            }
        }

        /// <summary>
        ///     组件宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "组件高度",
            Category = "样式设置",
            SubCategory = "样式设置",
            PropertyType = EditorType.None
            )]
        public double ControlHeight
        {
            get { return m_ControlHeight; }
            set
            {
                m_ControlHeight = value;
                RaisePropertyChanged(() => ControlHeight);
            }
        }

        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            return columns;
        }
    }
}