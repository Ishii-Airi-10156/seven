using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace seven.delivery
{
    public partial class GoodsList : Form
    {
        private readonly string sqlConnctionString = ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;

        public GoodsList()
        {
            InitializeComponent();
        }

        private void GoodsList_Load(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadGoodsData();
        }

        private void LoadGoodsData()
        {
            dataGridView1.Rows.Clear();

            StringBuilder sql = new StringBuilder("SELECT * FROM sales ");


            if (numericUpDown1.Value > 0)
            {
                sql.Append(" WHERE category_no = @c");
            }

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnctionString;
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = sql.ToString();


            if (numericUpDown1.Value > 0)
            {
                command.Parameters.AddWithValue("@c", (int)numericUpDown1.Value);
            }

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                int rowIdx = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIdx].Cells[0].Value = reader["sales_id"];
                dataGridView1.Rows[rowIdx].Cells[1].Value = reader["sales_name"];
                dataGridView1.Rows[rowIdx].Cells[2].Value = reader["price"];
                dataGridView1.Rows[rowIdx].Cells[3].Value = reader["category_no"];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GoodsEdit form = new GoodsEdit();
            form.ShowDialog();

            LoadGoodsData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                int goodsId = (int)dataGridView1.CurrentRow.Cells[0].Value;
                string goodsName=(string)dataGridView1.CurrentRow.Cells[1].Value;
                int price=(int)dataGridView1.CurrentRow.Cells[2].Value;
                int categoryNo=(int)dataGridView1.CurrentRow.Cells[3].Value;

                GoodsEdit form = new GoodsEdit(goodsId,goodsName,price,categoryNo);
                form.ShowDialog();

                LoadGoodsData();
        }
    }
}



