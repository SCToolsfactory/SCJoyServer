namespace SCJoyServer
{
  partial class FrmStatus
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) ) {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStatus));
      this.btStartStop = new System.Windows.Forms.Button();
      this.txPort = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.cbxJoystick = new System.Windows.Forms.CheckedListBox();
      this.lblSignal = new System.Windows.Forms.Label();
      this.cbxTcp = new System.Windows.Forms.CheckBox();
      this.cbxUdp = new System.Windows.Forms.CheckBox();
      this.cbxReport = new System.Windows.Forms.CheckBox();
      this.pnlState = new System.Windows.Forms.Panel();
      this.btDebugWin = new System.Windows.Forms.Button();
      this.txLocIP = new System.Windows.Forms.TextBox();
      this.lblStatusTxt = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lbxClients = new System.Windows.Forms.ListBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.ICON = new System.Windows.Forms.NotifyIcon(this.components);
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.cbxKBon = new System.Windows.Forms.CheckBox();
      this.lblSCdx = new System.Windows.Forms.Label();
      this.lblVJoy = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.lblWCliStatusTxt = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.lblUpSignal = new System.Windows.Forms.Label();
      this.btUpDir = new System.Windows.Forms.Button();
      this.txUpDir = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.pnlUpState = new System.Windows.Forms.Panel();
      this.btUpStartStop = new System.Windows.Forms.Button();
      this.txRemIP = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.txRemPort = new System.Windows.Forms.TextBox();
      this.fswUploader = new System.IO.FileSystemWatcher();
      this.SFD = new System.Windows.Forms.SaveFileDialog();
      this.lblVersion = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fswUploader)).BeginInit();
      this.SuspendLayout();
      // 
      // btStartStop
      // 
      this.btStartStop.Location = new System.Drawing.Point(9, 128);
      this.btStartStop.Name = "btStartStop";
      this.btStartStop.Size = new System.Drawing.Size(55, 40);
      this.btStartStop.TabIndex = 0;
      this.btStartStop.Text = "Start Servers";
      this.btStartStop.UseVisualStyleBackColor = true;
      this.btStartStop.Click += new System.EventHandler(this.btStartStop_Click);
      // 
      // txPort
      // 
      this.txPort.Location = new System.Drawing.Point(78, 42);
      this.txPort.Name = "txPort";
      this.txPort.Size = new System.Drawing.Size(91, 22);
      this.txPort.TabIndex = 2;
      this.txPort.Text = "34123";
      this.txPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(48, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Local IP:";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.cbxJoystick);
      this.groupBox1.Controls.Add(this.lblSignal);
      this.groupBox1.Controls.Add(this.cbxTcp);
      this.groupBox1.Controls.Add(this.cbxUdp);
      this.groupBox1.Controls.Add(this.cbxReport);
      this.groupBox1.Controls.Add(this.pnlState);
      this.groupBox1.Controls.Add(this.btDebugWin);
      this.groupBox1.Controls.Add(this.txLocIP);
      this.groupBox1.Controls.Add(this.lblStatusTxt);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.txPort);
      this.groupBox1.Controls.Add(this.btStartStop);
      this.groupBox1.Location = new System.Drawing.Point(9, 104);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(271, 204);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Server";
      // 
      // cbxJoystick
      // 
      this.cbxJoystick.FormattingEnabled = true;
      this.cbxJoystick.Location = new System.Drawing.Point(175, 17);
      this.cbxJoystick.Name = "cbxJoystick";
      this.cbxJoystick.Size = new System.Drawing.Size(90, 72);
      this.cbxJoystick.TabIndex = 13;
      // 
      // lblSignal
      // 
      this.lblSignal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblSignal.Location = new System.Drawing.Point(175, 174);
      this.lblSignal.Name = "lblSignal";
      this.lblSignal.Size = new System.Drawing.Size(90, 23);
      this.lblSignal.TabIndex = 11;
      this.lblSignal.Text = "1235";
      this.lblSignal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cbxTcp
      // 
      this.cbxTcp.AutoSize = true;
      this.cbxTcp.Location = new System.Drawing.Point(70, 151);
      this.cbxTcp.Name = "cbxTcp";
      this.cbxTcp.Size = new System.Drawing.Size(43, 17);
      this.cbxTcp.TabIndex = 10;
      this.cbxTcp.Text = "TCP";
      this.cbxTcp.UseVisualStyleBackColor = true;
      // 
      // cbxUdp
      // 
      this.cbxUdp.AutoSize = true;
      this.cbxUdp.Checked = true;
      this.cbxUdp.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxUdp.Location = new System.Drawing.Point(70, 128);
      this.cbxUdp.Name = "cbxUdp";
      this.cbxUdp.Size = new System.Drawing.Size(48, 17);
      this.cbxUdp.TabIndex = 9;
      this.cbxUdp.Text = "UDP";
      this.cbxUdp.UseVisualStyleBackColor = true;
      // 
      // cbxReport
      // 
      this.cbxReport.AutoSize = true;
      this.cbxReport.Location = new System.Drawing.Point(70, 178);
      this.cbxReport.Name = "cbxReport";
      this.cbxReport.Size = new System.Drawing.Size(99, 17);
      this.cbxReport.TabIndex = 8;
      this.cbxReport.Text = "Report Clients";
      this.cbxReport.UseVisualStyleBackColor = true;
      this.cbxReport.CheckedChanged += new System.EventHandler(this.cbxReport_CheckedChanged);
      // 
      // pnlState
      // 
      this.pnlState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.pnlState.Location = new System.Drawing.Point(175, 95);
      this.pnlState.Name = "pnlState";
      this.pnlState.Size = new System.Drawing.Size(90, 26);
      this.pnlState.TabIndex = 7;
      // 
      // btDebugWin
      // 
      this.btDebugWin.Location = new System.Drawing.Point(9, 174);
      this.btDebugWin.Name = "btDebugWin";
      this.btDebugWin.Size = new System.Drawing.Size(55, 23);
      this.btDebugWin.TabIndex = 5;
      this.btDebugWin.Text = "dbg";
      this.btDebugWin.UseVisualStyleBackColor = true;
      this.btDebugWin.Click += new System.EventHandler(this.btDebugWin_Click);
      // 
      // txLocIP
      // 
      this.txLocIP.Location = new System.Drawing.Point(78, 17);
      this.txLocIP.Name = "txLocIP";
      this.txLocIP.Size = new System.Drawing.Size(91, 22);
      this.txLocIP.TabIndex = 1;
      this.txLocIP.Text = "192.168.1.1";
      this.txLocIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // lblStatusTxt
      // 
      this.lblStatusTxt.AutoSize = true;
      this.lblStatusTxt.Location = new System.Drawing.Point(75, 98);
      this.lblStatusTxt.Name = "lblStatusTxt";
      this.lblStatusTxt.Size = new System.Drawing.Size(26, 13);
      this.lblStatusTxt.TabIndex = 4;
      this.lblStatusTxt.Text = "idle";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 45);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(56, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "First Port:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 98);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(42, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Status:";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.lbxClients);
      this.groupBox2.Location = new System.Drawing.Point(3, 314);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(271, 135);
      this.groupBox2.TabIndex = 5;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Clients";
      // 
      // lbxClients
      // 
      this.lbxClients.FormattingEnabled = true;
      this.lbxClients.Location = new System.Drawing.Point(6, 19);
      this.lbxClients.Name = "lbxClients";
      this.lbxClients.ScrollAlwaysVisible = true;
      this.lbxClients.Size = new System.Drawing.Size(259, 108);
      this.lbxClients.TabIndex = 5;
      // 
      // timer1
      // 
      this.timer1.Interval = 500;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // IL
      // 
      this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
      this.IL.TransparentColor = System.Drawing.Color.Transparent;
      this.IL.Images.SetKeyName(0, "off");
      this.IL.Images.SetKeyName(1, "on");
      this.IL.Images.SetKeyName(2, "error");
      // 
      // ICON
      // 
      this.ICON.Icon = ((System.Drawing.Icon)(resources.GetObject("ICON.Icon")));
      this.ICON.Text = "SCJoyServer";
      this.ICON.Visible = true;
      this.ICON.DoubleClick += new System.EventHandler(this.ICON_DoubleClick);
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.cbxKBon);
      this.groupBox3.Controls.Add(this.lblSCdx);
      this.groupBox3.Controls.Add(this.lblVJoy);
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Location = new System.Drawing.Point(9, 25);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(271, 73);
      this.groupBox3.TabIndex = 6;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Device Support";
      // 
      // cbxKBon
      // 
      this.cbxKBon.AutoSize = true;
      this.cbxKBon.Enabled = false;
      this.cbxKBon.Location = new System.Drawing.Point(97, 46);
      this.cbxKBon.Name = "cbxKBon";
      this.cbxKBon.Size = new System.Drawing.Size(40, 17);
      this.cbxKBon.TabIndex = 7;
      this.cbxKBon.Text = "on";
      this.cbxKBon.UseVisualStyleBackColor = true;
      this.cbxKBon.CheckedChanged += new System.EventHandler(this.cbxKBon_CheckedChanged);
      // 
      // lblSCdx
      // 
      this.lblSCdx.AutoSize = true;
      this.lblSCdx.Location = new System.Drawing.Point(146, 47);
      this.lblSCdx.Name = "lblSCdx";
      this.lblSCdx.Size = new System.Drawing.Size(16, 13);
      this.lblSCdx.TabIndex = 6;
      this.lblSCdx.Text = "...";
      // 
      // lblVJoy
      // 
      this.lblVJoy.AutoSize = true;
      this.lblVJoy.Location = new System.Drawing.Point(145, 25);
      this.lblVJoy.Name = "lblVJoy";
      this.lblVJoy.Size = new System.Drawing.Size(16, 13);
      this.lblVJoy.TabIndex = 5;
      this.lblVJoy.Text = "...";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 47);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(93, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "SCdx - Keyboard:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 25);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(80, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "vJoy - Joystick:";
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.lblWCliStatusTxt);
      this.groupBox4.Controls.Add(this.label9);
      this.groupBox4.Controls.Add(this.lblUpSignal);
      this.groupBox4.Controls.Add(this.btUpDir);
      this.groupBox4.Controls.Add(this.txUpDir);
      this.groupBox4.Controls.Add(this.label8);
      this.groupBox4.Controls.Add(this.pnlUpState);
      this.groupBox4.Controls.Add(this.btUpStartStop);
      this.groupBox4.Controls.Add(this.txRemIP);
      this.groupBox4.Controls.Add(this.label6);
      this.groupBox4.Controls.Add(this.label7);
      this.groupBox4.Controls.Add(this.txRemPort);
      this.groupBox4.Location = new System.Drawing.Point(9, 455);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(271, 161);
      this.groupBox4.TabIndex = 7;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Uploader";
      // 
      // lblWCliStatusTxt
      // 
      this.lblWCliStatusTxt.AutoSize = true;
      this.lblWCliStatusTxt.Location = new System.Drawing.Point(75, 97);
      this.lblWCliStatusTxt.Name = "lblWCliStatusTxt";
      this.lblWCliStatusTxt.Size = new System.Drawing.Size(26, 13);
      this.lblWCliStatusTxt.TabIndex = 22;
      this.lblWCliStatusTxt.Text = "idle";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(6, 97);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(42, 13);
      this.label9.TabIndex = 21;
      this.label9.Text = "Status:";
      // 
      // lblUpSignal
      // 
      this.lblUpSignal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblUpSignal.Location = new System.Drawing.Point(175, 132);
      this.lblUpSignal.Name = "lblUpSignal";
      this.lblUpSignal.Size = new System.Drawing.Size(90, 23);
      this.lblUpSignal.TabIndex = 20;
      this.lblUpSignal.Text = "1235";
      this.lblUpSignal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // btUpDir
      // 
      this.btUpDir.Location = new System.Drawing.Point(237, 64);
      this.btUpDir.Name = "btUpDir";
      this.btUpDir.Size = new System.Drawing.Size(28, 20);
      this.btUpDir.TabIndex = 19;
      this.btUpDir.Text = "...";
      this.btUpDir.UseVisualStyleBackColor = true;
      this.btUpDir.Click += new System.EventHandler(this.btUpDir_Click);
      // 
      // txUpDir
      // 
      this.txUpDir.Location = new System.Drawing.Point(78, 64);
      this.txUpDir.Name = "txUpDir";
      this.txUpDir.Size = new System.Drawing.Size(161, 22);
      this.txUpDir.TabIndex = 18;
      this.txUpDir.Text = "D:\\something";
      this.txUpDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(6, 67);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(66, 13);
      this.label8.TabIndex = 17;
      this.label8.Text = "Upload Dir:";
      // 
      // pnlUpState
      // 
      this.pnlUpState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.pnlUpState.Location = new System.Drawing.Point(175, 90);
      this.pnlUpState.Name = "pnlUpState";
      this.pnlUpState.Size = new System.Drawing.Size(90, 26);
      this.pnlUpState.TabIndex = 16;
      // 
      // btUpStartStop
      // 
      this.btUpStartStop.Location = new System.Drawing.Point(6, 116);
      this.btUpStartStop.Name = "btUpStartStop";
      this.btUpStartStop.Size = new System.Drawing.Size(55, 40);
      this.btUpStartStop.TabIndex = 15;
      this.btUpStartStop.Text = "Start Client";
      this.btUpStartStop.UseVisualStyleBackColor = true;
      this.btUpStartStop.Click += new System.EventHandler(this.btUpStartStop_Click);
      // 
      // txRemIP
      // 
      this.txRemIP.Location = new System.Drawing.Point(78, 13);
      this.txRemIP.Name = "txRemIP";
      this.txRemIP.Size = new System.Drawing.Size(161, 22);
      this.txRemIP.TabIndex = 11;
      this.txRemIP.Text = "192.168.1.1";
      this.txRemIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 41);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(31, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Port:";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 16);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(53, 13);
      this.label7.TabIndex = 13;
      this.label7.Text = "Server IP:";
      // 
      // txRemPort
      // 
      this.txRemPort.Location = new System.Drawing.Point(78, 38);
      this.txRemPort.Name = "txRemPort";
      this.txRemPort.Size = new System.Drawing.Size(161, 22);
      this.txRemPort.TabIndex = 12;
      this.txRemPort.Text = "9042";
      this.txRemPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // fswUploader
      // 
      this.fswUploader.EnableRaisingEvents = true;
      this.fswUploader.Filter = "*.json";
      this.fswUploader.SynchronizingObject = this;
      this.fswUploader.Changed += new System.IO.FileSystemEventHandler(this.fswUploader_Changed);
      this.fswUploader.Created += new System.IO.FileSystemEventHandler(this.fswUploader_Changed);
      // 
      // SFD
      // 
      this.SFD.DefaultExt = "json";
      this.SFD.Filter = "Json Files|*.json|All files|*.*";
      this.SFD.OverwritePrompt = false;
      this.SFD.SupportMultiDottedExtensions = true;
      // 
      // lblVersion
      // 
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new System.Drawing.Point(6, 9);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(44, 13);
      this.lblVersion.TabIndex = 8;
      this.lblVersion.Text = "label10";
      // 
      // FrmStatus
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(289, 624);
      this.Controls.Add(this.lblVersion);
      this.Controls.Add(this.groupBox4);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "FrmStatus";
      this.Text = "SC VJoy Server";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.fswUploader)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btStartStop;
    private System.Windows.Forms.TextBox txPort;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblStatusTxt;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ListBox lbxClients;
    private System.Windows.Forms.TextBox txLocIP;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Button btDebugWin;
    private System.Windows.Forms.ImageList IL;
    private System.Windows.Forms.Panel pnlState;
    private System.Windows.Forms.CheckBox cbxReport;
    private System.Windows.Forms.NotifyIcon ICON;
    private System.Windows.Forms.CheckBox cbxTcp;
    private System.Windows.Forms.CheckBox cbxUdp;
    private System.Windows.Forms.Label lblSignal;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label lblSCdx;
    private System.Windows.Forms.Label lblVJoy;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.CheckBox cbxKBon;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.TextBox txRemIP;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txRemPort;
    private System.Windows.Forms.Panel pnlUpState;
    private System.Windows.Forms.Button btUpStartStop;
    private System.IO.FileSystemWatcher fswUploader;
    private System.Windows.Forms.Button btUpDir;
    private System.Windows.Forms.TextBox txUpDir;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.SaveFileDialog SFD;
    private System.Windows.Forms.Label lblUpSignal;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label lblVersion;
    private System.Windows.Forms.Label lblWCliStatusTxt;
    private System.Windows.Forms.CheckedListBox cbxJoystick;
  }
}

