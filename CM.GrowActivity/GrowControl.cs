using System;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.GrowActivity
{
    public class GrowControl : ChartControllerBase
    {
        /// <summary>
        ///     接收数据线程锁
        /// </summary>
        private static readonly object m_ReceiveLock = new object();

        /// <summary>
        ///     DVM
        /// </summary>
        private readonly GrowDvm m_DdDvm;

        private string m_JiDiValue;

        private string m_StateValue;

        private string m_TimeValue;

        private string m_TypeValue;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public GrowControl(GrowDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_DdDvm = dvm;
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string TypeValue
        {
            get { return m_TypeValue; }
            set
            {
                m_TypeValue = value;
                OnPropertyChanged("TypeValue");
            }
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string JiDiValue
        {
            get { return m_JiDiValue; }
            set
            {
                m_JiDiValue = value;
                OnPropertyChanged("JiDiValue");
            }
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string TimeValue
        {
            get { return m_TimeValue; }
            set
            {
                m_TimeValue = value;
                OnPropertyChanged("TimeValue");
            }
        }

        /// <summary>
        ///     显示内容
        /// </summary>
        public string StateValue
        {
            get { return m_StateValue; }
            set
            {
                m_StateValue = value;
                OnPropertyChanged("StateValue");
            }
        }

        public event Action DataChanged;

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
                DataChanged();
            }

            lock (m_ReceiveLock)
            {
                foreach (var row in adt.Rows)
                {
                    TypeValue = row[m_DdDvm.TypeField.AsName].ToString();
                    JiDiValue = row[m_DdDvm.JiDiField.AsName].ToString();
                    TimeValue = row[m_DdDvm.TimeField.AsName].ToString();
                    StateValue = row[m_DdDvm.StateField.AsName].ToString();
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