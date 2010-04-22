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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbAmerican = new System.Windows.Forms.RadioButton();
			this.rbInternational = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.nudStoryAge = new System.Windows.Forms.NumericUpDown();
			this.nudToneFreq = new System.Windows.Forms.NumericUpDown();
			this.nudCodeSpeed = new System.Windows.Forms.NumericUpDown();
			this.nudPollInterval = new System.Windows.Forms.NumericUpDown();
			this.txtFeedUrl = new System.Windows.Forms.TextBox();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudStoryAge)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPollInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statBarLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 170);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(450, 22);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statBarLabel
			// 
			this.statBarLabel.Name = "statBarLabel";
			this.statBarLabel.Size = new System.Drawing.Size(47, 17);
			this.statBarLabel.Text = "Yomama";
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
			this.groupBox1.Controls.Add(this.rbAmerican);
			this.groupBox1.Controls.Add(this.rbInternational);
			this.groupBox1.Location = new System.Drawing.Point(17, 84);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(114, 69);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Morse Code";
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
			this.rbInternational.UseVisualStyleBackColor = true;
			this.rbInternational.CheckedChanged += new System.EventHandler(this.rbInternational_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(87, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Poll Interval (min)";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(161, 100);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Speed (WPM)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(161, 133);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Tone Freq (Hz)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(274, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(78, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Story Age (min)";
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(345, 111);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(84, 27);
			this.btnStartStop.TabIndex = 12;
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// nudStoryAge
			// 
			this.nudStoryAge.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "StoryAge", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudStoryAge.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudStoryAge.Location = new System.Drawing.Point(358, 46);
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
			this.nudStoryAge.Size = new System.Drawing.Size(71, 20);
			this.nudStoryAge.TabIndex = 8;
			this.nudStoryAge.Value = global::com.dc3.Properties.Settings.Default.StoryAge;
			this.nudStoryAge.ValueChanged += new System.EventHandler(this.nudStoryAge_ValueChanged);
			// 
			// nudToneFreq
			// 
			this.nudToneFreq.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "ToneFreq", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudToneFreq.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudToneFreq.Location = new System.Drawing.Point(254, 131);
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
			this.nudToneFreq.Size = new System.Drawing.Size(71, 20);
			this.nudToneFreq.TabIndex = 7;
			this.nudToneFreq.Value = global::com.dc3.Properties.Settings.Default.ToneFreq;
			this.nudToneFreq.ValueChanged += new System.EventHandler(this.nudToneFreq_ValueChanged);
			// 
			// nudCodeSpeed
			// 
			this.nudCodeSpeed.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "CodeSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudCodeSpeed.Location = new System.Drawing.Point(254, 98);
			this.nudCodeSpeed.Name = "nudCodeSpeed";
			this.nudCodeSpeed.Size = new System.Drawing.Size(71, 20);
			this.nudCodeSpeed.TabIndex = 6;
			this.nudCodeSpeed.Value = global::com.dc3.Properties.Settings.Default.CodeSpeed;
			this.nudCodeSpeed.ValueChanged += new System.EventHandler(this.nudCodeSpeed_ValueChanged);
			// 
			// nudPollInterval
			// 
			this.nudPollInterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "PollInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudPollInterval.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudPollInterval.Location = new System.Drawing.Point(178, 46);
			this.nudPollInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
			this.nudPollInterval.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudPollInterval.Name = "nudPollInterval";
			this.nudPollInterval.Size = new System.Drawing.Size(71, 20);
			this.nudPollInterval.TabIndex = 4;
			this.nudPollInterval.Value = global::com.dc3.Properties.Settings.Default.PollInterval;
			this.nudPollInterval.ValueChanged += new System.EventHandler(this.nudPollInterval_ValueChanged);
			// 
			// txtFeedUrl
			// 
			this.txtFeedUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::com.dc3.Properties.Settings.Default, "FeedURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.txtFeedUrl.Location = new System.Drawing.Point(90, 14);
			this.txtFeedUrl.Name = "txtFeedUrl";
			this.txtFeedUrl.Size = new System.Drawing.Size(339, 20);
			this.txtFeedUrl.TabIndex = 1;
			this.txtFeedUrl.Text = global::com.dc3.Properties.Settings.Default.FeedURL;
			this.txtFeedUrl.TextChanged += new System.EventHandler(this.txtFeedUrl_TextChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(450, 192);
			this.Controls.Add(this.btnStartStop);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.nudStoryAge);
			this.Controls.Add(this.nudToneFreq);
			this.Controls.Add(this.nudCodeSpeed);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nudPollInterval);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFeedUrl);
			this.Controls.Add(this.statusStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "RSS to Morse";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudStoryAge)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPollInterval)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TextBox txtFeedUrl;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbAmerican;
		private System.Windows.Forms.RadioButton rbInternational;
		private System.Windows.Forms.NumericUpDown nudPollInterval;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nudCodeSpeed;
		private System.Windows.Forms.NumericUpDown nudToneFreq;
		private System.Windows.Forms.NumericUpDown nudStoryAge;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.ToolStripStatusLabel statBarLabel;
	}
}

