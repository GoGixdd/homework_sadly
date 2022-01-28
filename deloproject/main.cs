using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deloproject
{
    public partial class main : Form
    {
        private string _username;
        public static int SelectedTaskId = 0;
        public static bool isAddingNewTask = false;
        private sqlmanager dbinstance;

        public main(string username, sqlmanager sqlInstance)
        {
            _username = username;
            dbinstance = sqlInstance;

            InitializeComponent();
            statuslogin.Text = "Логин: " + _username;
            lockPanel();
            refreshLists();
        }

        private void refreshLists()
        {
            var tasks = dbinstance.isGetAllTask(_username);
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            foreach (var task in tasks)
            {
                string outed = "[" + task.Item1.ToString() + "] [" + task.Item3 + "] " + task.Item2;
                if (task.Item5 == "Сделать")
                    listBox1.Items.Add(outed);
                else if (task.Item5 == "В процессе")
                    listBox2.Items.Add(outed);
                else if (task.Item5 == "Готово")
                    listBox3.Items.Add(outed);
            }
        }

        private void button1_Click(object sender, EventArgs e) //addTask
        {
            isAddingNewTask = true;
            button1.Visible = false;
            button3.Visible = true;
            button3.BackColor = Color.White;
            clearPanel();
            unlockPanel();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTaskId = getCurrentId(listBox1);
            var t = dbinstance.isGetTask(SelectedTaskId);
            showDataInPanel(t.Item1.ToString(), t.Item3, t.Item4, t.Item5, t.Item2);
        }

        private int getCurrentId(ListBox ex)
        {
            int outed = 0;
            foreach (var item in ex.SelectedItems)
            {
                string[] Raw = item.ToString().Split(']');

                outed = int.Parse(Raw[0].Substring(1));
            }
            return outed;
        }

        private void lockPanel()
        {
            textBox4.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox1.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void unlockPanel()
        {
            textBox1.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            comboBox1.Enabled = true;
        }

        private void clearPanel()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox1.Text = "";
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTaskId = getCurrentId(listBox2);
            var t = dbinstance.isGetTask(SelectedTaskId);
            showDataInPanel(t.Item1.ToString(), t.Item3, t.Item4, t.Item5, t.Item2);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedTaskId = getCurrentId(listBox3);
            var t = dbinstance.isGetTask(SelectedTaskId);
            showDataInPanel(t.Item1.ToString(), t.Item3, t.Item4, t.Item5, t.Item2);
        }

        private void showDataInPanel(string ID, string data, string worker, string status, string desc)
        {
            if (!isAddingNewTask)
            {
                textBox2.Text = ID;
                textBox1.Text = data;
                textBox3.Text = worker;
                comboBox1.Text = status;
                textBox4.Text = desc;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dbinstance.isCreatedTask(textBox4.Text, textBox1.Text, textBox3.Text, comboBox1.Text, _username);
            isAddingNewTask = false;
            refreshLists();
            lockPanel();
            button1.Visible = true;
            button3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dbinstance.isDeletedTask(SelectedTaskId);
            refreshLists();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbinstance.isMovedTask(SelectedTaskId, comboBox2.Text);
            refreshLists();
        }
    }
}