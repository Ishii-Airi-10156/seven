using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seven
{
    public partial class OrderManagement : Form
    {
        private readonly string sqlConnectionString =
             ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public OrderManagement()
        {
            InitializeComponent();
            search();
        }
        private void search()
        {
            try
            {
                dataGridView1.Rows.Clear();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT o.order_no,c.customer_name,s.sales_name,e.emp_name,o.order_date,o.okihai,o.okibasho,o.delivered " +
                    "FROM order_item o JOIN customer c ON o.customer_id=c.customer_id JOIN sales s ON o.sales_id=s.sales_id " +
                    " JOIN employee e ON o.emp_id=e.emp_id ");
                SqlConnection con = new SqlConnection();
                con.ConnectionString = sqlConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql.ToString();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DateTime dt = (DateTime)reader["order_date"];

                    dataGridView1.Rows.Add();
                    int rowIdx = dataGridView1.Rows.Count - 1;
                    dataGridView1.Rows[rowIdx].Cells[0].Value = reader["order_no"];
                    dataGridView1.Rows[rowIdx].Cells[1].Value = reader["customer_name"];
                    dataGridView1.Rows[rowIdx].Cells[2].Value = reader["sales_name"];
                    dataGridView1.Rows[rowIdx].Cells[3].Value = reader["emp_name"];
                    dataGridView1.Rows[rowIdx].Cells[4].Value = dt.ToString("yyyy/MM/dd");
                    dataGridView1.Rows[rowIdx].Cells[5].Value = reader["okihai"];
                    dataGridView1.Rows[rowIdx].Cells[6].Value = reader["okibasho"];
                    dataGridView1.Rows[rowIdx].Cells[7].Value = reader["delivered"];
                }
                reader.Close();
                con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int n;
                int j;
                StringBuilder sql= new StringBuilder();
                sql.Append("UPDATE order_item SET okihai=@p1,delivered = @p2 WHERE order_no=@p3"); 
                SqlConnection con = new SqlConnection();
                con.ConnectionString = sqlConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = 60;
                if (0==(int)dataGridView1.CurrentRow.Cells[5].Value)
                {
                    n = 0;
                    cmd.Parameters.Add("@p1", SqlDbType.Bit).Value = n;
                }
                else
                {
                    n = 1;
                    cmd.Parameters.Add("@p1", SqlDbType.Bit).Value = n;
                }
                if (0==(int)dataGridView1.CurrentRow.Cells[7].Value)
                {
                    j = 0;
                    cmd.Parameters.Add("@p2", SqlDbType.Bit).Value = j;
                }
                else
                {
                    j = 1;
                    cmd.Parameters.Add("@p2", SqlDbType.Bit).Value = j;
                } 
                cmd.Parameters.Add("@p3", SqlDbType.Int).Value = (int)dataGridView1.CurrentRow.Cells[0].Value;
                cmd.ExecuteNonQuery();
                con.Close();
                search();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

}
