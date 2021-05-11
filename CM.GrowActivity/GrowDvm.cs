using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.GrowActivity
{
    [Serializable]
    public class GrowDvm : ChartDataViewModel
    {
        private double m_ImageFrom = -118;

        private MarginAlignmentEnum m_ImageMargin = MarginAlignmentEnum.Left;

        private int m_ImageTime = 2;

        private double m_ImageTo;

        /// <summary>
        ///     样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("动画开始位置", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double ImageFrom
        {
            get { return m_ImageFrom; }
            set
            {
                m_ImageFrom = value;
                RaisePropertyChanged(() => ImageFrom);
            }
        }

        /// <summary>
        ///     样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("动画结束位置", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double ImageTo
        {
            get { return m_ImageTo; }
            set
            {
                m_ImageTo = value;
                RaisePropertyChanged(() => ImageTo);
            }
        }

        /// <summary>
        ///     样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("图片动画时间", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual int ImageTime
        {
            get { return m_ImageTime; }
            set
            {
                m_ImageTime = value;
                RaisePropertyChanged(() => ImageTime);
            }
        }

        /// <summary>
        ///     样式设置 - 基本文字 - 文字大小
        /// </summary>
        [Synchronous]
        [PropertyDescription("动画方向", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual MarginAlignmentEnum ImageMargin
        {
            get { return m_ImageMargin; }
            set
            {
                m_ImageMargin = value;
                RaisePropertyChanged(() => ImageMargin);
            }
        }

        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.Add(TypeField);
            columns.Add(JiDiField);
            columns.Add(TimeField);
            columns.Add(StateField);
            return columns;
        }

        #region DepotNameField

        private DimensionColumnModel m_TypeField;

        /// <summary>
        ///     行为
        /// </summary>
        [Synchronous]
        [PropertyDescription("行为",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel TypeField
        {
            get { return m_TypeField; }
            set
            {
                m_TypeField = value;
                RaisePropertyChanged(() => TypeField);
            }
        }

        private DimensionColumnModel m_JiDiField;

        /// <summary>
        ///     基地
        /// </summary>
        [Synchronous]
        [PropertyDescription("基地",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel JiDiField
        {
            get { return m_JiDiField; }
            set
            {
                m_JiDiField = value;
                RaisePropertyChanged(() => JiDiField);
            }
        }

        private DimensionColumnModel m_TimeField;

        /// <summary>
        ///     时间
        /// </summary>
        [Synchronous]
        [PropertyDescription("时间",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel TimeField
        {
            get { return m_TimeField; }
            set
            {
                m_TimeField = value;
                RaisePropertyChanged(() => TimeField);
            }
        }

        private DimensionColumnModel m_StateField;

        /// <summary>
        ///     状态
        /// </summary>
        [Synchronous]
        [PropertyDescription("状态",
            Category = "数据设置",
            SubCategory = "数据设置",
            PropertyType = EditorType.Field,
            IsNecessary = true,
            RefreshChartData = true
            )]
        public DimensionColumnModel StateField
        {
            get { return m_StateField; }
            set
            {
                m_StateField = value;
                RaisePropertyChanged(() => StateField);
            }
        }

        #endregion
    }

    public enum MarginAlignmentEnum
    {
        /// <summary>
        ///     上
        /// </summary>
        // Token: 0x04001221 RID: 4641
        Top,

        /// <summary>
        ///     左
        /// </summary>
        // Token: 0x04001222 RID: 4642
        Left,

        /// <summary>
        ///     右
        /// </summary>
        // Token: 0x04001223 RID: 4643
        Bottom,

        /// <summary>
        ///     右
        /// </summary>
        // Token: 0x04001222 RID: 4642
        Right
    }
}