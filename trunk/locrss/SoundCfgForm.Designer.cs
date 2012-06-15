namespace com.dc3
{
	partial class SoundCfgForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoundCfgForm));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.lblTiming = new System.Windows.Forms.Label();
			this.nudRiseFall = new System.Windows.Forms.NumericUpDown();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.trkNoise = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cbSoundDevs = new System.Windows.Forms.ComboBox();
			this.chkStatic = new System.Windows.Forms.CheckBox();
			this.chkFading = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.nudRiseFall)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkNoise)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(168, 152);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(72, 25);
			this.cmdOK.TabIndex = 6;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(83, 152);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(72, 25);
			this.cmdCancel.TabIndex = 5;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			// 
			// lblTiming
			// 
			this.lblTiming.AutoSize = true;
			this.lblTiming.Location = new System.Drawing.Point(12, 59);
			this.lblTiming.Name = "lblTiming";
			this.lblTiming.Size = new System.Drawing.Size(160, 13);
			this.lblTiming.TabIndex = 15;
			this.lblTiming.Text = "Tone envelope rise/fall time (ms)";
			// 
			// nudRiseFall
			// 
			this.nudRiseFall.Location = new System.Drawing.Point(197, 57);
			this.nudRiseFall.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            0});
			this.nudRiseFall.Name = "nudRiseFall";
			this.nudRiseFall.Size = new System.Drawing.Size(43, 20);
			this.nudRiseFall.TabIndex = 1;
			this.toolTip.SetToolTip(this.nudRiseFall, "Sets the tone envelope rise and fall times");
			// 
			// trkNoise
			// 
			this.trkNoise.AutoSize = false;
			this.trkNoise.LargeChange = 1;
			this.trkNoise.Location = new System.Drawing.Point(78, 87);
			this.trkNoise.Maximum = 4;
			this.trkNoise.Name = "trkNoise";
			this.trkNoise.Size = new System.Drawing.Size(171, 28);
			this.trkNoise.TabIndex = 2;
			this.trkNoise.TickStyle = System.Windows.Forms.TickStyle.None;
			this.toolTip.SetToolTip(this.trkNoise, "Adds noise to the sound output");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 89);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 18;
			this.label1.Text = "Noise level";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(11, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(106, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Sound output device";
			// 
			// cbSoundDevs
			// 
			this.cbSoundDevs.DisplayMember = "00000000-0000-0000-0000-000000000000";
			this.cbSoundDevs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSoundDevs.FormattingEnabled = true;
			this.cbSoundDevs.Location = new System.Drawing.Point(12, 24);
			this.cbSoundDevs.Name = "cbSoundDevs";
			this.cbSoundDevs.Size = new System.Drawing.Size(230, 21);
			this.cbSoundDevs.TabIndex = 0;
			this.toolTip.SetToolTip(this.cbSoundDevs, "Selects the sound output device (sound card) to use");
			this.cbSoundDevs.ValueMember = "00000000-0000-0000-0000-000000000000";
			this.cbSoundDevs.SelectedIndexChanged += new System.EventHandler(this.cbSoundDevs_SelectedIndexChanged);
			// 
			// chkStatic
			// 
			this.chkStatic.AutoSize = true;
			this.chkStatic.Location = new System.Drawing.Point(15, 119);
			this.chkStatic.Name = "chkStatic";
			this.chkStatic.Size = new System.Drawing.Size(94, 17);
			this.chkStatic.TabIndex = 3;
			this.chkStatic.Text = "Static Crashes";
			this.toolTip.SetToolTip(this.chkStatic, "Adds static crashes to sound output");
			this.chkStatic.UseVisualStyleBackColor = true;
			// 
			// chkFading
			// 
			this.chkFading.AutoSize = true;
			this.chkFading.Location = new System.Drawing.Point(171, 119);
			this.chkFading.Name = "chkFading";
			this.chkFading.Size = new System.Drawing.Size(75, 17);
			this.chkFading.TabIndex = 4;
			this.chkFading.Text = "HF Fading";
			this.toolTip.SetToolTip(this.chkFading, "Adds ionospheric fading to sound output");
			this.chkFading.UseVisualStyleBackColor = true;
			// 
			// SoundCfgForm
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(253, 190);
			this.Controls.Add(this.chkFading);
			this.Controls.Add(this.chkStatic);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbSoundDevs);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.Controls.Add(this.trkNoise);
			this.Controls.Add(this.lblTiming);
			this.Controls.Add(this.nudRiseFall);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SoundCfgForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sound Settings";
			((System.ComponentModel.ISupportInitialize)(this.nudRiseFall)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkNoise)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblTiming;
		private System.Windows.Forms.NumericUpDown nudRiseFall;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.TrackBar trkNoise;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbSoundDevs;
		private System.Windows.Forms.CheckBox chkStatic;
		private System.Windows.Forms.CheckBox chkFading;
	}
}