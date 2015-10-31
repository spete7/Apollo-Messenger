using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SQLDatabase
{
    public partial class Login : Form
    {
        public string user;
        public string pass;
        private string conn;
        private MySqlConnection connect;

        public Login()
        {
            InitializeComponent();
        }

        private void db_connection()
        {
            try
            {
                conn = "Server=110.20.157.66;Database=prototypeAlpha;Uid=peterp;Pwd=cena77;";
                connect = new MySqlConnection(conn);
                connect.Open();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool validate_login(string user, string pass)
        {
            db_connection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select * from users where username=@user and password=@pass";
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Connection = connect;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                connect.Close();
                return true;
            }
            else
            {
                connect.Close();
                return false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateAccount frm = new CreateAccount();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            user = textBox1.Text;
            pass = textBox2.Text;
            if (user == "" || pass == "")
            {
                MessageBox.Show("Empty Fields Detected ! Please fill up all the fields");
                return;
            }
            bool r = validate_login(user, pass);
            if (r)
            {
                MessageBox.Show("Login Successful");
                this.Hide();
                Home_Screen frm = new Home_Screen(textBox1.Text);
                frm.Show();
                
            }
            else
                MessageBox.Show("Incorrect Login Credentials");
        }

    }
}
