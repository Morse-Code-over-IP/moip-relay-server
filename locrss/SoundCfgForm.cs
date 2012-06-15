//
// 02-Jun-11	rbd		1.8.0 - Vary label and re-use timing control for two
//						purposes, depending on the mode.
// 14-Jul-11	rbd		2.1.4 - Add noise level slider and property
// 28-Nov-11	rbd		2.2.0 - (SF #3444355) Hide DirectX checkbox, 
//						effectively hardwire to DirectX. SF #3432844 Add 
//						sound device selection.
// 11-Jun-12	rbd		2.6.0 - SF #3534418 Static crashes and HF fading.
//						Rename timing comp items to rise fall to reflect 
//						their true purpose. Remove old DirectX control and
//						members. No more SF tones.
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

		public int RiseFall
		{
			get { return (int)nudRiseFall.Value; }
			set { nudRiseFall.Value = (decimal)value; }
		}

		public int NoiseLevel
		{
			get { return trkNoise.Value; }
			set { trkNoise.Value = value; }
		}

		public bool StaticCrashes
		{
			get { return chkStatic.Checked; }
			set { chkStatic.Checked = value; }
		}

		public bool HfFading
		{
			get { return chkFading.Checked; }
			set { chkFading.Checked = value; }
		}

		private void cbSoundDevs_SelectedIndexChanged(object sender, EventArgs e)
		{
			_soundDevGUID = ((SoundDev)cbSoundDevs.SelectedItem).info.DriverGuid.ToString();
		}
	}

}
