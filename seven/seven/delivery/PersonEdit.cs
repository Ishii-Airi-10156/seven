﻿using System;
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
    public partial class PersonEdit : Form
    {
        string a;
        private readonly string sqlConnectionString =
            ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;
        
        public PersonEdit()
        {
            InitializeComponent();
            numericUpDown1.Enabled = false;
            label1.Text = "従業員情報登録";
        }
        public PersonEdit(int emp_id, string emp_name, string area, int truck_no,int co)
        {
            InitializeComponent();
            textBox1.Text = emp_id.ToString();
            textBox2.Text = emp_name;
            a = area;
            textBox3.Text = truck_no.ToString();
            numericUpDown1.Value = Convert.ToDecimal(co);
            label1.Text = "従業員情報編集";
            numericUpDown1.Enabled = true;

        }
        private void PersonEdit_Load(object sender, EventArgs e)
        {
            
        }
        private void DateUpdate()
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                connection.Open();
                string sql = "update employee set emp_name = @p2, area = @p3, truck_no = @p4 ,co = @p5 where emp_id = @p1";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@p1", SqlDbType.Int).Value = textBox1.Text;
                command.Parameters.Add("@p2", SqlDbType.NVarChar).Value = textBox2.Text;
                command.Parameters.Add("@p3", SqlDbType.NVarChar).Value = comboBox1.Text;
                command.Parameters.Add("@p4", SqlDbType.Int).Value = textBox3.Text;
                command.Parameters.Add("@p5", SqlDbType.Int).Value = numericUpDown1.Value;
                int result = command.ExecuteNonQuery();
                
                
            }
            
        }
        private void Insert()
        {
            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            { 
                connection.Open();
                string sql = "insert into employee(emp_name,area,truck_no,active) values(@p1,@p2,@p3,@p4)";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@p1", SqlDbType.NVarChar).Value = textBox2.Text;
                command.Parameters.Add("@p2", SqlDbType.NVarChar).Value = comboBox1.Text;
                command.Parameters.Add("@p3", SqlDbType.Int).Value = textBox3.Text;
                command.Parameters.Add("@p4", SqlDbType.Bit).Value = 0;
                int result = command.ExecuteNonQuery();
            }
            
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool c = true;
            if(String.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "名前を入力してください");

                c = false;
                
            }
            if(int.TryParse(textBox2.Text, out int nnum))
            {
                errorProvider1.SetError(textBox2, "名前は数字を入力できません");
                c = false;

            }
            if(textBox2.Text.Length > 10)
            {
                errorProvider1.SetError(textBox2, "10文字以内で入力してください");
                c = false;
            }
            if (String.IsNullOrEmpty(comboBox1.Text))
            {
                errorProvider1.SetError(comboBox1, "エリアを選択してください");
                c = false;
                
            }
            if (String.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "トラックナンバーを入力してください");
                c = false;
                
            }
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                if (!int.TryParse(textBox3.Text, out int num))
                {
                    errorProvider1.SetError(textBox3, "数字を入力してください");
                    c = false;
                    return;
                }
                
                try
                {
                    using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                    {
                        connection.Open();
                        string sql = "select count(truck_no) from truck ";
                        SqlCommand command = new SqlCommand(sql, connection);
                        int result = (int)command.ExecuteScalar();
                        if (result < num || num <= 0)
                        {
                            errorProvider1.SetError(textBox3, "存在しないトラックナンバーです");
                            c = false;
                            return;
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

                try
                {
                    using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                    {
                        connection.Open();
                        string sql = "select truck_capacity from truck where truck_no = @p1";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.Add("@p1", SqlDbType.Int).Value = textBox3.Text;
                        int result = (int)command.ExecuteScalar();
                        if (numericUpDown1.Value > result)
                        {
                            errorProvider1.SetError(numericUpDown1, "トラックの積載量を超えています");
                            c = false;

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
            if(c == false)
            {
                return;
            }
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                DialogResult dialogResult =
                    MessageBox.Show("データを編集しますか？", "確認",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                DateUpdate();
            }
            else
            {
                DialogResult dialogResult =
                    MessageBox.Show("データを追加しますか？", "確認",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                Insert();
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Citem()
        {
            a = comboBox1.Text;
            comboBox1.Items.Clear();
            
            List<string> area = new List<string>()
            {
                "足立区","荒川区","板橋区","江戸川区","大田区","葛飾区","北区","江東区","品川区",
                "渋谷区","新宿区","杉並区","墨田区","世田谷区","台東区","中央区","千代田区","豊島区",
                "中野区","練馬区","文京区","港区","目黒区"
            };
            comboBox1.Items.AddRange(area.ToArray());
            comboBox1.Text = a;
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            Citem();
        }

        private void PersonEdit_Load_1(object sender, EventArgs e)
        {
            
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            List<string> area = new List<string>()
            {
                "足立区","荒川区","板橋区","江戸川区","大田区","葛飾区","北区","江東区","品川区",
                "渋谷区","新宿区","杉並区","墨田区","世田谷区","台東区","中央区","千代田区","豊島区",
                "中野区","練馬区","文京区","港区","目黒区"
            };
            comboBox1.Items.AddRange(area.ToArray());
            comboBox1.Text = a;
        }
    }
}
