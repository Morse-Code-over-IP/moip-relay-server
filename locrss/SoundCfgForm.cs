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
	}

}
