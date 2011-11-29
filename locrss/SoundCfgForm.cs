//
// 02-Jun-11	rbd		1.8.0 - Vary label and re-use timing control for two
//						purposes, depending on the mode.
// 14-Jul-11	rbd		2.1.4 - Add noise level slider and property
// 28-Nov-11	rbd		2.2.0 - (SF #3444355) Hide DirectX checkbox, 
//						effectively hardwire to DirectX. SF #3432844 Add 
//						sound device selection.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.DirectSound;

namespace com.dc3
{
	public partial class SoundCfgForm : Form
	{
		private struct SoundDev
		{
			public DeviceInformation info;
			public override string ToString() { return info.Description; }	// Allow display in list
			public SoundDev(DeviceInformation di) { info = di; }
		}

		private string _soundDevGUID = "00000000-0000-0000-0000-000000000000";

		public SoundCfgForm(string guid)
		{
			InitializeComponent();
			_soundDevGUID = guid;
			DevicesCollection myDevices = new DevicesCollection();
			int iSel = -1;
			int i = 0;
			foreach (DeviceInformation info in myDevices)
			{
				SoundDev sd = new SoundDev(info);
				cbSoundDevs.Items.Add(sd);
				if (info.DriverGuid.ToString() == _soundDevGUID)
					iSel = i;
				i += 1;
			}
			cbSoundDevs.SelectedIndex = iSel;
		}

		public string SoundDevGUID
		{
			get { return _soundDevGUID; }
		}

		public int TimingComp	// TODO Rename to envelope time, also nudTimingComp control name
		{
			get { return (int)nudTimingComp.Value; }
			set { nudTimingComp.Value = (decimal)value; }
		}

		public bool UseDirectX
		{
			//get { return chkDirectX.Checked; }
			//set { chkDirectX.Checked = value; }
			get { return true; }
			set { }
		}

		public int NoiseLevel
		{
			get { return trkNoise.Value; }
			set { trkNoise.Value = value; }
		}

		private void cbSoundDevs_SelectedIndexChanged(object sender, EventArgs e)
		{
			_soundDevGUID = ((SoundDev)cbSoundDevs.SelectedItem).info.DriverGuid.ToString();
		}

		//private void chkDirectX_CheckedChanged(object sender, EventArgs e)
		//{
		//    if (chkDirectX.Checked)
		//    {
		//        lblTiming.Text = "Rise/Fall (ms)";
		//        toolTip.SetToolTip(nudTimingComp, "Sets the envelope rise and fall times");
		//    }
		//    else
		//    {
		//        lblTiming.Text = "Timing comp (ms)";
		//        toolTip.SetToolTip(nudTimingComp, "Reduce inter-character spacing for sound latency");
		//    }
		//}
	}

}
