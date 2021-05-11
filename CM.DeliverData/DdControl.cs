using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.DeliverData
{
    public class DdControl : ChartControllerBase
    {
        /// <summary>
        ///     接收数据线程锁
        /// </summary>
        private static readonly object m_ReceiveLock = new object();

        /// <summary>
        ///     DVM
        /// </summary>
        private readonly DdDvm m_DdDvm;

        private string m_Count;

        private string m_DepotName;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public DdControl(DdDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_DdDvm = dvm;
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string DepotName
        {
            get { return m_DepotName; }
            set
            {
                m_DepotName = value;
                OnPropertyChanged("DepotName");
            }
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string Count
        {
            get { return m_Count; }
            set
            {
                m_Count = value;
                OnPropertyChanged("Count");
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
                    DepotName = row[m_DdDvm.DepotNameField.AsName].ToString();
                    Count = row[m_DdDvm.CountField.AsName].ToString();
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