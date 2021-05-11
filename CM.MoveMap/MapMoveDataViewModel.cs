using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CM.MoveMap
{
    [Serializable]
    public class MapMoveDataViewModel : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            return columns;
        }

        #region 中心点

        private double m_CenterLon = 122.254779;

        /// <summary>
        ///     样式设置 - 基本样式 - 中心点经度
        /// </summary>
        [Synchronous]
        [PropertyDescription("中心点经度", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double CenterLon
        {
            get { return m_CenterLon; }
            set
            {
                m_CenterLon = value;
                RaisePropertyChanged(() => CenterLon);
            }
        }

        private double m_CenterLat = 31.65055;

        /// <summary>
        ///     样式设置 - 基本样式 - 中心点纬度
        /// </summary>
        [Synchronous]
        [PropertyDescription("中心点纬度", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double CenterLat
        {
            get { return m_CenterLat; }
            set
            {
                m_CenterLat = value;
                RaisePropertyChanged(() => CenterLat);
            }
        }

        private int m_CenterLevel = 11;

        /// <summary>
        ///     样式设置 - 基本样式 - 初始显示层级
        /// </summary>
        [Synchronous]
        [PropertyDescription("初始显示层级", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual int CenterLevel
        {
            get { return m_CenterLevel; }
            set
            {
                m_CenterLevel = value;
                RaisePropertyChanged(() => CenterLevel);
            }
        }

        #endregion

        #region 控件宽高

        private double m_ControlWidth = 500;

        /// <summary>
        ///     样式设置 - 基本样式 - 控件宽度
        /// </summary>
        [Synchronous]
        [PropertyDescription("控件宽度", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double ControlWidth
        {
            get { return m_ControlWidth; }
            set
            {
                m_ControlWidth = value;
                RaisePropertyChanged(() => ControlWidth);
            }
        }

        private double m_ControlHeight = 500;

        /// <summary>
        ///     样式设置 - 基本样式 - 控件高度
        /// </summary>
        [Synchronous]
        [PropertyDescription("控件高度", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        public virtual double ControlHeight
        {
            get { return m_ControlHeight; }
            set
            {
                m_ControlHeight = value;
                RaisePropertyChanged(() => ControlHeight);
            }
        }

        #endregion

        #region Margin

        private VerticalAlignmentEnum m_BasicVerticalAlignment = VerticalAlignmentEnum.Top;

        /// <summary>
        ///     上下对齐
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("上下对齐", Category = DescriptionEnum.样式设置, SubCategory = "位置样式")]
        public virtual VerticalAlignmentEnum BasicVerticalAlignment
        {
            get { return m_BasicVerticalAlignment; }
            set
            {
                m_BasicVerticalAlignment = value;
                RaisePropertyChanged(() => BasicVerticalAlignment);
            }
        }

        private HorizontalAlignmentEnum m_BasicHorizontalAlignment = HorizontalAlignmentEnum.Left;

        /// <summary>
        ///     左右对齐
        /// </summary>
        [Synchronous]
        [JsonConverter(typeof (StringEnumConverter))]
        [PropertyDescription("左右对齐", Category = DescriptionEnum.样式设置, SubCategory = "位置样式")]
        public virtual HorizontalAlignmentEnum BasicHorizontalAlignment
        {
            get { return m_BasicHorizontalAlignment; }
            set
            {
                m_BasicHorizontalAlignment = value;
                RaisePropertyChanged(() => BasicHorizontalAlignment);
            }
        }

        private double m_MarginLeft;

        /// <summary>
        ///     样式设置 - 基本样式 - 居左距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("居左距离", Category = DescriptionEnum.样式设置, SubCategory = "位置样式")]
        public virtual double MarginLeft
        {
            get { return m_MarginLeft; }
            set
            {
                m_MarginLeft = value;
                RaisePropertyChanged(() => MarginLeft);
            }
        }

        private double m_MarginRight;

        /// <summary>
        ///     样式设置 - 基本样式 - 居右距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("居右距离", Category = DescriptionEnum.样式设置, SubCategory = "位置样式")]
        public virtual double MarginRight
        {
            get { return m_MarginRight; }
            set
            {
                m_MarginRight = value;
                RaisePropertyChanged(() => MarginRight);
            }
        }

        private double m_MarginUp;

        /// <summary>
        ///     样式设置 - 基本样式 - 居上距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("居上距离", Category = DescriptionEnum.样式设置, SubCategory = "位置样式")]
        public virtual double MarginUp
        {
            get { return m_MarginUp; }
            set
            {
                m_MarginUp = value;
                RaisePropertyChanged(() => MarginUp);
            }
        }

        private double m_MarginDown;

        /// <summary>
        ///     样式设置 - 基本样式 - 居下距离
        /// </summary>
        [Synchronous]
        [PropertyDescription("居下距离", Category = DescriptionEnum.样式设置, SubCategory = "位置样式")]
        public virtual double MarginDown
        {
            get { return m_MarginDown; }
            set
            {
                m_MarginDown = value;
                RaisePropertyChanged(() => MarginDown);
            }
        }

        #endregion
    }
}