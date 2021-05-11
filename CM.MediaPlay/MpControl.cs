using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.MediaPlay
{
    public class MpControl : ChartControllerBase
    {
        private double m_MyHeight;
        private double m_MyWidth;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public MpControl(MpDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
        }

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

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }
    }
}