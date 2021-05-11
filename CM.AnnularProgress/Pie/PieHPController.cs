using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.AnnularProgress.Pie
{
    /// <summary>
    ///     高性能饼图的控制器
    /// </summary>
    public class PieHPController : ChartControllerBase
    {
        private AdapterDataTable m_DT = new AdapterDataTable();

        private bool m_IsMap;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public PieHPController(PieDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
        }

        /// <summary>
        ///     每次推送过来的数据
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

        /// <summary>
        ///     是否为地图上添加柱图
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
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            DT = adt;
        }

        /// <summary>
        ///     刷新图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
            var d = dvm as PieDataViewModel;
            if (d != null && d.LegendStyle != null && d.LegendStyle.SolutionColorList != null)
            {
                d.LegendStyle.SolutionColorList.Clear();
            }
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     时间轴停止
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