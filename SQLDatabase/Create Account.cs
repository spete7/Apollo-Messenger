using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace SQLDatabase
{
    public partial class CreateAccount : Form
    {
        private string MyConnection2;
        private MySqlConnection MyConn2;

        public CreateAccount()
        {
            InitializeComponent();
        }

        private void db_connection()
        {
            try
            {
                MyConnection2 = "Server=110.20.157.66;Database=prototypeAlpha;Uid=peterp;Pwd=cena77;";
                MyConn2 = new MySqlConnection(MyConnection2);
                MyConn2.Open();
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        private bool checkDuplicate(string user)
        {
            db_connection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select * from users where username=@user";
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Connection = MyConn2;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                MyConn2.Close();
                return true;
            }
            else
            {
                MyConn2.Close();
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string pass = textBox2.Text;

            bool r = checkDuplicate(user);
            if (!r)
            {
                try
                {                   
                    db_connection();

                    string Query = 
                        "insert into users(username, password) values('" + this.textBox1.Text + "','" + this.textBox2.Text + "');";

                    //MyConn2 = new MySqlConnection(MyConnection2);

                    //This is command class which will handle the query and connection object. 

                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);

                    MySqlDataReader MyReader2;

                    //MyConn2.Open();

                    MyReader2 = MyCommand2.ExecuteReader();     // Here our query will be executed and data saved into the database. 

                    MessageBox.Show("Account Created!");

                    while (MyReader2.Read())
                    {



                    }

                    MyConn2.Close();

                    this.Hide();

                }

                catch (Exception ex)
                {



                    MessageBox.Show(ex.Message);

                }
            }
            else
                MessageBox.Show("Username is taken.");
             
 
          }
       }

        
    }
