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

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }
        public GoodsEdit(int goodsId,string goodsName,int price,int bb)
        {
            InitializeComponent();
            textBox1.Text = goodsId.ToString();
            textBox2.Text = goodsName.ToString();
            textBox3.Text = price.ToString();
            comboBox1.SelectedIndex=bb;
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

            if (!int.TryParse(textBox3.Text, out int price))
            {
                errorProvider1.SetError(textBox3, "価格を入力してください");
                textBox3.Focus();
                error = true;
            }
            if (String.IsNullOrEmpty(comboBox1.Text))
            {
                errorProvider1.SetError(comboBox1, "カテゴリーを選択してください");
                comboBox1.Focus();
                error = true;
            }
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "商品名を入力してください");
                textBox2.Focus();
                error = true;
            }

            else
            {
                if (textBox2.Text.Length >=30)
                {
                    errorProvider1.SetError(textBox2, "30文字以内で入力してください");
                    textBox2.Focus();
                    error = true;
                }
            }




                if (error)
            {
                return;
            }


            if (String.IsNullOrEmpty(textBox1.Text))
            {
                DialogResult result = MessageBox.Show("データを登録しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                InsertGoods();

            }
            else if (int.TryParse(textBox1.Text, out int goodsId))
            {
                DialogResult result = MessageBox.Show("データを編集しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                UpdateGoods();
            }

            this.Close();
        }

        private void InsertGoods()
        {
            try
            {

                
                int aa=comboBox1.SelectedIndex;
                int bb;
                if (aa == 0)
                {
                    bb = 11;
                }
                else if (aa == 1)
                {
                    bb = 12;
                }
                else if(aa == 2)
                {
                    bb = 21;
                }
                else
                {
                    bb =22;
                }

           
                string sql = "INSERT INTO sales(sales_name,category_no,category_name,price) " + " VALUES (@p1, @p2, @p3,@p4) ";

                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnctionString;
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql;


                command.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox2.Text;
                command.Parameters.Add("@p4", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                command.Parameters.Add("@p2",SqlDbType.Int).Value = bb;
                command.Parameters.Add("@p3", SqlDbType.NVarChar).Value = comboBox1.Text;

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

        private void UpdateGoods()
        {
            try
            {
                int aa = comboBox1.SelectedIndex;
                int bb;
                if (aa == 0)
                {
                    bb = 11;
                }
                else if (aa == 1)
                {
                    bb = 12;
                }
                else if (aa == 2)
                {
                    bb = 21;
                }
                else
                {
                    bb = 22;
                }

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
                command.Parameters["@p3"].Value = bb;
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar == '-')
            {
                e.Handled = true; 
            }
        }
    }
}
