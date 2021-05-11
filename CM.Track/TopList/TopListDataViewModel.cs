using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.Track.TopList
{
    [Serializable]
    public class TopListDataViewModel : ChartDataViewModel, ILegendColor
    {
        public ChartLegendHelperModel GetLegendColumns(string propertyName)
        {
            var model = new ChartLegendHelperModel();
            model.UseLegend = true;
            model.LegendColumns.Add(NameField);
            return model;
        }

        public ChartStyleModel GetLegendStyleProperties(string propertyName)
        {
            return LegendStyle;
        }

        public override List<DataColumnModel> GetColumns()
        {
            var cols = new List<DataColumnModel>();
            cols.Add(CountField);
            cols.Add(NameField);
            return cols;
        }

        #region 数据

        private MeasureColumnModel m_CountField;

        [Synchronous]
        [PropertyDescription("数值", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public MeasureColumnModel CountField
        {
            get { return m_CountField; }
            set
            {
                m_CountField = value;
                RaisePropertyChanged(() => CountField);
            }
        }

        private LegendColumnModel m_NameField;

        [Synchronous]
        [PropertyDescription("名称", Category = DescriptionEnum.数据设置, SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.Field, IsNecessary = true, RefreshChartData = true)]
        public LegendColumnModel NameField
        {
            get { return m_NameField; }
            set
            {
                m_NameField = value;
                RaisePropertyChanged(() => NameField);
            }
        }

        #endregion

        #region 样式

        private ChartStyleModel m_LegendStyle = new ChartStyleModel();

        /// <summary>
        ///     枚举颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("枚举颜色", Category = "样式设置", SubCategory = "颜色样式", PropertyType = EditorType.ColorCollection)
        ]
        public ChartStyleModel LegendStyle
        {
            get { return m_LegendStyle; }
            set
            {
                m_LegendStyle = value;
                IsDVMEdited = true;
                RaisePropertyChanged(() => LegendStyle);
            }
        }

        #endregion

        #region 其他

        private bool m_IsDVMEdited;

        /// <summary>
        ///     DVM是否被修改过，除了修改仪表盘类型以外，所有的DVM属性修改都会造成该值被设置为true
        /// </summary>
        [Synchronous]
        public bool IsDVMEdited
        {
            get { return m_IsDVMEdited; }
            set { m_IsDVMEdited = value; }
        }

        #endregion
    }
}