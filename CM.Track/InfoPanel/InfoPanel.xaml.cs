using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using CefSharp;
using CM.Track.InfoPanel.CefControl;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.Track.InfoPanel
{
    /// <summary>
    ///     InfoPanelControl.xaml 的交互逻辑
    /// </summary>
    public partial class InfoPanel : ChartViewBase
    {
        #region 属性

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly InfoPanelController m_Controller;

        /// <summary>
        ///     是否加载过图表
        /// </summary>
        private bool m_IsLoaded;

        /// <summary>
        ///     显隐控制定时器
        /// </summary>
        private Timer m_Timer;

        #endregion

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public InfoPanel(ChartViewBaseModel model)
            : base(model)
        {
            InitCef();
            InitializeComponent();
            TheWebBrowser.FrameLoadEnd += TheWebBrowser_FrameLoadEnd;
            m_Controller = (InfoPanelController)Controllers[0];
            DataContext = m_Controller;
            Loaded += InfoPanelControl_Loaded;
            m_Controller.DataChanged += m_Controller_DataChanged;
            Visibility = Visibility.Hidden;

            m_Timer = new Timer(m_Controller.DVM.DurationData * 1000);
            m_Timer.AutoReset = false;
            m_Timer.Elapsed += M_Timer_Elapsed;
        }

        /// <summary>
        /// 接收数据回调
        /// </summary>
        /// <param name="obj"></param>
        private void m_Controller_DataChanged(AdapterDataTable obj)
        {
            ExcuteJs(m_Controller.Id);
            farmer.Text = m_Controller.Farmer;
            usetype.Text = m_Controller.UseType;
            area.Text = m_Controller.Area;
            town.Text = m_Controller.Town;
            count.Text = m_Controller.Count;
            amount.Text = m_Controller.Amount;
            totalprice.Text = m_Controller.TotalPrice;
            btprice.Text = m_Controller.BtPrice;
            scprice.Text = m_Controller.ScPrice;
            product.Text = m_Controller.Product;
            bttotal.Text = m_Controller.BtTotal;
            btused.Text = m_Controller.BtUsed;
            btsurplus.Text = m_Controller.BtSurplus;
            indenttotal.Text = m_Controller.IndentTotal;
            recycletotal.Text = m_Controller.RecycleTotal;
            Visibility = Visibility.Visible;
            this.TheWebBrowser.Visibility = Visibility.Visible;
            m_Timer.Start();
        }

        /// <summary>
        ///     定时器回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Visibility = Visibility.Hidden;
            }));

            m_Timer.Stop();
        }

        /// <summary>
        /// 处理自身
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer.Dispose();
                TheWebBrowser.GetBrowser().CloseBrowser(true);
            }
        }

        /// <summary>
        ///     控件的加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoPanelControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
                return;
            }

            OnDadChartLoaded();
            m_IsLoaded = true;

            this.TheWebBrowser.Address = "http://10.244.251.79/Index.aspx"; //设置浏览器地址
            TheWebBrowser.Height = 670;
        }

        #region Cef相关

        /// <summary>
        /// 初始化Cef相关
        /// </summary>
        private void InitCef()
        {
            if (Cef.IsInitialized)
            {
                return;
            }

            CefSettings setting = new CefSettings();
            var osVersion = Environment.OSVersion;
            if (osVersion.Version.Major == 6 && osVersion.Version.Minor == 1)
            {
                //开启GPU
                setting.CefCommandLineArgs.Add("disable-gpu", "1");
            }
            Cef.Initialize(setting, true, false);
        }
        /// <summary>
        /// 加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TheWebBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                String login =
                @"(function() {
                        var user = document.getElementById('txtUserName');
                        var pwd = document.getElementById('txtPassword');
                        var sub = document.getElementById('btnSubmit');
                        user.setAttribute('value','admin');
                        pwd.setAttribute('value','cmcm123456');
                        sub.click();
                    })()";
                e.Browser.MainFrame.ExecuteJavaScriptAsync(login); //login
            }

            if (e.Frame.IsMain)
            {
                String hide =
                @"(function() { 
                        var divs = document.body.children;
                        for(var i=0;i<divs.length;i++){
                            if(divs[i].id!='mapDiv'){
                                divs[i].style = 'display:none';
                            }
                        }
                        $('.esriScalebarRuler').hide();
                        $('.scaleLabelDiv').hide();
                    })()";
                e.Browser.MainFrame.ExecuteJavaScriptAsync(hide); //hide
                e.Browser.MainFrame.ExecuteJavaScriptAsync("document.body.style.overflow = 'hidden'"); //hidescrollbar
            }
        }

        private void ExcuteJs(string id)
        {
            TheWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync("top.locateHelper.findLotById(" + id + ")");
        }

        #endregion

        #region 重写

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }

        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        #endregion
    }
}