namespace Raw_File_Uploader
{
    partial class file_lock
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
            this.button1 = new System.Windows.Forms.Button();
            this.filepath = new System.Windows.Forms.TextBox();
            this.check_freq = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.view_result = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(56, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(304, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Select a File or type in file path";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // filepath
            // 
            this.filepath.Location = new System.Drawing.Point(56, 69);
            this.filepath.Name = "filepath";
            this.filepath.Size = new System.Drawing.Size(304, 20);
            this.filepath.TabIndex = 1;
            // 
            // check_freq
            // 
            this.check_freq.Location = new System.Drawing.Point(247, 101);
            this.check_freq.Name = "check_freq";
            this.check_freq.Size = new System.Drawing.Size(93, 20);
            this.check_freq.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wait bettween checks (secs)";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(75, 152);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "start";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(168, 152);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // view_result
            // 
            this.view_result.Location = new System.Drawing.Point(265, 152);
            this.view_result.Name = "view_result";
            this.view_result.Size = new System.Drawing.Size(75, 23);
            this.view_result.TabIndex = 3;
            this.view_result.Text = "view result";
            this.view_result.UseVisualStyleBackColor = true;
            this.view_result.Click += new System.EventHandler(this.view_result_Click);
            // 
            // file_lock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 236);
            this.Controls.Add(this.view_result);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.check_freq);
            this.Controls.Add(this.filepath);
            this.Controls.Add(this.button1);
            this.Name = "file_lock";
            this.Text = "File lock checker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox filepath;
        private System.Windows.Forms.TextBox check_freq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button view_result;
    }
}