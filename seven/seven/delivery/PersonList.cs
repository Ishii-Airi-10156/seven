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

namespace seven.delivery
{
    public partial class PersonList : Form
    {
        int co;
        List<int> list = new List<int>();
        private readonly string sqlConnectionString =
            ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        
        public PersonList()
        {
            InitializeComponent();
        }

        private void PersonList_Load(object sender, EventArgs e)
        {
            ShowList();
            
        }
        private void ShowList()
        {
            try
            {
                dataGridView1.Rows.Clear();
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select emp.emp_id,emp.active,emp.emp_name,emp.area,emp.truck_no,t.truck_capacity ,emp.co from employee emp join truck t on emp.truck_no = t.truck_no");
                    SqlCommand command = new SqlCommand(sql.ToString(), connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            co = (int)reader["co"];
                            list.Add(co);
                            dataGridView1.Rows.Add(reader["emp_id"], reader["active"], reader["emp_name"], reader["area"], reader["truck_no"], co + "/" + reader["truck_capacity"]);
                            
                        }
                        
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("データベース接続エラー: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラー: " + ex.Message);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int e_num = (int)dataGridView1.CurrentRow.Cells[0].Value;
            string e_name = (string)dataGridView1.CurrentRow.Cells[2].Value;
            string e_area = (string)dataGridView1.CurrentRow.Cells[3].Value;
            int e_tno = (int)dataGridView1.CurrentRow.Cells[4].Value;
            int ico = list[dataGridView1.CurrentRow.Index];

            PersonEdit from = new PersonEdit(e_num, e_name, e_area, e_tno,ico);
            from.ShowDialog();
            from.Dispose();
            ShowList();
            foreach (DataGridViewRow dt in dataGridView1.Rows)
            {
                if ((int)dt.Cells[0].Value == e_num)
                {
                    dt.Selected = true;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count1 = dataGridView1.RowCount - 1;
            PersonEdit from = new PersonEdit();
            from.ShowDialog();
            from.Dispose();

            ShowList();
            int count2 = dataGridView1.RowCount - 1;
            if (count1 != count2)
            {
                dataGridView1[0, count2].Selected = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult =
                    MessageBox.Show("選択したデータを削除しますか？", "確認",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                int emp_id = (int)dataGridView1.CurrentRow.Cells[0].Value;

                string sqp = "delete from employee where emp_id = @p1";
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqp, connection);
                    command.Parameters.Add("@p1", SqlDbType.Int).Value = emp_id;
                    int result = command.ExecuteNonQuery();
                    ShowList();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("データベース接続エラー: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("エラー: " + ex.Message);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int emp_id = (int)dataGridView1.CurrentRow.Cells[0].Value;
            int truck_no = (int)dataGridView1.CurrentRow.Cells[4].Value;
            bool active = (bool)dataGridView1.CurrentRow.Cells[1].Value;
            if (active == false)
            {

                string sql;
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    sql = "select count(*) from employee where truck_no = @p1 and active = 1";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@p1", SqlDbType.Int).Value = truck_no;
                    int result = (int)command.ExecuteScalar();
                    if (result > 0)
                    {
                        MessageBox.Show("このトラックは使用中です");
                        ShowList();
                        return;
                        
                    }
                }
                sql = "update employee set active = 1 where emp_id = @p1";
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@p1", SqlDbType.Int).Value = emp_id;
                    int result = command.ExecuteNonQuery();
                    ShowList();
                }
            }
            else
            {
                string sql = "update employee set active = 0 where emp_id = @p1";
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@p1", SqlDbType.Int).Value = emp_id;
                    int result = command.ExecuteNonQuery();
                    ShowList();
                }
            }
        }
    }
}
