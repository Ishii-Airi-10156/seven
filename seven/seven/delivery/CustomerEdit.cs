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

        public CustomerEdit(int customer_id)
        {
            InitializeComponent();
            textBox1.Text = customer_id.ToString();
        }

        private void CustomerEdit_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                ReadCusInfo();
            }
         }

        private void ReadCusInfo()
        {
            string sql = "SELECT * FROM customer WHERE customer_id = @p";

            
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnectionString;
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql;

            command.Parameters.Add("@p", SqlDbType.Int);

            command.Parameters["@p"].Value = textBox1.Text;

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                textBox1.Text = (string)reader["customer_id"];
                textBox2.Text = (string)reader["customer_name"];
                textBox3.Text = (string)reader["address"];
                textBox4.Text = (string)reader["tele_number"];
                textBox5.Text = (string)reader["email_address"];

            }
            reader.Close();
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "必ず入力してください");
                return;
            }

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "必ず入力してください");
                return;
            }

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "必ず入力してください");
                return;
            }

            if (String.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "必ず入力してください");
                return;
            }

            if (String.IsNullOrEmpty(textBox4.Text))
            {
                errorProvider1.SetError(textBox4, "必ず入力してください");
                return;
            }

            if (String.IsNullOrEmpty(textBox5.Text))
            {
                errorProvider1.SetError(textBox5, "必ず入力してください");
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

        private void UpdateCusInfo()
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
