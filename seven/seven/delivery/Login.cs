using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seven
{
    public partial class Login : Form
    {

        private readonly string sqlConnectionString =
               ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        public Login()
        {
              InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool error = false;
            try
            {
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = sqlConnectionString;
                connection.Open();

                string sql = " select count(emp_id) from employee where emp_id = @p1 ";


                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = sql.ToString();
                command.Parameters.Add("@p1", SqlDbType.Int).Value = textBox1.Text;

                int reader = (int)command.ExecuteScalar();
                if ((int)reader == 0)
                {
                    errorProvider1.SetError(textBox1, "IDが違います");
                    textBox1.Focus();
                    error = true;
                }

                //if ((int)reader == 0)
                //{
                //    errorProvider1.SetError(textBox1, "数字で入力してください");
                //    textBox1.Focus();
                //    error = true;
                //}

            }
            catch(SqlException sqlexc)
            {
                MessageBox.Show(sqlexc.Message);
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "必ず入力してください");
                textBox1.Focus();
                error = true;
                
            }

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "必ず入力してください");
                textBox2.Focus();
                error = true;
                return;
            }

            if (textBox2.Text != "Luckyseven" || (textBox2.Text.Length <= 6 && textBox2.Text.Length >= 16))
            {
                errorProvider1.SetError(textBox2, "パスワードが違います。7文字以上15文字以内で入力してください");
                textBox2.Focus();
                error = true;
            }
            if (error)
            {
                return;
            }
            
            this.Hide();
            Menu menu = new Menu();
            menu.ShowDialog();
            menu.Dispose();
            textBox1.Clear();
            textBox2.Clear();
            this.Show();
                  
        }

       
    }
}
