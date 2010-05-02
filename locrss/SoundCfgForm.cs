using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
#if NODX
			chkDirectX.Enabled = false;
#endif
		}

		public int TimingComp
		{
			get { return (int)nudTimingComp.Value; }
			set { nudTimingComp.Value = (decimal)value; }
		}
#if NODX
		public bool UseDirectX
		{
			get { return false; }
			set {  }
		}
#else
		public bool UseDirectX
		{
			get { return chkDirectX.Checked; }
			set { chkDirectX.Checked = value; }
		}
#endif
	}

}
