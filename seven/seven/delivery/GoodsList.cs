using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data;

namespace seven.delivery
{
    public partial class GoodsList : Form
    {
        private readonly string sqlConnctionString = ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;

        public GoodsList()
        {
            InitializeComponent();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void GoodsList_Load(object sender, EventArgs e)
        {

            comboBox1.SelectedIndex = 0;
            LoadGoodsData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void LoadGoodsData()
        {
            dataGridView1.Rows.Clear();

            int aa = comboBox1.SelectedIndex;
            int bb=0;
            

            StringBuilder sql = new StringBuilder("SELECT * FROM sales ");


            if (aa == 1)
            {
                bb = 11;
                sql.Append(" WHERE category_no = @c");
            }
            else if (aa == 2)
            {
                bb = 12;
                sql.Append(" WHERE category_no = @c");
            }
            else if (aa == 3)
            {
                bb = 21;
                sql.Append(" WHERE category_no = @c");
            }
            else if(aa == 4) 
            {
                bb = 22;
                sql.Append(" WHERE category_no = @c");
            }


            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnctionString;
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql.ToString();

            command.Parameters.AddWithValue("@c",bb);
            
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int rowIdx = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIdx].Cells[0].Value = reader["sales_id"];
                dataGridView1.Rows[rowIdx].Cells[1].Value = reader["sales_name"];
                dataGridView1.Rows[rowIdx].Cells[2].Value = reader["price"];
                dataGridView1.Rows[rowIdx].Cells[3].Value = reader["category_no"];
                dataGridView1.Rows[rowIdx].Cells[4].Value = reader["category_name"];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string title = "商品情報登録";
            GoodsEdit form = new GoodsEdit(title);
            form.ShowDialog();
            LoadGoodsData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("編集したい行を選択してください");
                return;
            }
            
            int goodsId = (int)dataGridView1.CurrentRow.Cells[0].Value;
            string goodsName=(string)dataGridView1.CurrentRow.Cells[1].Value;
            int price=(int)dataGridView1.CurrentRow.Cells[2].Value;
            string categoryName=(string)dataGridView1.CurrentRow.Cells[4].Value;

            int bb;
            if (categoryName == "要冷凍")
            {
                bb = 0;
            }
            else if (categoryName =="冷凍不要" )
            {
                bb = 1;
            }
            else if (categoryName == "割れ物")
            {
                bb = 2;
            }
            else
            {
                bb = 3;
            }

            GoodsEdit form = new GoodsEdit(goodsId,goodsName,price,bb);
                form.ShowDialog();

                LoadGoodsData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
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

                    string sql = "DELETE sales WHERE sales_id = @p";

                    SqlConnection connection = new SqlConnection();
                    connection.ConnectionString = sqlConnctionString;
                    connection.Open();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sql.ToString();

                    command.Parameters.Add("@p", SqlDbType.Int).Value = customerId;

                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    LoadGoodsData();

                    MessageBox.Show("ID：" + customerId + "を削除しました。");
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
    }
}



