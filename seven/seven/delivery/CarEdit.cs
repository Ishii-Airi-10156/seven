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
    public partial class CarEdit : Form
    {
        private readonly string sqlConnectionString =
             ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public CarEdit()
        {
            InitializeComponent();
            label7.Visible = false;
            textBox5.Visible = false;
        }
        public CarEdit(int no)
        {
            InitializeComponent();
            textBox5.Text = no.ToString();
            textBox5.ReadOnly = true;
            Read();
        }

        private void CarEdit_Load(object sender, EventArgs e)
        {

        }
        private void Insert(int n)
        {
            string sql = "INSERT INTO truck(truck_capacity,active)" +
                "VALUES('" + textBox6.Text + "'," + n + ")";
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
            int n;
            if (checkBox1.Checked)
            {
                n = 1;
            }
            else
            {
                n = 0;
            }
            int n2 = 0;
            if (String.IsNullOrEmpty(textBox5.Text))
            {
                errorProvider1.Clear();
                if (String.IsNullOrEmpty(textBox6.Text))
                {
                    errorProvider1.SetError(textBox6, "積載量を入力してください");
                    textBox6.Clear();
                    return;
                }
                else if (!int.TryParse(textBox6.Text,out n2))
                {
                    errorProvider1.SetError(textBox6,"数値を入力してください");
                    textBox6.Clear();
                    return;
                }
                else if (Convert.ToInt32(textBox6.Text) >= 250)
                {
                    errorProvider1.SetError(textBox6, "積載量が規定値を超えています");
                    textBox6.Clear();
                    return;
                }
                else
                {
                    Insert(n);
                }
                
            }
            else
            {
                errorProvider1.Clear();
                if (String.IsNullOrEmpty(textBox6.Text))
                {
                    errorProvider1.SetError(textBox6, "積載量を入力してください");
                    textBox6.Clear();
                    return;
                }
                 else if (!int.TryParse(textBox6.Text, out n2))
                {
                    errorProvider1.SetError(textBox6, "数値を入力してください");
                    textBox6.Clear();
                    return;
                }
                else if (Convert.ToInt32(textBox6.Text) >= 250)
                {
                    errorProvider1.SetError(textBox6, "積載量が規定値を超えています");
                    textBox6.Clear();
                    return;
                }
                else
                {
                    Update(n);
                }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Update(int n)
        {
            
            string sql = "UPDATE truck " + "SET truck_capacity=@p1,active=@p2 " +
                "WHERE truck_no=@p3";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.CommandTimeout = 60;
            cmd.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox6.Text;
            cmd.Parameters.Add("@p2", SqlDbType.Int).Value = n;
            cmd.Parameters.Add("@p3", SqlDbType.Int).Value = textBox5.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            this.Close();
        }
        private void Read()
        {
            string sql = "SELECT*FROM truck WHERE truck_no = @p";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            cmd.Parameters.Add("@p", SqlDbType.Int);
            cmd.Parameters["@p"].Value = textBox5.Text;
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                textBox6.Text =Convert.ToString(reader["truck_capacity"]);
                checkBox1.Checked= Convert.ToBoolean(reader["active"]);
            }
            reader.Close();
            con.Close();
        }
    }
}
