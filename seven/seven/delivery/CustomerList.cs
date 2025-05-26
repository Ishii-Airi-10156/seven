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
using System.Net.Mail;

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
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            ViewCustomerList();

        }

        private void ViewCustomerList()
        {
            try
            {

                dataGridView1.Rows.Clear();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnectionString;
                connection.Open();



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
                    dataGridView1.Rows[idx].Cells[1].Value = reader["customer_name"];
                    dataGridView1.Rows[idx].Cells[2].Value = reader["address"];
                    dataGridView1.Rows[idx].Cells[3].Value = reader["tele_number"];
                    dataGridView1.Rows[idx].Cells[4].Value = reader["email_address"];
                }

                reader.Close();
                connection.Close();
            }
            catch (SqlException sqlexc)
            {
                MessageBox.Show(sqlexc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();

                StringBuilder sql = new StringBuilder();
                sql.Append("SELECT * FROM customer ");

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnectionString;
                connection.Open();

                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    sql.Append("WHERE customer_name LIKE @p1 ");

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sql.ToString();
                    string name = "%" + textBox1.Text.Trim() + "%";
                    command.Parameters.Add("@p1", SqlDbType.NVarChar).Value = name;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add();
                        int idx = dataGridView1.Rows.Count - 1;

                        dataGridView1.Rows[idx].Cells[0].Value = reader["customer_id"];
                        dataGridView1.Rows[idx].Cells[1].Value = reader["customer_name"];
                        dataGridView1.Rows[idx].Cells[2].Value = reader["address"];
                        dataGridView1.Rows[idx].Cells[3].Value = reader["tele_number"];
                        dataGridView1.Rows[idx].Cells[4].Value = reader["email_address"];

                    }
                    reader.Close();
                }

                else
                {
                    dataGridView1.Rows.Clear();
                }

                connection.Close();
            }
            catch (SqlException sqlexc)
            {
                MessageBox.Show(sqlexc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            errorProvider1.Clear();

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("編集したい行を選択してください");
                return;
            }

            else
            {
                int customerid = (int)dataGridView1.CurrentRow.Cells[0].Value;
                string customername = (string)dataGridView1.CurrentRow.Cells[1].Value;
                string  address= (string)dataGridView1.CurrentRow.Cells[2].Value;
                string telenumber = (string)dataGridView1.CurrentRow.Cells[3].Value;
                string emailaddress = (string)dataGridView1.CurrentRow.Cells[4].Value;
                                                 

                CustomerEdit edit = new CustomerEdit(customerid, emailaddress, customername, address, telenumber);
                edit.ShowDialog();

                ViewCustomerList();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if ((int)row.Cells[0].Value == customerid)
                    {
                        row.Selected = true;
                    }
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            CustomerEdit edit = new CustomerEdit();
            if (edit.ShowDialog() == DialogResult.Yes)
            {

                ViewCustomerList();

                int count = dataGridView1.RowCount - 1;
                dataGridView1[0, count].Selected = true;

             }
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("削除したい行を選択してください");
                return;
            }

            else
            {
                DialogResult dialogResult =
                    MessageBox.Show("データを削除しますか？", "確認",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                try
                {

                    int customerId = (int)dataGridView1.CurrentRow.Cells[0].Value;

                    string sql = "DELETE customer WHERE customer_id = @p";

                    SqlConnection connection = new SqlConnection();
                    connection.ConnectionString = sqlConnectionString;
                    connection.Open();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sql.ToString();

                    command.Parameters.Add("@p", SqlDbType.Int).Value = customerId;

                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    ViewCustomerList();

                    MessageBox.Show("ID：" + customerId +"を削除しました。");
                }
                catch (SqlException sqlexc)
                {
                    MessageBox.Show(sqlexc.Message);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ViewCustomerList();
        }
    }
}
