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
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }
        private void search()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            int count1 = dataGridView1.RowCount - 1;
            CarEdit form = new CarEdit();
            form.ShowDialog();
            search();
            int count2 = dataGridView1.RowCount-1;
            if(count1!=count2)
            {
                dataGridView1[0, count2].Selected = true;
            }       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int truckNo = (int)dataGridView1.CurrentRow.Cells[0].Value;
                CarEdit form = new CarEdit(truckNo);
                form.ShowDialog();
                search();
                foreach (DataGridViewRow dt in dataGridView1.Rows)
                {
                    if ((int)dt.Cells[0].Value == truckNo)
                    {
                        dt.Selected = true;
                    }

                }
            }
            else
            {
                MessageBox.Show("a");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow!=null)
            {
                DialogResult result = MessageBox.Show("データを削除しますか?", "確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    delete();
                }
                else if (result == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("a");
            }
        }
        private void delete()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int n;
                string sql = "UPDATE truck " + "SET active=@p1 " +
                    "WHERE truck_no=@p2";
                SqlConnection con = new SqlConnection();
                con.ConnectionString = sqlConnectionString;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 60;
                if (!(Boolean)dataGridView1.CurrentRow.Cells[2].Value)
                {
                    n = 1;
                }
                else
                {
                    n = 0;
                }
                cmd.Parameters.Add("@p1", SqlDbType.Bit).Value = n;
                cmd.Parameters.Add("@p2", SqlDbType.Int).Value = (int)dataGridView1.CurrentRow.Cells[0].Value;
                cmd.ExecuteNonQuery();
                con.Close();
                search();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
