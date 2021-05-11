using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;

namespace CM.CmDataGrid.Model
{
    public class ColumnModel : NotificationObject
    {
        private string m_HeaderWidth = "";

        public ColumnModel()
        {
            Cells = new ObservableCollection<CellModel>();
            HeaderName = "";
        }

        public string HeaderName { get; set; }

        public string HeaderWidth
        {
            get { return m_HeaderWidth; }
            set
            {
                m_HeaderWidth = value;
                RaisePropertyChanged("HeaderWidth");
            }
        }


        public ObservableCollection<CellModel> Cells { get; set; }
    }
}