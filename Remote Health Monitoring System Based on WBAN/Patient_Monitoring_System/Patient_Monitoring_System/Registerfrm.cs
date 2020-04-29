using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Drawing2D;

namespace Patient_Monitoring_System
{
    public partial class Registerfrm : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        OpenFileDialog opFile;
        string fname = "";
        public Registerfrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            opFile = new OpenFileDialog();
            opFile.Title = "Select a Image";
            opFile.Filter = "Jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            opFile.InitialDirectory = "c:";

            opFile.FileName = "";
            opFile.Multiselect = false;

            if (opFile.ShowDialog() != DialogResult.OK)
                return;
            string filename = System.IO.Path.GetFileName(opFile.FileName);
           // MessageBox.Show(filename);
            Image img = Image.FromFile(opFile.FileName);
            Bitmap b = new Bitmap(img);
            System.Drawing.Image i = resizeImage(b, new Size(512, 512));
            fname=@"C:\Patient\"+filename;
            i.Save(@"C:\Patient\"+filename, System.Drawing.Imaging.ImageFormat.Png);
            pictureBox1.ImageLocation = @"C:\Patient\"+filename;

        }
        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {

            //Get the image current width

            int sourceWidth = imgToResize.Width;

            //Get the image current height

            int sourceHeight = imgToResize.Height;



            float nPercent = 0;

            float nPercentW = 0;

            float nPercentH = 0;

            //Calulate  width with new desired size

            nPercentW = ((float)size.Width / (float)sourceWidth);

            //Calculate height with new desired size

            nPercentH = ((float)size.Height / (float)sourceHeight);



            if (nPercentH < nPercentW)

                nPercent = nPercentH;

            else

                nPercent = nPercentW;

            //New Width

            int destWidth = (int)(sourceWidth * nPercent);

            //New Height

            int destHeight = (int)(sourceHeight * nPercent);



            Bitmap b = new Bitmap(destWidth, destHeight);

            Graphics g = Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw image with new width and height

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            g.Dispose();

            return (System.Drawing.Image)b;

        }
        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox10.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "")
            {
                MessageBox.Show("Please Fill the All details");
                textBox1.Clear();
                textBox10.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox1.Focus();
            }
            else
            {
                if (textBox10.Text.Equals(textBox9.Text) == true)
                {
                    try
                    {

                        string strqry = string.Format("INSERT into ptbl(pname,pid,address,gphno,gmailid,phyname,phyphno,UID,PWD,photo) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",textBox1.Text,textBox2.Text,textBox3.Text,textBox4.Text,textBox5.Text,textBox6.Text,textBox7.Text,textBox8.Text,textBox9.Text,fname);
                        con.Open();
                        cmd = new SqlCommand(strqry,con);
                        int n = cmd.ExecuteNonQuery();
                        if(n>0)
                        MessageBox.Show("Registration Successfull");
                        else
                            MessageBox.Show("Registration Error");
                        cmd.Cancel();
                        con.Close();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                }
                else
                {
                    MessageBox.Show("Password Mismatched");
                    textBox9.Text = "";
                    textBox10.Text = "";
                    textBox9.Focus();
                }

            }
        }

        private void Registerfrm_Load(object sender, EventArgs e)
        {
            try
            {
                string sqlpath = @"Data Source=.\sqlexpress;AttachDbFilename=C:\Users\bhuvanesh\Desktop\Final\PatinetDB\patientmanDB.mdf;Integrated Security=True;User Instance=True";
                con = new SqlConnection(sqlpath);
            }
            catch (Exception ex) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dispose();
           // this.Hide();
        }
    }
}
