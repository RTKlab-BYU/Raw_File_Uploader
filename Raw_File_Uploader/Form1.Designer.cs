
namespace Raw_File_Uploader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Uploader = new System.Windows.Forms.Label();
            this.foldertxt = new System.Windows.Forms.TextBox();
            this.filepath = new System.Windows.Forms.TextBox();
            this.txtserver = new System.Windows.Forms.TextBox();
            this.txtusername = new System.Windows.Forms.TextBox();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.txtsamplename = new System.Windows.Forms.TextBox();
            this.txtprojectname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.filetype = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.minisize = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.folderbutton = new System.Windows.Forms.Button();
            this.filebutton = new System.Windows.Forms.Button();
            this.monitor = new System.Windows.Forms.Button();
            this.txtdescription = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.output = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.single_upload = new System.Windows.Forms.Button();
            this.folder_uploader = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.nocopy = new System.Windows.Forms.CheckBox();
            this.TempData = new System.Windows.Forms.CheckBox();
            this.version_number = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.recipient_email = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.alert_threshold = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.frequency_threshold = new System.Windows.Forms.TextBox();
            this.lastchangtimefield = new System.Windows.Forms.TextBox();
            this.Explaination = new System.Windows.Forms.ToolTip(this.components);
            this.qctool = new System.Windows.Forms.ComboBox();
            this.mbrlist = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Uploader
            // 
            this.Uploader.AutoSize = true;
            this.Uploader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Uploader.Location = new System.Drawing.Point(12, 29);
            this.Uploader.Name = "Uploader";
            this.Uploader.Size = new System.Drawing.Size(115, 17);
            this.Uploader.TabIndex = 0;
            this.Uploader.Text = "Folder to Monitor";
            // 
            // foldertxt
            // 
            this.foldertxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foldertxt.Location = new System.Drawing.Point(133, 26);
            this.foldertxt.Name = "foldertxt";
            this.foldertxt.Size = new System.Drawing.Size(526, 23);
            this.foldertxt.TabIndex = 1;
            this.Explaination.SetToolTip(this.foldertxt, "The instrument data acquisition folder");
            // 
            // filepath
            // 
            this.filepath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filepath.Location = new System.Drawing.Point(133, 71);
            this.filepath.Name = "filepath";
            this.filepath.Size = new System.Drawing.Size(526, 23);
            this.filepath.TabIndex = 3;
            this.Explaination.SetToolTip(this.filepath, "Folder to be uploaded");
            // 
            // txtserver
            // 
            this.txtserver.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtserver.Location = new System.Drawing.Point(133, 112);
            this.txtserver.Name = "txtserver";
            this.txtserver.Size = new System.Drawing.Size(526, 23);
            this.txtserver.TabIndex = 5;
            this.Explaination.SetToolTip(this.txtserver, "Data server address, e.g., http://192.168.102.188/files/api/");
            // 
            // txtusername
            // 
            this.txtusername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtusername.Location = new System.Drawing.Point(133, 157);
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(150, 23);
            this.txtusername.TabIndex = 6;
            this.Explaination.SetToolTip(this.txtusername, "User name of the data system");
            // 
            // txtpassword
            // 
            this.txtpassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtpassword.Location = new System.Drawing.Point(387, 157);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.Size = new System.Drawing.Size(216, 23);
            this.txtpassword.TabIndex = 7;
            this.Explaination.SetToolTip(this.txtpassword, "password of that user");
            this.txtpassword.UseSystemPasswordChar = true;
            // 
            // txtsamplename
            // 
            this.txtsamplename.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtsamplename.Location = new System.Drawing.Point(133, 200);
            this.txtsamplename.Name = "txtsamplename";
            this.txtsamplename.Size = new System.Drawing.Size(150, 23);
            this.txtsamplename.TabIndex = 9;
            // 
            // txtprojectname
            // 
            this.txtprojectname.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtprojectname.Location = new System.Drawing.Point(387, 197);
            this.txtprojectname.Name = "txtprojectname";
            this.txtprojectname.Size = new System.Drawing.Size(213, 23);
            this.txtprojectname.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "File to upload";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Server address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "User name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(289, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "Sample Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(289, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Project Name";
            // 
            // filetype
            // 
            this.filetype.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filetype.Location = new System.Drawing.Point(744, 160);
            this.filetype.Name = "filetype";
            this.filetype.Size = new System.Drawing.Size(118, 23);
            this.filetype.TabIndex = 8;
            this.Explaination.SetToolTip(this.filetype, "File type to monitor");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(640, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 17);
            this.label8.TabIndex = 8;
            this.label8.Text = "File Type";
            // 
            // minisize
            // 
            this.minisize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minisize.Location = new System.Drawing.Point(744, 203);
            this.minisize.Name = "minisize";
            this.minisize.Size = new System.Drawing.Size(118, 23);
            this.minisize.TabIndex = 11;
            this.Explaination.SetToolTip(this.minisize, "minimal size of file to be uploaded (smaller will be ignored");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(606, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 17);
            this.label9.TabIndex = 8;
            this.label9.Text = "Minimal Size (MB)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-2, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(153, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = " (leave blank will use file name)";
            // 
            // folderbutton
            // 
            this.folderbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.folderbutton.Location = new System.Drawing.Point(678, 16);
            this.folderbutton.Name = "folderbutton";
            this.folderbutton.Size = new System.Drawing.Size(108, 33);
            this.folderbutton.TabIndex = 2;
            this.folderbutton.Text = "Folder";
            this.folderbutton.UseVisualStyleBackColor = true;
            this.folderbutton.Click += new System.EventHandler(this.folderbutton_Click);
            // 
            // filebutton
            // 
            this.filebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filebutton.Location = new System.Drawing.Point(683, 64);
            this.filebutton.Name = "filebutton";
            this.filebutton.Size = new System.Drawing.Size(108, 37);
            this.filebutton.TabIndex = 4;
            this.filebutton.Text = "File";
            this.filebutton.UseVisualStyleBackColor = true;
            this.filebutton.Click += new System.EventHandler(this.filebutton_Click);
            // 
            // monitor
            // 
            this.monitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monitor.Location = new System.Drawing.Point(825, 16);
            this.monitor.Name = "monitor";
            this.monitor.Size = new System.Drawing.Size(176, 78);
            this.monitor.TabIndex = 19;
            this.monitor.Text = "Monitor";
            this.Explaination.SetToolTip(this.monitor, "start/stop the monitor");
            this.monitor.UseVisualStyleBackColor = true;
            this.monitor.Click += new System.EventHandler(this.monitor_Click);
            // 
            // txtdescription
            // 
            this.txtdescription.Location = new System.Drawing.Point(108, 310);
            this.txtdescription.Name = "txtdescription";
            this.txtdescription.Size = new System.Drawing.Size(478, 91);
            this.txtdescription.TabIndex = 15;
            this.txtdescription.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(12, 310);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "Description";
            // 
            // output
            // 
            this.output.BackColor = System.Drawing.SystemColors.Info;
            this.output.Location = new System.Drawing.Point(12, 416);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.Size = new System.Drawing.Size(933, 228);
            this.output.TabIndex = 14;
            this.output.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(12, 396);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 17);
            this.label11.TabIndex = 8;
            this.label11.Text = "Output";
            // 
            // single_upload
            // 
            this.single_upload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.single_upload.Location = new System.Drawing.Point(24, 650);
            this.single_upload.Name = "single_upload";
            this.single_upload.Size = new System.Drawing.Size(161, 23);
            this.single_upload.TabIndex = 20;
            this.single_upload.Text = "Manual upload single file";
            this.single_upload.UseVisualStyleBackColor = true;
            this.single_upload.Click += new System.EventHandler(this.single_upload_Click);
            // 
            // folder_uploader
            // 
            this.folder_uploader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.folder_uploader.Location = new System.Drawing.Point(365, 650);
            this.folder_uploader.Name = "folder_uploader";
            this.folder_uploader.Size = new System.Drawing.Size(161, 23);
            this.folder_uploader.TabIndex = 21;
            this.folder_uploader.Text = "Upload entire folder";
            this.folder_uploader.UseVisualStyleBackColor = true;
            this.folder_uploader.Click += new System.EventHandler(this.folder_uploader_Click);
            // 
            // exit
            // 
            this.exit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit.Location = new System.Drawing.Point(678, 650);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(161, 23);
            this.exit.TabIndex = 22;
            this.exit.Text = "Exit";
            this.exit.UseVisualStyleBackColor = true;
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // nocopy
            // 
            this.nocopy.AutoSize = true;
            this.nocopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nocopy.Location = new System.Drawing.Point(845, 114);
            this.nocopy.Name = "nocopy";
            this.nocopy.Size = new System.Drawing.Size(120, 21);
            this.nocopy.TabIndex = 16;
            this.nocopy.Text = "No Copy mode";
            this.Explaination.SetToolTip(this.nocopy, "Enable it to view file with Xcalibur Qual Browser during Acquisition");
            this.nocopy.UseVisualStyleBackColor = true;
            // 
            // TempData
            // 
            this.TempData.AutoSize = true;
            this.TempData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TempData.Location = new System.Drawing.Point(591, 337);
            this.TempData.Name = "TempData";
            this.TempData.Size = new System.Drawing.Size(134, 21);
            this.TempData.TabIndex = 18;
            this.TempData.Text = "Temporary Data ";
            this.Explaination.SetToolTip(this.TempData, "Temporary data, like wash or clean, will delete after 3 month (can be changed in " +
        "the detail page)");
            this.TempData.UseVisualStyleBackColor = true;
            // 
            // version_number
            // 
            this.version_number.AutoSize = true;
            this.version_number.Location = new System.Drawing.Point(880, 684);
            this.version_number.Name = "version_number";
            this.version_number.Size = new System.Drawing.Size(0, 13);
            this.version_number.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(792, 684);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Current Version:";
            // 
            // recipient_email
            // 
            this.recipient_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recipient_email.Location = new System.Drawing.Point(140, 259);
            this.recipient_email.Name = "recipient_email";
            this.recipient_email.Size = new System.Drawing.Size(150, 23);
            this.recipient_email.TabIndex = 12;
            this.Explaination.SetToolTip(this.recipient_email, "Will send email to this address for the alert, leave it blank will disable it");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(-2, 261);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(138, 17);
            this.label13.TabIndex = 8;
            this.label13.Text = "Alert Recipient Email";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(301, 259);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(136, 17);
            this.label14.TabIndex = 8;
            this.label14.Text = "Alert threshold (min)";
            // 
            // alert_threshold
            // 
            this.alert_threshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alert_threshold.Location = new System.Drawing.Point(443, 259);
            this.alert_threshold.Name = "alert_threshold";
            this.alert_threshold.Size = new System.Drawing.Size(150, 23);
            this.alert_threshold.TabIndex = 13;
            this.Explaination.SetToolTip(this.alert_threshold, "If no data file have been created since last run finished, will not check while i" +
        "t\'s running. 60 min analysis should set 60x2");
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(599, 262);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(125, 17);
            this.label15.TabIndex = 8;
            this.label15.Text = "Frequency (hours)";
            // 
            // frequency_threshold
            // 
            this.frequency_threshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frequency_threshold.Location = new System.Drawing.Point(730, 259);
            this.frequency_threshold.Name = "frequency_threshold";
            this.frequency_threshold.Size = new System.Drawing.Size(150, 23);
            this.frequency_threshold.TabIndex = 14;
            this.Explaination.SetToolTip(this.frequency_threshold, "How often/minimal interal for sending alert");
            // 
            // lastchangtimefield
            // 
            this.lastchangtimefield.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastchangtimefield.Location = new System.Drawing.Point(292, 288);
            this.lastchangtimefield.Name = "lastchangtimefield";
            this.lastchangtimefield.Size = new System.Drawing.Size(152, 23);
            this.lastchangtimefield.TabIndex = 23;
            // 
            // Explaination
            // 
            this.Explaination.AutoPopDelay = 15000;
            this.Explaination.InitialDelay = 500;
            this.Explaination.ReshowDelay = 100;
            this.Explaination.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Explaination.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // qctool
            // 
            this.qctool.FormattingEnabled = true;
            this.qctool.ImeMode = System.Windows.Forms.ImeMode.On;
            this.qctool.Items.AddRange(new object[] {
            "None",
            "Msfragger",
            "Maxquant",
            "Protein Discovery",
            "MBR_maxquant"});
            this.qctool.Location = new System.Drawing.Point(658, 310);
            this.qctool.Name = "qctool";
            this.qctool.Size = new System.Drawing.Size(121, 21);
            this.qctool.TabIndex = 16;
            this.Explaination.SetToolTip(this.qctool, "Tools used to do automatic QC analysis");
            // 
            // mbrlist
            // 
            this.mbrlist.Location = new System.Drawing.Point(845, 310);
            this.mbrlist.Name = "mbrlist";
            this.mbrlist.Size = new System.Drawing.Size(100, 20);
            this.mbrlist.TabIndex = 24;
            this.Explaination.SetToolTip(this.mbrlist, "list of raw files to be used for MBR, e.g., 908, 909,911");
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(592, 310);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(60, 17);
            this.label17.TabIndex = 8;
            this.label17.Text = "QC Tool";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(782, 310);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 17);
            this.label16.TabIndex = 8;
            this.label16.Text = "MBR list";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(899, 199);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 706);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mbrlist);
            this.Controls.Add(this.qctool);
            this.Controls.Add(this.lastchangtimefield);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.version_number);
            this.Controls.Add(this.TempData);
            this.Controls.Add(this.nocopy);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.folder_uploader);
            this.Controls.Add(this.single_upload);
            this.Controls.Add(this.output);
            this.Controls.Add(this.txtdescription);
            this.Controls.Add(this.monitor);
            this.Controls.Add(this.filebutton);
            this.Controls.Add(this.folderbutton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtprojectname);
            this.Controls.Add(this.minisize);
            this.Controls.Add(this.frequency_threshold);
            this.Controls.Add(this.filetype);
            this.Controls.Add(this.alert_threshold);
            this.Controls.Add(this.recipient_email);
            this.Controls.Add(this.txtsamplename);
            this.Controls.Add(this.txtpassword);
            this.Controls.Add(this.txtusername);
            this.Controls.Add(this.txtserver);
            this.Controls.Add(this.filepath);
            this.Controls.Add(this.foldertxt);
            this.Controls.Add(this.Uploader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Raw File Uploader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Uploader;
        private System.Windows.Forms.TextBox foldertxt;
        private System.Windows.Forms.TextBox filepath;
        private System.Windows.Forms.TextBox txtserver;
        private System.Windows.Forms.TextBox txtusername;
        private System.Windows.Forms.TextBox txtpassword;
        private System.Windows.Forms.TextBox txtsamplename;
        private System.Windows.Forms.TextBox txtprojectname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox filetype;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox minisize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button folderbutton;
        private System.Windows.Forms.Button filebutton;
        private System.Windows.Forms.Button monitor;
        private System.Windows.Forms.RichTextBox txtdescription;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button single_upload;
        private System.Windows.Forms.Button folder_uploader;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.CheckBox nocopy;
        private System.Windows.Forms.CheckBox TempData;
        private System.Windows.Forms.Label version_number;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox recipient_email;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox alert_threshold;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox frequency_threshold;
        private System.Windows.Forms.TextBox lastchangtimefield;
        private System.Windows.Forms.ToolTip Explaination;
        private System.Windows.Forms.ComboBox qctool;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox mbrlist;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button button1;
    }
}

