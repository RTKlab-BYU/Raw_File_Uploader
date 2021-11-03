using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raw_File_Uploader
{
    public partial class Form1 : Form
    {

        FileSystemWatcher watcher = new FileSystemWatcher();
        private bool monitor_on = false;
        public Form1()
        {
            InitializeComponent();
            //txtserver.Text = "http://10.37.240.41/files/api/";
            txtserver.Text = "http://192.168.102.188/files/api/";

            txtusername.Text = "admin";
            txtpassword.Text = "admin";
            txtprojectname.Text = "test2";
            filetype.Text = "*.raw";
            minisize.Text = "100";
            var lastupload = "";
            DateTime lastuploadtime = DateTime.Now;
            var context = SynchronizationContext.Current;



            watcher.Changed += (s, e) =>
            {
                FileInfo file = new FileInfo(e.FullPath);
                System.Threading.Thread.Sleep(1000);
                //if (monitor && IsFileunLocked(file)) {
                if (monitor_on && !IsLocatedByHomePage(file))
                {

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


            };



        }




        private void uploadfile(string filelocation)
        {
            output.AppendText(Environment.NewLine + DateTime.Now + $" Start Uploading {filelocation}");


            var client = new RestClient(txtserver.Text);
            client.Timeout = 30 * 60 * 1000;// 1000 ms = 1s, 30 min = 30*60*1000
            client.Authenticator = new HttpBasicAuthenticator(txtusername.Text, txtpassword.Text);

            if (!File.Exists(filelocation))
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
                request.AddParameter("run_name", Path.GetFileNameWithoutExtension(filelocation));
            }
            else
            {
                request.AddParameter("run_name", txtsamplename.Text);

            }

            request.AddParameter("project_name", txtprojectname.Text);
            request.AddParameter("run_desc", txtdescription.Text);
            request.AddParameter("spectromine_qc", SpectromineQc.Checked);
            request.AddParameter("maxquant_qc", MaxquantQc.Checked);
            request.AddParameter("temp_data", TempData.Checked);
            request.AddFile("rawfile", filelocation);
            var response = client.Execute(request);
            output.AppendText(Environment.NewLine + response.Content);
            output.AppendText(Environment.NewLine + DateTime.Now + $" {filelocation} uploaded");

        }






        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exit_Click(object sender, EventArgs e)
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

        private void folderbutton_Click(object sender, EventArgs e)
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

        private void filebutton_Click(object sender, EventArgs e)
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


            // tell the watcher where to look
            watcher.Path = @foldertxt.Text;
            watcher.Filter = filetype.Text;
            // You must add this line - this allows events to fire.
            watcher.EnableRaisingEvents = triggle;
            if (monitor_on is true)
            {
                output.AppendText(Environment.NewLine + $"start to monitor folder {foldertxt.Text} for {filetype.Text} ");
            }
            else
            {
                output.AppendText(Environment.NewLine + $"Stop to monitor folder {foldertxt.Text} ");
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
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return false;
            }

            //file is not locked
            return true;
        }


        private bool IsLocatedByHomePage(FileInfo file)
        {

            //return FileUtil.WhoIsLocking(file.FullName).First().ProcessName == "HomePage";
            var process_list = FileUtil.WhoIsLocking(file.FullName);

            foreach (Process lockprocess in process_list)
            {
                if (lockprocess.ProcessName == "HomePage") { return true; }
                //if (lockprocess.ProcessName == "WINWORD") { return true; }


            }


            //First().ProcessName == "HomePage";
            return false;

        }

        private void single_upload_Click(object sender, EventArgs e)
        {
            uploadfile(filepath.Text);

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
                        output.AppendText(Environment.NewLine + "Uploading" + file);
                        uploadfile(file);
                    }
                    else
                    {
                        output.AppendText(Environment.NewLine + "file:" + file + $"less than setting {Int32.Parse(minisize.Text) * 1000000}, will NOT be uploaded ");

                    }
                }

            }

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
