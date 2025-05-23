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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace seven
{
    public partial class OrderEdit : Form
    {
        private readonly string sqlConnectionString =
             ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public OrderEdit()
        {
            InitializeComponent();
            CustomerRead();
            SalesRead();
            EmployeeRead();
        }
        public void CustomerRead()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT customer_id,customer_name FROM customer ");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add($"{reader["customer_id"]} { reader["customer_name"]}");
                
            }
        }
        public void SalesRead()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT sales_id,sales_name FROM sales ");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox2.Items.Add($"{reader["sales_id"]} {reader["sales_name"]}");
            }
        }
        public void EmployeeRead()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT emp_id,emp_name FROM employee ");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox3.Items.Add($"{reader["emp_id"]} {reader["emp_name"]}");
            }
        }

        public void Insert()
        {
            string sql = "INSERT INTO order_item(customer_id,sales_id,order_date,emp_id,okihai,okibasho,delivered,cancel)" +
               "VALUES('" + comboBox1 + "'," + comboBox2+ ","+ comboBox1+" )";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            con.Close();
            this.Close();
        }
    }
    
}
