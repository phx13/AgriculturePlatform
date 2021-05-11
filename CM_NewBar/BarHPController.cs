using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.DataViewModels.Basic;
using Digihail.DAD3.Models.Interfaces;

namespace CM_NewBar
{
    /// <summary>
    /// 高性能柱图的控制器
    /// </summary>
    public class BarHPController : ChartControllerBase
    {
        private AdapterDataTable m_DT = new AdapterDataTable();
        /// <summary>
        /// 每次推送过来的数据
        /// </summary>
        public AdapterDataTable DT
        {
            get { return m_DT; }
            set
            {
                m_DT = value;
                OnPropertyChanged("DT");
            }
        }

        private bool m_IsMap = false;
        /// <summary>
        /// 是否为地图上添加柱图
        /// </summary>
        public bool IsMap
        {
            get { return m_IsMap; }
            set
            {
                m_IsMap = value;
                OnPropertyChanged("IsMap");
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public BarHPController(BarDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            this.DT = adt;
        }

        /// <summary>
        /// 刷新图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        /// 清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        /// 时间轴停止
        /// </summary>
        public override void OnAVEPlayerStoped()
        {
            if (!m_IsMap)
            {
                LoadInitDatas();
            }
        }
    }
}
