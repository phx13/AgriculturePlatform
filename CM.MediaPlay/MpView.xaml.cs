using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using CM.Track.SwitchTrack;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.MediaPlay
{
    public partial class MpView : ChartViewBase, IDisposable
    {
        private readonly MpControl m_Controller;

        private readonly MessageAggregator m_MessageAggregator;
        private readonly MpDvm m_MpDvm;

        private int m_CurrentIndex;

        private string[] m_HisFiles;

        public MpView(ChartViewBaseModel model)
            : base(model)
        {
            InitializeComponent();

            m_Controller = (MpControl) Controllers[0];

            DataContext = m_Controller;

            Loaded += MpView_Loaded;

            m_MessageAggregator = new MessageAggregator();
            //m_MessageAggregator.GetMessage<VideoChangedMessage>().Subscribe(RevVideoChanged);

            m_MpDvm = (MpDvm) DataViewModels[0];
        }

        void IDisposable.Dispose()
        {
            winFormsHost.Child.Dispose();
        }

        private void RevVideoChanged(bool obj)
        {
            if (m_MpDvm.ActMediaCatalog != "" && m_MpDvm.HisMediaCatalog != "")
            {
                if (obj)
                {
                    Act.Visibility = Visibility.Visible;
                    His.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Act.Visibility = Visibility.Collapsed;
                    His.Visibility = Visibility.Visible;
                }
            }
        }

        private void MpView_Loaded(object sender, RoutedEventArgs e)
        {
            OnDadChartLoaded();

            m_Controller.MyHeight = ActualHeight;
            m_Controller.MyWidth = ActualWidth;

            var hisCatalog = m_MpDvm.HisMediaCatalog;

            m_HisFiles = Directory.GetFiles(hisCatalog);

            if (m_HisFiles.Length <= 0)
            {
                return;
            }

            if (m_MpDvm.IsAct)
            {
                Act.Visibility = Visibility.Visible;
                His.Visibility = Visibility.Collapsed;
                //var form = new Form1();
                //winFormsHost.Child = form;
            }
            else
            {
                Act.Visibility = Visibility.Collapsed;
                His.Visibility = Visibility.Visible;
            }

            HisMedia.MediaEnded += HisMedia_MediaEnded;
            HisMedia.Source = new Uri(m_HisFiles[0], UriKind.RelativeOrAbsolute);
            HisMedia.RenderSize = new Size(m_Controller.MyWidth, m_Controller.MyHeight);
            HisMedia.Play();
        }

        private void HisMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            m_CurrentIndex++;
            if (m_CurrentIndex < m_HisFiles.Length)
            {
                HisMedia.Source = new Uri(m_HisFiles[m_CurrentIndex], UriKind.RelativeOrAbsolute);
            }
            else
            {
                m_CurrentIndex = 0;
                HisMedia.Source = new Uri(m_HisFiles[m_CurrentIndex], UriKind.RelativeOrAbsolute);
            }
            HisMedia.RenderSize = new Size(m_Controller.MyWidth, m_Controller.MyHeight);

            HisMedia.Stop();
            HisMedia.Play();
        }


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
    }
}