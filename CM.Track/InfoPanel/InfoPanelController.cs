using System;
using System.Linq;
using System.Timers;
using System.Windows;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.Track.InfoPanel
{
    /// <summary>
    ///     控制器
    /// </summary>
    public class InfoPanelController : ChartControllerBase
    {
        #region 构造

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public InfoPanelController(InfoPanelDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DVM = dvm;
        }

        #endregion

        #region 属性

        public event Action<AdapterDataTable> DataChanged;

        private InfoPanelDataViewModel m_DVM;

        /// <summary>
        ///     DVM
        /// </summary>
        public InfoPanelDataViewModel DVM
        {
            get { return m_DVM; }
            set
            {
                m_DVM = value;
                OnPropertyChanged("DVM");
            }
        }

        private string m_Id;

        /// <summary>
        ///     地块编号
        /// </summary>
        public string Id
        {
            get { return m_Id; }
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string m_Farmer;

        /// <summary>
        ///     种养户
        /// </summary>
        public string Farmer
        {
            get { return m_Farmer; }
            set
            {
                m_Farmer = value;
                OnPropertyChanged("Farmer");
            }
        }

        private string m_UseType;

        /// <summary>
        ///     用途
        /// </summary>
        public string UseType
        {
            get { return m_UseType; }
            set
            {
                m_UseType = value;
                OnPropertyChanged("UseType");
            }
        }

        private string m_Area;

        /// <summary>
        ///     面积
        /// </summary>
        public string Area
        {
            get { return m_Area; }
            set
            {
                m_Area = value;
                OnPropertyChanged("Area");
            }
        }

        private string m_Town;

        /// <summary>
        ///     乡镇
        /// </summary>
        public string Town
        {
            get { return m_Town; }
            set
            {
                m_Town = value;
                OnPropertyChanged("Town");
            }
        }

        private string m_Product;

        /// <summary>
        ///     商品名称
        /// </summary>
        public string Product
        {
            get { return m_Product; }
            set
            {
                m_Product = value;
                OnPropertyChanged("Product");
            }
        }

        private string m_Count;

        /// <summary>
        ///     购买数量
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

        private string m_Amount;

        /// <summary>
        ///     单价
        /// </summary>
        public string Amount
        {
            get { return m_Amount; }
            set
            {
                m_Amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private string m_TotalPrice;

        /// <summary>
        ///     总价
        /// </summary>
        public string TotalPrice
        {
            get { return m_TotalPrice; }
            set
            {
                m_TotalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        private string m_BtPrice;

        /// <summary>
        ///     补贴金额
        /// </summary>
        public string BtPrice
        {
            get { return m_BtPrice; }
            set
            {
                m_BtPrice = value;
                OnPropertyChanged("BtPrice");
            }
        }

        private string m_ScPrice;

        /// <summary>
        ///     自付金额
        /// </summary>
        public string ScPrice
        {
            get { return m_ScPrice; }
            set
            {
                m_ScPrice = value;
                OnPropertyChanged("ScPrice");
            }
        }

        private string m_BtTotal;

        /// <summary>
        ///     年度补贴
        /// </summary>
        public string BtTotal
        {
            get { return m_BtTotal; }
            set
            {
                m_BtTotal = value;
                OnPropertyChanged("BtTotal");
            }
        }

        private string m_BtUsed;

        /// <summary>
        ///     已用补贴
        /// </summary>
        public string BtUsed
        {
            get { return m_BtUsed; }
            set
            {
                m_BtUsed = value;
                OnPropertyChanged("BtUsed");
            }
        }

        private string m_BtSurplus;

        /// <summary>
        ///     剩余补贴
        /// </summary>
        public string BtSurplus
        {
            get { return m_BtSurplus; }
            set
            {
                m_BtSurplus = value;
                OnPropertyChanged("BtSurplus");
            }
        }

        private string m_IndentTotal;

        /// <summary>
        ///     农药总采购量
        /// </summary>
        public string IndentTotal
        {
            get { return m_IndentTotal; }
            set
            {
                m_IndentTotal = value;
                OnPropertyChanged("IndentTotal");
            }
        }

        private string m_RecycleTotal;

        /// <summary>
        ///     回收量
        /// </summary>
        public string RecycleTotal
        {
            get { return m_RecycleTotal; }
            set
            {
                m_RecycleTotal = value;
                OnPropertyChanged("RecycleTotal");
            }
        }

        #endregion

        #region 重写

        /// <summary>
        ///     图表初始化和时间轴播放时，接收当前图表数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
            {
                return;
            }
            var row = adt.Rows.Last();
            Id = row[DVM.IdField.AsName].ToString();
            Farmer = row[DVM.FarmerField.AsName].ToString();
            UseType = row[DVM.UseTypeField.AsName].ToString();
            Area = row[DVM.AreaField.AsName].ToString();
            Town = row[DVM.TownField.AsName].ToString();
            Product = row[DVM.ProductField.AsName].ToString().Substring(0, 6);
            Count = row[DVM.CountField.AsName] + "元";
            Amount = row[DVM.AmountField.AsName] + "元";
            TotalPrice = row[DVM.TotalPriceField.AsName] + "元";
            BtPrice = row[DVM.BtPriceField.AsName] + "元";
            ScPrice = row[DVM.ScPriceField.AsName] + "元";
            BtTotal = row[DVM.BtTotalField.AsName] + "元";
            BtUsed = row[DVM.BtUsedField.AsName] + "元";
            BtSurplus = row[DVM.BtSurplusField.AsName] + "元";
            IndentTotal = row[DVM.IndentTotalField.AsName].ToString();
            RecycleTotal = row[DVM.RecycleTotalField.AsName].ToString();
            if (DataChanged != null)
                DataChanged(adt);
        }

        

        /// <summary>
        ///     刷新现有图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }

        /// <summary>
        ///     清空图表
        /// </summary>
        /// <param name="dvm"></param>
        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        #endregion
    }
}