namespace SCJoyServer_Client
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
    private void InitializeComponent()
    {
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btSendNext = new System.Windows.Forms.Button();
      this.pnlState = new System.Windows.Forms.Panel();
      this.txLocIP = new System.Windows.Forms.TextBox();
      this.rbJS2 = new System.Windows.Forms.RadioButton();
      this.rbJS1 = new System.Windows.Forms.RadioButton();
      this.lblStatusTxt = new System.Windows.Forms.Label();
      this.lblStatusCol = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.txPort = new System.Windows.Forms.TextBox();
      this.btStartStop = new System.Windows.Forms.Button();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.lbxClients = new System.Windows.Forms.ListBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btSendNext);
      this.groupBox1.Controls.Add(this.pnlState);
      this.groupBox1.Controls.Add(this.txLocIP);
      this.groupBox1.Controls.Add(this.rbJS2);
      this.groupBox1.Controls.Add(this.rbJS1);
      this.groupBox1.Controls.Add(this.lblStatusTxt);
      this.groupBox1.Controls.Add(this.lblStatusCol);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.txPort);
      this.groupBox1.Controls.Add(this.btStartStop);
      this.groupBox1.Location = new System.Drawing.Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(271, 177);
      this.groupBox1.TabIndex = 5;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Server";
      // 
      // btSendNext
      // 
      this.btSendNext.Location = new System.Drawing.Point(9, 134);
      this.btSendNext.Name = "btSendNext";
      this.btSendNext.Size = new System.Drawing.Size(126, 37);
      this.btSendNext.TabIndex = 8;
      this.btSendNext.Text = "Send Next";
      this.btSendNext.UseVisualStyleBackColor = true;
      this.btSendNext.Click += new System.EventHandler(this.btSendNext_Click);
      // 
      // pnlState
      // 
      this.pnlState.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.pnlState.Location = new System.Drawing.Point(161, 68);
      this.pnlState.Name = "pnlState";
      this.pnlState.Size = new System.Drawing.Size(78, 26);
      this.pnlState.TabIndex = 7;
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
      // rbJS2
      // 
      this.rbJS2.AutoSize = true;
      this.rbJS2.Checked = true;
      this.rbJS2.Location = new System.Drawing.Point(217, 100);
      this.rbJS2.Name = "rbJS2";
      this.rbJS2.Size = new System.Drawing.Size(48, 17);
      this.rbJS2.TabIndex = 4;
      this.rbJS2.TabStop = true;
      this.rbJS2.Text = "UDP";
      this.rbJS2.UseVisualStyleBackColor = true;
      // 
      // rbJS1
      // 
      this.rbJS1.AutoSize = true;
      this.rbJS1.Location = new System.Drawing.Point(161, 100);
      this.rbJS1.Name = "rbJS1";
      this.rbJS1.Size = new System.Drawing.Size(46, 17);
      this.rbJS1.TabIndex = 3;
      this.rbJS1.Text = "TCP";
      this.rbJS1.UseVisualStyleBackColor = true;
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
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(63, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Connect IP:";
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
      // btStartStop
      // 
      this.btStartStop.Location = new System.Drawing.Point(9, 100);
      this.btStartStop.Name = "btStartStop";
      this.btStartStop.Size = new System.Drawing.Size(126, 28);
      this.btStartStop.TabIndex = 0;
      this.btStartStop.Text = "Start Client";
      this.btStartStop.UseVisualStyleBackColor = true;
      this.btStartStop.Click += new System.EventHandler(this.btStartStop_Click);
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.lbxClients);
      this.groupBox2.Location = new System.Drawing.Point(12, 195);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(271, 202);
      this.groupBox2.TabIndex = 6;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Log";
      // 
      // lbxClients
      // 
      this.lbxClients.FormattingEnabled = true;
      this.lbxClients.HorizontalScrollbar = true;
      this.lbxClients.Location = new System.Drawing.Point(6, 19);
      this.lbxClients.Name = "lbxClients";
      this.lbxClients.ScrollAlwaysVisible = true;
      this.lbxClients.Size = new System.Drawing.Size(259, 173);
      this.lbxClients.TabIndex = 5;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(542, 627);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "Form1";
      this.Text = "SCJoyServer - Client";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Panel pnlState;
    private System.Windows.Forms.TextBox txLocIP;
    private System.Windows.Forms.RadioButton rbJS2;
    private System.Windows.Forms.RadioButton rbJS1;
    private System.Windows.Forms.Label lblStatusTxt;
    private System.Windows.Forms.Label lblStatusCol;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txPort;
    private System.Windows.Forms.Button btStartStop;
    private System.Windows.Forms.Button btSendNext;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ListBox lbxClients;
  }
}

