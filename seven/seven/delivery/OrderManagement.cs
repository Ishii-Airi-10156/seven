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
                    " JOIN employee e ON o.emp_id=e.emp_id WHERE o.cancel=0 ");
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
            if (dataGridView1.CurrentCell.ColumnIndex == 7)
            {
                Deliverd();
                if (radioButton1.Checked)
                {
                    Narrowdown1();
                }
                else if (radioButton2.Checked)
                {
                    Narrowdown2();
                }
                else if(radioButton3.Checked)
                {
                    Narrowdown3();
                }
                else 
                {
                    search();
                }
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                Okihai();
                if (radioButton1.Checked)
                {
                    Narrowdown1();
                }
                else if (radioButton2.Checked)
                {
                    Narrowdown2();
                }
                else if (radioButton3.Checked)
                {
                    Narrowdown3();
                }
                else
                {
                    search();
                }
            }

        }
        private void Okihai()
        {
            try
            {
                int n;
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE order_item SET okihai=@p1 WHERE order_no=@p2");
                SqlConnection con = new SqlConnection();
                con.ConnectionString = sqlConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = 60;
                if (!(Boolean)dataGridView1.CurrentRow.Cells[5].Value)
                {
                    n = 1;
                }
                else
                {
                    n = 0;
                }
                cmd.Parameters.Add("@p1", SqlDbType.Bit).Value = n;
                cmd.Parameters.Add("@p2", SqlDbType.Int).Value = (int)dataGridView1.CurrentRow.Cells[0].Value;
                cmd.ExecuteNonQuery();
                con.Close();
                search();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Deliverd()
        {
            try
            {
                int n;
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE order_item SET delivered = @p1 WHERE order_no=@p2");
                SqlConnection con = new SqlConnection();
                con.ConnectionString = sqlConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = 60;
                if (!(Boolean)dataGridView1.CurrentRow.Cells[7].Value)
                {
                    n = 1;
                }
                else
                {
                    n = 0;
                }
                cmd.Parameters.Add("@p1", SqlDbType.Bit).Value = n;
                cmd.Parameters.Add("@p2", SqlDbType.Int).Value = (int)dataGridView1.CurrentRow.Cells[0].Value;
                cmd.ExecuteNonQuery();
                con.Close();
                search();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Narrowdown1();
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Narrowdown2();
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Narrowdown3();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            search();
        }

        private void Narrowdown1()
        {
            try
            {
                dataGridView1.Rows.Clear();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT o.order_no,c.customer_name,s.sales_name,e.emp_name,o.order_date,o.okihai,o.okibasho,o.delivered " +
                    "FROM order_item o JOIN customer c ON o.customer_id=c.customer_id JOIN sales s ON o.sales_id=s.sales_id " +
                    " JOIN employee e ON o.emp_id=e.emp_id　WHERE o.delivered=0 AND cancel=0 ");
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
        private void Narrowdown2()
        {
            try
            {
                dataGridView1.Rows.Clear();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT o.order_no,c.customer_name,s.sales_name,e.emp_name,o.order_date,o.okihai,o.okibasho,o.delivered " +
                    "FROM order_item o JOIN customer c ON o.customer_id=c.customer_id JOIN sales s ON o.sales_id=s.sales_id " +
                    " JOIN employee e ON o.emp_id=e.emp_id　WHERE e.active=1 AND o.cancel=0");
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
        private void Narrowdown3()
        {
            try
            {
                dataGridView1.Rows.Clear();
                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT o.order_no,c.customer_name,s.sales_name,e.emp_name,o.order_date,o.okihai,o.okibasho,o.delivered " +
                    "FROM order_item o JOIN customer c ON o.customer_id=c.customer_id JOIN sales s ON o.sales_id=s.sales_id " +
                    " JOIN employee e ON o.emp_id=e.emp_id　WHERE o.cancel=1 ");
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("本当にキャンセルしてもよろしいですか?", "キャンセル",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                return;
            }
            if (dataGridView1.CurrentRow != null)
            {
                cancel();
            }
            else
            {
                MessageBox.Show("");
                return;
            }
            if (radioButton1.Checked)
            {
                Narrowdown1();
            }
            else if (radioButton2.Checked)
            {
                Narrowdown2();
            }
            else if (radioButton3.Checked)
            {
                Narrowdown3();
            }
            else
            {
                search();
            }
        }
        private void cancel()
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("UPDATE order_item SET cancel=@p1 WHERE order_no=@p2");
                SqlConnection con = new SqlConnection();
                con.ConnectionString = sqlConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql.ToString();
                cmd.CommandTimeout = 60;
                cmd.Parameters.Add("@p1", SqlDbType.Bit).Value = 1;
                cmd.Parameters.Add("@p2", SqlDbType.Int).Value = (int)dataGridView1.CurrentRow.Cells[0].Value;
                cmd.ExecuteNonQuery();
                con.Close();
                search();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OrderEdit form= new OrderEdit();
            form.ShowDialog();
            search();
        }
    }
}

