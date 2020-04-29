using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Recognition;
using System.IO;
using System.Speech.Synthesis;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Threading;


namespace Patient_Monitoring_System
{
    public partial class HomeFrm : Form
    {
        public string response;
        string gMailAccount = "patientmonitoring4@gmail.com";
        string passw = "bhuvi8050bhuvi";
        //string umailid = "akashvannal1997@gmail.com";
        string uname, uid;
        SqlConnection con;
        string gpn, ppn, gmid,fname;
        SqlCommand cmd;
        public HomeFrm(string un,string uid)
        {
            uname = un;
            this.uid = uid;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = System.DateTime.Now.ToString();
        }

        private void HomeFrm_Load(object sender, EventArgs e)
        {

            try
            {
                string sqlpath = @"Data Source=.\sqlexpress;AttachDbFilename=C:\Users\bhuvanesh\Desktop\Final\PatinetDB\patientmanDB.mdf;Integrated Security=True;User Instance=True";
                con = new SqlConnection(sqlpath);
                string strqry = string.Format("Select gphno,gmailid,phyphno,photo from ptbl where UID='{0}' and pid='{1}'",uname,uid);
             
                con.Open();
                cmd = new SqlCommand(strqry, con);
                SqlDataReader rs = cmd.ExecuteReader();
                if (rs.Read())
                {
                    gpn = rs[0].ToString();
                    gmid = rs[1].ToString();
                    ppn = rs[2].ToString();
                    fname = rs[3].ToString();
                    pictureBox1.Image = Image.FromFile(fname);
                    timer1.Enabled = true;
                    SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
                    //textBox1.Text = e.Result.Text;
                    textBox1.Text = uname;
                    textBox2.Text = uid;
                    _synthesizer.Speak("WELCOME to Patient Monitoring System");
                    _synthesizer.Speak("Patient Name is " + uname);
                    _synthesizer.Speak("Patient ID is " + uid);
                }
                else
                {
                    label1.Text = "Invalid Data format";
                }
                cmd.Cancel();
                con.Close();
                rs.Close();
            }
            catch (Exception ex) { }
            
        }

        public void send_mail()
        {
            NetworkCredential loginInfo = new NetworkCredential(gMailAccount, passw);
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(gMailAccount);
            msg.To.Add(new MailAddress(gmid));
            msg.Subject = "Patient Moniroting System";
            string fmsg = "Alert Messgage";
            msg.Body = fmsg;
            msg.IsBodyHtml = true;
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = loginInfo;
                client.Send(msg);
                MessageBox.Show("Mail Sent Successfully");
                groupBox2.BackColor = Color.Red;
                label7.Text = "Mail Sent Successfully";
                SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
                //textBox1.Text = e.Result.Text;
                _synthesizer.Speak("Transaction Completed Successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Message Not sent");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            send_mail();
        }

        private bool sendsms(string mno)
        {
            string sUserID = "sateeshgiri@gmail.com";
            string sPwd = "Kiran@2019";
            string sSID = "DEMOOO";
            string sURL = "";
            string message = "Alert: Patient in danger pls Contact the Concern Person";
            sURL = "http://apps.smslane.com/vendorsms/pushsms.aspx?user=" + sUserID + "&password=" + sPwd + "&msisdn=91" + mno + "&sid=" + sSID + "&msg=" + message + "&fl=0";
            string sResponse = GetResponse(sURL);
            groupBox2.BackColor = Color.Red;
            this.label7.Text = "Message Sent";
            //Response.Write(sResponse);
            return true;
            //result = System.Text.Encoding.UTF8.GetString(sURL);
            
        }
        public static string GetResponse(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
                return "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //sendsms("9900883084");
            //sendsms("7019875711");
            sendsms(gpn);
            sendsms(ppn);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = textBox6.Text;
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            //serialPort1.ReadTimeout = 500;
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);
            //label7.Text = serialPort1.ReadLine();
            timer2.Enabled = true;
            timer2.Interval = 1000;
            timer2.Tick += new System.EventHandler(OnTimerEvent2);
            label8.Text = "OPENED";
        }

      
        
       
        private void OnTimerEvent2(object sender, EventArgs e)
        {
            try
            {
                //label7.Text = Dist_Buffer[1];
                textBox3.Text = Temp_Value.ToString();
                textBox4.Text = pres_Value.ToString();
                if(heart_Value != "0")
                textBox5.Text = heart_Value.ToString();
                int t = int.Parse(Temp_Value);
                int p = int.Parse(pres_Value);
                int h = int.Parse(heart_Value);
                if (t >= 50 && p >= 70)
                {
                    sendsms(gpn);
                    sendsms(ppn);
                    send_mail();
                }
                else if (h >= 150)
                {
                    sendsms(gpn);
                    sendsms(ppn);
                    send_mail();
                }
                else
                {
                    groupBox2.BackColor = Color.Green;
                    this.label7.Text = "Normal";
                }
              
            }
            catch (Exception ex)
            {
                label7.Text = "Normal"; 
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            label8.Text = "Closed";
        }

        string RX_BUFFER;
        string[] Temp_Buffer, Dist_Buffer, Obj_Buffer;

        string Temp_Value = "0", pres_Value = "0", heart_Value = "0";
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            //RX_BUFFER = serialPort1.ReadLine();
            
            RX_BUFFER = serialPort1.ReadLine();
            RX_BUFFER = RX_BUFFER.TrimEnd();
            if (RX_BUFFER.Contains("P"))
            {
                try
                {
                    Dist_Buffer = RX_BUFFER.Split(':');
                    pres_Value = Dist_Buffer[1];
                }
                catch (Exception ex) { pres_Value = "7"; }
            }
            else
            if (RX_BUFFER.Contains("B"))
            {
                Obj_Buffer = RX_BUFFER.Split(':');
                heart_Value = Obj_Buffer[1];
                if (int.Parse(heart_Value) <= 0)
                    heart_Value = "0";
                Console.WriteLine(heart_Value);
            }
            else if (RX_BUFFER.Contains("T"))
            {
                Temp_Buffer = RX_BUFFER.Split(':');

                Temp_Value = Temp_Buffer[1];
            }  
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
