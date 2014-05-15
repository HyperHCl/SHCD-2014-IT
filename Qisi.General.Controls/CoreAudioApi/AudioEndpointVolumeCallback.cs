using CoreAudioApi.Interfaces;
using System;
using System.Runtime.InteropServices;
namespace CoreAudioApi
{
	internal class AudioEndpointVolumeCallback : IAudioEndpointVolumeCallback
	{
		private AudioEndpointVolume _Parent;
		internal AudioEndpointVolumeCallback(AudioEndpointVolume parent)
		{
			this._Parent = parent;
		}
		[PreserveSig]
		public int OnNotify(IntPtr NotifyData)
		{
			AUDIO_VOLUME_NOTIFICATION_DATA aUDIO_VOLUME_NOTIFICATION_DATA = (AUDIO_VOLUME_NOTIFICATION_DATA)Marshal.PtrToStructure(NotifyData, typeof(AUDIO_VOLUME_NOTIFICATION_DATA));
			IntPtr value = Marshal.OffsetOf(typeof(AUDIO_VOLUME_NOTIFICATION_DATA), "ChannelVolume");
			IntPtr ptr = (IntPtr)((long)NotifyData + (long)value);
			float[] array = new float[aUDIO_VOLUME_NOTIFICATION_DATA.nChannels];
			int num = 0;
			while ((long)num < (long)((ulong)aUDIO_VOLUME_NOTIFICATION_DATA.nChannels))
			{
				array[num] = (float)Marshal.PtrToStructure(ptr, typeof(float));
				num++;
			}
			AudioVolumeNotificationData notificationData = new AudioVolumeNotificationData(aUDIO_VOLUME_NOTIFICATION_DATA.guidEventContext, aUDIO_VOLUME_NOTIFICATION_DATA.bMuted, aUDIO_VOLUME_NOTIFICATION_DATA.fMasterVolume, array);
			this._Parent.FireNotification(notificationData);
			return 0;
		}
	}
}
