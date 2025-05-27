using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
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

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "IDを入力してください");
                errorProvider1.SetError(textBox2, "IDが不正なため、パスワードの認証ができません");
                textBox1.Focus();
                error = true;

            }
            else
            {
                if (!int.TryParse(textBox1.Text, out int num))
                {
                    errorProvider1.SetError(textBox1, "IDが違います");
                    errorProvider1.SetError(textBox2, "IDが不正なため、パスワードの認証ができません");
                    textBox1.Focus();
                    error = true;

                }
                if (textBox1.Text.Length >= 4)
                {
                    errorProvider1.SetError(textBox1, "IDが違います");
                    errorProvider1.SetError(textBox2, "IDが不正なため、パスワードの認証ができません");
                    textBox1.Focus();
                    error = true;
                    
                }
               
                if (error == false && num >= 1000 || num <= -1)
                {
                    errorProvider1.SetError(textBox1, "IDが違います");
                    errorProvider1.SetError(textBox2, "IDが不正なため、パスワードの認証ができません");
                    textBox1.Focus();
                    error = true;

                }
                else if (error == false && num <= 1000 || num > 0)
                {
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
                            errorProvider1.SetError(textBox2, "IDが不正なため、パスワードの認証ができません");
                            textBox1.Focus();
                            error = true;
                        }
                    connection.Close();
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

            if (String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "パスワードを入力してください");
                textBox2.Focus();
                error = true;
            }
            else
            {
                if (textBox2.Text.Length <= 6 || textBox2.Text.Length >= 16)
                {
                    errorProvider1.SetError(textBox2, "7文字以上15文字以内で入力してください");
                    textBox2.Focus();
                    error = true;
                }
            }

            
            if(error == false)
            {
                try
                {
                    

                    SqlConnection connection = new SqlConnection();
                    connection.ConnectionString = sqlConnectionString;
                    connection.Open();

                    string sql = " select pass from pass where emp_id = @p2 ";

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = sql.ToString();
                    command.Parameters.Add("@p2", SqlDbType.Int).Value = textBox1.Text;

                    string reader = (string)command.ExecuteScalar();

                   

                    if (reader != textBox2.Text)
                    {
                        errorProvider1.SetError(textBox2, "パスワードが違います");
                        textBox2.Focus();
                        return;
                    }

                   connection.Close();

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
