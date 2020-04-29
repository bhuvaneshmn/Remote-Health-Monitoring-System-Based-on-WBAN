using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Patient_Monitoring_System
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader rs;
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registerfrm frm = new Registerfrm();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                string sql = string.Format("Select pname,pid from ptbl where UID='{0}' and PWD='{1}'",textBox1.Text,textBox2.Text);
                cmd = new SqlCommand(sql,con);
                rs = cmd.ExecuteReader();
                if (rs.Read())
                {
                    HomeFrm frm = new HomeFrm(rs[0].ToString(),rs[1].ToString());
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid User name or Password");
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
                rs.Close();
                cmd.Cancel();
                con.Close();
            }
            catch (Exception ex) { }
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string sqlpath = @"Data Source=.\sqlexpress;AttachDbFilename=C:\Users\bhuvanesh\Desktop\Final\PatinetDB\patientmanDB.mdf;Integrated Security=True;User Instance=True";
                con = new SqlConnection(sqlpath);
            }
            catch (Exception ex) { }
        }
    }
}
