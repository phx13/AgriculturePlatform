using System.Windows.Media;
using Microsoft.Practices.Prism.ViewModel;

namespace CM.Track.TopList
{
    public class EnumColorModel : NotificationObject
    {
        private SolidColorBrush m_EnumColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        private int m_EnumCount;
        private string m_EnumName = string.Empty;

        /// <summary>
        ///     类型
        /// </summary>
        public string EnumName
        {
            get { return m_EnumName; }
            set
            {
                m_EnumName = value;
                RaisePropertyChanged("EnumName");
            }
        }

        /// <summary>
        ///     数值
        /// </summary>
        public int EnumCount
        {
            get { return m_EnumCount; }
            set
            {
                m_EnumCount = value;
                RaisePropertyChanged("EnumCount");
            }
        }

        /// <summary>
        ///     颜色
        /// </summary>
        public SolidColorBrush EnumColor
        {
            get { return m_EnumColor; }
            set
            {
                m_EnumColor = value;
                RaisePropertyChanged("EnumColor");
            }
        }
    }
}