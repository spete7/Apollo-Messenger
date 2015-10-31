using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net.WebSockets;

namespace SQLDatabase
{
    public partial class Home_Screen : Form
    {
        private string conn;
        private MySqlConnection connect;
        Login f;

        public Home_Screen(string strTxtBox)
        {
            InitializeComponent();
            toolStripLabel1.Text = strTxtBox;

        }

        private bool checkImage()
        {
            db_connection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select users.imagePath from users where username= '" + toolStripLabel1.Text + "';";
            cmd.Connection = connect;
            MySqlDataReader readR = cmd.ExecuteReader();
            if (readR.Read())
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
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

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

            string filePath = openFileDialog1.FileName;
            string directory = Path.GetDirectoryName(filePath);
            string name = Path.GetFileName(filePath);
            string fileExt = Path.GetExtension(name);
            //MessageBox.Show(name);
            //MessageBox.Show(filePath);    <---These are for debugging purposes only
            //MessageBox.Show(directory);

            if (fileExt == ".png" || fileExt == ".jpg" || fileExt == ".jpeg")
            {
                try
                {
                    string server = "ftp://jayyeah977.no-ip.org/";
                    string username = "ftpadmin";
                    string password = "ftpadmin";

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server + name);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.UseBinary = true;

                    request.Credentials = new NetworkCredential(username, password);

                    byte[] fileContents = File.ReadAllBytes(directory + "/" + name);

                    request.ContentLength = fileContents.Length;

                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(fileContents, 0, fileContents.Length);
                        requestStream.Close();
                    }
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                    MessageBox.Show("Upload File Complete", response.StatusDescription);

                    response.Close();

                    db_connection();
                    string query = "UPDATE users SET imagePath = '" + server + name + "' WHERE username = '" + toolStripLabel1.Text + "';";
                    MySqlCommand cmd = new MySqlCommand(query, connect);
                    MySqlDataReader readR = cmd.ExecuteReader();
                    while (readR.Read())
                    {

                    }

                    connect.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("You may only upload in the png, jpg or jpeg image formats!");
            }

            loadProfileImage();
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }
        }

        public void loadProfileImage()
        {
            if (checkImage())
            {
                string directory = Path.GetTempPath();

                try
                {
                    string server = "ftp://jayyeah977.no-ip.org/";
                    string username = "ftpadmin";
                    string password = "ftpadmin";
                    string imagePath;


                    using (MySqlConnection connection = new MySqlConnection("Server=110.20.157.66;Database=prototypeAlpha;Uid=peterp;Pwd=cena77;"))
                    using (MySqlCommand command = new MySqlCommand("SELECT users.imagePath FROM users WHERE username =@user;"))
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.Parameters.AddWithValue("@user", toolStripLabel1);
                        imagePath = command.ExecuteScalar().ToString();
                        //MessageBox.Show(imagePath);
                        connection.Close();
                    }
                    string name = Path.GetFileName(imagePath);

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(imagePath);
                    request.Proxy = null;
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.UsePassive = false;
                    request.KeepAlive = true;
                    request.UseBinary = true;

                    request.Credentials = new NetworkCredential(username, password);

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    using (Stream responseStream = response.GetResponseStream())
                    using (FileStream destination = File.Create(directory + name))
                    {
                        CopyStream(responseStream, destination);
                    }
                    MessageBox.Show("Download Complete");

                    string fileData = directory + name;
                    string imageDir = Path.GetPathRoot(fileData);
                    MessageBox.Show(fileData);

                    toolStripButton1.Image = Bitmap.FromFile(fileData);
                    pictureBox1.Image = Bitmap.FromFile(fileData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void Home_Screen_Load(object sender, EventArgs e)
        {
            loadProfileImage();
        }

        private void Home_Screen_FormClosing(object sender, FormClosingEventArgs e) {
            if (string.Equals((sender as Button).Name, @"CloseButton"))
            {
                if (f != null)
                    f.Close();
            }
                // Do something proper to CloseButton.
            else
            {
                if (f != null)
                    f.Close();
            }
                // Then assume that X has been clicked and act accordingly.
}

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            label2.Text = toolStripLabel1.Text;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //pictureBox1.Image = toolStripButton1.Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}
