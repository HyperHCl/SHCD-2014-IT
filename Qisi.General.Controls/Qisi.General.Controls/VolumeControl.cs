using CoreAudioApi;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Qisi.General.Controls
{
	public class VolumeControl : UserControl
	{
		private IContainer components;
		private TrackBar trackBar1;
		[DllImport("winmm.dll")]
		public static extern long waveOutSetVolume(uint deviceID, uint Volume);
		[DllImport("winmm.dll")]
		public static extern long waveOutGetVolume(uint deviceID, out uint Volume);
		public VolumeControl()
		{
			this.InitializeComponent();
		}
		private void GetVolume()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				try
				{
					MMDeviceEnumerator mMDeviceEnumerator = new MMDeviceEnumerator();
					MMDevice defaultAudioEndpoint = mMDeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
					this.trackBar1.Value = (int)(defaultAudioEndpoint.AudioEndpointVolume.MasterVolumeLevelScalar * (float)(this.trackBar1.Maximum - this.trackBar1.Minimum));
					return;
				}
				catch
				{
					this.trackBar1.Value = 0;
					return;
				}
			}
			uint deviceID = 0u;
			uint num;
			VolumeControl.waveOutGetVolume(deviceID, out num);
			uint num2 = num & 65535u;
			uint num3 = (num & 4294901760u) >> 16;
			this.trackBar1.Value = (int.Parse(num2.ToString()) | int.Parse(num3.ToString())) * (this.trackBar1.Maximum - this.trackBar1.Minimum) / 65535;
		}
		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				try
				{
					MMDeviceEnumerator mMDeviceEnumerator = new MMDeviceEnumerator();
					MMDevice defaultAudioEndpoint = mMDeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
					defaultAudioEndpoint.AudioEndpointVolume.MasterVolumeLevelScalar = (float)this.trackBar1.Value / (float)(this.trackBar1.Maximum - this.trackBar1.Minimum);
					return;
				}
				catch
				{
					return;
				}
			}
			uint num = (uint)(65535.0 * (double)this.trackBar1.Value / (double)(this.trackBar1.Maximum - this.trackBar1.Minimum));
			if (num < 0u)
			{
				num = 0u;
			}
			if (num > 65535u)
			{
				num = 65535u;
			}
			uint num2 = num;
			uint num3 = num;
			VolumeControl.waveOutSetVolume(0u, num2 << 16 | num3);
		}
		private void VolumeControl_Load(object sender, EventArgs e)
		{
			this.GetVolume();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.trackBar1 = new TrackBar();
			((ISupportInitialize)this.trackBar1).BeginInit();
			base.SuspendLayout();
			this.trackBar1.AutoSize = false;
			this.trackBar1.BackColor = Color.White;
			this.trackBar1.Dock = DockStyle.Fill;
			this.trackBar1.Location = new Point(0, 0);
			this.trackBar1.Margin = new Padding(2);
			this.trackBar1.Maximum = 100;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new Size(118, 30);
			this.trackBar1.TabIndex = 0;
			this.trackBar1.Scroll += new EventHandler(this.trackBar1_Scroll);
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.BorderStyle = BorderStyle.FixedSingle;
			base.Controls.Add(this.trackBar1);
			this.Cursor = Cursors.Hand;
			this.DoubleBuffered = true;
			base.Margin = new Padding(2);
			base.Name = "VolumeControl";
			base.Size = new Size(118, 30);
			base.Load += new EventHandler(this.VolumeControl_Load);
			((ISupportInitialize)this.trackBar1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
