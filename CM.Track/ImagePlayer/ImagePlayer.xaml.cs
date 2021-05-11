using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.Track.ImagePlayer
{
    /// <summary>
    ///     ImagePlayerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImagePlayer : ChartViewBase
    {
        private int index;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (index >= m_FilesCollection.Count)
            {
                index = 0;
            }
            img.Source = LoadImage(m_Files[index]);
            index++;
        }

        /// <summary>
        ///     加载图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public BitmapImage LoadImage(string path)
        {
            BitmapImage image = null;

            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            if (!File.Exists(path))
            {
                return null;
            }

            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var binReader = new BinaryReader(fs);

                var fileInfo = new FileInfo(path);

                var bytes = binReader.ReadBytes((int) fileInfo.Length);

                binReader.Close();

                image = new BitmapImage();

                image.BeginInit();
                image.StreamSource = new MemoryStream(bytes);

                image.EndInit();
            }
            return image;
        }

        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public ImagePlayer(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = (ImagePlayerController) Controllers[0];
            DataContext = m_Controller;
            m_Controller.View = this;
            Loaded += ImagePlayerControl_Loaded;
            DispatcherTimer.Tick += dispatcherTimer_Tick;
            DispatcherTimer.Start();
        }

        /// <summary>
        ///     控件的加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImagePlayerControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
            }
            OnDadChartLoaded();

            var catalog = m_Controller.DVM.Catalog;
            m_Files = Directory.GetFiles(catalog);

            m_FilesCollection = m_Files.ToList();
            if (m_FilesCollection.Contains("C:\\AVCFiles\\Applications\\Photos\\Thumbs.db"))
            {
                m_FilesCollection.Remove("C:\\AVCFiles\\Applications\\Photos\\Thumbs.db");
            }

            img.Source = LoadImage(m_FilesCollection[0]);
            m_IsLoaded = true;
            m_Controller.MyHeight = ActualHeight;
            m_Controller.MyWidth = ActualWidth;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly ImagePlayerController m_Controller;

        private string[] m_Files;
        private List<string> m_FilesCollection = new List<string>();
        public DispatcherTimer DispatcherTimer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 4)};

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