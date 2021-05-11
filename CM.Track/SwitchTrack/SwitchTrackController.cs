using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.CCP4.Helper;
using Digihail.CCP4.Models.LauncherMessage;
using Digihail.CCPSOE.Models.Group;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;
using Microsoft.Practices.Prism.ViewModel;

namespace CM.Track.SwitchTrack
{
    /// <summary>
    ///     控制器
    /// </summary>
    public class SwitchTrackController : ChartControllerBase
    {
        #region 构造

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public SwitchTrackController(SwitchTrackDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
        }

        #endregion

        #region 属性

        /// <summary>
        ///     面板高度
        /// </summary>
        public double MyHeight
        {
            get { return m_MyHeight; }
            set
            {
                m_MyHeight = value;
                OnPropertyChanged("MyHeight");
            }
        }

        /// <summary>
        ///     面板宽度
        /// </summary>
        public double MyWidth
        {
            get { return m_MyWidth; }
            set
            {
                m_MyWidth = value;
                OnPropertyChanged("MyWidth");
            }
        }

        /// <summary>
        ///     乡镇列表
        /// </summary>
        public ObservableCollection<VillageModel> VillageModelList
        {
            get { return m_VillageModelList; }
            set
            {
                m_VillageModelList = value;
                OnPropertyChanged("VillageModelList");
            }
        }

        private ObservableCollection<VillageModel> m_VillageModelList = new ObservableCollection<VillageModel>();

        /// <summary>
        ///     视频列表
        /// </summary>
        public ObservableCollection<VideoModel> VideoModelList
        {
            get { return m_VideoModelList; }
            set
            {
                m_VideoModelList = value;
                OnPropertyChanged("VideoModelList");
            }
        }

        private ObservableCollection<VideoModel> m_VideoModelList = new ObservableCollection<VideoModel>();

        private double m_MyHeight;
        private double m_MyWidth;

        #endregion

        #region 重写

        /// <summary>
        ///     图表初始化和时间轴播放时，接收当前图表数据
        /// </summary>
        /// <param name="adt"></param>
        public override void ReceiveData(AdapterDataTable adt)
        {
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

    /// <summary>
    ///     乡镇模型
    /// </summary>
    public class VillageModel : NotificationObject
    {
        private Point m_Point;
        private string m_Village = string.Empty;

        public string Village
        {
            get { return m_Village; }
            set
            {
                m_Village = value;
                RaisePropertyChanged("Village");
            }
        }

        public Point Point
        {
            get { return m_Point; }
            set
            {
                m_Point = value;
                RaisePropertyChanged("Point");
            }
        }
    }

    /// <summary>
    ///     视频模型
    /// </summary>
    public class VideoModel : NotificationObject
    {
        private string m_Video;

        public string Video
        {
            get { return m_Video; }
            set
            {
                m_Video = value;
                RaisePropertyChanged("Video");
            }
        }

        private string m_Channel;

        public string Channel
        {
            get { return m_Channel; }
            set
            {
                m_Channel = value;
                RaisePropertyChanged("Channel");
            }
        }
    }

    /// <summary>
    ///     消息管理类
    /// </summary>
    public class MessageManager
    {
        private MessageAggregator m_MessageAggregator;

        /// <summary>
        ///     消息机制
        /// </summary>
        public MessageAggregator MessageAggregator
        {
            get
            {
                if (null == m_MessageAggregator)
                {
                    m_MessageAggregator = new MessageAggregator();
                }
                return m_MessageAggregator;
            }
        }

        #region 发送消息

        /// <summary>
        ///     筛选消息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="pageIdList"></param>
        public void SendSelectedSettingInfo(string column, string value, List<string> pageIdList)
        {
            var selectData = new SelectedSettingInfoData
            {
                OperationType = "选中",
                ColumnName = column,
                Value = value,
                PageIdList = pageIdList
            };
            MessageAggregator.GetMessage<SelectedSettingMessage>().Publish(selectData);
        }

        /// <summary>
        ///     取消筛选消息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <param name="pageIdList"></param>
        public void SendUnSelectedSettingInfo(List<string> pageIdList)
        {
            var selectData = new SelectedSettingInfoData
            {
                OperationType = "取消选中",
                PageIdList = pageIdList
            };
            MessageAggregator.GetMessage<SelectedSettingMessage>().Publish(selectData);
        }

        /// <summary>
        ///     跟随消息
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="mapLevel"></param>
        /// <param name="pageName"></param>
        public void SendTransLocationInfo(Point centerPoint, double mapLevel, string pageName)
        {
            var moveData = new Gis2DMoveModel
            {
                CenterLong = centerPoint.X,
                CenterLat = centerPoint.Y,
                MapLevel = mapLevel,
                PageName = pageName
            };
            MessageAggregator.GetMessage<Gis2DMoveMessage>().Publish(moveData);
        }

        public void SendDeleteObjectMessage(bool isAct)
        {
            MessageAggregator.GetMessage<DeleteObjectMessage>().Publish(isAct);
        }

        /// <summary>
        ///     切状态消息
        /// </summary>
        /// <param name="statu"></param>
        public void SendStatuChangeInfo(string statu)
        {
            if (CommonManager.IsAutoSkipStatus)
            {
                var statusModel = PresentationStatusManager.Instance.StatusList.FirstOrDefault(t => t.Name.Equals(statu));
                if (statusModel != null)
                {
                    CCPHelper.Instance.OpenStatusLayout(statusModel);
                }
                var changeStatudata = new ObjectGroupFollowData(statu, Guid.NewGuid(), Guid.NewGuid());
                MessageAggregator.GetMessage<ObjectGroupFollowMessage>().Publish(changeStatudata);
            }
        }

        public void SendSwitchVideoMessage(string channelId)
        {
            MessageAggregator.GetMessage<VideoChangedMessage>().Publish(channelId);
        }

        #endregion
    }

    public class VideoChangedMessage : CompositeCommunictionMessage<string>
    {
    }

    public class DeleteObjectMessage : CompositeCommunictionMessage<bool>
    {
    }
}