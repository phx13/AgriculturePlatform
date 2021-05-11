using System;
using System.Collections.ObjectModel;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.Track.TopList
{
    public class TopListController : ChartControllerBase
    {
        /// <summary>
        ///     Table委托
        /// </summary>
        public Action<AdapterDataTable> DatetableAction;

        public TopListController(TopListDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            TopListDvm = dvm;
        }

        /// <summary>
        ///     dvm
        /// </summary>
        public TopListDataViewModel TopListDvm { get; set; }

        #region DependencyProperty

        private double m_MyHeight;
        private double m_MyWidth;
        private ObservableCollection<EnumColorModel> m_ModelList = new ObservableCollection<EnumColorModel>();

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
        ///     枚举类
        /// </summary>
        public ObservableCollection<EnumColorModel> ModelList
        {
            get { return m_ModelList; }
            set
            {
                m_ModelList = value;
                OnPropertyChanged("ModelList");
            }
        }

        #endregion

        #region 重写

        public override void ReceiveData(AdapterDataTable adt)
        {
            DatetableAction(adt); //调用委托
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}