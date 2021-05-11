using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.AnnularProgress.ShowText
{
    public class ShowTextControl : ChartControllerBase
    {
        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public ShowTextControl(ShowTextDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = dvm;
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

            foreach (var row in adt.Rows)
            {
                var str = row[m_DdDvm.TextField.AsName].ToString();

                str = str.Replace("</br>", "");
                str = str.Replace(" ", "");

                TextValue = str;
            }
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        #region Prop

        private ShowTextDvm m_DdDvm;

        /// <summary>
        ///     显示内容
        /// </summary>
        public ShowTextDvm DVM
        {
            get { return m_DdDvm; }
            set
            {
                m_DdDvm = value;
                OnPropertyChanged("DVM");
            }
        }

        private string m_TextValue;

        /// <summary>
        ///     显示内容
        /// </summary>
        public string TextValue
        {
            get { return m_TextValue; }
            set
            {
                m_TextValue = value;
                OnPropertyChanged("TextValue");
            }
        }

        #endregion
    }
}