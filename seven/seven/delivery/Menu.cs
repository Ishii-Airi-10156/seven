using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using seven.delivery;

namespace seven
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerList form = new CustomerList();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GoodsList form = new GoodsList();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PersonList form = new PersonList();
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CarInfo form = new CarInfo();
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OrderManagement form=new OrderManagement();
            form.ShowDialog();
        }
    }
}
