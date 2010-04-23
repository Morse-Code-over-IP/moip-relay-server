﻿namespace com.dc3
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
			this.statBarCrawl = new System.Windows.Forms.ToolStripStatusLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.rbAmerican = new System.Windows.Forms.RadioButton();
			this.rbInternational = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.rbSounder = new System.Windows.Forms.RadioButton();
			this.rbTone = new System.Windows.Forms.RadioButton();
			this.cbFeedUrl = new System.Windows.Forms.ComboBox();
			this.nudSounder = new System.Windows.Forms.NumericUpDown();
			this.nudToneFreq = new System.Windows.Forms.NumericUpDown();
			this.nudStoryAge = new System.Windows.Forms.NumericUpDown();
			this.nudPollInterval = new System.Windows.Forms.NumericUpDown();
			this.nudCodeSpeed = new System.Windows.Forms.NumericUpDown();
			this.statusStrip1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStoryAge)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPollInterval)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).BeginInit();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statBarLabel,
            this.statBarCrawl});
			this.statusStrip1.Location = new System.Drawing.Point(0, 240);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(391, 22);
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
			this.statBarCrawl.Size = new System.Drawing.Size(238, 17);
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
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.nudCodeSpeed);
			this.groupBox1.Controls.Add(this.rbAmerican);
			this.groupBox1.Controls.Add(this.rbInternational);
			this.groupBox1.Location = new System.Drawing.Point(17, 75);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(263, 69);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Morse Code";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(119, 33);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(74, 13);
			this.label3.TabIndex = 11;
			this.label3.Text = "Speed (WPM)";
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
			this.label2.Location = new System.Drawing.Point(74, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Poll Interval (min)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(238, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(78, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Story Age (min)";
			// 
			// btnStartStop
			// 
			this.btnStartStop.Location = new System.Drawing.Point(296, 138);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(78, 27);
			this.btnStartStop.TabIndex = 12;
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
			this.groupBox2.Location = new System.Drawing.Point(17, 156);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(263, 69);
			this.groupBox2.TabIndex = 13;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sound Output";
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
			// rbSounder
			// 
			this.rbSounder.AutoSize = true;
			this.rbSounder.Location = new System.Drawing.Point(16, 42);
			this.rbSounder.Name = "rbSounder";
			this.rbSounder.Size = new System.Drawing.Size(153, 17);
			this.rbSounder.TabIndex = 1;
			this.rbSounder.TabStop = true;
			this.rbSounder.Text = "Telegraph Sounder (select)";
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
			this.rbTone.UseVisualStyleBackColor = true;
			this.rbTone.CheckedChanged += new System.EventHandler(this.rbTone_CheckedChanged);
			// 
			// cbFeedUrl
			// 
			this.cbFeedUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbFeedUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbFeedUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::com.dc3.Properties.Settings.Default, "FeedURL", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cbFeedUrl.FormattingEnabled = true;
			this.cbFeedUrl.Location = new System.Drawing.Point(74, 14);
			this.cbFeedUrl.Name = "cbFeedUrl";
			this.cbFeedUrl.Size = new System.Drawing.Size(300, 21);
			this.cbFeedUrl.TabIndex = 14;
			this.cbFeedUrl.Text = global::com.dc3.Properties.Settings.Default.FeedURL;
			this.cbFeedUrl.TextChanged += new System.EventHandler(this.cbFeedUrl_TextChanged);
			// 
			// nudSounder
			// 
			this.nudSounder.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "SounderNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
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
			this.nudSounder.Value = global::com.dc3.Properties.Settings.Default.SounderNumber;
			this.nudSounder.ValueChanged += new System.EventHandler(this.nudSounder_ValueChanged);
			// 
			// nudToneFreq
			// 
			this.nudToneFreq.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "ToneFreq", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
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
			this.nudToneFreq.Value = global::com.dc3.Properties.Settings.Default.ToneFreq;
			this.nudToneFreq.ValueChanged += new System.EventHandler(this.nudToneFreq_ValueChanged);
			// 
			// nudStoryAge
			// 
			this.nudStoryAge.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "StoryAge", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudStoryAge.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudStoryAge.Location = new System.Drawing.Point(322, 46);
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
			this.nudStoryAge.TabIndex = 8;
			this.nudStoryAge.Value = global::com.dc3.Properties.Settings.Default.StoryAge;
			this.nudStoryAge.ValueChanged += new System.EventHandler(this.nudStoryAge_ValueChanged);
			// 
			// nudPollInterval
			// 
			this.nudPollInterval.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "PollInterval", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudPollInterval.Location = new System.Drawing.Point(165, 46);
			this.nudPollInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
			this.nudPollInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.nudPollInterval.Name = "nudPollInterval";
			this.nudPollInterval.Size = new System.Drawing.Size(45, 20);
			this.nudPollInterval.TabIndex = 4;
			this.nudPollInterval.Value = global::com.dc3.Properties.Settings.Default.PollInterval;
			this.nudPollInterval.ValueChanged += new System.EventHandler(this.nudPollInterval_ValueChanged);
			// 
			// nudCodeSpeed
			// 
			this.nudCodeSpeed.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::com.dc3.Properties.Settings.Default, "CodeSpeed", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.nudCodeSpeed.Location = new System.Drawing.Point(196, 31);
			this.nudCodeSpeed.Name = "nudCodeSpeed";
			this.nudCodeSpeed.Size = new System.Drawing.Size(51, 20);
			this.nudCodeSpeed.TabIndex = 10;
			this.nudCodeSpeed.Value = global::com.dc3.Properties.Settings.Default.CodeSpeed;
			this.nudCodeSpeed.ValueChanged += new System.EventHandler(this.nudCodeSpeed_ValueChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(391, 262);
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
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSounder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudToneFreq)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudStoryAge)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPollInterval)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCodeSpeed)).EndInit();
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
	}
}
