using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

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

                string sql = "SELECT * FROM sales";
                if (numericUpDown1.Value > 0)
                {
                    sql += " WHERE category_no = @c";
                }

                using (SqlConnection connection = new SqlConnection(sqlConnctionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (numericUpDown1.Value > 0)
                        {
                            command.Parameters.AddWithValue("@c", numericUpDown1.Value);
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int rowIdx = dataGridView1.Rows.Add();
                                dataGridView1.Rows[rowIdx].Cells[0].Value = reader["sales_id"];
                                dataGridView1.Rows[rowIdx].Cells[1].Value = reader["sales_name"];
                                dataGridView1.Rows[rowIdx].Cells[2].Value = reader["price"];
                                dataGridView1.Rows[rowIdx].Cells[3].Value = reader["category_no"];
                            }
                        }
                    }
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new GoodsEdit();
            form.ShowDialog();
            LoadGoodsData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int goodsId = (int)dataGridView1.CurrentRow.Cells[0].Value;
                var form = new GoodsEdit(goodsId);
                form.ShowDialog();
                LoadGoodsData();
            }
        }
    }
}



