using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deloproject
{
    public partial class auth : Form
    {
        public sqlmanager dbInstance = new sqlmanager("projectdb.db");

        public auth()
        {
            InitializeComponent();
        }

        public void newform(string username)
        {
            this.Hide();
            var form2 = new main(username, dbInstance);
            form2.Closed += (s, args) => this.Close();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBox_login.Text) && !String.IsNullOrWhiteSpace(textBox_pwd.Text))
            {
                dbInstance.InitializeDB();
                if (dbInstance.isCorrectLogin(textBox_login.Text, textBox_pwd.Text))
                {
                    newform(textBox_login.Text);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbInstance.InitializeDB();
            if (!String.IsNullOrWhiteSpace(textBox_login.Text) && !String.IsNullOrWhiteSpace(textBox_pwd.Text))
            {
                dbInstance.isRegistered(textBox_login.Text, textBox_pwd.Text);
                newform(textBox_login.Text);
            }
            else
            {
                MessageBox.Show("Заполните все поля!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbInstance.InitializeDB();
            dbInstance.cmd.CommandText = @"CREATE TABLE IF NOT EXISTS users(id INTEGER PRIMARY KEY, login TEXT, password TEXT)";
            dbInstance.cmd.ExecuteNonQuery();
        }
    }
}