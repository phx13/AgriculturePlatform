using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using CM.CmDataGrid.Model;
using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.CmDataGrid
{
    public class DgControl : ChartControllerBase
    {
        /// <summary>
        ///     线程锁
        /// </summary>
        private static readonly object m_Lock = new object();

        private DataModel m_Datas = new DataModel();

        private DgDvm m_DgDvm;

        /// <summary>
        /// </summary>
        private Timer m_Timer;

        /// <summary>
        ///     构造
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        public DgControl(DgDvm dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            DgDvm = dvm;

            if (DgDvm.TableColumns.Columns.Count == 0)
            {
                foreach (var model in DgDvm.ColumnFields)
                {
                    var header = new HeaderModel();
                    header.HeaderName = model.AsName;
                    header.HeaderText = model.ColumnName;
                    header.HeaderWidth = "Auto";
                    Datas.Headers.Add(header);

                    var row = new ColumnModel();
                    row.HeaderName = header.HeaderName;
                    row.HeaderWidth = header.HeaderWidth;
                    Datas.Columns.Add(row);
                }
            }
            else
            {
                foreach (var model in DgDvm.TableColumns.Columns)
                {
                    var header = new HeaderModel();
                    header.HeaderName = model.Column.AsName;
                    header.HeaderText = model.Title;
                    header.HeaderWidth = model.Length;
                    Datas.Headers.Add(header);

                    var row = new ColumnModel();
                    row.HeaderName = header.HeaderName;
                    row.HeaderWidth = header.HeaderWidth;
                    Datas.Columns.Add(row);
                }
            }

            DgDvm.PropertyChanged += m_DgDvm_PropertyChanged;


            m_Timer = new Timer(TimerCallback, null, 0, m_DgDvm.TimerInterval);
        }

        public DataModel Datas
        {
            get { return m_Datas; }
            set
            {
                m_Datas = value;
                OnPropertyChanged("Datas");
            }
        }

        public DgDvm DgDvm
        {
            get { return m_DgDvm; }
            set
            {
                m_DgDvm = value;
                OnPropertyChanged("DgDvm");
            }
        }

        /// <summary>
        ///     定时器回调
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallback(object obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var row = new List<CellModel>();
                foreach (var column in Datas.Columns)
                {
                    if (column.Cells.Count <= 0)
                    {
                        return;
                    }

                    row.Add(column.Cells[0]);
                    column.Cells.RemoveAt(0);
                }

                for (var i = 0; i < row.Count; i++)
                {
                    Datas.Columns[i].Cells.Add(row[i]);
                }
            });
        }

        /// <summary>
        ///     列头数据更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_DgDvm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TableColumns")
            {
                Datas.Headers.Clear();

                var rows = Datas.Columns;

                Datas.Columns = new ObservableCollection<ColumnModel>();

                foreach (var model in DgDvm.TableColumns.Columns)
                {
                    var header = new HeaderModel();
                    header.HeaderName = model.Column.AsName;
                    header.HeaderText = model.Title;
                    header.HeaderWidth = model.Length;
                    Datas.Headers.Add(header);

                    var column = rows.First(r => r.HeaderName == header.HeaderName);
                    column.HeaderWidth = model.Length;

                    Datas.Columns.Add(column);
                }
            }
        }

        public override void ReceiveData(AdapterDataTable adt)
        {
            if (adt == null || adt.Rows == null || adt.Rows.Count <= 0)
            {
                return;
            }

            lock (m_Lock)
            {
                AnalysisData(adt);
            }
        }

        private void AnalysisData(AdapterDataTable table)
        {
            foreach (var row in table.Rows)
            {
                foreach (var model in Datas.Columns)
                {
                    var content = row[model.HeaderName].ToString();
                    var cell = new CellModel();
                    cell.Content = content;
                    model.Cells.Add(cell);
                }
            }
        }

        public override void ClearChart(ChartDataViewModel dvm)
        {
        }

        public override void RefreshChart(ChartDataViewModel dvm)
        {
        }
    }
}