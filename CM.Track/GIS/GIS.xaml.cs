using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using CefSharp;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.Track.GIS
{
    /// <summary>
    ///     GISControl.xaml 的交互逻辑
    /// </summary>
    public partial class GIS : ChartViewBase
    {
        #region 属性

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly GISController m_Controller;

        /// <summary>
        ///     是否加载过图表
        /// </summary>
        private bool m_IsLoaded;
        #endregion

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public GIS(ChartViewBaseModel model)
            : base(model)
        {
            InitCef();
            InitializeComponent();
            TheWebBrowser.FrameLoadEnd += TheWebBrowser_FrameLoadEnd;
            m_Controller = (GISController)Controllers[0];
            DataContext = m_Controller;
            Loaded += GISControl_Loaded;
        }

        /// <summary>
        /// 处理自身
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
                TheWebBrowser.GetBrowser().CloseBrowser(true);
        }

        /// <summary>
        ///     控件的加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GISControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
                return;
            }

            OnDadChartLoaded();
            m_IsLoaded = true;

            this.TheWebBrowser.Address = "http://10.244.251.79/Index.aspx"; //设置浏览器地址
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

    public enum TypeId
    {
        粮田=1,
    }

    public struct Param
    {

    }
}