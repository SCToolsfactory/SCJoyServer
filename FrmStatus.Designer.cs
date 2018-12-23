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
      this.cbxJoystick = new System.Windows.Forms.ComboBox();
      this.lblSignal = new System.Windows.Forms.Label();
      this.cbxTcp = new System.Windows.Forms.CheckBox();
      this.cbxUdp = new System.Windows.Forms.CheckBox();
      this.cbxReport = new System.Windows.Forms.CheckBox();
      this.pnlState = new System.Windows.Forms.Panel();
      this.btDebugWin = new System.Windows.Forms.Button();
      this.txLocIP = new System.Windows.Forms.TextBox();
      this.lblStatusTxt = new System.Windows.Forms.Label();
      this.lblStatusCol = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lbxClients = new System.Windows.Forms.ListBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.IL = new System.Windows.Forms.ImageList(this.components);
      this.ICON = new System.Windows.Forms.NotifyIcon(this.components);
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.lblSCdx = new System.Windows.Forms.Label();
      this.lblVJoy = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // btStartStop
      // 
      this.btStartStop.Location = new System.Drawing.Point(9, 100);
      this.btStartStop.Name = "btStartStop";
      this.btStartStop.Size = new System.Drawing.Size(55, 40);
      this.btStartStop.TabIndex = 0;
      this.btStartStop.Text = "Start Server";
      this.btStartStop.UseVisualStyleBackColor = true;
      this.btStartStop.Click += new System.EventHandler(this.btStartStop_Click);
      // 
      // txPort
      // 
      this.txPort.Location = new System.Drawing.Point(78, 42);
      this.txPort.Name = "txPort";
      this.txPort.Size = new System.Drawing.Size(161, 20);
      this.txPort.TabIndex = 2;
      this.txPort.Text = "34123";
      this.txPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(49, 13);
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
      this.groupBox1.Controls.Add(this.lblStatusCol);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.txPort);
      this.groupBox1.Controls.Add(this.btStartStop);
      this.groupBox1.Location = new System.Drawing.Point(12, 91);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(271, 177);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Server";
      // 
      // cbxJoystick
      // 
      this.cbxJoystick.FormattingEnabled = true;
      this.cbxJoystick.Location = new System.Drawing.Point(161, 100);
      this.cbxJoystick.Name = "cbxJoystick";
      this.cbxJoystick.Size = new System.Drawing.Size(104, 21);
      this.cbxJoystick.TabIndex = 12;
      // 
      // lblSignal
      // 
      this.lblSignal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblSignal.Location = new System.Drawing.Point(161, 146);
      this.lblSignal.Name = "lblSignal";
      this.lblSignal.Size = new System.Drawing.Size(104, 23);
      this.lblSignal.TabIndex = 11;
      this.lblSignal.Text = "1235";
      this.lblSignal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // cbxTcp
      // 
      this.cbxTcp.AutoSize = true;
      this.cbxTcp.Location = new System.Drawing.Point(70, 123);
      this.cbxTcp.Name = "cbxTcp";
      this.cbxTcp.Size = new System.Drawing.Size(47, 17);
      this.cbxTcp.TabIndex = 10;
      this.cbxTcp.Text = "TCP";
      this.cbxTcp.UseVisualStyleBackColor = true;
      // 
      // cbxUdp
      // 
      this.cbxUdp.AutoSize = true;
      this.cbxUdp.Checked = true;
      this.cbxUdp.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbxUdp.Location = new System.Drawing.Point(70, 100);
      this.cbxUdp.Name = "cbxUdp";
      this.cbxUdp.Size = new System.Drawing.Size(49, 17);
      this.cbxUdp.TabIndex = 9;
      this.cbxUdp.Text = "UDP";
      this.cbxUdp.UseVisualStyleBackColor = true;
      // 
      // cbxReport
      // 
      this.cbxReport.AutoSize = true;
      this.cbxReport.Location = new System.Drawing.Point(70, 150);
      this.cbxReport.Name = "cbxReport";
      this.cbxReport.Size = new System.Drawing.Size(92, 17);
      this.cbxReport.TabIndex = 8;
      this.cbxReport.Text = "Report Clients";
      this.cbxReport.UseVisualStyleBackColor = true;
      this.cbxReport.CheckedChanged += new System.EventHandler(this.cbxReport_CheckedChanged);
      // 
      // pnlState
      // 
      this.pnlState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.pnlState.Location = new System.Drawing.Point(161, 68);
      this.pnlState.Name = "pnlState";
      this.pnlState.Size = new System.Drawing.Size(78, 26);
      this.pnlState.TabIndex = 7;
      // 
      // btDebugWin
      // 
      this.btDebugWin.Location = new System.Drawing.Point(9, 146);
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
      this.txLocIP.Size = new System.Drawing.Size(161, 20);
      this.txLocIP.TabIndex = 1;
      this.txLocIP.Text = "192.168.1.1";
      this.txLocIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // lblStatusTxt
      // 
      this.lblStatusTxt.AutoSize = true;
      this.lblStatusTxt.Location = new System.Drawing.Point(112, 70);
      this.lblStatusTxt.Name = "lblStatusTxt";
      this.lblStatusTxt.Size = new System.Drawing.Size(23, 13);
      this.lblStatusTxt.TabIndex = 4;
      this.lblStatusTxt.Text = "idle";
      // 
      // lblStatusCol
      // 
      this.lblStatusCol.AutoSize = true;
      this.lblStatusCol.Location = new System.Drawing.Point(75, 70);
      this.lblStatusCol.Name = "lblStatusCol";
      this.lblStatusCol.Size = new System.Drawing.Size(31, 13);
      this.lblStatusCol.TabIndex = 4;
      this.lblStatusCol.Text = "____";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 45);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(29, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Port:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 70);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Status:";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.lbxClients);
      this.groupBox2.Location = new System.Drawing.Point(12, 274);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(271, 202);
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
      this.lbxClients.Size = new System.Drawing.Size(259, 173);
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
      this.groupBox3.Controls.Add(this.lblSCdx);
      this.groupBox3.Controls.Add(this.lblVJoy);
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Location = new System.Drawing.Point(12, 12);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(271, 73);
      this.groupBox3.TabIndex = 6;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Device Support";
      // 
      // lblSCdx
      // 
      this.lblSCdx.AutoSize = true;
      this.lblSCdx.Location = new System.Drawing.Point(101, 47);
      this.lblSCdx.Name = "lblSCdx";
      this.lblSCdx.Size = new System.Drawing.Size(16, 13);
      this.lblSCdx.TabIndex = 6;
      this.lblSCdx.Text = "...";
      // 
      // lblVJoy
      // 
      this.lblVJoy.AutoSize = true;
      this.lblVJoy.Location = new System.Drawing.Point(100, 25);
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
      this.label5.Size = new System.Drawing.Size(89, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "SCdx - Keyboard:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 25);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(79, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "vJoy - Joystick:";
      // 
      // FrmStatus
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(293, 484);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
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
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btStartStop;
    private System.Windows.Forms.TextBox txPort;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label lblStatusTxt;
    private System.Windows.Forms.Label lblStatusCol;
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
    private System.Windows.Forms.ComboBox cbxJoystick;
  }
}

