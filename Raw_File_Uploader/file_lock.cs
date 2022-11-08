using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Windows.Forms;
using System.IO;

namespace Raw_File_Uploader
{
    public partial class file_lock : Form
    {


        private static bool start_loop = false;

        public file_lock()
        {
            InitializeComponent();
            check_freq.Text = "30"; //default check every 30 secs
            Thread t = new Thread(checklock);
            t.Name = "lock_check_thread";
            t.IsBackground = true;
            t.Start();
        }
        private void checklock()
        {
            string path = @"result.txt";
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine((Environment.NewLine + DateTime.Now + $"file lock check started"));
            }

            while (true)
            {
                while (start_loop)
                {
                    List<Process> processes = FileUtil.WhoIsLocking(filepath.Text);
                    if (processes.Count == 0)
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.WriteLine((Environment.NewLine + DateTime.Now + $" file {filepath.Text} is not locked"));
                        }
                    else
                        using (StreamWriter sw = File.AppendText(path))
                        {

                            sw.WriteLine((Environment.NewLine + DateTime.Now + $" file {filepath.Text} is loced by the following processes:" + FileUtil.WhoIsLocking(filepath.Text).FirstOrDefault().ProcessName));
                        }
                    int delay;
                    if (check_freq.Text != "")
                        delay = Int32.Parse(check_freq.Text) + 1;
                    else
                        delay = 1;

                    Thread.Sleep(delay * 1000);
                }
                Thread.Sleep(3000);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Raw Files",

                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "All files (*.*)|*.*",
                RestoreDirectory = true,

            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filepath.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            start_loop = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            start_loop = false;

        }

        private void view_result_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", @"result.txt");

        }
    }
}
