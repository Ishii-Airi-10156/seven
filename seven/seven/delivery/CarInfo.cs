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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace seven
{
    public partial class CarInfo : Form
    {
        private readonly string sqlConnectionString =
             ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public CarInfo()
        {
            InitializeComponent();
        }
        private void search()
        {
            dataGridView1.Rows.Clear();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT*FROM truck ");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1.Rows.Add();
                int rowIdx = dataGridView1.Rows.Count - 1;
                dataGridView1.Rows[rowIdx].Cells[0].Value = reader["truck_no"];
                dataGridView1.Rows[rowIdx].Cells[1].Value = reader["truck_capacity"];
                dataGridView1.Rows[rowIdx].Cells[2].Value = reader["active"];
            }
            reader.Close();
            con.Close();
        }

        private void CarInfo_Load(object sender, EventArgs e)
        {
            search();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CarEdit form = new CarEdit();
            form.ShowDialog();
            search();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int truckNo = (int)dataGridView1.CurrentRow.Cells[0].Value;
            CarEdit form = new CarEdit(truckNo);
            form.ShowDialog();
            search();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult =MessageBox.Show("本当に削除してもよろしいですか?", "削除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dataGridView1.Rows.Count==((int)dialogResult))
            {
                return;
            }
            delete();
        }
        private void delete()
        {
            
            string sql = ("DELETE FROM truck WHERE truck_no=@p");
            SqlConnection con = new SqlConnection();
            con.ConnectionString = sqlConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@p", SqlDbType.Int);
            cmd.Parameters["@p"].Value = (int)dataGridView1.CurrentRow.Cells[0].Value;
            cmd.Connection = con;
            cmd.CommandText = sql.ToString();
            cmd.ExecuteNonQuery();
            con.Close();
            search();
        }
    }
}
