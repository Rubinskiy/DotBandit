using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotBandit
{
    public partial class Password : Form
    {
        public Password()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == Properties.Settings.Default.Pass)
            {
                MainBuilder Bandit = new MainBuilder();
                Bandit.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong Password!", "DotBandit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Password_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.DontShow)
                new MainBuilder().ShowDialog();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                textBox1.UseSystemPasswordChar = true;
            }
        }
    }
}
