namespace com.dc3
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statBarCrawl = new System.Windows.Forms.ToolStripStatusLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.nudTimingComp = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.nudCodeSpeed = new System.Windows.Forms.NumericUpDown();
			this.rbAmerican = new System.Windows.Forms.RadioButton();
			this.rbInternational = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.nudSounder = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.nudToneFreq = new System.Windows.Forms.NumericUpDown();
			this.rbSounder = new System.Windows.Forms.RadioButton();
			this.rbTone = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.picTestSerial = new System.Windows.Forms.PictureBox();
			this.chkUseSerial = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.nudSerialPort = new System.Windows.Forms.NumericUpDown();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnClearCache = new System.Windows.Forms.Button();
			this.llHelp = new System.Windows.Forms.LinkLabel();
			this.picHelp = new System.Windows.Forms.PictureBox();
			this.picRSS = new System.Windows.Forms.PictureBox();
			this.llRSSFeeds = new System.Windows.Forms.LinkLabel();
			this.cbFeedUrl = new System.Windows.Forms.ComboBox();
			this.nudStoryAge = new System.Windows.Forms.NumericUpDown();
			this.nudPollInterval = new System.Windows.Forms.NumericUpDown();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTimingComp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).BeginInit();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTestSerial)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSerialPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picRSS)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStoryAge)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPollInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statBarLabel,
            this.statBarCrawl});
			this.statusStrip1.Location = new System.Drawing.Point(0, 253);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(410, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statBarLabel
			// 
			this.statBarLabel.AutoSize = false;
			this.statBarLabel.Name = "statBarLabel";
			this.statBarLabel.Size = new System.Drawing.Size(150, 17);
			this.statBarLabel.Text = "Initializing...";
			// 
			// statBarCrawl
			// 
			this.statBarCrawl.AutoSize = false;
			this.statBarCrawl.BackColor = System.Drawing.Color.Black;
			this.statBarCrawl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.statBarCrawl.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statBarCrawl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.statBarCrawl.Name = "statBarCrawl";
			this.statBarCrawl.Size = new System.Drawing.Size(258, 17);
			this.statBarCrawl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Feed URL";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.nudTimingComp);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.nudCodeSpeed);
			this.groupBox1.Controls.Add(this.rbAmerican);
			this.groupBox1.Controls.Add(this.rbInternational);
			this.groupBox1.Location = new System.Drawing.Point(16, 79);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(263, 69);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Morse Code";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(101, 44);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(90, 13);
			this.label7.TabIndex = 13;
			this.label7.Text = "Timing Comp (ms)";
			// 
			// nudTimingComp
			// 
			this.nudTimingComp.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "TimingComp", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudTimingComp.Location = new System.Drawing.Point(195, 42);
			this.nudTimingComp.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.nudTimingComp.Name = "nudTimingComp";
			this.nudTimingComp.Size = new System.Drawing.Size(51, 20);
			this.nudTimingComp.TabIndex = 1;
			this.toolTip.SetToolTip(this.nudTimingComp, "Increase to tighten up space between mark symbols");
			this.nudTimingComp.Value = global::com.dc3.Properties.Settings.Default.TimingComp;
			this.nudTimingComp.ValueChanged += new System.EventHandler(this.nudTimingComp_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(118, 21);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Speed (WPM)";
			// 
			// nudCodeSpeed
			// 
			this.nudCodeSpeed.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "CodeSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudCodeSpeed.Location = new System.Drawing.Point(195, 19);
			this.nudCodeSpeed.Maximum = new decimal(new int[] {
            35,
            0,
            0,
            0});
			this.nudCodeSpeed.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudCodeSpeed.Name = "nudCodeSpeed";
			this.nudCodeSpeed.Size = new System.Drawing.Size(51, 20);
			this.nudCodeSpeed.TabIndex = 0;
			this.toolTip.SetToolTip(this.nudCodeSpeed, "Code speed, words per minute");
			this.nudCodeSpeed.Value = global::com.dc3.Properties.Settings.Default.CodeSpeed;
			this.nudCodeSpeed.ValueChanged += new System.EventHandler(this.nudCodeSpeed_ValueChanged);
			// 
			// rbAmerican
			// 
			this.rbAmerican.AutoSize = true;
			this.rbAmerican.Location = new System.Drawing.Point(16, 42);
			this.rbAmerican.Name = "rbAmerican";
			this.rbAmerican.Size = new System.Drawing.Size(69, 17);
			this.rbAmerican.TabIndex = 1;
			this.rbAmerican.TabStop = true;
			this.rbAmerican.Text = "American";
			this.toolTip.SetToolTip(this.rbAmerican, "Use American code and send messages in telegram format");
			this.rbAmerican.UseVisualStyleBackColor = true;
			this.rbAmerican.CheckedChanged += new System.EventHandler(this.rbAmerican_CheckedChanged);
			// 
			// rbInternational
			// 
			this.rbInternational.AutoSize = true;
			this.rbInternational.Location = new System.Drawing.Point(16, 19);
			this.rbInternational.Name = "rbInternational";
			this.rbInternational.Size = new System.Drawing.Size(83, 17);
			this.rbInternational.TabIndex = 0;
			this.rbInternational.TabStop = true;
			this.rbInternational.Text = "International";
			this.toolTip.SetToolTip(this.rbInternational, "Use INternational code and send messages in radio format");
			this.rbInternational.UseVisualStyleBackColor = true;
			this.rbInternational.CheckedChanged += new System.EventHandler(this.rbInternational_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Poll Interval (min)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(158, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(78, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Story Age (min)";
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(298, 209);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(95, 27);
			this.btnStartStop.TabIndex = 7;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.nudSounder);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.nudToneFreq);
			this.groupBox2.Controls.Add(this.rbSounder);
			this.groupBox2.Controls.Add(this.rbTone);
			this.groupBox2.Location = new System.Drawing.Point(16, 160);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(263, 75);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sound Output";
			// 
			// nudSounder
			// 
			this.nudSounder.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "SounderNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudSounder.Location = new System.Drawing.Point(196, 43);
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
			this.nudSounder.TabIndex = 1;
			this.toolTip.SetToolTip(this.nudSounder, "Sounder type - change to hear sample");
			this.nudSounder.Value = global::com.dc3.Properties.Settings.Default.SounderNumber;
			this.nudSounder.ValueChanged += new System.EventHandler(this.nudSounder_ValueChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(115, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Tone Freq (Hz)";
			// 
			// nudToneFreq
			// 
			this.nudToneFreq.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "ToneFreq", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudToneFreq.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudToneFreq.Location = new System.Drawing.Point(196, 20);
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
			this.nudToneFreq.TabIndex = 0;
			this.toolTip.SetToolTip(this.nudToneFreq, "Tone frequency - change to hear sample");
			this.nudToneFreq.Value = global::com.dc3.Properties.Settings.Default.ToneFreq;
			this.nudToneFreq.ValueChanged += new System.EventHandler(this.nudToneFreq_ValueChanged);
			// 
			// rbSounder
			// 
			this.rbSounder.AutoSize = true;
			this.rbSounder.Location = new System.Drawing.Point(16, 43);
			this.rbSounder.Name = "rbSounder";
			this.rbSounder.Size = new System.Drawing.Size(153, 17);
			this.rbSounder.TabIndex = 1;
			this.rbSounder.TabStop = true;
			this.rbSounder.Text = "Telegraph Sounder (select)";
			this.toolTip.SetToolTip(this.rbSounder, "Use telegraph sounder sound for code output");
			this.rbSounder.UseVisualStyleBackColor = true;
			this.rbSounder.CheckedChanged += new System.EventHandler(this.rbSounder_CheckedChanged);
			// 
			// rbTone
			// 
			this.rbTone.AutoSize = true;
			this.rbTone.Location = new System.Drawing.Point(16, 20);
			this.rbTone.Name = "rbTone";
			this.rbTone.Size = new System.Drawing.Size(81, 17);
			this.rbTone.TabIndex = 0;
			this.rbTone.TabStop = true;
			this.rbTone.Text = "Radio Tone";
			this.toolTip.SetToolTip(this.rbTone, "Use radio tones for code output");
			this.rbTone.UseVisualStyleBackColor = true;
			this.rbTone.CheckedChanged += new System.EventHandler(this.rbTone_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.picTestSerial);
			this.groupBox3.Controls.Add(this.chkUseSerial);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.nudSerialPort);
			this.groupBox3.Location = new System.Drawing.Point(298, 79);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(95, 69);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Ext Sounder";
			// 
			// picTestSerial
			// 
			this.picTestSerial.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picTestSerial.Image = ((System.Drawing.Image)(resources.GetObject("picTestSerial.Image")));
			this.picTestSerial.Location = new System.Drawing.Point(62, 46);
			this.picTestSerial.Name = "picTestSerial";
			this.picTestSerial.Size = new System.Drawing.Size(18, 18);
			this.picTestSerial.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picTestSerial.TabIndex = 17;
			this.picTestSerial.TabStop = false;
			this.toolTip.SetToolTip(this.picTestSerial, "Click to send 4 dots to the sounder");
			this.picTestSerial.Click += new System.EventHandler(this.picTestSerial_Click);
			// 
			// chkUseSerial
			// 
			this.chkUseSerial.AutoSize = true;
			this.chkUseSerial.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUseSerial.Checked = global::com.dc3.Properties.Settings.Default.UseSerial;
			this.chkUseSerial.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::com.dc3.Properties.Settings.Default, "UseSerial", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkUseSerial.Location = new System.Drawing.Point(10, 47);
			this.chkUseSerial.Name = "chkUseSerial";
			this.chkUseSerial.Size = new System.Drawing.Size(45, 17);
			this.chkUseSerial.TabIndex = 1;
			this.chkUseSerial.Text = "Use";
			this.toolTip.SetToolTip(this.chkUseSerial, "Disable sounds and use real sounder");
			this.chkUseSerial.UseVisualStyleBackColor = true;
			this.chkUseSerial.CheckedChanged += new System.EventHandler(this.chkUseSerial_CheckedChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 23);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(26, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Port";
			// 
			// nudSerialPort
			// 
			this.nudSerialPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "SerialPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudSerialPort.Location = new System.Drawing.Point(43, 21);
			this.nudSerialPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudSerialPort.Name = "nudSerialPort";
			this.nudSerialPort.Size = new System.Drawing.Size(37, 20);
			this.nudSerialPort.TabIndex = 0;
			this.toolTip.SetToolTip(this.nudSerialPort, "Select serial port for real sounder");
			this.nudSerialPort.Value = global::com.dc3.Properties.Settings.Default.SerialPort;
			this.nudSerialPort.ValueChanged += new System.EventHandler(this.nudSerialPort_ValueChanged);
			// 
			// btnClearCache
			// 
			this.btnClearCache.Location = new System.Drawing.Point(298, 44);
			this.btnClearCache.Name = "btnClearCache";
			this.btnClearCache.Size = new System.Drawing.Size(95, 24);
			this.btnClearCache.TabIndex = 5;
			this.btnClearCache.Text = "Clear Cache";
			this.toolTip.SetToolTip(this.btnClearCache, "Clear the \"seen story\" cache now");
			this.btnClearCache.UseVisualStyleBackColor = true;
			this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
			// 
			// llHelp
			// 
			this.llHelp.AutoSize = true;
			this.llHelp.Location = new System.Drawing.Point(328, 183);
			this.llHelp.Name = "llHelp";
			this.llHelp.Size = new System.Drawing.Size(46, 13);
			this.llHelp.TabIndex = 12;
			this.llHelp.TabStop = true;
			this.llHelp.Text = "Help me";
			this.toolTip.SetToolTip(this.llHelp, "Click to see RSS Morse help");
			this.llHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llHelp_LinkClicked);
			// 
			// picHelp
			// 
			this.picHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picHelp.Image = ((System.Drawing.Image)(resources.GetObject("picHelp.Image")));
			this.picHelp.InitialImage = null;
			this.picHelp.Location = new System.Drawing.Point(306, 183);
			this.picHelp.Name = "picHelp";
			this.picHelp.Size = new System.Drawing.Size(16, 16);
			this.picHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picHelp.TabIndex = 13;
			this.picHelp.TabStop = false;
			this.toolTip.SetToolTip(this.picHelp, "Click to see RSS Morse help");
			this.picHelp.Click += new System.EventHandler(this.picHelp_Click);
			// 
			// picRSS
			// 
			this.picRSS.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picRSS.Image = ((System.Drawing.Image)(resources.GetObject("picRSS.Image")));
			this.picRSS.InitialImage = null;
			this.picRSS.Location = new System.Drawing.Point(306, 160);
			this.picRSS.Name = "picRSS";
			this.picRSS.Size = new System.Drawing.Size(16, 17);
			this.picRSS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picRSS.TabIndex = 15;
			this.picRSS.TabStop = false;
			this.toolTip.SetToolTip(this.picRSS, "Click to see Yahoo! RSS Feeds");
			this.picRSS.Click += new System.EventHandler(this.picRSS_Click);
			// 
			// llRSSFeeds
			// 
			this.llRSSFeeds.AutoSize = true;
			this.llRSSFeeds.Location = new System.Drawing.Point(328, 162);
			this.llRSSFeeds.Name = "llRSSFeeds";
			this.llRSSFeeds.Size = new System.Drawing.Size(58, 13);
			this.llRSSFeeds.TabIndex = 14;
			this.llRSSFeeds.TabStop = true;
			this.llRSSFeeds.Text = "RSS feeds";
			this.toolTip.SetToolTip(this.llRSSFeeds, "Click to see Yahoo! RSS Feeds");
			this.llRSSFeeds.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llRSSFeeds_LinkClicked);
			// 
			// cbFeedUrl
			// 
			this.cbFeedUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbFeedUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbFeedUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::com.dc3.Properties.Settings.Default, "FeedURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cbFeedUrl.FormattingEnabled = true;
			this.cbFeedUrl.Location = new System.Drawing.Point(74, 14);
			this.cbFeedUrl.Name = "cbFeedUrl";
			this.cbFeedUrl.Size = new System.Drawing.Size(319, 21);
			this.cbFeedUrl.TabIndex = 0;
			this.cbFeedUrl.Text = global::com.dc3.Properties.Settings.Default.FeedURL;
			this.toolTip.SetToolTip(this.cbFeedUrl, "RSS Feed URL - May be file or web feed");
			this.cbFeedUrl.TextChanged += new System.EventHandler(this.cbFeedUrl_TextChanged);
			// 
			// nudStoryAge
			// 
			this.nudStoryAge.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "StoryAge", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudStoryAge.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudStoryAge.Location = new System.Drawing.Point(239, 46);
			this.nudStoryAge.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
			this.nudStoryAge.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudStoryAge.Name = "nudStoryAge";
			this.nudStoryAge.Size = new System.Drawing.Size(52, 20);
			this.nudStoryAge.TabIndex = 2;
			this.toolTip.SetToolTip(this.nudStoryAge, "Don\'t re-send stories sent within this time");
			this.nudStoryAge.Value = global::com.dc3.Properties.Settings.Default.StoryAge;
			this.nudStoryAge.ValueChanged += new System.EventHandler(this.nudStoryAge_ValueChanged);
			// 
			// nudPollInterval
			// 
			this.nudPollInterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "PollInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudPollInterval.Location = new System.Drawing.Point(104, 46);
			this.nudPollInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
			this.nudPollInterval.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.nudPollInterval.Name = "nudPollInterval";
			this.nudPollInterval.Size = new System.Drawing.Size(45, 20);
			this.nudPollInterval.TabIndex = 1;
			this.toolTip.SetToolTip(this.nudPollInterval, "Interval between checks of RSS feed for new stories");
			this.nudPollInterval.Value = global::com.dc3.Properties.Settings.Default.PollInterval;
			this.nudPollInterval.ValueChanged += new System.EventHandler(this.nudPollInterval_ValueChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(410, 275);
			this.Controls.Add(this.picRSS);
			this.Controls.Add(this.llRSSFeeds);
			this.Controls.Add(this.picHelp);
			this.Controls.Add(this.llHelp);
			this.Controls.Add(this.btnClearCache);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.cbFeedUrl);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnStartStop);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.nudStoryAge);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nudPollInterval);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.statusStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "RSS to Morse V1.2";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudTimingComp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picTestSerial)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSerialPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picHelp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picRSS)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStoryAge)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPollInterval)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbAmerican;
		private System.Windows.Forms.RadioButton rbInternational;
		private System.Windows.Forms.NumericUpDown nudPollInterval;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nudStoryAge;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.ToolStripStatusLabel statBarLabel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rbSounder;
		private System.Windows.Forms.RadioButton rbTone;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nudCodeSpeed;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown nudToneFreq;
		private System.Windows.Forms.NumericUpDown nudSounder;
		private System.Windows.Forms.ToolStripStatusLabel statBarCrawl;
		private System.Windows.Forms.ComboBox cbFeedUrl;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown nudSerialPort;
		private System.Windows.Forms.CheckBox chkUseSerial;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button btnClearCache;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown nudTimingComp;
		private System.Windows.Forms.LinkLabel llHelp;
		private System.Windows.Forms.PictureBox picHelp;
		private System.Windows.Forms.PictureBox picRSS;
		private System.Windows.Forms.LinkLabel llRSSFeeds;
		private System.Windows.Forms.PictureBox picTestSerial;
	}
}

