using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using System.Threading;
using Digihail.ScreenMatrix.Proxy;
using System;
using System.Net.Sockets;
using System.Text;

namespace CM.Track.SwitchTrack
{
    /// <summary>
    ///     SwitchTrackControl.xaml 的交互逻辑
    /// </summary>
    public partial class SwitchTrack : ChartViewBase
    {
        /// <summary>
        ///     乡镇按钮方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VillageButton_Click(object sender, RoutedEventArgs e)
        {
            m_VillageButton.IsChecked = false;
            m_VillageButton = sender as ToggleButton;

            if (m_VillageButton != null)
            {
                m_VillageButton.IsChecked = true;
                if (m_VillageButton.Content.ToString() == "重置")
                {
                    m_MessageManager.SendUnSelectedSettingInfo(m_PageIdList);
                }
                else if (m_VillageButton.Content.ToString() == "总体概览")
                {
                    m_MessageManager.SendStatuChangeInfo("总体");
                }
                else if (m_VillageButton.Content.ToString() == "绿色发展")
                {
                    m_MessageManager.SendStatuChangeInfo("绿色");
                }
                else if (m_VillageButton.Content.ToString() == "产业提升")
                {
                    m_MessageManager.SendStatuChangeInfo("产业");
                }
                else if (m_VillageButton.Content.ToString() == "PPT")
                {
                    new Thread(new ParameterizedThreadStart(this.ChangeScenario))
                    {
                        IsBackground = true
                    }.Start("gload 1 4\r\n");
                }
                else
                {
                    var vm =
                        m_Controller.VillageModelList
                            .ToList().FirstOrDefault(t => t.Village == m_VillageButton.Content.ToString());
                    if (vm != null)
                    {
                        var point = vm.Point;
                        m_MessageManager.SendTransLocationInfo(point, 11d, "控制端");
                    }

                    m_MessageManager.SendSelectedSettingInfo("Village", m_VillageButton.Content.ToString(), m_PageIdList);
                }
            }
        }

        private void ChangeScenario(object command)
        {
            try
            {
                WriteCommandWorker(command.ToString());
            }
            catch (Exception ex)
            {

            }
        }

        public void BeginWriteCommand(string command)
        {
            WriteCommandDelegate dele = new WriteCommandDelegate(this.WriteCommandWorker);
            dele.BeginInvoke(command, new AsyncCallback(this.WriteCommandFinished), null);
        }

        public string WriteCommandWorker(string command)
        {
            string serverIp = "192.168.0.3";
            int serverPort = 8000;
            string response = "";
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(serverIp, serverPort);
                NetworkStream clientStream = client.GetStream();
                byte[] requestBuffer = Encoding.ASCII.GetBytes(command);
                clientStream.Write(requestBuffer, 0, requestBuffer.Length);
                clientStream.Flush();
                Thread.Sleep(200);
                clientStream.Close();
                clientStream.Dispose();
                client.Close();
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        private void WriteCommandFinished(IAsyncResult result)
        {

        }

        private delegate string WriteCommandDelegate(string command);

        /// <summary>
        ///     视频按钮方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            m_VideoButton.IsChecked = false;
            m_VideoButton = sender as ToggleButton;

            if (m_VideoButton != null)
            {
                m_VideoButton.IsChecked = true;
                m_MessageManager.SendSwitchVideoMessage(m_VideoButton.Tag.ToString());
            }
        }

        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="model"></param>
        public SwitchTrack(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_MessageManager = new MessageManager();
            m_Controller = (SwitchTrackController)Controllers[0];
            DataContext = m_Controller;
            Loaded += SwitchTrackControl_Loaded;
        }

        /// <summary>
        ///     控件的加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchTrackControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_IsLoaded)
            {
            }
            OnDadChartLoaded();

            m_Controller.VillageModelList = new ObservableCollection<VillageModel>();
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "堡镇",
                Point = new Point(121.624896, 31.542242)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "陈家镇",
                Point = new Point(121.839144, 31.532304)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "城桥镇",
                Point = new Point(121.404574, 31.633902)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "港西镇",
                Point = new Point(121.422697, 31.693428)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "港沿镇",
                Point = new Point(121.662848, 31.593759)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "横沙乡",
                Point = new Point(121.848412, 31.346223)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "建设镇",
                Point = new Point(121.462006, 31.660874)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "绿华镇",
                Point = new Point(121.226967, 31.768727)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "庙镇",
                Point = new Point(121.356228, 31.71899)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "三星镇",
                Point = new Point(121.294288, 31.749217)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "竖新镇",
                Point = new Point(121.610358, 31.62061)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "向化镇",
                Point = new Point(121.730085, 31.526279)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "新村乡",
                Point = new Point(121.338402, 31.835599)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "新河镇",
                Point = new Point(121.531104, 31.587501)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "长兴镇",
                Point = new Point(121.700387, 31.396329)
            });
            m_Controller.VillageModelList.Add(new VillageModel
            {
                Village = "中兴镇",
                Point = new Point(121.764672, 31.52981)
            });

            var villageSortObj = new SortDescription("Village", ListSortDirection.Ascending);
            if (!VillageListBox.Items.SortDescriptions.Contains(villageSortObj))
            {
                VillageListBox.Items.SortDescriptions.Add(villageSortObj);
            }

            m_Controller.VideoModelList = new ObservableCollection<VideoModel>();
            m_Controller.VideoModelList.Add(new VideoModel
            {
                Video = "新弘",
                Channel = "1000004$1$0$0"
            });
            m_Controller.VideoModelList.Add(new VideoModel
            {
                Video = "万禾",
                Channel = "1000005$1$0$0"
            });
            m_Controller.VideoModelList.Add(new VideoModel
            {
                Video = "北湖",
                Channel = "1000006$1$0$0"
            });
            m_Controller.VideoModelList.Add(new VideoModel
            {
                Video = "春润",
                Channel = "1000007$1$0$0"
            });
            m_Controller.VideoModelList.Add(new VideoModel
            {
                Video = "新平",
                Channel = "1000008$1$0$0"
            });

            var videoSortObj = new SortDescription("Video", ListSortDirection.Ascending);
            if (!VideoListBox.Items.SortDescriptions.Contains(videoSortObj))
            {
                VideoListBox.Items.SortDescriptions.Add(videoSortObj);
            }

            m_IsLoaded = true;
            m_Controller.MyHeight = ActualHeight;
            m_Controller.MyWidth = ActualWidth;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     控制器
        /// </summary>
        private readonly SwitchTrackController m_Controller;

        /// <summary>
        ///     消息类实例
        /// </summary>
        private readonly MessageManager m_MessageManager;

        /// <summary>
        ///     是否加载过图表
        /// </summary>
        private bool m_IsLoaded;

        private readonly List<string> m_PageIdList = new List<string> { "bc090116-8847-416c-8d7d-65beef0df531" };

        /// <summary>
        ///     乡镇按钮实例
        /// </summary>
        private ToggleButton m_VillageButton = new ToggleButton();

        /// <summary>
        ///     视频按钮实例
        /// </summary>
        private ToggleButton m_VideoButton = new ToggleButton();

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