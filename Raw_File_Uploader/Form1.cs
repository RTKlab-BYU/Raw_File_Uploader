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
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Runtime.InteropServices.ComTypes;

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
            txtserver.Text = "http://127.0.0.1/files/api/"; 
            minisize.Text = "100";
            upload_delay.Text = "10";
            alert_threshold.Text = "30";
            frequency_threshold.Text = "8";
            upload_delay.Text = "10";
            bypasskword.Text = "ignore";
            max_size.Text = "5000";
            filetype_combo.SelectedIndex = 0;
            var lastupload = "";
            DateTime lastuploadtime = DateTime.Now;
            log.Debug("Uploader started");      //FIXME check this gramar
            var context = SynchronizationContext.Current; // for cross thread update to UI
            version_number.Text = GetPublishedVersion();

            watcher.Changed += (s, e) =>

            {
                MessageBox.Show(s.ToString());
                FileInfo file_obj = new FileInfo(e.FullPath);
                if (upload_delay.Text != null)
                    Thread.Sleep(Int32.Parse(upload_delay.Text) * 1000);
                lastchangetime = DateTime.Now;

                if (folder_uploading.Checked) //for folder based system
                {
                    String folder_name = e.Name.Split('\\').ToList()[0];
                    if (monitor_on && !IsLocatedbyAcqprogam(file_obj))
                    {
                        try
                        {
                                if (lastupload != folder_name | (DateTime.Now - lastuploadtime > new TimeSpan(0, 10, 0))) // if same file name is triggerred in less than 10 min, to prevent double uploading

                                {
                                    context.Post(val => output.AppendText(Environment.NewLine + $"File /{folder_name}/  will be uploaded"), s);
                                    log.Debug(Environment.NewLine + $"File /{folder_name}/  will be uploaded");
                                    System.Threading.Thread.Sleep(5000);
                                    context.Post(val => uploadfolder(foldertxt.Text + '\\' + folder_name), s);
                                    lastupload = folder_name;
                                    lastuploadtime = DateTime.Now;
                                }
 
                        }
                        catch (Exception e1)
                        {
                            context.Post(val => output.AppendText(Environment.NewLine + "error detected" + e1), s);
                            log.Error(e1);
                        }
                    }
                }
                else //file based system
                {
                    if (monitor_on && !IsLocatedbyAcqprogam(file_obj))
                    {
                        try
                        {
                            if (!file_obj.Name.Contains(bypasskword.Text))
                            {
                                if (file_obj.Length > Int32.Parse(minisize.Text) * 1000000 && file_obj.Length < long.Parse(max_size.Text) * 1000000)
                                {
                                    if (lastupload != e.FullPath | (DateTime.Now - lastuploadtime > new TimeSpan(0, 10, 0))) // if same file name is triggerred in less than 10 min, to prevent double uploading
                                    {
                                        context.Post(val => output.AppendText(Environment.NewLine + $"File /{e.Name}/ with final size {file_obj.Length / 1000000} MB will be uploaded"), s);
                                        context.Post(val => filepath.Text = e.FullPath, s);
                                        log.Debug($"File /{e.Name}/ with final size {file_obj.Length / 1000000} MB will be uploaded");
                                        if (upload_delay.Text != null)
                                            Thread.Sleep(Int32.Parse(upload_delay.Text) * 1000);
                                        context.Post(val => uploadfile(filepath.Text), s);
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
                }
            };
        }


        private string GetPublishedVersion()
        {
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            return "Not network deployed";
        }

        private Boolean uploadfolder(string folderlocation) //upload invidual folder (for folder based data)

        {
            //zip file will be placed inside temp/, check if a temp patch exits, create one if not.
            string temppath = Directory.GetParent(folderlocation).FullName + @"\temp\";
            bool exists = Directory.Exists(temppath);
            if (!exists)
                Directory.CreateDirectory(temppath);
            var zip_filename =  new DirectoryInfo(folderlocation).Name;
            string zipPath = temppath + zip_filename+ ".zip";
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipFile.CreateFromDirectory(folderlocation, zipPath);
            FileInfo zip_file_obj = new FileInfo(zipPath);

            if (zip_file_obj.Length > Int32.Parse(minisize.Text) * 1000000 && zip_file_obj.Length < long.Parse(max_size.Text) * 1000000)
            {
                if (!zip_file_obj.Name.Contains(bypasskword.Text))
                {

                    uploadfile(zipPath);
                    return true;
                }

                else
                {
                    output.AppendText(Environment.NewLine + DateTime.Now + " folder name contain bypass keyword, will NOT be uploaded.");

                    log.Debug(" folder name contain bypass keyword, will NOT be uploaded.");


                }
            }
            else
            {
                log.Debug(" File size is not within setting range , will NOT be uploaded.");
                output.AppendText(Environment.NewLine + DateTime.Now + "File size is not within setting range , will NOT be uploaded.");
            }

            return false;


        }

        private Boolean uploadmultiplefiles(string filelocations) //upload multiple files
        {
            List<string> filelist = filelocations.Split(',').ToList();
            foreach (String file in filelist)
                if (file != "") 
                { 
                    if (!File.Exists(file))
                    {
                        MessageBox.Show("invalid file");
                    }
                    else
                    {
                        uploadfile(file);
                    }
                }
            return true;

        }










        private Boolean uploadfile(string filelocation)
        {
            if (!check_connection(true))
            {
                output.AppendText($"Connection to server failed");
                return false;
            }
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
                bool exists = Directory.Exists(temppath);
                if (!exists)
                    Directory.CreateDirectory(temppath);
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
                    Thread.Sleep(30000);
                    try //2nd try
                    {
                        File.Copy(filelocation, newfilelocation, true);
                    }
                    catch
                    {
                        output.AppendText(Environment.NewLine + DateTime.Now + $" {newfilelocation} uploading failed second time");

                        return false;
                    }
                }

            }

            var options = new RestClientOptions(txtserver.Text + "SampleRecord/")
            {
                ThrowOnAnyError = true,
                MaxTimeout = 1000*60*60  //60 min
            };
            var client = new RestClient(options);
            client.Authenticator = new HttpBasicAuthenticator(txtusername.Text, txtpassword.Text);
            if (!File.Exists(newfilelocation))
            {
                MessageBox.Show("Please check file location");
                return false;
            }
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            //request.Parameters.clear();
            request.AddHeader("Content-Type", "multipart/form-data");
            string uploadfilename;
            uploadfilename = Path.GetFileNameWithoutExtension(newfilelocation);
            request.AddParameter("record_name", uploadfilename);
            //Get user id
            string[] parts = userslist.Text.Split('_');

            request.AddParameter("project_name", String.Join(",", txtprojectname.Text, enablebatch.Checked, txtbatchname.Text, parts[0]),false);
            // the project name field is used for multiple purpose, comma is used for separate them
            request.AddParameter("record_description", txtdescription.Text);
            request.AddParameter("file_vendor", filetype_combo.Text);
            add_info(request, uploadfilename);
            request.AddParameter("is_temp", TempData.Checked);
            request.AddFile("temp_rawfile", newfilelocation);
            request.Timeout = 2147483647;
            
            try { 
                var response = client.Execute(request);

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
                    output.AppendText(Environment.NewLine + DateTime.Now + $" {filelocation} upload might failed, please check the server if file is uploaded.");
                    output.Select(output.TextLength, 0);
                    output.SelectionColor = Color.Black;
                    output.AppendText(Environment.NewLine);
                    log.Warn($" {filelocation}   upload might failed, please check the server if file is uploaded.");

                    return false;
                }



            }
            catch (Exception e)
            {
                output.Select(output.TextLength, 0);
                output.SelectionColor = Color.Orange;
                output.AppendText(Environment.NewLine + DateTime.Now + $" {filelocation} upload might failed, please check the server if file is uploaded. {e} or it took longer than expected to process the file");
                output.Select(output.TextLength, 0);
                output.SelectionColor = Color.Black;
                output.AppendText(Environment.NewLine);
                log.Warn($" {filelocation}  upload might failed, please check the server if file is uploaded. {e} or it took longer than expected to process the file");
                log.Error(e);

                return false;

            }
            finally
            {
                if (!nocopy.Checked)
                    File.Delete(newfilelocation);
                if (folder_uploading.Checked) //delete the zipfile if upload folder
                    File.Delete(filelocation);
            }
            

           
        }

        private RestRequest add_info(RestRequest request,String file_name)
        {
            TextBox[] fieldArray = {column_sn,spe_sn, sample_type, factor_1_name,
                  factor_1_value, factor_2_name, factor_2_value,
                  factor_3_name, factor_3_value, factor_4_name,
                  factor_4_value, factor_5_name, factor_5_value,
                  factor_6_name, factor_6_value, factor_7_name,
                  factor_7_value, factor_8_name, factor_8_value };
            foreach (TextBox s in fieldArray)
            {
                if ((is_extract.Checked) & (s.Text.StartsWith("[")) & (s.Text.EndsWith("]")))
                {
                    List<string> listStrLineElements = file_name.Split(delimiter.Text.ToCharArray()[0]).ToList();
                    String resultString = Regex.Match(s.Text, @"-?\d+").Value;
                    try
                    {                        
                        request.AddParameter(s.Name, listStrLineElements[Int32.Parse(resultString)]);
                    }
                    catch(Exception ex) 
                    {
                        log.Error(ex.Message);
                    }
                }
                else
                    request.AddParameter(s.Name, s.Text);
            }
            return request;
        }



        private void folderbutton_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                foldertxt.Text = folderDlg.SelectedPath;
                String folder_name = Path.GetFileName(foldertxt.Text);
            }
        }

        private void filebutton_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Mass Files",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Mass files (" + file_extension.Text+ ")| "+ file_extension.Text+"|All files (*.*)|*.*",
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
            if (validate_required()){

                if (!isDirectoryValid(foldertxt.Text))
                {
                    MessageBox.Show("invalid folder");
                    return;
                }
                if (monitor_on is true)
                    watchtriggle(false, Color.Red);
                else
                {
                    if (check_connection(true))
                        watchtriggle(true, Color.Green);
                }

            }

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
            if (folder_uploading.Checked)
            {
                watcher.IncludeSubdirectories = true;
                watcher.Filter = final_file.Text;
            }
            else
            {
                watcher.Filter = file_extension.Text;
            }
                watcher.EnableRaisingEvents = triggle;
            if (monitor_on is true)
            {
                output.AppendText(Environment.NewLine + DateTime.Now + $" start to monitor folder {foldertxt.Text} for {file_extension.Text} ");
                log.Debug($" start to monitor folder {foldertxt.Text} for {file_extension.Text} ");
            }
            else
            {                
                t.Abort();
                output.AppendText(Environment.NewLine + DateTime.Now + $" Stop to monitor folder {foldertxt.Text} ");
                log.Debug($" Stop to monitor folder {foldertxt.Text} ");
            }
        }
        private bool IsLocatedbyAcqprogam(FileInfo file)
        {
            var process_list = FileUtil.WhoIsLocking(file.FullName);
            foreach (Process lockprocess in process_list)
            {
                if (lockprocess.ProcessName == acq_prog.Text) { return true; }
            }
            return false;
        }

        private void single_upload_Click(object sender, EventArgs e)


        {
            if (validate_required())
            {
                if (folder_uploading.Checked)
                {
                    if (foldertxt.Text.Split('.').ToList().LastOrDefault() == file_extension.Text.Split('.').ToList().LastOrDefault())
                    {
                        uploadfolder(foldertxt.Text);
                    }
                    else
                    {
                        MessageBox.Show("selected folder doesn't match the uploading setting: " + file_extension.Text + " Folder.");
                    }
                }
                else
                {
                    uploadmultiplefiles(filepath.Text);
                }
            }
        }

        private void OnMyComboBoxChanged(object sender, EventArgs e) //settings for different vendors
        {





            if (filetype_combo.SelectedIndex == 0) //Thermo raw file
            {
                file_extension.Text = "*.raw";
                file_extension.Enabled = false;
                acq_prog.Text = "HomePage";
                acq_prog.Enabled = false;
                folder_uploading.Checked = false;
                folder_uploading.Enabled = false;
                final_file.Text ="";
                final_file.Enabled = false;
            }
            else if (filetype_combo.SelectedIndex == 1) //Agilent MIDAC (IMS)
            {
                file_extension.Text = "*.d";
                acq_prog.Text = "AgtVoyAcgEng";
                folder_uploading.Checked = true;
                final_file.Text = "MSProfile.bin";

                final_file.Enabled = false;
                acq_prog.Enabled = false;
                file_extension.Enabled = false;
                folder_uploading.Enabled = false;
            }
            else if (filetype_combo.SelectedIndex == 2) //Bruker Tims LC finish
            {
                acq_prog.Text = "HyStarNT";
                file_extension.Text = "*.d";
                folder_uploading.Checked = true;
                final_file.Text = "chromatography-data.sqlite";
                upload_delay.Text = "10";
                acq_prog.Enabled = false;

                final_file.Enabled = false;
                file_extension.Enabled = false;
                folder_uploading.Enabled = false;

            }

            else if (filetype_combo.SelectedIndex == 3) //Bruker Tims MS finish
            {
                acq_prog.Text = "timsEngine";
                file_extension.Text = "*.d";
                folder_uploading.Checked = true;
                final_file.Text = "analysis.tdf_bin";
                upload_delay.Text = "10";

                final_file.Enabled = false;
                acq_prog.Enabled = false;
                file_extension.Enabled = false;
                folder_uploading.Enabled = false;



            }
            else if (filetype_combo.SelectedIndex == 4) //Others
            {
                file_extension.Enabled = true;
                acq_prog.Enabled = true;
                folder_uploading.Enabled = true;
                final_file.Enabled = true;
            }
        }

        private void folderupload_checkboxchanged(object sender, EventArgs e)
        {
            if (folder_uploading.Checked)
            {
                final_file.Enabled = true;
                filepath.Enabled = false;
            }
            else
            {
                final_file.Enabled = false;
                filepath.Enabled = true;
                final_file.Text = "";
            }
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
                else // backup option, may stop working anytime!!!
                {  
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

            if (validate_required())
            {

                if (!isDirectoryValid(foldertxt.Text))
                {
                    MessageBox.Show("invalid folder");
                    return;
                }
                string[] files = Directory.GetFiles(foldertxt.Text, file_extension.Text);
                if (folder_uploading.Checked) //upload multiple folders
                {
                    string[] sub_directories = Directory.GetDirectories(foldertxt.Text, file_extension.Text);
                    DialogResult dialogResult2 = MessageBox.Show($"There are {sub_directories.Count()} {file_extension.Text} files, You are sure to upload them all?", "Confirm", MessageBoxButtons.YesNo);
                    if (dialogResult2 == DialogResult.Yes)
                    {
                        foreach (string upload_folder in sub_directories)
                        {
                            output.AppendText(Environment.NewLine + DateTime.Now + " Uploading" + upload_folder);
                            uploadfolder(upload_folder);
                        }
                    }
                }
                else //upload multiple files
                {
                    DialogResult dialogResult = MessageBox.Show($"There are {files.Count()} {file_extension.Text} files, You are sure to upload them all?", "Confirm", MessageBoxButtons.YesNo);
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
            var request = new RestRequest();
            var client = new RestClient(txtserver.Text + "auth/");
            client.Authenticator = new HttpBasicAuthenticator(txtusername.Text, txtpassword.Text);
            request.Method = Method.Post;
            var response = client.Execute(request);

            if (response.Content is "" || response.Content is null)
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
            Process.Start("https://github.com/xiaofengxie128/Raw_File_Uploader");
        }

        private void log_view_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", @"C:\ProgramData\RawfileUploader\RawfileUploader.log");
        }

        private Boolean validate_required()
        {
            if (string.IsNullOrWhiteSpace(txtprojectname.Text))
            {
                MessageBox.Show("Project name can't be empty");
                return false;
            }

            if ((txtprojectname.Text.Length + txtbatchname.Text.Length) > 40)
            {
                MessageBox.Show("The combined length of project name and batch name can't be longer than 40, please reduce their length and try again.");
                return false;

            }
        
            return true;


        }




        private void whoislockmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            file_lock Filelockform = new file_lock();
            Filelockform.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient(txtserver.Text);
                client.Authenticator = new HttpBasicAuthenticator(txtusername.Text, txtpassword.Text);
                var request = new RestRequest("/Users/", Method.Get);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Accept", "application/json");
                var response = client.Execute(request);


                UserResponseDictionary all_users_response = JsonConvert.DeserializeObject<UserResponseDictionary>(response.Content);
                userslist.Items.Clear();

                foreach (AppUsers eachuser in all_users_response.AppUsers)
                {
                    userslist.Items.Add($"{eachuser.pk}_{eachuser.username}");
                }
                if (userslist.Items.Count != 0)
                    userslist.SelectedIndex = 0;
            }
            catch (Exception err)
            {
                MessageBox.Show("Can not obtain List from server, check server condition/address/username/password in the server tab or put in manully. " + err.Message);

            }
        }
    }


    //Helper classes
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


    public static  class FileUtil
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



    // user_response class


    public class UserResponseDictionary
    {
        [JsonProperty("count")]
        public int count { get; set; }
        [JsonProperty("next")]

        public string next { get; set; }
        [JsonProperty("previous")]

        public string previous { get; set; }
        [JsonProperty("results")]
        public List<AppUsers> AppUsers { get; set; }
    }

    public class AppUsers
    {
        [JsonProperty("id")]
        public int pk { get; set; }
        [JsonProperty("username")]
        public string username { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
    }





}
