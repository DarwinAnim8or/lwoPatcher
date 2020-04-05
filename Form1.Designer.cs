namespace lwoPatcher
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.bStartPlay = new System.Windows.Forms.Button();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(758, 530);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("http://cache.lbbstudios.net/lwopatcher/index.html", System.UriKind.Absolute);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 584);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.fileNameLabel.Location = new System.Drawing.Point(12, 555);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(135, 17);
            this.fileNameLabel.TabIndex = 2;
            this.fileNameLabel.Text = "Click Start to Patch..";
            // 
            // bStartPlay
            // 
            this.bStartPlay.Location = new System.Drawing.Point(522, 610);
            this.bStartPlay.Name = "bStartPlay";
            this.bStartPlay.Size = new System.Drawing.Size(248, 31);
            this.bStartPlay.TabIndex = 3;
            this.bStartPlay.Text = "Start";
            this.bStartPlay.UseVisualStyleBackColor = true;
            this.bStartPlay.Click += new System.EventHandler(this.bStartPlay_Click);
            // 
            // cbServer
            // 
            this.cbServer.AllowDrop = false;
            this.cbServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServer.FormattingEnabled = false;
            this.cbServer.Items.AddRange(new object[] {
            "LBB Minimal",
            "LBB Trunk"});
            this.cbServer.Location = new System.Drawing.Point(522, 555);
            this.cbServer.MaxDropDownItems = 12;
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(248, 24);
            this.cbServer.TabIndex = 0;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(782, 653);
            this.Controls.Add(this.cbServer);
            this.Controls.Add(this.bStartPlay);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.webBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LEGO Universe - lwoPatcher (v0.1)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.Button bStartPlay;
        private System.Windows.Forms.ComboBox cbServer;
    }
}

