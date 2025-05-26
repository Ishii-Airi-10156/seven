using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Net;
using System.Data;


namespace seven.delivery
{
    public partial class GoodsEdit : Form
    {
        private readonly string sqlConnctionString = ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public GoodsEdit()
        {
            InitializeComponent();
        }
        public GoodsEdit(int goodsId)
        {
            InitializeComponent();
            textBox1.Text = goodsId.ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GoodsEdit_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                ReadGoods();
            }
        }

        private void ReadGoods()
        {
            string sql = "select * from sales where category_no=@p ";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnctionString;
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql;

            command.Parameters.Add("@p", SqlDbType.Int);

            command.Parameters["@p"].Value=textBox1.Text;

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                textBox2.Text = (string)reader["sales_name"];
                textBox3.Text = (string)reader["price"];
                textBox4.Text = (string)reader["category_no"];
            }
            reader.Close();
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                InsertGoods();
            }
            else
            {
                UpdateGoods();
            }

            this.Close();
        }

        private void InsertGoods()
        {
            string sql = "insert into sales(sales_id,sales_name,price,category_no)" + "values('" + textBox2 + "'," + textBox3 + "," + textBox4 + ",";

            SqlConnection connection=new SqlConnection();
            connection.ConnectionString = sqlConnctionString;
            connection.Open();

            SqlCommand command=new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql;

            int result= command.ExecuteNonQuery();
            Console.WriteLine("更新件数："+result);

            connection.Close ();
        }

        private void UpdateGoods()
        {
            string sql = "update sales" + "set sales_name=@p1,price=@p2,category_no=@p3" + "where sales_no=@p4";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnctionString;
            connection.Open();

            SqlCommand command=new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql;

            command.Parameters.Add("@p1",SqlDbType.NVarChar).Value=textBox2.Text;
            command.Parameters.Add("@p2", SqlDbType.Int).Value = textBox3.Text;
            command.Parameters.Add("@p3", SqlDbType.Int).Value = textBox4.Text;
            command.Parameters.Add("@p4", SqlDbType.Int).Value = textBox1.Text;

            int result = command.ExecuteNonQuery();
            Console.WriteLine("更新件数：" + result);

            connection.Close ();
        }

    }
}
