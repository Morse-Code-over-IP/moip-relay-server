namespace com.dc3.morse
{
	partial class frmMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.pnlHotSpot = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnTestSerial = new System.Windows.Forms.Button();
			this.chkUseSerial = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.nudSerialPort = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbExtSounder = new System.Windows.Forms.RadioButton();
			this.nudSounder = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.nudToneFreq = new System.Windows.Forms.NumericUpDown();
			this.rbSounder = new System.Windows.Forms.RadioButton();
			this.rbTone = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pnlModeB = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.nudCodeSpeed = new System.Windows.Forms.NumericUpDown();
			this.rbIambicB = new System.Windows.Forms.RadioButton();
			this.rbStraightKey = new System.Windows.Forms.RadioButton();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.tbVolume = new System.Windows.Forms.TrackBar();
			this.pnlHotSpot.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSerialPort)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlHotSpot
			// 
			this.pnlHotSpot.BackColor = System.Drawing.Color.Blue;
			this.pnlHotSpot.Controls.Add(this.label2);
			this.pnlHotSpot.Location = new System.Drawing.Point(314, 17);
			this.pnlHotSpot.Name = "pnlHotSpot";
			this.pnlHotSpot.Size = new System.Drawing.Size(94, 64);
			this.pnlHotSpot.TabIndex = 0;
			this.toolTip.SetToolTip(this.pnlHotSpot, "Place mouse here for mouse keying");
			this.pnlHotSpot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlHotSpot_MouseDown);
			this.pnlHotSpot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlHotSpot_MouseUp);
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Yellow;
			this.label2.Location = new System.Drawing.Point(12, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(67, 39);
			this.label2.TabIndex = 0;
			this.label2.Text = "Mouse Keying Area";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip.SetToolTip(this.label2, "Position cursor over this area for mouse keying");
			this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlHotSpot_MouseDown);
			this.label2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlHotSpot_MouseUp);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnTestSerial);
			this.groupBox3.Controls.Add(this.chkUseSerial);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.nudSerialPort);
			this.groupBox3.Location = new System.Drawing.Point(314, 94);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(95, 93);
			this.groupBox3.TabIndex = 16;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Serial I/O";
			// 
			// btnTestSerial
			// 
			this.btnTestSerial.Location = new System.Drawing.Point(12, 64);
			this.btnTestSerial.Name = "btnTestSerial";
			this.btnTestSerial.Size = new System.Drawing.Size(67, 22);
			this.btnTestSerial.TabIndex = 14;
			this.btnTestSerial.Text = "Test";
			this.toolTip.SetToolTip(this.btnTestSerial, "Test serial output to sounder");
			this.btnTestSerial.UseVisualStyleBackColor = true;
			this.btnTestSerial.Click += new System.EventHandler(this.btnTestSerial_Click);
			// 
			// chkUseSerial
			// 
			this.chkUseSerial.AutoSize = true;
			this.chkUseSerial.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUseSerial.Checked = global::com.dc3.morse.Properties.Settings.Default.UseSerial;
			this.chkUseSerial.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::com.dc3.morse.Properties.Settings.Default, "UseSerial", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkUseSerial.Location = new System.Drawing.Point(8, 45);
			this.chkUseSerial.Name = "chkUseSerial";
			this.chkUseSerial.Size = new System.Drawing.Size(72, 17);
			this.chkUseSerial.TabIndex = 13;
			this.chkUseSerial.Text = "Use serial";
			this.toolTip.SetToolTip(this.chkUseSerial, "Enable serial port key and sounder (if selected)");
			this.chkUseSerial.UseVisualStyleBackColor = true;
			this.chkUseSerial.CheckedChanged += new System.EventHandler(this.chkUseSerial_CheckedChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(10, 22);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(26, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Port";
			// 
			// nudSerialPort
			// 
			this.nudSerialPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "SerialPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudSerialPort.Location = new System.Drawing.Point(43, 20);
			this.nudSerialPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudSerialPort.Name = "nudSerialPort";
			this.nudSerialPort.Size = new System.Drawing.Size(37, 20);
			this.nudSerialPort.TabIndex = 0;
			this.toolTip.SetToolTip(this.nudSerialPort, "Select serial port for key/paddle and optional real sounder");
			this.nudSerialPort.Value = global::com.dc3.morse.Properties.Settings.Default.SerialPort;
			this.nudSerialPort.ValueChanged += new System.EventHandler(this.nudSerialPort_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbExtSounder);
			this.groupBox2.Controls.Add(this.nudSounder);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.nudToneFreq);
			this.groupBox2.Controls.Add(this.rbSounder);
			this.groupBox2.Controls.Add(this.rbTone);
			this.groupBox2.Location = new System.Drawing.Point(12, 94);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(263, 93);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sound Output";
			// 
			// rbExtSounder
			// 
			this.rbExtSounder.AutoSize = true;
			this.rbExtSounder.Location = new System.Drawing.Point(16, 65);
			this.rbExtSounder.Name = "rbExtSounder";
			this.rbExtSounder.Size = new System.Drawing.Size(223, 17);
			this.rbExtSounder.TabIndex = 14;
			this.rbExtSounder.TabStop = true;
			this.rbExtSounder.Text = "Physical telegraph sounder (via serial port)";
			this.toolTip.SetToolTip(this.rbExtSounder, "Use radio tone for output");
			this.rbExtSounder.UseVisualStyleBackColor = true;
			this.rbExtSounder.CheckedChanged += new System.EventHandler(this.rbExtSounder_CheckedChanged);
			// 
			// nudSounder
			// 
			this.nudSounder.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "SounderNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudSounder.Location = new System.Drawing.Point(196, 42);
			this.nudSounder.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
			this.nudSounder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudSounder.Name = "nudSounder";
			this.nudSounder.Size = new System.Drawing.Size(50, 20);
			this.nudSounder.TabIndex = 13;
			this.toolTip.SetToolTip(this.nudSounder, "Sounder type - change to hear sample");
			this.nudSounder.Value = global::com.dc3.morse.Properties.Settings.Default.SounderNumber;
			this.nudSounder.ValueChanged += new System.EventHandler(this.nudSounder_ValueChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(115, 21);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Tone Freq (Hz)";
			// 
			// nudToneFreq
			// 
			this.nudToneFreq.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "ToneFreq", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudToneFreq.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudToneFreq.Location = new System.Drawing.Point(196, 19);
			this.nudToneFreq.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			this.nudToneFreq.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudToneFreq.Name = "nudToneFreq";
			this.nudToneFreq.Size = new System.Drawing.Size(51, 20);
			this.nudToneFreq.TabIndex = 11;
			this.toolTip.SetToolTip(this.nudToneFreq, "Tone frequency - change to hear sample");
			this.nudToneFreq.Value = global::com.dc3.morse.Properties.Settings.Default.ToneFreq;
			this.nudToneFreq.ValueChanged += new System.EventHandler(this.nudToneFreq_ValueChanged);
			// 
			// rbSounder
			// 
			this.rbSounder.AutoSize = true;
			this.rbSounder.Location = new System.Drawing.Point(16, 42);
			this.rbSounder.Name = "rbSounder";
			this.rbSounder.Size = new System.Drawing.Size(153, 17);
			this.rbSounder.TabIndex = 1;
			this.rbSounder.TabStop = true;
			this.rbSounder.Text = "Telegraph Sounder (select)";
			this.toolTip.SetToolTip(this.rbSounder, "Use telegraph sounder sound for output");
			this.rbSounder.UseVisualStyleBackColor = true;
			this.rbSounder.CheckedChanged += new System.EventHandler(this.rbSounder_CheckedChanged);
			// 
			// rbTone
			// 
			this.rbTone.AutoSize = true;
			this.rbTone.Location = new System.Drawing.Point(16, 19);
			this.rbTone.Name = "rbTone";
			this.rbTone.Size = new System.Drawing.Size(81, 17);
			this.rbTone.TabIndex = 0;
			this.rbTone.TabStop = true;
			this.rbTone.Text = "Radio Tone";
			this.toolTip.SetToolTip(this.rbTone, "Use radio tone for output");
			this.rbTone.UseVisualStyleBackColor = true;
			this.rbTone.CheckedChanged += new System.EventHandler(this.rbTone_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pnlModeB);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.nudCodeSpeed);
			this.groupBox1.Controls.Add(this.rbIambicB);
			this.groupBox1.Controls.Add(this.rbStraightKey);
			this.groupBox1.Location = new System.Drawing.Point(12, 10);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(263, 72);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Keyer Mode";
			// 
			// pnlModeB
			// 
			this.pnlModeB.BackColor = System.Drawing.Color.Navy;
			this.pnlModeB.Location = new System.Drawing.Point(233, 45);
			this.pnlModeB.Name = "pnlModeB";
			this.pnlModeB.Size = new System.Drawing.Size(10, 11);
			this.pnlModeB.TabIndex = 6;
			this.toolTip.SetToolTip(this.pnlModeB, "Flashes when trailing symbol added by Mode-B");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(98, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Speed (WPM)";
			// 
			// nudCodeSpeed
			// 
			this.nudCodeSpeed.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "CodeSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudCodeSpeed.Location = new System.Drawing.Point(175, 42);
			this.nudCodeSpeed.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
			this.nudCodeSpeed.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudCodeSpeed.Name = "nudCodeSpeed";
			this.nudCodeSpeed.Size = new System.Drawing.Size(42, 20);
			this.nudCodeSpeed.TabIndex = 4;
			this.toolTip.SetToolTip(this.nudCodeSpeed, "Code speed for iambic-B mode");
			this.nudCodeSpeed.Value = global::com.dc3.morse.Properties.Settings.Default.CodeSpeed;
			this.nudCodeSpeed.ValueChanged += new System.EventHandler(this.nudSpeed_ValueChanged);
			// 
			// rbIambicB
			// 
			this.rbIambicB.AutoSize = true;
			this.rbIambicB.Location = new System.Drawing.Point(15, 42);
			this.rbIambicB.Name = "rbIambicB";
			this.rbIambicB.Size = new System.Drawing.Size(66, 17);
			this.rbIambicB.TabIndex = 3;
			this.rbIambicB.TabStop = true;
			this.rbIambicB.Text = "Iambic-B";
			this.toolTip.SetToolTip(this.rbIambicB, "Iambic B-mode keying (\"squeeze mode\")");
			this.rbIambicB.UseVisualStyleBackColor = true;
			this.rbIambicB.CheckedChanged += new System.EventHandler(this.rbIambicB_CheckedChanged);
			// 
			// rbStraightKey
			// 
			this.rbStraightKey.AutoSize = true;
			this.rbStraightKey.Location = new System.Drawing.Point(15, 19);
			this.rbStraightKey.Name = "rbStraightKey";
			this.rbStraightKey.Size = new System.Drawing.Size(81, 17);
			this.rbStraightKey.TabIndex = 2;
			this.rbStraightKey.TabStop = true;
			this.rbStraightKey.Text = "Straight key";
			this.toolTip.SetToolTip(this.rbStraightKey, "Simple straight keying - no automation");
			this.rbStraightKey.UseVisualStyleBackColor = true;
			this.rbStraightKey.CheckedChanged += new System.EventHandler(this.rbStraightKey_CheckedChanged);
			// 
			// tbVolume
			// 
			this.tbVolume.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "Volume", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.tbVolume.Location = new System.Drawing.Point(281, 94);
			this.tbVolume.Name = "tbVolume";
			this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbVolume.Size = new System.Drawing.Size(45, 101);
			this.tbVolume.TabIndex = 21;
			this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.None;
			this.toolTip.SetToolTip(this.tbVolume, "Adjust the sound volume");
			this.tbVolume.Value = global::com.dc3.morse.Properties.Settings.Default.Volume;
			this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 202);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.pnlHotSpot);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.tbVolume);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(169, 185);
			this.Name = "frmMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Morse Keyer";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.pnlHotSpot.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSerialPort)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel pnlHotSpot;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button btnTestSerial;
		private System.Windows.Forms.CheckBox chkUseSerial;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nudSerialPort;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown nudSounder;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudToneFreq;
		private System.Windows.Forms.RadioButton rbSounder;
		private System.Windows.Forms.RadioButton rbTone;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudCodeSpeed;
		private System.Windows.Forms.RadioButton rbIambicB;
		private System.Windows.Forms.RadioButton rbStraightKey;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.RadioButton rbExtSounder;
		private System.Windows.Forms.TrackBar tbVolume;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnlModeB;
	}
}

