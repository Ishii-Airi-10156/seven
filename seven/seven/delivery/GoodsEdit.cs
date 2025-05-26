using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
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
        public GoodsEdit(int goodsId,string goodsName,int price,int categoryNo)
        {
            InitializeComponent();
            textBox1.Text = goodsId.ToString();
            textBox2.Text = goodsName.ToString();
            textBox3.Text = price.ToString();
            textBox4.Text = categoryNo.ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GoodsEdit_Load(object sender, EventArgs e)
        {
            ActiveControl = textBox2;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool error = false;

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "商品名を入力してください");
                error = true;
            }

            if (!int.TryParse(textBox3.Text, out int price))
            {
                errorProvider1.SetError(textBox3, "価格を入力してください");
                error = true;
            }

            if (!int.TryParse(textBox4.Text, out int categoryNo))
            {
                errorProvider1.SetError(textBox4, "カテゴリーNoを入力してください");
                error = true;
            }
            if(!int.TryParse(textBox4.Text, out int sales_id))
            {
                errorProvider1.SetError(textBox1, "商品IDを入力してください");
                error = true;
            }
            if (error)
            {
                return;
            }


            if (String.IsNullOrEmpty(textBox1.Text))
            {
                InsertGoods(textBox2.Text, price, categoryNo);
                
            }
            else if (int.TryParse(textBox1.Text, out int goodsId))
            {
                UpdateGoods(goodsId, textBox2.Text, price, categoryNo);

            }

            this.Close();
        }

        private void InsertGoods(string name, int price, int categoryNo)
        {
            try
            {
                string sql = "INSERT INTO sales (sales_name, price, category_no)  "+" VALUES (@p1, @p2, @p3) ";

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnctionString;
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;


                command.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox2.Text;
                command.Parameters.Add("@p2", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                command.Parameters.Add("@p3", SqlDbType.Int).Value = int.Parse(textBox4.Text);

                int result=command.ExecuteNonQuery();
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

        private void UpdateGoods(int id, string name, int price, int categoryNo)
        {
            try
            {
                string sql = "update sales set sales_name=@p1, price=@p2, category_no=@p3 where sales_id=@p4 ";

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnctionString;
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;


                command.Parameters.Add("@p1", SqlDbType.NVarChar);
                command.Parameters.Add("@p2", SqlDbType.Int);
                command.Parameters.Add("@p3", SqlDbType.Int);
                command.Parameters.Add("@p4", SqlDbType.Int);

                command.Parameters["@p1"].Value = textBox2.Text;
                command.Parameters["@p2"].Value = int.Parse(textBox3.Text);
                command.Parameters["@p3"].Value = int.Parse(textBox4.Text);
                command.Parameters["@p4"].Value = int.Parse(textBox1.Text);

                int result = command.ExecuteNonQuery();

            }
            catch(SqlException sqlexc)
            {
                MessageBox.Show(sqlexc.Message);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

    }
}
