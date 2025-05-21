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

namespace seven
{
    public partial class CarInfo : Form
    {
        private readonly string sqlConnectionString =
             ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public CarInfo()
        {
            InitializeComponent();
        }
        private void search()
        {
            dataGridView1.Rows.Clear();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT*FROM truck ");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1.Rows.Add();
                int rowIdx = dataGridView1.Rows.Count - 1;
                dataGridView1.Rows[rowIdx].Cells[0].Value = reader["active"];
                dataGridView1.Rows[rowIdx].Cells[1].Value = reader["truck_no"];
                dataGridView1.Rows[rowIdx].Cells[2].Value = reader["truck_capacity"];
            }
            reader.Close();
            con.Close();
        }

        private void CarInfo_Load(object sender, EventArgs e)
        {
            search();
        }
    }
}
