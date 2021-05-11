using System.Collections.Generic;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using System.Linq;
using System;

namespace CM.Track.Recognition
{
    /// <summary>
    ///     控制器
    /// </summary>
    public class RecognitionController : ChartControllerBase
    {
        #region 构造

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public RecognitionController(RecognitionDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = dvm;
        }

        #endregion

        #region 属性

        public ChartViewBase View { get; set; }

        /// <summary>
        ///     面板高度
        /// </summary>
        public double MyHeight
        {
            get { return m_MyHeight; }
            set
            {
                m_MyHeight = value;
                OnPropertyChanged("MyHeight");
            }
        }

        /// <summary>
        ///     面板宽度
        /// </summary>
        public double MyWidth
        {
            get { return m_MyWidth; }
            set
            {
                m_MyWidth = value;
                OnPropertyChanged("MyWidth");
            }
        }

        private double m_MyHeight;
        private double m_MyWidth;
        private RecognitionDataViewModel m_DVM;

        /// <summary>
        ///     DVM
        /// </summary>
        public RecognitionDataViewModel DVM
        {
            get { return m_DVM; }
            set
            {
                m_DVM = value;
                OnPropertyChanged("DVM");
            }
        }

        public event Action<AdapterDataTable> DataChanged;
        private string m_Image;

        public string Image
        {
            get { return m_Image; }
            set
            {
                m_Image = value;
                OnPropertyChanged("Image");
            }
        }

        #endregion

        #region 重写

        /// <summary>
        ///     图表初始化和时间轴播放时，接收当前图表数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                return;
            }
            var row = adt.Rows.Last();
            Image = row[DVM.Image.AsName].ToString();
            if (DataChanged != null)
                DataChanged(adt);
        }

        /// <summary>
        ///     刷新现有图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}