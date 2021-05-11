using System;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.VideoAnalysis
{
    public class VideoAnalysisControl : ChartControllerBase
    {
        /// <summary>
        ///     接收数据线程锁
        /// </summary>
        private static readonly object m_ReceiveLock = new object();

        /// <summary>
        ///     DVM
        /// </summary>
        private VideoAnalysisDvm m_DdDvm;

        private string m_ImagePath;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public VideoAnalysisControl(VideoAnalysisDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_DdDvm = dvm;
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string ImagePath
        {
            get { return m_ImagePath; }
            set
            {
                m_ImagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public VideoAnalysisDvm DVM
        {
            get { return m_DdDvm; }
            set
            {
                m_DdDvm = value;
                OnPropertyChanged("DVM");
            }
        }

        public event Action<AdapterDataTable> DataChanged;

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                return;
            }

            if (DataChanged != null)
            {

            }

            lock (m_ReceiveLock)
            {

            }
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }
    }
}