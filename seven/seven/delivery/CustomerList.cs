using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace seven
{
    public partial class CustomerList : Form
    {

        private readonly string sqlConnectionString =
               ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;

        public CustomerList()
        {
            InitializeComponent();
        }

        private void CustomerList_Load(object sender, EventArgs e)
        {

            ViewCustomerList();

        }

        private void ViewCustomerList()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnectionString;
            connection.Open();

            dataGridView1.Rows.Clear();

            string sql = "SELECT * FROM customer ";

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql.ToString();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                dataGridView1.Rows.Add();
                int idx = dataGridView1.Rows.Count - 1;

                dataGridView1.Rows[idx].Cells[0].Value = reader["customer_id"];
                dataGridView1.Rows[idx].Cells[1].Value = reader["email_address"];
                dataGridView1.Rows[idx].Cells[2].Value = reader["customer_name"];
                dataGridView1.Rows[idx].Cells[3].Value = reader["address"];
                dataGridView1.Rows[idx].Cells[4].Value = reader["tele_number"];
            }

            reader.Close();
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM customer ");

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnectionString;
            connection.Open();

            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                sql.Append("WHERE customer_name LIKE '%" + textBox1.Text + "%' ");

                               SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql.ToString();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dataGridView1.Rows.Add();
                    int idx = dataGridView1.Rows.Count - 1;

                    dataGridView1.Rows[idx].Cells[0].Value = reader["customer_id"];
                    dataGridView1.Rows[idx].Cells[1].Value = reader["email_address"];
                    dataGridView1.Rows[idx].Cells[2].Value = reader["customer_name"];
                    dataGridView1.Rows[idx].Cells[3].Value = reader["address"];
                    dataGridView1.Rows[idx].Cells[4].Value = reader["tele_number"];
   
                }
                reader.Close();
            }

            else
            {
                dataGridView1.Rows.Clear();
            }
           
            connection.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CustomerEdit edit = new CustomerEdit();
            edit.ShowDialog();
            edit.Dispose();

            ViewCustomerList();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            CustomerEdit edit = new CustomerEdit();
            edit.ShowDialog();
            edit.Dispose();

            ViewCustomerList();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult =
                MessageBox.Show("選択したデータを削除しますか？", "確認",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            int customerId = (int)dataGridView1.CurrentRow.Cells[0].Value;

            string sql = "DELETE customer WHERE customer_id = @p";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnectionString;
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql.ToString();

            command.Parameters.Add("@p" , SqlDbType.Int).Value = customerId;

            int result = command.ExecuteNonQuery();
            connection.Close();


            ViewCustomerList();
            MessageBox.Show("削除しました。");

         }
    }
}
