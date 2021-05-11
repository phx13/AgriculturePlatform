using Microsoft.Practices.Prism.ViewModel;

namespace CM.CmDataGrid.Model
{
    public class HeaderModel : NotificationObject
    {
        private string m_HeaderName = "";
        private string m_HeaderText = "";

        private string m_HeaderWidth = "";

        /// <summary>
        ///     列别名
        /// </summary>
        public string HeaderText
        {
            get { return m_HeaderText; }
            set
            {
                m_HeaderText = value;
                RaisePropertyChanged("HeaderText");
            }
        }

        public string HeaderWidth
        {
            get { return m_HeaderWidth; }
            set
            {
                m_HeaderWidth = value;
                RaisePropertyChanged("HeaderWidth");
            }
        }

        /// <summary>
        ///     原始的列名称
        /// </summary>
        public string HeaderName
        {
            get { return m_HeaderName; }
            set
            {
                m_HeaderName = value;
                RaisePropertyChanged("HeaderName");
            }
        }
    }
}