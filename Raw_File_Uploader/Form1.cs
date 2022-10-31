using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using System.Xml.Linq;
using System.Net;

namespace Raw_File_Uploader
{

    public partial class Form1 : Form
    {
        public static DateTime lastchangetime = DateTime.Now;
        public static DateTime lastemailtime = DateTime.Today.AddDays(-1);
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        FileSystemWatcher watcher = new FileSystemWatcher();
        private bool monitor_on = false;
        public Form1()
        {
            InitializeComponent();
            txtserver.Text = "http://192.168.102.188/files/api/"; 
            filetype.Text = "*.raw";
            minisize.Text = "100";
            alert_threshold.Text = "30";
            frequency_threshold.Text = "8";
            bypasskword.Text = "ignore";
            max_size.Text = "2200";
            qctool.SelectedIndex = 1;
            // 0 is none, 1 is msfragger, 2 is Maxquant, 3 is Protein Discovery OTOT, 4 is matchbetween run with maxquant,5 is Protein Discovery OTIT
            storage_option.SelectedIndex = 0;
            sample_type.SelectedIndex = 0;
            // 0 is human, 1 is BSA
            var lastupload = "";
            DateTime lastuploadtime = DateTime.Now;
            log.Debug("Uploader started");
            var context = SynchronizationContext.Current; // for cross thread update to UI




            version_number.Text = GetPublishedVersion();

            watcher.Changed += (s, e) =>
            {
                FileInfo file = new FileInfo(e.FullPath);
                System.Threading.Thread.Sleep(10000);
                lastchangetime = DateTime.Now;

                if (monitor_on && !IsLocatedByHomePage(file))
                {
                    FileInfo file_info = new FileInfo(e.FullPath);


                        try
                    {
                        if (!file_info.Name.Contains(bypasskword.Text))
                        {

                            if (new FileInfo(e.FullPath).Length > Int32.Parse(minisize.Text) * 1000000 && new FileInfo(e.FullPath).Length < long.Parse(max_size.Text) * 1000000)   
                            {
                                if (lastupload != e.FullPath | (DateTime.Now - lastuploadtime > new TimeSpan(0, 30, 0))) // if same file name is triggerred in less than 30 min, to prevent double uploading
                                {

                                    context.Post(val => output.AppendText(Environment.NewLine + $"File /{e.Name}/ with final size {new FileInfo(e.FullPath).Length / 1000000} MB will be uploaded"), s);
                                    context.Post(val => filepath.Text = e.FullPath, s);
                                    log.Debug($"File /{ e.Name}/ with final size { new FileInfo(e.FullPath).Length / 1000000} MB will be uploaded");


                                    context.Post(val => multipleuploadfile(filepath.Text), s);

                                    lastupload = e.FullPath;
                                    lastuploadtime = DateTime.Now;
                                }

                            }
                            else
                            {
                                context.Post(val => output.AppendText(Environment.NewLine + $"{e.Name} final size is {new FileInfo(e.FullPath).Length}, smaller than setting {Int32.Parse(minisize.Text) * 1000000} or bigger than setting {Int32.Parse(max_size.Text) * 1000000} , will NOT be uploaded"), s);
                                log.Debug($"{e.Name} final size is {new FileInfo(e.FullPath).Length}, smaller than setting {Int32.Parse(minisize.Text) * 1000000} or bigger than setting {Int32.Parse(max_size.Text) * 1000000} , will NOT be uploaded");


                            }
                        }
                        else
                        {
                            context.Post(val => output.AppendText(Environment.NewLine + $"File /{e.Name}/ contains bypass keyword {bypasskword.Text}, will not be uploaded"), s);
                            log.Debug($"File /{e.Name}/ contains bypass keyword {bypasskword.Text}, will not be uploaded");
                        }

                    }
                        catch (Exception e1)
                        {
                            context.Post(val => output.AppendText(Environment.NewLine + "error detected" + e1), s);
                            log.Error(e1);

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


        private Boolean uploadmultiplefiles(string filelocations) //upload multiple files
        {
            List<string> filelist = filelocations.Split(',').ToList();
            foreach (String file in filelist)
                if (file != "") { 
                    if (!File.Exists(file))
                    {
                        MessageBox.Show("invalid file");

                    }
                else
                {
                    multipleuploadfile(file);

                }
                }
            return true;

        }







        private Boolean multipleuploadfile(string filelocation) //enable multiple uploading in case one  failed

        {

            if  (!uploadfile(filelocation) )


            {

                if (Retry.Checked)
                {

                    System.Threading.Thread.Sleep(180000); //wait 3 min before upload again
                    if (!uploadfile(filelocation))
                    {
                        output.Select(output.TextLength, 0);
                        output.SelectionColor = Color.Red;
                        output.AppendText($" {Path.GetFileNameWithoutExtension(filelocation)} uploading failed twice, may need to be uploaded manually");
                        output.SelectionColor = Color.Black;
                        log.Error($" {Path.GetFileNameWithoutExtension(filelocation)} uploading failed twice, may need to be uploaded manually");

                    }
                }
                else
                {
                    output.SelectionColor = Color.Red;
                    output.AppendText($" {Path.GetFileNameWithoutExtension(filelocation)} uploading may failed, check if need to be uploaded manually");
                    output.SelectionColor = Color.Black;
                    log.Error($" {Path.GetFileNameWithoutExtension(filelocation)} uploading may failed, check if need to be uploaded manually");

                }
            
            }

            return true;


        }


        private Boolean uploadfile(string filelocation)
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

                        return false;
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
                return false;
            }

            var request = new RestRequest();

            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");
            request.Parameters.Clear();
            request.AddHeader("Content-Type", "multipart/form-data");

            string uploadfilename;
            if (String.IsNullOrEmpty(txtsamplename.Text))
                {
                uploadfilename = Path.GetFileNameWithoutExtension(newfilelocation);
            }
            else
            {
                uploadfilename = txtsamplename.Text;

            }


            if (!String.IsNullOrEmpty(qc_enablekeyword.Text) && !uploadfilename.Contains(qc_enablekeyword.Text))
            {

                request.AddParameter("qc_tool", 0);

            }
            else
            {

                request.AddParameter("qc_tool", qctool.SelectedIndex);


            }


            request.AddParameter("run_name", uploadfilename);
            request.AddParameter("project_name", txtprojectname.Text);
            request.AddParameter("run_desc", txtdescription.Text);
            request.AddParameter("column_sn", column_sn.Text);
            request.AddParameter("spe_sn", spe_sn.Text);
            request.AddParameter("sample_obj", sample_type.SelectedIndex);
            request.AddParameter("storage_option", storage_option.SelectedIndex);

            request.AddParameter("temp_data", TempData.Checked);
            request.AddFile("rawfile", newfilelocation);
            request.ReadWriteTimeout = 2147483647;
            request.Timeout = 2147483647;
            var response = client.Execute(request);




            if (!nocopy.Checked)
            {
                File.Delete(newfilelocation);
            }
            if (response.StatusCode == HttpStatusCode.Created)
                    {
                output.Select(output.TextLength, 0);
                output.SelectionColor = Color.Green;
                output.AppendText(Environment.NewLine + DateTime.Now + $" {filelocation} was sucessfully uploaded");
                log.Debug(response.Content);
                log.Info($" {filelocation} was sucessfully uploaded");

                output.Select(output.TextLength, 0);
                output.SelectionColor = Color.Black;
                output.AppendText(Environment.NewLine);

                return true;
            }
            else
            {
                output.AppendText(Environment.NewLine + response.Content);
                output.Select(output.TextLength, 0);
                output.SelectionColor = Color.Orange;
                output.AppendText($" {filelocation} upload failed, will try again in 3 min if failed first time");
                output.Select(output.TextLength, 0);
                output.SelectionColor = Color.Black;
                output.AppendText(Environment.NewLine);
                log.Warn($" {filelocation} upload failed, will try again in 3 min if failed first time");

                return false;
            }



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
            }
        }




        private void filebutton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Mass Files",

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = "Mass files (" + filetype.Text+ ")| "+filetype.Text+"|All files (*.*)|*.*",
                RestoreDirectory = true,
                Multiselect = true,
            };
            filepath.Text = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog1.FileNames) 
                    filepath.AppendText(file+",");
                    

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
            {
                if (check_connection(true))
                watchtriggle(true, Color.Green); }

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
            Thread t = new Thread(AlertCheck);
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
                log.Debug($" start to monitor folder {foldertxt.Text} for {filetype.Text} ");
            }
            else
            {                
                t.Abort();

                output.AppendText(Environment.NewLine + DateTime.Now + $" Stop to monitor folder {foldertxt.Text} ");
                log.Debug($" Stop to monitor folder {foldertxt.Text} ");

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


            if (check_connection(true))
                /*                multipleuploadfile(filepath.Text);
                 *                
                */
                uploadmultiplefiles(filepath.Text);

        }


        private void AlertCheck()
        {        
              DateTime lastchangetime = DateTime.Now;
              DateTime lastemailtime = DateTime.Today.AddDays(-1);


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
                if (Properties.Settings.Default.custom_emailserver)
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(Properties.Settings.Default.SMTP_server);
                    mail.From = new MailAddress(Properties.Settings.Default.sender);
                    mail.To.Add(recipient_email.Text);
                    mail.Subject = Properties.Settings.Default.notification_sub;
                    mail.Body = Properties.Settings.Default.notification_body;
                    SmtpServer.Port = Int32.Parse(Properties.Settings.Default.server_port);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.sender, Properties.Settings.Default.password);
                    SmtpServer.EnableSsl = Properties.Settings.Default.enable_ssl;
                    SmtpServer.Send(mail);


                }
                else {



                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("proteindatanotifcation@gmail.com");
                    mail.To.Add(recipient_email.Text);
                    mail.Subject = "Acquisition Finished or Stopped";
                    mail.Body = "This is a notification from Raw file uploader to notify you the Acquisition has finished or stopped ";

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("proteindatanotifcation@gmail.com", "rnijfyaiumacgfqi");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);

                }
                log.Debug("Email notification sent");

            }
            catch (Exception ex)
            {
            log.Warn($"Notification failed due to {ex.Message}");

            }

        }
        private void folder_uploader_Click(object sender, EventArgs e)
                                {
            if (!isDirectoryValid(foldertxt.Text))
            {
                MessageBox.Show("invalid folder");

                return;
            }
            string[] files = Directory.GetFiles(foldertxt.Text, "*.raw*");
            string[] subDirs = Directory.GetDirectories(foldertxt.Text);

            DialogResult dialogResult = MessageBox.Show($"There are about {files.Count()} files, You are sure to upload them all?", "Confirm", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes && (check_connection(true))
)
            {
                foreach (string file in files)
                {
                    if (new FileInfo(file).Length > Int32.Parse(minisize.Text) * 1000000)
                    {
                        output.AppendText(Environment.NewLine + DateTime.Now + " Uploading" + file);
                        multipleuploadfile(file);
                    }
                    else
                    {
                        output.AppendText(Environment.NewLine + DateTime.Now + " file:" + file + $"less than setting {Int32.Parse(minisize.Text) * 1000000}, will NOT be uploaded ");

                    }
                }

            }

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

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Uploader Configure File|*.xml";
            saveFileDialog1.Title = "Save a Configure File";
            
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SettingsProvider.setfilename(saveFileDialog1.FileName);
                
                
                TabControl.TabPageCollection pages = tabControl.TabPages;

                foreach (TabPage page in pages) {

                    foreach (Control control in page.Controls)
                    {

                        if (control is TextBox)
                        {
                            if (control.Name != "txtpassword" && control.Name != "lastchangtimefield")
                            SettingsProvider.SetValue(page.Name, control.Name, control.Text);

                        }
                        else if (control is CheckBox)
                        {
                            SettingsProvider.SetValue(page.Name, control.Name, ((CheckBox)control).Checked.ToString());
                            
                        }
                        else if (control is RichTextBox)
                        {
                            SettingsProvider.SetValue(page.Name, control.Name, control.Text);

                        }

                        else if (control is ComboBox) {
                            SettingsProvider.SetValue(page.Name, control.Name, ((ComboBox)control).Text);
                        }


                    }

                }
            }
        }

        private void loadSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse xml Files",

                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xml",
                Filter = "Uploader Configure files (*.xml)|*.xml|All files (*.*)|*.*",
                RestoreDirectory = true,

            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SettingsProvider.setfilename(openFileDialog1.FileName);


                TabControl.TabPageCollection pages = tabControl.TabPages;

                    foreach (TabPage page in pages)
                    {

                        foreach (Control control in page.Controls)
                                {

                        if (control is TextBox)
                        {
                            if (control.Name != "txtpassword" && control.Name != "lastchangtimefield")
                            {

                                control.Text = SettingsProvider.GetValue(page.Name, control.Name, null);
                                ((TextBox)control).Text = SettingsProvider.GetValue(page.Name, control.Name, null);
                            }
                        }

                        else if (control is RichTextBox)
                        {
                            ((RichTextBox)control).Text = SettingsProvider.GetValue(page.Name, control.Name, null);

                        }
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = Convert.ToBoolean(SettingsProvider.GetValue(page.Name, control.Name, null));
                        }
                        else if (control is ComboBox)
                        {
                            ((ComboBox)control).Text =SettingsProvider.GetValue(page.Name, control.Name, null);
                        }
                    }

                }
                

            }
        }

        private void verify_account_Click(object sender, EventArgs e)
        {
            check_connection(false);
        }

        private Boolean check_connection(Boolean backgroundtask)
        {

            //The method used here is a workaround, not really validate password, only check if not timeout or wrong username/password, assume it's good.
            var request = new RestRequest();
            var client = new RestClient(txtserver.Text + "auth/");

            client.Authenticator = new HttpBasicAuthenticator(txtusername.Text, txtpassword.Text);
            request.Method = Method.POST;
            client.Timeout = 2 * 1000;// 1000 ms = 1s

            var response = client.Execute(request);

            //MessageBox.Show(response.Content);
            if (response.Content is "")
            {
                MessageBox.Show("Can't connect to server");
                return false;

            }
            else if (response.Content is "{\"detail\":\"Invalid username/password.\"}")
            {

                MessageBox.Show("Wrong user or password");
                return false;
            }
            else if (response.Content.Contains("AssertionError at /files/api/auth/"))
            {

                if (backgroundtask is false)
                    MessageBox.Show("Success");
                return true;

            }
            else
            {
                MessageBox.Show("Something else is wrong");
                return false;


            }
        }

        private void emailServerSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emailsetting settingsForm = new emailsetting();
            settingsForm.Show();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/xiaofengxie128/Raw_File_Uploader");
        }

        private void qctool_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", @"C:\ProgramData\RawfileUploader\RawfileUploader.log");
        }

        private void log_view_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", @"C:\ProgramData\RawfileUploader\RawfileUploader.log");

        }
    }


    //https://stackoverflow.com/questions/36820196/visual-c-sharp-storing-and-reading-custom-options-to-and-from-a-custom-xml-in-ap

    public static class SettingsProvider
    {
        private static string settingsFileName;

        private static XDocument settings;

        static SettingsProvider()
        {
            try
            {
                settings = XDocument.Load(settingsFileName);
            }
            catch
            {
                settings = XDocument.Parse("<Settings/>");
            }
        }

        public static string GetValue(string section, string key, string defaultValue)
        {
            XElement settingElement = GetSettingElement(section, key);

            return settingElement == null ? defaultValue : settingElement.Value;
        }

        
        public static void SetValue(string section, string key, string value)
        {
            XElement settingElement = GetSettingElement(section, key, true);

            settingElement.Value = value;
            settings.Save(settingsFileName);
        }
        public static void setfilename(string value)
        {

            settingsFileName = value;
            try
            {
                settings = XDocument.Load(settingsFileName);
            }
            catch
            {
                settings = XDocument.Parse("<Settings/>");
            }
        }
        private static XElement GetSettingElement(string section, string key, bool createIfNotExist = false)
        {
            XElement sectionElement =
                settings
                    .Root
                    .Elements(section)
                    .FirstOrDefault();

            if (sectionElement == null)
            {
                if (createIfNotExist)
                {
                    sectionElement = new XElement(section);
                    settings.Root.Add(sectionElement);
                }
                else
                {
                    return null;
                }
            }

            XElement settingElement =
                sectionElement
                    .Elements(key)
                    .FirstOrDefault();

            if (settingElement == null)
            {
                if (createIfNotExist)
                {
                    settingElement = new XElement(key);
                    sectionElement.Add(settingElement);
                }
            }

            return settingElement;
        }

        public static void RemoveSetting(string section, string key)
        {
            XElement settingElement = GetSettingElement(section, key);

            if (settingElement == null)
            {
                return;
            }

            XElement sectionElement = settingElement.Parent;

            settingElement.Remove();

            if (sectionElement.IsEmpty)
            {
                sectionElement.Remove();
            }

            settings.Save(settingsFileName);
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
                        //cause program to exit silent, disabled to see what happens
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
