//
// 02-Jun-11	rbd		1.8.0 - Vary label and re-use timing control for two
//						purposes, depending on the mode.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace com.dc3
{
	public partial class SoundCfgForm : Form
	{
		public SoundCfgForm()
		{
			InitializeComponent();
		}

		public int TimingComp
		{
			get { return (int)nudTimingComp.Value; }
			set { nudTimingComp.Value = (decimal)value; }
		}

		public bool UseDirectX
		{
			get { return chkDirectX.Checked; }
			set { chkDirectX.Checked = value; }
		}

		private void chkDirectX_CheckedChanged(object sender, EventArgs e)
		{
			if (chkDirectX.Checked)
			{
				lblTiming.Text = "Rise/Fall (ms)";
				toolTip.SetToolTip(nudTimingComp, "Sets the envelope rise and fall times");
			}
			else
			{
				lblTiming.Text = "Timing comp (ms)";
				toolTip.SetToolTip(nudTimingComp, "Reduce inter-character spacing for sound latency");
			}
		}
	}

}
