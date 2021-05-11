using Microsoft.Practices.Prism.ViewModel;

namespace CM.CmDataGrid.Model
{
    public class CellModel : NotificationObject
    {
        private string m_Content;

        public string Content
        {
            get { return m_Content; }
            set
            {
                m_Content = value;
                RaisePropertyChanged("Content");
            }
        }
    }
}