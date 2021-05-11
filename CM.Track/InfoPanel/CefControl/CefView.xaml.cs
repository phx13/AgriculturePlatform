using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;

namespace CM.Track.InfoPanel.CefControl
{
    public partial class CefView : UserControl
    {
        /// <summary>
        /// 构造
        /// </summary>
        public CefView()
        {
            InitializeComponent();

            InitCef();
        }

        /// <summary>
        /// 初始化CEF相关
        /// </summary>
        private void InitCef()
        {
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
        /// 重置浏览器地址
        /// </summary>
        /// <param name="url"></param>
        public void SetUrl(string url)
        {
            this.TheWebBrowser.Address = url;
        }
    }
}
