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
			this.chkDirectX = new System.Windows.Forms.CheckBox();
			this.nudTimingComp = new System.Windows.Forms.NumericUpDown();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.nudTimingComp)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(97, 73);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(72, 25);
			this.cmdOK.TabIndex = 0;
			this.cmdOK.Text = "OK";
			this.cmdOK.UseVisualStyleBackColor = true;
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(19, 73);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(72, 25);
			this.cmdCancel.TabIndex = 1;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			// 
			// lblTiming
			// 
			this.lblTiming.AutoSize = true;
			this.lblTiming.Location = new System.Drawing.Point(10, 41);
			this.lblTiming.Name = "lblTiming";
			this.lblTiming.Size = new System.Drawing.Size(89, 13);
			this.lblTiming.TabIndex = 15;
			this.lblTiming.Text = "Timing comp (ms)";
			// 
			// chkDirectX
			// 
			this.chkDirectX.AutoSize = true;
			this.chkDirectX.Location = new System.Drawing.Point(15, 12);
			this.chkDirectX.Name = "chkDirectX";
			this.chkDirectX.Size = new System.Drawing.Size(130, 17);
			this.chkDirectX.TabIndex = 16;
			this.chkDirectX.Text = "Use DirectX for sound";
			this.toolTip.SetToolTip(this.chkDirectX, "Use Microsoft DirectX for sound output");
			this.chkDirectX.UseVisualStyleBackColor = true;
			this.chkDirectX.CheckedChanged += new System.EventHandler(this.chkDirectX_CheckedChanged);
			// 
			// nudTimingComp
			// 
			this.nudTimingComp.Location = new System.Drawing.Point(101, 39);
			this.nudTimingComp.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.nudTimingComp.Name = "nudTimingComp";
			this.nudTimingComp.Size = new System.Drawing.Size(43, 20);
			this.nudTimingComp.TabIndex = 14;
			this.toolTip.SetToolTip(this.nudTimingComp, "Reduce inter-character spacing for sound latency");
			// 
			// SoundCfgForm
			// 
			this.AcceptButton = this.cmdOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(189, 113);
			this.Controls.Add(this.chkDirectX);
			this.Controls.Add(this.lblTiming);
			this.Controls.Add(this.nudTimingComp);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SoundCfgForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sound Settings";
			((System.ComponentModel.ISupportInitialize)(this.nudTimingComp)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblTiming;
		private System.Windows.Forms.NumericUpDown nudTimingComp;
		private System.Windows.Forms.CheckBox chkDirectX;
		private System.Windows.Forms.ToolTip toolTip;
	}
}