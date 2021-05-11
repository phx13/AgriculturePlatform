using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.Track.Recognition
{
    /// <summary>
    ///     RecognitionControl.xaml 的交互逻辑
    /// </summary>
    public partial class Recognition : ChartViewBase
    {
        /// <summary>
        /// 接收数据回调
        /// </summary>
        /// <param name="obj"></param>
        private void m_Controller_DataChanged(AdapterDataTable obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var btyarray = GetImageFromResponse(m_Controller.Image, null);
                var ms = new MemoryStream(btyarray);
                img.Source = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
            }));
        }

        /// <summary>
        /// 网络图片读取到内存流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static byte[] GetImageFromResponse(string url, string cookie = null)
        {
            var request = WebRequest.Create(url);
            if (!string.IsNullOrWhiteSpace(cookie))
            {
                request.Headers[HttpRequestHeader.Cookie] = cookie;
            }

            var response = request.GetResponse();
            byte[] bytes;
            using (var stream = response.GetResponseStream())
            {
                using (var ms = new MemoryStream())
                {
                    var buffer = new byte[1024];
                    var current = 0;
                    do
                    {
                        ms.Write(buffer, 0, current);
                    } while ((current = stream.Read(buffer, 0, buffer.Length)) != 0);

                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }

        /// <summary>
        ///     加载图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //public Image LoadImage(string path)
        //{
        //    Image image = null;

        //    if (string.IsNullOrEmpty(path))
        //    {
        //        return null;
        //    }

        //    var req = (HttpWebRequest) WebRequest.Create(path);
        //    req.Method = "GET";
        //    var response = req.GetResponse() as HttpWebResponse;
        //    var stm = response.GetResponseStream();
        //    image = Image.FromStream(stm);
        //    return image;
        //}

        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public Recognition(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = (RecognitionController)Controllers[0];
            DataContext = m_Controller;
            m_Controller.View = this;
            m_Controller.DataChanged += m_Controller_DataChanged;

            Loaded += RecognitionControl_Loaded;
        }

        /// <summary>
        ///     控件的加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecognitionControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
            }
            OnDadChartLoaded();
            m_IsLoaded = true;
            m_Controller.MyHeight = ActualHeight;
            m_Controller.MyWidth = ActualWidth;
        }

        //private void InitAnimation()
        //{
        //    m_DoubleAnimation.From = 0.3d;
        //    m_DoubleAnimation.To = 1;
        //    m_DoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
        //    m_DoubleAnimation.AutoReverse = true;
        //    m_DoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
        //    Storyboard.SetTarget(m_DoubleAnimation, mask);
        //    Storyboard.SetTargetProperty(m_DoubleAnimation, new PropertyPath(OpacityProperty));
        //    m_Storyboard.Children.Add(m_DoubleAnimation);
        //    m_Storyboard.Begin();
        //}

        #endregion

        #region 属性

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly RecognitionController m_Controller;

        public DispatcherTimer DispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 4) };

        private readonly Storyboard m_Storyboard = new Storyboard();

        private readonly DoubleAnimation m_DoubleAnimation = new DoubleAnimation();

        /// <summary>
        ///     是否加载过图表
        /// </summary>
        private bool m_IsLoaded;

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