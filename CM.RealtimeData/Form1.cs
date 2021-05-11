using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace CM.RealtimeData
{
    public partial class Form1 : Form
    {
        private string _connStr;
        public Form1()
        {
            InitializeComponent();
            _connStr = ConfigurationManager.ConnectionStrings["ConnectionStringMain"].ToString();
            SqlDependency.Start(_connStr);//传入连接字符串,启动基于数据库的监听
            UpdateGrid();
            Console.Read();
            SqlDependency.Stop(_connStr);//传入连接字符串,启动基于数据库的监听
        }

        private void UpdateGrid()
        {
            using (SqlConnection connection = new SqlConnection(_connStr))
            {
                connection.Open();
                //依赖是基于某一张表的,而且查询语句只能是简单查询语句,不能带top或*,同时必须指定所有者,即类似[dbo].[]
                using (SqlCommand command = new SqlCommand("SELECT * FROM green_supervise_indentinfo", connection))
                {
                    command.CommandType = CommandType.Text;
                    //connection.Open();
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                    using (SqlDataReader sdr = command.ExecuteReader())
                    {
                        Console.WriteLine();
                        while (sdr.Read())
                        {
                            Console.WriteLine("AssyAcc:{0}\tSnum:{1}\t", sdr["Process_Cmn_AssyAcc"].ToString(), sdr["Process_Cmn_Snum"].ToString());
                        }
                        sdr.Close();
                    }
                }
            }
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change) //只有数据发生变化时,才重新获取并数据
            {
                UpdateGrid();
            }
        }
    }
}
