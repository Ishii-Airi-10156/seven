using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace seven
{
    public partial class CustomerList : Form
    {

        private readonly string sqlConnectionString =
               ConfigurationManager.ConnectionStrings["delivery_system"].ConnectionString;

        public CustomerList()
        {
            InitializeComponent();
        }

        private void CustomerList_Load(object sender, EventArgs e)
        {

            string sql = "SELECT * FROM customer ";

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = sqlConnectionString;
            connection.Open();

        }
    }
}
