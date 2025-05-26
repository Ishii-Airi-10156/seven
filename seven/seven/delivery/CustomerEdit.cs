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
    public partial class CustomerEdit : Form
    {
        private readonly string sqlConnectionString =
            ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public CustomerEdit()
        {
            InitializeComponent();
        }

        public CustomerEdit(int customerid, string emailaddress, string customername, string address, string telenumber)
        {
            InitializeComponent();
            textBox1.Text = customerid.ToString();
            textBox2.Text = customername.ToString();
            textBox3.Text = address.ToString();
            textBox4.Text = telenumber.ToString();
            textBox5.Text = emailaddress.ToString();

        }

        private void CustomerEdit_Load(object sender, EventArgs e)
        {
            
         }

        private void UpdateCusInfo()
        {
            try
            {

                string sql = "UPDATE customer " +
                    "SET customer_name = @p1, address = @p2, tele_number = @p3, email_address = @p4 " +
                    "WHERE customer_id = @p5";

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnectionString;
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;


                command.Parameters.Add("@p1", SqlDbType.NVarChar);
                command.Parameters.Add("@p2", SqlDbType.NVarChar);
                command.Parameters.Add("@p3", SqlDbType.VarChar);
                command.Parameters.Add("@p4", SqlDbType.VarChar);
                command.Parameters.Add("@p5", SqlDbType.Int);

                command.Parameters["@p1"].Value = textBox2.Text;
                command.Parameters["@p2"].Value = textBox3.Text;
                command.Parameters["@p3"].Value = textBox4.Text;
                command.Parameters["@p4"].Value = textBox5.Text;
                command.Parameters["@p5"].Value = textBox1.Text;

                int result = command.ExecuteNonQuery();

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
            errorProvider1.Clear();
            bool error = false;

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "必ず入力してください");
                error = true;
            }

            if (String.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "必ず入力してください");
                error = true;
            }

            if (String.IsNullOrEmpty(textBox4.Text))
            {
                errorProvider1.SetError(textBox4, "必ず入力してください");
                error = true;
            }

            if (String.IsNullOrEmpty(textBox5.Text))
            {
                errorProvider1.SetError(textBox5, "必ず入力してください");
                error = true;
            }

            if (error)
            {
                return;
            }

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                InsertCusInfo();
            }

            else
            {
                //更新モードならUPDATE処理
                UpdateCusInfo();
            }

            this.Close();
        }
        private void InsertCusInfo()
        {
            try
            {

                string sql = "INSERT INTO customer(customer_name,address,tele_number,email_address) " +
                                   "VALUES (@p1, @p2, @p3,@p4)";

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnectionString;
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;


                command.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox2.Text;
                command.Parameters.Add("@p2", SqlDbType.NVarChar).Value = textBox3.Text;
                command.Parameters.Add("@p3", SqlDbType.NVarChar).Value = textBox4.Text;
                command.Parameters.Add("@p4", SqlDbType.NVarChar).Value = textBox5.Text;

                command.ExecuteNonQuery();

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
            this.Close();
        }
    }
}
