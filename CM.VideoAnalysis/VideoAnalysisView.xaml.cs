using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Text;
using CM.Track.SwitchTrack;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;

namespace CM.VideoAnalysis
{
    public partial class VideoAnalysisView : ChartViewBase
    {
        private readonly DoubleAnimation m_Animation;
        private readonly VideoAnalysisDvm m_DVM;
        private VideoAnalysisControl m_Controller;
        private Timer m_Timer;
        private string m_Channel;
        private Get_RealStream_Info_t m_Info;

        public VideoAnalysisView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();
            m_Controller = Controllers[0] as VideoAnalysisControl;
            m_DVM = (VideoAnalysisDvm)model.DataViewModels[0];
            DataContext = m_DVM;
            MessageAggregator m_MessageAggregator = new MessageAggregator();
            m_MessageAggregator.GetMessage<VideoChangedMessage>().Subscribe(RevVideoChanged);
            Loaded += VideoAnalysisView_Loaded;
            //Loaded += (s, e) => { OnDadChartLoaded(); };
        }

        private ObservableCollection<ImageInfo> data;

        void VideoAnalysisView_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();
            var createResult = DPSDK_Create(dpsdk_sdk_type_e.DPSDK_CORE_SDK_SERVER, ref this.nPDLLHandle);//初始化数据交互接口
            var initResult = DPSDK_InitExt();//初始化解码播放接口
            if (createResult == (IntPtr)0 && initResult == (IntPtr)0)
            {
                Login_Info_t loginInfo = new Login_Info_t
                {
                    szIp = "112.25.210.130",
                    nPort = (uint)9000,
                    szUsername = "CMNW",
                    szPassword = "shcmnw123",
                    nProtocol = dpsdk_protocol_version_e.DPSDK_PROTOCOL_VERSION_II,
                    iType = 1
                };
                var loginResult = DPSDK_Login(this.nPDLLHandle, ref loginInfo, (IntPtr)10000);
                if (loginResult == (IntPtr)0)
                {
                    IntPtr loadGroupResult = DPSDK_LoadDGroupInfo(nPDLLHandle, ref nGroupLen, (IntPtr)60000);
                    if (loadGroupResult == (IntPtr)0)
                    {
                        OpenVideo("1000004$1$0$0");
                    }
                }
            }
        }

        /// <summary>
        /// 设置动画
        /// </summary>
        void Run()
        {
            //动画速率
            double speed = 0.0;
            //定时器，定时修改ImageInfo中各属性，从而实现动画效果
            DispatcherTimer timer = new DispatcherTimer();
            //时间间隔
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (ss, ee) =>
            {
                #region
                //设置圆心x,y
                double centerX = this.ActualWidth / 2;
                double centerY = this.ActualHeight / 2 - 50;//减去适当的值，因为会设置最下面的图片最大，上面的图片较小

                //设置图片最大的宽、高
                double maxWidth = 400;
                double maxHeight = 300;

                //设置椭圆的长边和短边
                double a = centerX - maxWidth / 2.0;
                double b = centerY - maxHeight / 2.0;

                //运动一周后恢复为0
                speed = speed < 360 ? speed : 0.0;
                //运运的速度 此“增值”和  timer.Interval对动画的流畅性有影响
                speed += 1.2;
                //运动距离，即运动的弧度数;
                var ainhd = speed * Math.PI / 180;
                //每个图片之间相隔的角度
                var angle = (360.0 / data.Count) * Math.PI / 180.0;
                //图片序号
                int index = 0;
                foreach (var img in data)
                {
                    //最下面一张图ZIndex最大，Opacity最大，长宽最大

                    img.ZIndex = (int)img.Top;
                    //当前图片与最下面一张图片的Y的比值
                    var allpers = img.Top / (centerY + b);
                    //不要小于0.2，太小了就看不见了，可以适当调整
                    allpers = Math.Max(0.6, allpers);
                    //设置图片大小
                    img.Width = allpers * maxWidth;
                    img.Height = allpers * maxHeight;
                    //设置透明度
                    img.Opactity = Math.Max(allpers * 1.5, 0.4);

                    //公式：x=sin * a //+ centerX因为默认wpf默认左上角为坐标原点；//- img.Width / 2.0是以图片中心点作为运动轨迹
                    img.Left = Math.Sin((angle * index + ainhd)) * a + centerX - img.Width / 2.0;//x=sin * a
                    //y=cos * b
                    img.Top = Math.Cos((angle * index + ainhd)) * b + centerY - img.Height / 2.0;

                    index++;
                }
                #endregion
            };
            //启动计时器，开始动画
            timer.Start();
        }

        private void RevVideoChanged(string obj)
        {
            m_Channel = obj;
            DPSDK_StopRealplayBySeq(nPDLLHandle, realseq, (IntPtr)10000);
            OpenVideo(m_Channel);
        }

        private void OpenVideo(string id)
        {
            //HwndSource hwndSource = (HwndSource)PresentationSource.FromVisual(realvideo);
            //if (hwndSource != null)
            //{
            //    IntPtr handle = hwndSource.Handle;
            //    m_Info = new Get_RealStream_Info_t
            //    {
            //        szCameraId = id,
            //        nRight = dpsdk_check_right_e.DPSDK_CORE_NOT_CHECK_RIGHT,
            //        nStreamType = dpsdk_stream_type_e.DPSDK_CORE_STREAMTYPE_MAIN,
            //        nMediaType = dpsdk_media_type_e.DPSDK_CORE_MEDIATYPE_ALL,
            //        nTransType = dpsdk_trans_type_e.DPSDK_CORE_TRANSTYPE_TCP
            //    };
            //    DPSDK_StartRealplay(nPDLLHandle, out realseq, ref m_Info, handle, (IntPtr)10000);
            //}
            IntPtr handle = realvideo.Handle;
            m_Info = new Get_RealStream_Info_t
            {
                szCameraId = id,
                nRight = dpsdk_check_right_e.DPSDK_CORE_NOT_CHECK_RIGHT,
                nStreamType = dpsdk_stream_type_e.DPSDK_CORE_STREAMTYPE_MAIN,
                nMediaType = dpsdk_media_type_e.DPSDK_CORE_MEDIATYPE_ALL,
                nTransType = dpsdk_trans_type_e.DPSDK_CORE_TRANSTYPE_TCP
            };
            DPSDK_StartRealplay(nPDLLHandle, out realseq, ref m_Info, handle, (IntPtr)10000);

        }

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr DPSDK_Create(dpsdk_sdk_type_e nType, ref IntPtr nPDLLHandle);

        public enum dpsdk_sdk_type_e
        {
            /// <summary>
            /// 服务模式使用
            /// </summary>
            DPSDK_CORE_SDK_SERVER = 1
        }

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr DPSDK_InitExt();

        private IntPtr nPDLLHandle = (IntPtr)0;
        public IntPtr nGroupLen = IntPtr.Zero;
        private IntPtr realseq = default(IntPtr);

        /// <summary>
        /// 登录信息
        /// </summary>
        public struct Login_Info_t
        {
            /// <summary>
            /// 服务IP
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 46)]
            public string szIp;
            /// <summary>
            /// 服务端口
            /// </summary>
            public uint nPort;
            /// <summary>
            /// 用户名
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szUsername;
            /// <summary>
            /// 密码
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szPassword;
            /// <summary>
            /// 协议库类型
            /// </summary>
            public dpsdk_protocol_version_e nProtocol;
            /// <summary>
            /// 登陆类型，1为PC客户端, 2为手机客户端
            /// </summary>
            public uint iType;
        }

        public enum dpsdk_protocol_version_e
        {
            /// <summary>
            /// 一代协议
            /// </summary>
            DPSDK_PROTOCOL_VERSION_I = 1,
            /// <summary>
            /// 二代协议
            /// </summary>
            DPSDK_PROTOCOL_VERSION_II = 2,
        }

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr DPSDK_Login(IntPtr nPDLLHandle, ref Login_Info_t pLoginInfo, IntPtr nTimeout);

        [StructLayout(LayoutKind.Sequential)]
        public struct Get_RealStream_Info_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szCameraId;                                                           // 通道ID
            public dpsdk_check_right_e nRight;										// 是否检测权限
            public dpsdk_stream_type_e nStreamType;								// 码流类型 参考dpsdk_stream_type_e；预览接口，此参数为分割数，参考dpsdk_stream_real_video_slite_e
            public dpsdk_media_type_e nMediaType;									// 媒体类型
            public dpsdk_trans_type_e nTransType;									// 传输类型
            public int nTrackID;                                   // 拉流TrackID，默认0
        }

        public enum dpsdk_check_right_e
        {
            DPSDK_CORE_CHECK_RIGHT = 0,					    // 检查
            DPSDK_CORE_NOT_CHECK_RIGHT = 1,					// 不检查
        }

        // 码流类型
        public enum dpsdk_stream_type_e
        {
            DPSDK_CORE_STREAMTYPE_MAIN = 1,					 // 主码流
            DPSDK_CORE_STREAMTYPE_SUB = 2,					 // 辅码流
        }

        // 媒体类型
        public enum dpsdk_media_type_e
        {
            DPSDK_CORE_MEDIATYPE_VIDEO = 1,					 // 视频
            DPSDK_CORE_MEDIATYPE_AUDI = 2,					 // 音频
            DPSDK_CORE_MEDIATYPE_ALL = 3,					 // 音频 + 视频
        }

        // 传输类型
        public enum dpsdk_trans_type_e
        {
            DPSDK_CORE_TRANSTYPE_UDP = 0,					 // UDP
            DPSDK_CORE_TRANSTYPE_TCP = 1,					 // TCP
        }

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        public static extern IntPtr DPSDK_StartRealplay(IntPtr nPDLLHandle, out IntPtr nRealSeq, ref Get_RealStream_Info_t pGetInfo, IntPtr hwnd, IntPtr nTimeout);

        [DllImport("DPSDK_Ext.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_StopRealplayBySeq(IntPtr nPDLLHandle, IntPtr nRealSeq, IntPtr nTimeout);

        [DllImport("DPSDK_Core.dll", CharSet = CharSet.Ansi)]
        private extern static IntPtr DPSDK_LoadDGroupInfo(IntPtr nPDLLHandle, ref IntPtr nGroupLen, IntPtr nTimeout);

        #region override
        public override void ClearSelectedItem(ClearSelectedItemModel clearModel)
        {
        }

        public override void ExportChart(ExportType type)
        {
        }

        public override void ReceiveData(Dictionary<string, AdapterDataTable> adtList)
        {
        }

        public override void RefreshStyle()
        {
        }

        public override void RefreshStyle(PropertyDescription propertyDescription)
        {
        }

        public override void SetSelectedItem(SetSelectedItemModel selectedModel)
        {
        }
        #endregion

        private void listview_Loaded(object sender, RoutedEventArgs e)
        {
            //准备数据源 ObservableCollection<T>
            data = new ObservableCollection<ImageInfo>();
            //获取程序所在目录中的images文件夹
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "images");
            //添加此目录中的图片文件到data中
            foreach (var file in di.GetFiles())
            {
                //验证是否为图片文件 （可以写一个方法来进行验证，此处仅支持jpg和png）
                if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".png")
                {
                    data.Add(new ImageInfo(file.FullName));
                }
            }
            //设置ListView的ItemsSource
            listview.ItemsSource = data;
            Run();
        }

        private void realvideo_Loaded(object sender, RoutedEventArgs e)
        {
            var createResult = DPSDK_Create(dpsdk_sdk_type_e.DPSDK_CORE_SDK_SERVER, ref this.nPDLLHandle);//初始化数据交互接口
            var initResult = DPSDK_InitExt();//初始化解码播放接口
            if (createResult == (IntPtr)0 && initResult == (IntPtr)0)
            {
                Login_Info_t loginInfo = new Login_Info_t
                {
                    szIp = "112.25.210.130",
                    nPort = (uint)9000,
                    szUsername = "CMNW",
                    szPassword = "shcmnw123",
                    nProtocol = dpsdk_protocol_version_e.DPSDK_PROTOCOL_VERSION_II,
                    iType = 1
                };
                var loginResult = DPSDK_Login(this.nPDLLHandle, ref loginInfo, (IntPtr)10000);
                if (loginResult == (IntPtr)0)
                {
                    IntPtr loadGroupResult = DPSDK_LoadDGroupInfo(nPDLLHandle, ref nGroupLen, (IntPtr)60000);
                    if (loadGroupResult == (IntPtr)0)
                    {
                        OpenVideo("1000004$1$0$0");
                    }
                }
            }
        }
    }

    public class ImageInfo : INotifyPropertyChanged
    {
        private int _zIndex;

        public int ZIndex
        {
            get { return _zIndex; }
            set
            {
                if (value != _zIndex)
                {
                    _zIndex = value;
                    this.NotifyPropertyChanged("ZIndex");
                }
            }
        }
        private double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                if (value != _left)
                {
                    _left = value;
                    this.NotifyPropertyChanged("Left");
                }
            }
        }
        private double _top;

        public double Top
        {
            get { return _top; }
            set
            {
                if (value != _top)
                {
                    _top = value;
                    this.NotifyPropertyChanged("Top");
                }
            }
        }
        private double _width;

        public double Width
        {
            get { return _width; }
            set
            {
                if (value != _width)
                {
                    _width = value;
                    this.NotifyPropertyChanged("Width");
                }
            }
        }
        private double _height;

        public double Height
        {
            get { return _height; }
            set
            {
                if (value != _height)
                {
                    _height = value;
                    this.NotifyPropertyChanged("Height");
                }
            }
        }

        private double _opacity = 1.0;

        public double Opactity
        {
            get { return _opacity; }
            set
            {
                if (value != _opacity)
                {
                    _opacity = value;
                    this.NotifyPropertyChanged("Opactity");
                }
            }
        }

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                if (value != _imagePath)
                {
                    _imagePath = value;
                    this.NotifyPropertyChanged("ImagePath");
                }
            }
        }

        public ImageInfo(string path)
        {
            this.ImagePath = path;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}