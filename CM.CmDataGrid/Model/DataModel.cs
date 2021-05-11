using System.Collections.ObjectModel;

namespace CM.CmDataGrid.Model
{
    public class DataModel
    {
        public DataModel()
        {
            Headers = new ObservableCollection<HeaderModel>();
            Columns = new ObservableCollection<ColumnModel>();
        }

        public ObservableCollection<HeaderModel> Headers { get; set; }

        public ObservableCollection<ColumnModel> Columns { get; set; }
    }
}