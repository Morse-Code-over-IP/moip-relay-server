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
			this.lblTiming = new System.Windows.Forms.Label();
			this.nudRiseFallMs = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.cbSoundDevs = new System.Windows.Forms.ComboBox();
			this.nudSounder = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.nudToneFreq = new System.Windows.Forms.NumericUpDown();
			this.rbSounder = new System.Windows.Forms.RadioButton();
			this.rbTone = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbBug = new System.Windows.Forms.RadioButton();
			this.chkSwapPaddles = new System.Windows.Forms.CheckBox();
			this.chkModeA = new System.Windows.Forms.CheckBox();
			this.pnlModeB = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.nudCodeSpeed = new System.Windows.Forms.NumericUpDown();
			this.rbIambic = new System.Windows.Forms.RadioButton();
			this.rbStraightKey = new System.Windows.Forms.RadioButton();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.picHelp = new System.Windows.Forms.PictureBox();
			this.llHelp = new System.Windows.Forms.LinkLabel();
			this.tbVolume = new System.Windows.Forms.TrackBar();
			this.pnlHotSpot.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSerialPort)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudRiseFallMs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlHotSpot
			// 
			this.pnlHotSpot.BackColor = System.Drawing.Color.Blue;
			this.pnlHotSpot.Controls.Add(this.label2);
			this.pnlHotSpot.Location = new System.Drawing.Point(319, 33);
			this.pnlHotSpot.Name = "pnlHotSpot";
			this.pnlHotSpot.Size = new System.Drawing.Size(94, 73);
			this.pnlHotSpot.TabIndex = 0;
			this.toolTip.SetToolTip(this.pnlHotSpot, "Place mouse here for mouse keying");
			this.pnlHotSpot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlHotSpot_MouseDown);
			this.pnlHotSpot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlHotSpot_MouseUp);
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Yellow;
			this.label2.Location = new System.Drawing.Point(13, 15);
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
			this.groupBox3.Location = new System.Drawing.Point(319, 138);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(95, 102);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Serial I/O";
			// 
			// btnTestSerial
			// 
			this.btnTestSerial.Location = new System.Drawing.Point(13, 69);
			this.btnTestSerial.Name = "btnTestSerial";
			this.btnTestSerial.Size = new System.Drawing.Size(67, 22);
			this.btnTestSerial.TabIndex = 2;
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
			this.chkUseSerial.Location = new System.Drawing.Point(8, 47);
			this.chkUseSerial.Name = "chkUseSerial";
			this.chkUseSerial.Size = new System.Drawing.Size(72, 17);
			this.chkUseSerial.TabIndex = 1;
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
			this.groupBox2.Controls.Add(this.lblTiming);
			this.groupBox2.Controls.Add(this.nudRiseFallMs);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.cbSoundDevs);
			this.groupBox2.Controls.Add(this.nudSounder);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.nudToneFreq);
			this.groupBox2.Controls.Add(this.rbSounder);
			this.groupBox2.Controls.Add(this.rbTone);
			this.groupBox2.Location = new System.Drawing.Point(17, 139);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(263, 148);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sound Output";
			// 
			// lblTiming
			// 
			this.lblTiming.AutoSize = true;
			this.lblTiming.Location = new System.Drawing.Point(32, 47);
			this.lblTiming.Name = "lblTiming";
			this.lblTiming.Size = new System.Drawing.Size(160, 13);
			this.lblTiming.TabIndex = 16;
			this.lblTiming.Text = "Tone envelope rise/fall time (ms)";
			// 
			// nudRiseFallMs
			// 
			this.nudRiseFallMs.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "RiseFall", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudRiseFallMs.Location = new System.Drawing.Point(196, 45);
			this.nudRiseFallMs.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.nudRiseFallMs.Name = "nudRiseFallMs";
			this.nudRiseFallMs.Size = new System.Drawing.Size(51, 20);
			this.nudRiseFallMs.TabIndex = 7;
			this.nudRiseFallMs.Value = global::com.dc3.morse.Properties.Settings.Default.RiseFall;
			this.nudRiseFallMs.ValueChanged += new System.EventHandler(this.nudRiseFallMs_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(14, 95);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(106, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Sound output device";
			// 
			// cbSoundDevs
			// 
			this.cbSoundDevs.DisplayMember = "00000000-0000-0000-0000-000000000000";
			this.cbSoundDevs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSoundDevs.FormattingEnabled = true;
			this.cbSoundDevs.Location = new System.Drawing.Point(16, 113);
			this.cbSoundDevs.Name = "cbSoundDevs";
			this.cbSoundDevs.Size = new System.Drawing.Size(230, 21);
			this.cbSoundDevs.TabIndex = 5;
			this.cbSoundDevs.ValueMember = "00000000-0000-0000-0000-000000000000";
			this.cbSoundDevs.SelectedIndexChanged += new System.EventHandler(this.cbSoundDevs_SelectedIndexChanged);
			// 
			// nudSounder
			// 
			this.nudSounder.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "SounderNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudSounder.Location = new System.Drawing.Point(196, 71);
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
			this.nudSounder.TabIndex = 4;
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
			this.label4.TabIndex = 1;
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
			this.nudToneFreq.TabIndex = 2;
			this.toolTip.SetToolTip(this.nudToneFreq, "Tone frequency - change to hear sample");
			this.nudToneFreq.Value = global::com.dc3.morse.Properties.Settings.Default.ToneFreq;
			this.nudToneFreq.ValueChanged += new System.EventHandler(this.nudToneFreq_ValueChanged);
			// 
			// rbSounder
			// 
			this.rbSounder.AutoSize = true;
			this.rbSounder.Location = new System.Drawing.Point(16, 71);
			this.rbSounder.Name = "rbSounder";
			this.rbSounder.Size = new System.Drawing.Size(153, 17);
			this.rbSounder.TabIndex = 3;
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
			this.groupBox1.Controls.Add(this.rbBug);
			this.groupBox1.Controls.Add(this.chkSwapPaddles);
			this.groupBox1.Controls.Add(this.chkModeA);
			this.groupBox1.Controls.Add(this.pnlModeB);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.nudCodeSpeed);
			this.groupBox1.Controls.Add(this.rbIambic);
			this.groupBox1.Controls.Add(this.rbStraightKey);
			this.groupBox1.Location = new System.Drawing.Point(17, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(263, 114);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Keyer Mode";
			// 
			// rbBug
			// 
			this.rbBug.AutoSize = true;
			this.rbBug.Location = new System.Drawing.Point(15, 41);
			this.rbBug.Name = "rbBug";
			this.rbBug.Size = new System.Drawing.Size(99, 17);
			this.rbBug.TabIndex = 7;
			this.rbBug.TabStop = true;
			this.rbBug.Text = "Semi-auto (bug)";
			this.toolTip.SetToolTip(this.rbBug, "Dits are automatic, dahs are manual (bug mode)");
			this.rbBug.UseVisualStyleBackColor = true;
			this.rbBug.CheckedChanged += new System.EventHandler(this.rbBug_CheckedChanged);
			// 
			// chkSwapPaddles
			// 
			this.chkSwapPaddles.AutoSize = true;
			this.chkSwapPaddles.Checked = global::com.dc3.morse.Properties.Settings.Default.SwapPaddles;
			this.chkSwapPaddles.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::com.dc3.morse.Properties.Settings.Default, "SwapPaddles", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkSwapPaddles.Location = new System.Drawing.Point(133, 88);
			this.chkSwapPaddles.Name = "chkSwapPaddles";
			this.chkSwapPaddles.Size = new System.Drawing.Size(95, 17);
			this.chkSwapPaddles.TabIndex = 5;
			this.chkSwapPaddles.Text = "Swap left/right";
			this.toolTip.SetToolTip(this.chkSwapPaddles, "Swap left/right paddles and mouse buttons");
			this.chkSwapPaddles.UseVisualStyleBackColor = true;
			this.chkSwapPaddles.CheckedChanged += new System.EventHandler(this.chkSwapPaddles_CheckedChanged);
			// 
			// chkModeA
			// 
			this.chkModeA.AutoSize = true;
			this.chkModeA.Checked = global::com.dc3.morse.Properties.Settings.Default.IambicA;
			this.chkModeA.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::com.dc3.morse.Properties.Settings.Default, "IambicA", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkModeA.Location = new System.Drawing.Point(15, 88);
			this.chkModeA.Name = "chkModeA";
			this.chkModeA.Size = new System.Drawing.Size(63, 17);
			this.chkModeA.TabIndex = 4;
			this.chkModeA.Text = "Mode A";
			this.toolTip.SetToolTip(this.chkModeA, "Iambic-A mode (no additional trailing elements)");
			this.chkModeA.UseVisualStyleBackColor = true;
			this.chkModeA.CheckedChanged += new System.EventHandler(this.chkModeA_CheckedChanged);
			// 
			// pnlModeB
			// 
			this.pnlModeB.BackColor = System.Drawing.Color.Black;
			this.pnlModeB.Location = new System.Drawing.Point(134, 67);
			this.pnlModeB.Name = "pnlModeB";
			this.pnlModeB.Size = new System.Drawing.Size(12, 13);
			this.pnlModeB.TabIndex = 6;
			this.toolTip.SetToolTip(this.pnlModeB, "Flashes when trailing symbol added by Mode-B");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(129, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(69, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Speed (wpm)";
			// 
			// nudCodeSpeed
			// 
			this.nudCodeSpeed.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "CodeSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudCodeSpeed.Location = new System.Drawing.Point(199, 42);
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
			this.nudCodeSpeed.TabIndex = 3;
			this.toolTip.SetToolTip(this.nudCodeSpeed, "Dit speed for semi-auto, code speed for iambic mode");
			this.nudCodeSpeed.Value = global::com.dc3.morse.Properties.Settings.Default.CodeSpeed;
			this.nudCodeSpeed.ValueChanged += new System.EventHandler(this.nudSpeed_ValueChanged);
			// 
			// rbIambic
			// 
			this.rbIambic.AutoSize = true;
			this.rbIambic.Location = new System.Drawing.Point(15, 63);
			this.rbIambic.Name = "rbIambic";
			this.rbIambic.Size = new System.Drawing.Size(96, 17);
			this.rbIambic.TabIndex = 1;
			this.rbIambic.TabStop = true;
			this.rbIambic.Text = "Iambic paddles";
			this.toolTip.SetToolTip(this.rbIambic, "Iambic keying (\"squeeze mode\")");
			this.rbIambic.UseVisualStyleBackColor = true;
			this.rbIambic.CheckedChanged += new System.EventHandler(this.rbIambic_CheckedChanged);
			// 
			// rbStraightKey
			// 
			this.rbStraightKey.AutoSize = true;
			this.rbStraightKey.Location = new System.Drawing.Point(15, 19);
			this.rbStraightKey.Name = "rbStraightKey";
			this.rbStraightKey.Size = new System.Drawing.Size(81, 17);
			this.rbStraightKey.TabIndex = 0;
			this.rbStraightKey.TabStop = true;
			this.rbStraightKey.Text = "Straight key";
			this.toolTip.SetToolTip(this.rbStraightKey, "Simple straight keying - no automation");
			this.rbStraightKey.UseVisualStyleBackColor = true;
			this.rbStraightKey.CheckedChanged += new System.EventHandler(this.rbStraightKey_CheckedChanged);
			// 
			// picHelp
			// 
			this.picHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picHelp.Image = ((System.Drawing.Image)(resources.GetObject("picHelp.Image")));
			this.picHelp.InitialImage = null;
			this.picHelp.Location = new System.Drawing.Point(332, 255);
			this.picHelp.Name = "picHelp";
			this.picHelp.Size = new System.Drawing.Size(16, 16);
			this.picHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picHelp.TabIndex = 23;
			this.picHelp.TabStop = false;
			this.toolTip.SetToolTip(this.picHelp, "Click to see RSS Morse help");
			this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
			// 
			// llHelp
			// 
			this.llHelp.AutoSize = true;
			this.llHelp.Location = new System.Drawing.Point(351, 256);
			this.llHelp.Name = "llHelp";
			this.llHelp.Size = new System.Drawing.Size(46, 13);
			this.llHelp.TabIndex = 4;
			this.llHelp.TabStop = true;
			this.llHelp.Text = "Help me";
			this.toolTip.SetToolTip(this.llHelp, "Click to see RSS Morse help");
			this.llHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHelp_LinkClicked);
			// 
			// tbVolume
			// 
			this.tbVolume.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.morse.Properties.Settings.Default, "Volume", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.tbVolume.Location = new System.Drawing.Point(286, 138);
			this.tbVolume.Name = "tbVolume";
			this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.tbVolume.Size = new System.Drawing.Size(45, 115);
			this.tbVolume.TabIndex = 3;
			this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.None;
			this.toolTip.SetToolTip(this.tbVolume, "Adjust the sound volume");
			this.tbVolume.Value = global::com.dc3.morse.Properties.Settings.Default.Volume;
			this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 300);
			this.Controls.Add(this.picHelp);
			this.Controls.Add(this.llHelp);
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
			this.Text = "Morse Keyer V3.1";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.pnlHotSpot.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSerialPort)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudRiseFallMs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
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
		private System.Windows.Forms.RadioButton rbIambic;
		private System.Windows.Forms.RadioButton rbStraightKey;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TrackBar tbVolume;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnlModeB;
		private System.Windows.Forms.CheckBox chkSwapPaddles;
		private System.Windows.Forms.CheckBox chkModeA;
		private System.Windows.Forms.PictureBox picHelp;
		private System.Windows.Forms.LinkLabel llHelp;
		private System.Windows.Forms.RadioButton rbBug;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbSoundDevs;
		private System.Windows.Forms.NumericUpDown nudRiseFallMs;
		private System.Windows.Forms.Label lblTiming;
	}
}

