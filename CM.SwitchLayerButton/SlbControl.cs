using System;
using System.Collections.ObjectModel;
using System.Linq;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.SwitchLayerButton
{
    public class SlbControl : ChartControllerBase
    {
        /// <summary>
        ///     DVM
        /// </summary>
        private readonly SlbDvm m_SlbDvm;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public SlbControl(SlbDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            m_SlbDvm = dvm;
            Datas = new ObservableCollection<DataModel>();
            //Datas.Add(new DataModel() { ButtonName = "地块划分" });
            //Datas.Add(new DataModel() { ButtonName = "基地划分" });
            //Datas.Add(new DataModel() { ButtonName = "三区划定" });
        }

        /// <summary>
        ///     数据集合
        /// </summary>
        public ObservableCollection<DataModel> Datas { get; set; }

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count <= 0)
            {
                return;
            }

            foreach (var row in adt.Rows)
            {
                var buttonName = row[m_SlbDvm.ButtonName.AsName].ToString();
                var model = Datas.FirstOrDefault(d => d.ButtonName == buttonName);
                if (model == null)
                {
                    model = new DataModel();
                    model.ButtonName = buttonName;
                    Datas.Add(model);
                }

                var layerName = row[m_SlbDvm.LayerName.AsName].ToString();
                var layerGuid = row[m_SlbDvm.LayerGuid.AsName].ToString();

                Guid guid;
                if (Guid.TryParse(layerGuid, out guid))
                {
                    model.LayerInfo[guid] = layerName;
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