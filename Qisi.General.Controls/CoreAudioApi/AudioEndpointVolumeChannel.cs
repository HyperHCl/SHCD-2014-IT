using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioEndpointVolumeChannel
	{
		private uint _Channel;
		private IAudioEndpointVolume _AudioEndpointVolume;
		public float VolumeLevel
		{
			get
			{
				float result;
				Marshal.ThrowExceptionForHR(this._AudioEndpointVolume.GetChannelVolumeLevel(this._Channel, out result));
				return result;
			}
			set
			{
				Marshal.ThrowExceptionForHR(this._AudioEndpointVolume.SetChannelVolumeLevel(this._Channel, value, Guid.Empty));
			}
		}
		public float VolumeLevelScalar
		{
			get
			{
				float result;
				Marshal.ThrowExceptionForHR(this._AudioEndpointVolume.GetChannelVolumeLevelScalar(this._Channel, out result));
				return result;
			}
			set
			{
				Marshal.ThrowExceptionForHR(this._AudioEndpointVolume.SetChannelVolumeLevelScalar(this._Channel, value, Guid.Empty));
			}
		}
		internal AudioEndpointVolumeChannel(IAudioEndpointVolume parent, int channel)
		{
			this._Channel = (uint)channel;
			this._AudioEndpointVolume = parent;
		}
	}
}
