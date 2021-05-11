using System.Collections.ObjectModel;

namespace CM.CmDataGrid.Model
{
    public class RowModel
    {
        public RowModel()
        {
            Cells = new ObservableCollection<CellModel>();
            HeaderName = "";
        }

        public string HeaderName { get; set; }

        public ObservableCollection<CellModel> Cells { get; set; }
    }
}