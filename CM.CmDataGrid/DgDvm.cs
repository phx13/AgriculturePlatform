using System;
using System.Collections.Generic;
using System.Windows;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.CmDataGrid
{
    [Serializable]
    public class DgDvm : ChartDataViewModel
    {
        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            columns.AddRange(ColumnFields);
            return columns;
        }

        #region 数据设置

        private MeasureFieldCollection m_ColumnFields = new MeasureFieldCollection();

        /// <summary>
        ///     表格字段
        /// </summary>
        [Synchronous]
        [PropertyDescription(
            "表格字段",
            Category = DescriptionEnum.数据设置,
            SubCategory = DescriptionEnum.数据设置,
            PropertyType = EditorType.MeasureCollection,
            IsNecessary = true,
            RefreshChartData = true)]
        public MeasureFieldCollection ColumnFields
        {
            get { return m_ColumnFields; }
            set
            {
                m_ColumnFields = value;
                RaisePropertyChanged(() => ColumnFields);
            }
        }

        #endregion

        #region 表头样式

        private TableColumnsModel m_TableColumns = new TableColumnsModel();

        /// <summary>
        ///     列名设置-列设置
        /// </summary>
        [Synchronous]
        [PropertyDescription("列名设置", Category = DescriptionEnum.样式设置, SubCategory = "表头样式",
            PropertyType = EditorType.TableColumns)]
        public TableColumnsModel TableColumns
        {
            get { return m_TableColumns; }
            set
            {
                m_TableColumns = value;
                RaisePropertyChanged(() => TableColumns);
            }
        }

        private string m_HeaderFontFamily = "微软雅黑";

        /// <summary>
        ///     表头字体-列设置
        /// </summary>
        [Synchronous]
        [PropertyDescription("表头字体", Category = DescriptionEnum.样式设置, SubCategory = "表头样式",
            PropertyType = EditorType.FontFamily)]
        public string HeaderFontFamily
        {
            get { return m_HeaderFontFamily; }
            set
            {
                m_HeaderFontFamily = value;
                RaisePropertyChanged(() => HeaderFontFamily);
            }
        }

        private double m_HeaderHeight = 30;

        /// <summary>
        ///     表头高度-列设置
        /// </summary>
        [Synchronous]
        [PropertyDescription("表头高度", Category = DescriptionEnum.样式设置, SubCategory = "表头样式")]
        public double HeaderHeight
        {
            get { return m_HeaderHeight; }
            set
            {
                m_HeaderHeight = value;
                RaisePropertyChanged(() => HeaderHeight);
            }
        }

        private string m_HeaderBackground = "#FF3A6550";

        /// <summary>
        ///     表头背景
        /// </summary>
        [Synchronous]
        [PropertyDescription("表头背景", Category = DescriptionEnum.样式设置, SubCategory = "表头样式",
            PropertyType = EditorType.Color)]
        public string HeaderBackground
        {
            get { return m_HeaderBackground; }
            set
            {
                m_HeaderBackground = value;
                RaisePropertyChanged(() => HeaderBackground);
            }
        }

        private string m_HeaderForeground = "#FFFFFF";

        /// <summary>
        ///     文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = "表头样式",
            PropertyType = EditorType.Color)]
        public string HeaderForeground
        {
            get { return m_HeaderForeground; }
            set
            {
                m_HeaderForeground = value;
                RaisePropertyChanged(() => HeaderForeground);
            }
        }

        private int m_HeaderFontSize = 18;

        /// <summary>
        ///     表头字号
        /// </summary>
        [Synchronous]
        [PropertyDescription("表头字号", Category = DescriptionEnum.样式设置, SubCategory = "表头样式")]
        public int HeaderFontSize
        {
            get { return m_HeaderFontSize; }
            set
            {
                m_HeaderFontSize = value;
                RaisePropertyChanged(() => HeaderFontSize);
            }
        }

        private HorizontalAlignment m_BasicHorizontalAlignment = HorizontalAlignment.Center;

        /// <summary>
        ///     左右对齐
        /// </summary>
        [Synchronous]
        [PropertyDescription("左右对齐", Category = DescriptionEnum.样式设置, SubCategory = "表头样式")]
        public virtual HorizontalAlignment HeaderHorizontalAlignment
        {
            get { return m_BasicHorizontalAlignment; }
            set
            {
                m_BasicHorizontalAlignment = value;
                RaisePropertyChanged(() => HeaderHorizontalAlignment);
            }
        }

        private string m_HeaderDividingLine = "#666666";

        /// <summary>
        ///     分割线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("分割线颜色", Category = DescriptionEnum.样式设置, SubCategory = "表头样式",
            PropertyType = EditorType.Color)]
        public string HeaderDividingLine
        {
            get { return m_HeaderDividingLine; }
            set
            {
                m_HeaderDividingLine = value;
                RaisePropertyChanged(() => HeaderDividingLine);
            }
        }

        private int m_HeaderDividingWidth = 1;

        /// <summary>
        ///     分割线宽-列设置
        /// </summary>
        [Synchronous]
        [PropertyDescription("分割线宽", Category = DescriptionEnum.样式设置, SubCategory = "表头样式")]
        public int HeaderDividingWidth
        {
            get { return m_HeaderDividingWidth; }
            set
            {
                m_HeaderDividingWidth = value;
                RaisePropertyChanged(() => HeaderDividingWidth);
                RaisePropertyChanged(() => HeaderDividingLineWidth);
            }
        }

        /// <summary>
        ///     分割线宽-列设置
        /// </summary>
        public string HeaderDividingLineWidth
        {
            get
            {
                if (m_HeaderDividingWidth == 0)
                {
                    return "0,0,1,0";
                }

                return "0,0," + m_HeaderDividingWidth + ",0";
            }
        }

        #endregion

        #region 表格样式

        private int m_GridFontSize = 16;

        /// <summary>
        ///     表格字号
        /// </summary>
        [Synchronous]
        [PropertyDescription("表格字号", Category = DescriptionEnum.样式设置, SubCategory = "表格样式")]
        public int GridFontSize
        {
            get { return m_GridFontSize; }
            set
            {
                m_GridFontSize = value;
                RaisePropertyChanged(() => GridFontSize);
            }
        }

        private string m_GridFontFamily = "微软雅黑";

        /// <summary>
        ///     表格字体
        /// </summary>
        [Synchronous]
        [PropertyDescription("表格字体", Category = DescriptionEnum.样式设置, SubCategory = "表格样式",
            PropertyType = EditorType.FontFamily)]
        public string GridFontFamily
        {
            get { return m_GridFontFamily; }
            set
            {
                m_GridFontFamily = value;
                RaisePropertyChanged(() => GridFontFamily);
            }
        }

        private string m_GridForeground = "#FFFFFF";

        /// <summary>
        ///     文字颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("文字颜色", Category = DescriptionEnum.样式设置, SubCategory = "表格样式",
            PropertyType = EditorType.Color)]
        public string GridForeground
        {
            get { return m_GridForeground; }
            set
            {
                m_GridForeground = value;
                RaisePropertyChanged(() => GridForeground);
            }
        }

        private string m_InterlacingColor1 = "#333333";

        /// <summary>
        ///     隔行色值1
        /// </summary>
        [Synchronous]
        [PropertyDescription("隔行色值1", Category = DescriptionEnum.样式设置, SubCategory = "表格样式",
            PropertyType = EditorType.Color)]
        public string InterlacingColor1
        {
            get { return m_InterlacingColor1; }
            set
            {
                m_InterlacingColor1 = value;
                RaisePropertyChanged(() => InterlacingColor1);
            }
        }

        private string m_InterlacingColor2 = "#444444";

        /// <summary>
        ///     隔行色值2
        /// </summary>
        [Synchronous]
        [PropertyDescription("隔行色值2", Category = DescriptionEnum.样式设置, SubCategory = "表格样式",
            PropertyType = EditorType.Color)]
        public string InterlacingColor2
        {
            get { return m_InterlacingColor2; }
            set
            {
                m_InterlacingColor2 = value;
                RaisePropertyChanged(() => InterlacingColor2);
            }
        }

        private double m_GridRowHeight = 30;

        /// <summary>
        ///     表格行高
        /// </summary>
        [Synchronous]
        [PropertyDescription("表格行高", Category = DescriptionEnum.样式设置, SubCategory = "表格样式")]
        public double GridRowHeight
        {
            get { return m_GridRowHeight; }
            set
            {
                m_GridRowHeight = value;
                RaisePropertyChanged(() => GridRowHeight);
            }
        }

        private string m_GridDividingLine = "#666666";

        /// <summary>
        ///     列分割线颜色
        /// </summary>
        [Synchronous]
        [PropertyDescription("列分割线颜色", Category = DescriptionEnum.样式设置, SubCategory = "表格样式",
            PropertyType = EditorType.Color)]
        public string GridDividingLine
        {
            get { return m_GridDividingLine; }
            set
            {
                m_GridDividingLine = value;
                RaisePropertyChanged(() => GridDividingLine);
            }
        }

        private int m_GridDividingWidth = 1;

        /// <summary>
        ///     分割线宽-列设置
        /// </summary>
        [Synchronous]
        [PropertyDescription("分割线宽", Category = DescriptionEnum.样式设置, SubCategory = "表格样式")]
        public int GridDividingWidth
        {
            get { return m_GridDividingWidth; }
            set
            {
                m_GridDividingWidth = value;
                RaisePropertyChanged(() => GridDividingWidth);
                RaisePropertyChanged(() => GridDividingLineWidth);
            }
        }

        /// <summary>
        ///     分割线宽-列设置
        /// </summary>
        public string GridDividingLineWidth
        {
            get
            {
                if (m_GridDividingWidth == 0)
                {
                    return "0,0,1,0";
                }

                return "0,0," + m_GridDividingWidth + ",0";
            }
        }

        private int m_TimerInterval = 1000;

        /// <summary>
        ///     动画间隔
        /// </summary>
        [Synchronous]
        [PropertyDescription("动画间隔（毫秒）", Category = DescriptionEnum.样式设置, SubCategory = "表格样式")]
        public int TimerInterval
        {
            get { return m_TimerInterval; }
            set
            {
                m_TimerInterval = value;
                RaisePropertyChanged(() => TimerInterval);
            }
        }

        #endregion
    }
}