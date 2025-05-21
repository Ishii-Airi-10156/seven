using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
namespace seven.delivery
{
    public partial class PersonList : Form
    {
        private readonly string sqlConnectionString =
             ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public PersonList()
        {
            InitializeComponent();
        }

        private void PersonList_Load(object sender, EventArgs e)
        {
            search();
        }
        private void search()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT*FROM employee ");
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
                dataGridView1.Rows[rowIdx].Cells[0].Value = reader["emp_id"];
                dataGridView1.Rows[rowIdx].Cells[1].Value = reader["emp_name"];
                dataGridView1.Rows[rowIdx].Cells[2].Value = reader["area"];
                dataGridView1.Rows[rowIdx].Cells[3].Value = reader["truck_no"];
                dataGridView1.Rows[rowIdx].Cells[4].Value = reader["active"];
            }
            reader.Close();
            con.Close();
        }
    }
}
