using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioEndPointVolumeVolumeRange
	{
		private float _VolumeMindB;
		private float _VolumeMaxdB;
		private float _VolumeIncrementdB;
		public float MindB
		{
			get
			{
				return this._VolumeMindB;
			}
		}
		public float MaxdB
		{
			get
			{
				return this._VolumeMaxdB;
			}
		}
		public float IncrementdB
		{
			get
			{
				return this._VolumeIncrementdB;
			}
		}
		internal AudioEndPointVolumeVolumeRange(IAudioEndpointVolume parent)
		{
			Marshal.ThrowExceptionForHR(parent.GetVolumeRange(out this._VolumeMindB, out this._VolumeMaxdB, out this._VolumeIncrementdB));
		}
	}
}
