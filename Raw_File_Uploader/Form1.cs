using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace Raw_File_Uploader
{

    public partial class Form1 : Form
    {
        public static DateTime lastchangetime = DateTime.Now;
        public static DateTime lastemailtime = DateTime.Today.AddDays(-1);

        FileSystemWatcher watcher = new FileSystemWatcher();
        private bool monitor_on = false;
        public Form1()
        {
            InitializeComponent();
            //txtserver.Text = "http://10.37.240.41/files/api/";
            txtserver.Text = "http://10.37.35.98:8000/files/api/";

            txtusername.Text = "XiaofengXie";
            txtpassword.Text = "p11111111";
            txtprojectname.Text = "test2";
            filetype.Text = "*.raw";
            minisize.Text = "100";
            alert_threshold.Text = "30";
            frequency_threshold.Text = "1";
            qctool.SelectedIndex = 1;
            // 0 is none, 1 is msfragger, 2 is Maxquant, 3 is Protein Discovery, 4 is matchbetween run with maxquant
            //recipient_email.Text = "xiexf128@gmail.com";

            var lastupload = "";
            DateTime lastuploadtime = DateTime.Now;
            lastchangtimefield.Text = lastchangetime.ToString();

            var context = SynchronizationContext.Current;




            version_number.Text = GetPublishedVersion();

            watcher.Changed += (s, e) =>
            {
                FileInfo file = new FileInfo(e.FullPath);
                System.Threading.Thread.Sleep(1000);
                lastchangetime = DateTime.Now;
                
                context.Post(val => lastchangtimefield.Text = lastchangetime.ToString(), s);
                if (monitor_on && !IsLocatedByHomePage(file))
                {
                    try { 

                    if (new FileInfo(e.FullPath).Length > Int32.Parse(minisize.Text) * 1000000)
                    {
                        if (lastupload != e.FullPath | (DateTime.Now - lastuploadtime > new TimeSpan(0, 10, 0))) // if same file name is triggerred in less than 10 min, to prevent double uploading
                        {
                            context.Post(val => output.AppendText(Environment.NewLine + "***********************************************"), s);

                            context.Post(val => output.AppendText(Environment.NewLine + $"File /{e.Name}/ with final size {new FileInfo(e.FullPath).Length / 1000000} MB, bigger than setting {Int32.Parse(minisize.Text)}, will be uploaded"), s);
                            context.Post(val => output.AppendText(Environment.NewLine + "***********************************************"), s);
                            context.Post(val => filepath.Text = e.FullPath, s);
                            context.Post(val => uploadfile(filepath.Text), s);
                            lastupload = e.FullPath;
                            lastuploadtime = DateTime.Now;
                        }

                    }
                    else
                    {
                        context.Post(val => output.AppendText(Environment.NewLine + $"{e.Name} final size is {new FileInfo(e.FullPath).Length}, smaller than setting {Int32.Parse(minisize.Text) * 1000000}, will NOT be uploaded"), s);


                    }


                }
                    catch (Exception e1)
                    {
                        context.Post(val => output.AppendText(Environment.NewLine + "error detected" +e1), s);

                    }
                }

            };



        }


        private string GetPublishedVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.
                    CurrentVersion.ToString();
            }
            return "Not network deployed";
        }

        private void uploadfile(string filelocation)
        {
            output.AppendText(Environment.NewLine + DateTime.Now + $" Start Uploading {filelocation}");
            string newfilelocation;
            if (nocopy.Checked) {
                newfilelocation = filelocation;

            }
            else
            {

                newfilelocation = Path.GetDirectoryName(filelocation) + @"\temp\" + Path.GetFileName(filelocation);

                //check if a temp patch exits, create one if not.
                string temppath = Path.GetDirectoryName(filelocation) + @"\temp\";

                bool exists = System.IO.Directory.Exists(temppath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(temppath);

                if (File.Exists(newfilelocation))
                {
                    File.Delete(newfilelocation);
                }

                try //for somereason, copy file failed sometimes....
                {
                    File.Copy(filelocation, newfilelocation, true);

                }

                catch
                {
                    output.AppendText(Environment.NewLine + DateTime.Now + $" {newfilelocation} uploading failed once will try again");

                    System.Threading.Thread.Sleep(30000);


                    try //2nd try
                    {
                        File.Copy(filelocation, newfilelocation, true);

                    }

                    catch
                    {
                        output.AppendText(Environment.NewLine + DateTime.Now + $" {newfilelocation} uploading failed second time");

                        return;
                    }
                    // Todo: Additional recovery here,
                    // like telling the calling code to re-open the file selection dialog
                }

            }

            var client = new RestClient(txtserver.Text);
            client.Timeout = 10 * 60 * 1000;// 1000 ms = 1s, 30 min = 30*60*1000

            client.Authenticator = new HttpBasicAuthenticator(txtusername.Text, txtpassword.Text);

            if (!File.Exists(newfilelocation))
            {
                MessageBox.Show("Please check file location");
                return;
            }

            var request = new RestRequest();

            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "multipart/form-data");
            if(String.IsNullOrEmpty(txtsamplename.Text))
            {
                request.AddParameter("run_name", Path.GetFileNameWithoutExtension(newfilelocation));
            }
            else
            {
                request.AddParameter("run_name", txtsamplename.Text);

            }

            request.AddParameter("project_name", txtprojectname.Text);
            request.AddParameter("run_desc", txtdescription.Text);
            request.AddParameter("qc_tool", qctool.SelectedIndex);
            request.AddParameter("temp_data", TempData.Checked);
            request.AddFile("rawfile", newfilelocation);
            request.ReadWriteTimeout = 2147483647;
            request.Timeout = 2147483647;
            var response = client.Execute(request);
            output.AppendText(Environment.NewLine + response.Content);
            output.AppendText(Environment.NewLine + DateTime.Now + $" {newfilelocation} uploaded");
            if (!nocopy.Checked)
            {
                File.Delete(newfilelocation);
            }
        }






        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exit_Click(object sender, EventArgs e)
        {

        }


        private void folderbutton_Click_1(object sender, EventArgs e)

        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                foldertxt.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void filebutton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Raw Files",

                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "raw",
                Filter = "raw files (*.raw)|*.raw|All files (*.*)|*.*",
                RestoreDirectory = true,

            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filepath.Text = openFileDialog1.FileName;
            }
        }

        private void monitor_Click(object sender, EventArgs e)
        {
            if (!isDirectoryValid(foldertxt.Text))
            {
                MessageBox.Show("invalid folder");

                return;
            }
            if (monitor_on is true)
            { watchtriggle(false, Color.Red); }
            else
            { watchtriggle(true, Color.Green); }

        }

        private bool isDirectoryValid(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void watchtriggle(Boolean triggle, Color btcolor)
        {


            monitor_on = triggle;
            monitor.BackColor = btcolor;
            Thread t = new Thread(myFun);
            t.Name = "finish_check_thread";
            t.IsBackground = true;
            t.Start();

            // tell the watcher where to look
            watcher.Path = @foldertxt.Text;
            watcher.Filter = filetype.Text;
            // You must add this line - this allows events to fire.
            watcher.EnableRaisingEvents = triggle;
            if (monitor_on is true)
            {
                output.AppendText(Environment.NewLine + DateTime.Now + $" start to monitor folder {foldertxt.Text} for {filetype.Text} ");
            }
            else
            {
                output.AppendText(Environment.NewLine + DateTime.Now + $" Stop to monitor folder {foldertxt.Text} ");
            }

        }
        private bool IsFileunLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {

                return false;
            }

            //file is not locked
            return true;
        }


        private bool IsLocatedByHomePage(FileInfo file)
        {

            var process_list = FileUtil.WhoIsLocking(file.FullName);

            foreach (Process lockprocess in process_list)
            {
                if (lockprocess.ProcessName == "HomePage") { return true; }


            }


            return false;

        }

        private void single_upload_Click(object sender, EventArgs e)
        {
            uploadfile(filepath.Text);

        }


        private void myFun()
        {
            while (monitor_on && !String.IsNullOrEmpty(recipient_email.Text)) { 
            TimeSpan difference = DateTime.Now - lastchangetime ;
            TimeSpan email_difference = DateTime.Now - lastemailtime;

                if (difference.Minutes > int.Parse(alert_threshold.Text) && email_difference.Hours > int.Parse(frequency_threshold.Text))
            {
                lastemailtime = DateTime.Now;
                send_notification();

            }

                Thread.Sleep(60000);
            }
        }
        private void send_notification()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("proteomicsdatamanager@gmail.com");
                mail.To.Add(recipient_email.Text);
                mail.Subject = "Acquisition Finished or Stopped";
                mail.Body = "This is a notification from Raw file uploader to notify you the Acquisition has finished or stopped ";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("proteomicsdatamanager@gmail.com", "WAm38HzgXu6WE^");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
             output.AppendText(Environment.NewLine + DateTime.Now + ex.Message);

            }

        }
        private void folder_uploader_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(foldertxt.Text, "*.raw*");
            string[] subDirs = Directory.GetDirectories(foldertxt.Text);

            DialogResult dialogResult = MessageBox.Show($"There are about {files.Count()} files, You are sure to upload them all?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                foreach (string file in files)
                {
                    if (new FileInfo(file).Length > Int32.Parse(minisize.Text) * 1000000)
                    {
                        output.AppendText(Environment.NewLine + DateTime.Now + " Uploading" + file);
                        uploadfile(file);
                    }
                    else
                    {
                        output.AppendText(Environment.NewLine + DateTime.Now + " file:" + file + $"less than setting {Int32.Parse(minisize.Text) * 1000000}, will NOT be uploaded ");

                    }
                }

            }

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void saveSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will close down the whole application. Confirm?", "Close Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                this.Activate();
            }
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }













    static public class FileUtil
    {
        [StructLayout(LayoutKind.Sequential)]
        struct RM_UNIQUE_PROCESS
        {
            public int dwProcessId;
            public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
        }

        const int RmRebootReasonNone = 0;
        const int CCH_RM_MAX_APP_NAME = 255;
        const int CCH_RM_MAX_SVC_NAME = 63;

        enum RM_APP_TYPE
        {
            RmUnknownApp = 0,
            RmMainWindow = 1,
            RmOtherWindow = 2,
            RmService = 3,
            RmExplorer = 4,
            RmConsole = 5,
            RmCritical = 1000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct RM_PROCESS_INFO
        {
            public RM_UNIQUE_PROCESS Process;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
            public string strAppName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
            public string strServiceShortName;

            public RM_APP_TYPE ApplicationType;
            public uint AppStatus;
            public uint TSSessionId;
            [MarshalAs(UnmanagedType.Bool)]
            public bool bRestartable;
        }

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
        static extern int RmRegisterResources(uint pSessionHandle,
                                              UInt32 nFiles,
                                              string[] rgsFilenames,
                                              UInt32 nApplications,
                                              [In] RM_UNIQUE_PROCESS[] rgApplications,
                                              UInt32 nServices,
                                              string[] rgsServiceNames);

        [DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
        static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

        [DllImport("rstrtmgr.dll")]
        static extern int RmEndSession(uint pSessionHandle);

        [DllImport("rstrtmgr.dll")]
        static extern int RmGetList(uint dwSessionHandle,
                                    out uint pnProcInfoNeeded,
                                    ref uint pnProcInfo,
                                    [In, Out] RM_PROCESS_INFO[] rgAffectedApps,
                                    ref uint lpdwRebootReasons);

        /// <summary>
        /// Find out what process(es) have a lock on the specified file.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <returns>Processes locking the file</returns>
        /// <remarks>See also:
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa373661(v=vs.85).aspx
        /// http://wyupdate.googlecode.com/svn-history/r401/trunk/frmFilesInUse.cs (no copyright in code at time of viewing)
        /// 
        /// </remarks>
        static public List<Process> WhoIsLocking(string path)
        {
            uint handle;
            string key = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();

            int res = RmStartSession(out handle, 0, key);

            if (res != 0)
                throw new Exception("Could not begin restart session.  Unable to determine file locker.");

            try
            {
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfoNeeded = 0,
                     pnProcInfo = 0,
                     lpdwRebootReasons = RmRebootReasonNone;

                string[] resources = new string[] { path }; // Just checking on one resource.

                res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);

                if (res != 0)
                    throw new Exception("Could not register resource.");

                //Note: there's a race condition here -- the first call to RmGetList() returns
                //      the total number of process. However, when we call RmGetList() again to get
                //      the actual processes this number may have increased.
                res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (res == ERROR_MORE_DATA)
                {
                    // Create an array to store the process results
                    RM_PROCESS_INFO[] processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;

                    // Get the list
                    res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);

                    if (res == 0)
                    {
                        processes = new List<Process>((int)pnProcInfo);

                        // Enumerate all of the results and add them to the 
                        // list to be returned
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            try
                            {
                                processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                            }
                            // catch the error -- in case the process is no longer running
                            catch (ArgumentException) { }
                        }
                    }
                    else
                        throw new Exception("Could not list processes locking resource.");
                }
                else if (res != 0)
                    throw new Exception("Could not list processes locking resource. Failed to get size of result.");
            }
            finally
            {
                RmEndSession(handle);
            }

            return processes;
        }
    }







}
