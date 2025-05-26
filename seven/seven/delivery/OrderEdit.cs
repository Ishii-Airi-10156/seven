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
        List<int> cus = new List<int>();
        List<int> sal = new List<int>();
        List<int> emp = new List<int>();
        public OrderEdit()
        {
            InitializeComponent();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            CustomerRead();
            SalesRead();
            EmployeeRead();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
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
                comboBox1.Items.Add(reader["customer_name"]);
                cus.Add(Convert.ToInt32(reader["customer_id"]));
                
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
                comboBox2.Items.Add(reader["sales_name"]);
                sal.Add(Convert.ToInt32(reader["sales_id"]));
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
                comboBox3.Items.Add(reader["emp_name"]);
                emp.Add(Convert.ToInt32(reader["emp_id"]));
            }
        }

        public void Insert()
        {
            int cusidx=comboBox1.SelectedIndex;
            int salidx=comboBox2.SelectedIndex;
            int empidx=comboBox3.SelectedIndex;
            int n = 0;
            if(checkBox1.Checked)
            {
                n = 1;
            }
            string sql = "INSERT INTO order_item(customer_id,sales_id,order_date,emp_id,okihai,okibasho,delivered,cancel)" +
               "VALUES(" + cus[cusidx] + "," + sal[salidx] + ",'" + dateTimePicker1.Value + "',"+ emp[empidx] + "," + n + ",'" + textBox4.Text + "'," + 0 + "," + 0 + ")";
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

        private void button1_Click(object sender, EventArgs e)
        {
            bool error = true;
            errorProvider1.Clear();
            if (String.IsNullOrEmpty(comboBox1.Text))
            {
                errorProvider1.SetError(comboBox1, "選択してください");
                error = false;
            }
            if (String.IsNullOrEmpty(comboBox2.Text))
            {
                errorProvider1.SetError(comboBox2, "選択してください");
                error = false;

            }
            if (String.IsNullOrEmpty(comboBox3.Text))
            {
                errorProvider1.SetError(comboBox3, "選択してください");
                error = false;
            }
            if (dateTimePicker1.Value < DateTime.Now)
            {
                errorProvider1.SetError(dateTimePicker2, "明日以降を登録してください");
                error = false;
            }
            if (checkBox1.Checked == false)
            {
                if (String.IsNullOrEmpty(textBox4.Text))
                {
                    errorProvider1.SetError(textBox4, "置き配がしないになっています");
                    error = false;
                    textBox4.Clear();
                    return;
                }
            }
            else if (checkBox1.Checked == true)
            {
                if (textBox4 == null)
                {
                    errorProvider1.SetError(textBox4, "配達場所を入力してください");
                    error = false;
                    return;
                }
                else if (!int.TryParse(textBox4.Text, out int n2))
                {
                    errorProvider1.SetError(textBox4, "文字でお願いします");
                    textBox4.Clear();
                    error = false;
                    return ;
                }
            }
            if(error)
            {
                DialogResult result = MessageBox.Show("本当に追加してもよろしいですか?", "追加",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No)
                {
                    return ;
                }
                Insert();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
