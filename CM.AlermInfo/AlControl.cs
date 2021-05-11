using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.AlermInfo
{
    public class AlControl : ChartControllerBase
    {
        /// <summary>
        ///     接收数据线程锁
        /// </summary>
        private static readonly object m_ReceiveLock = new object();

        /// <summary>
        ///     DVM
        /// </summary>
        private readonly AlDvm m_DdDvm;

        private string m_ShowCount;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public AlControl(AlDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_DdDvm = dvm;
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string ShowCount
        {
            get { return m_ShowCount; }
            set
            {
                m_ShowCount = value;
                OnPropertyChanged("ShowCount");
            }
        }

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

            lock (m_ReceiveLock)
            {
                foreach (var row in adt.Rows)
                {
                    ShowCount = row[m_DdDvm.AlermInfoField.AsName].ToString();
                }
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