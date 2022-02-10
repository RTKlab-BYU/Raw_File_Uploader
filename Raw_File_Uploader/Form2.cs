using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace Raw_File_Uploader
{
    public partial class emailsetting : Form
    {
        public emailsetting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.custom_emailserver = use_customemail.Checked;
            Properties.Settings.Default.SMTP_server = smtp.Text;
            Properties.Settings.Default.sender = sendertxt.Text;
            Properties.Settings.Default.server_port = serverporttext.Text;
            Properties.Settings.Default.password = emailpassword.Text;
            Properties.Settings.Default.enable_ssl = enabledssl.Checked;
            Properties.Settings.Default.notification_sub = notification_subject.Text;
            Properties.Settings.Default.notification_body = notifcation_body.Text;



            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
                       

        {
            use_customemail.Checked = Properties.Settings.Default.custom_emailserver;
            smtp.Text=Properties.Settings.Default.SMTP_server;
            sendertxt.Text=Properties.Settings.Default.sender;
            serverporttext.Text=Properties.Settings.Default.server_port;
            emailpassword.Text = Properties.Settings.Default.password;
            enabledssl.Checked = Properties.Settings.Default.enable_ssl;
            notification_subject.Text = Properties.Settings.Default.notification_sub;
            notifcation_body.Text = Properties.Settings.Default.notification_body;
        }

        private void testserver_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Make sure save before test.This will using the server setting to send a email notification to the sender.");
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Properties.Settings.Default.SMTP_server);
            mail.From = new MailAddress(Properties.Settings.Default.sender);
            mail.To.Add(Properties.Settings.Default.sender);
            mail.Subject = Properties.Settings.Default.notification_sub;
            mail.Body = Properties.Settings.Default.notification_body;
            SmtpServer.Port = Int32.Parse(Properties.Settings.Default.server_port);
            SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.sender, Properties.Settings.Default.password);
            SmtpServer.EnableSsl = Properties.Settings.Default.enable_ssl;
            SmtpServer.Send(mail);
        }
    }
}
