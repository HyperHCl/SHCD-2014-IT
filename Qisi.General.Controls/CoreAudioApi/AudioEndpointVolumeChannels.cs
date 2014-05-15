using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	public class AudioEndpointVolumeChannels
	{
		private IAudioEndpointVolume _AudioEndPointVolume;
		private AudioEndpointVolumeChannel[] _Channels;
		public int Count
		{
			get
			{
				int result;
				Marshal.ThrowExceptionForHR(this._AudioEndPointVolume.GetChannelCount(out result));
				return result;
			}
		}
		public AudioEndpointVolumeChannel this[int index]
		{
			get
			{
				return this._Channels[index];
			}
		}
		internal AudioEndpointVolumeChannels(IAudioEndpointVolume parent)
		{
			this._AudioEndPointVolume = parent;
			int count = this.Count;
			this._Channels = new AudioEndpointVolumeChannel[count];
			for (int i = 0; i < count; i++)
			{
				this._Channels[i] = new AudioEndpointVolumeChannel(this._AudioEndPointVolume, i);
			}
		}
	}
}
