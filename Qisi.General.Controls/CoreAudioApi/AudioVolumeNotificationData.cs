using System;
namespace CoreAudioApi
{
	public class AudioVolumeNotificationData
	{
		private Guid _EventContext;
		private bool _Muted;
		private float _MasterVolume;
		private int _Channels;
		private float[] _ChannelVolume;
		public Guid EventContext
		{
			get
			{
				return this._EventContext;
			}
		}
		public bool Muted
		{
			get
			{
				return this._Muted;
			}
		}
		public float MasterVolume
		{
			get
			{
				return this._MasterVolume;
			}
		}
		public int Channels
		{
			get
			{
				return this._Channels;
			}
		}
		public float[] ChannelVolume
		{
			get
			{
				return this._ChannelVolume;
			}
		}
		public AudioVolumeNotificationData(Guid eventContext, bool muted, float masterVolume, float[] channelVolume)
		{
			this._EventContext = eventContext;
			this._Muted = muted;
			this._MasterVolume = masterVolume;
			this._Channels = channelVolume.Length;
			this._ChannelVolume = channelVolume;
		}
	}
}
